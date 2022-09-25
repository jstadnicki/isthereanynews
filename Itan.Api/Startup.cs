using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Itan.Api.Middleware;
using Itan.Common;
using Itan.ModuleRegistrator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Itan.Api
{
    public class Startup
    {
        // private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            // _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<IConfiguration>(_configuration);
            services.AddCors(a => a.AddPolicy("itan",builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200", "https://www.isthereanynews.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));
            services.AddOptions();

            services.AddResponseCompression(o =>
            {
                o.Providers.Add<GzipCompressionProvider>();
                o.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);

            services
                .AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => { _configuration.Bind("AzureAdB2C", options); });

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority =
                        $"https://login.microsoftonline.com/tfp/{_configuration["AzureAdB2C:Tenant"]}/{_configuration["AzureAdB2C:Policy"]}/v2.0/";
                    jwtOptions.Audience = _configuration["AzureAdB2C:ClientId"];
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = AuthenticationFailed,
                    };
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new[]
                        {
                            "9181bdde-959f-42a6-a253-b10a6f05d883",
                            "67e91a82-4dbe-4d74-9c8e-68873f4a5e16"
                        },
                        ValidIssuer =
                            $"https://isthereanynewscodeblast.b2clogin.com/{_configuration["AzureAdB2C:Tenant"]}/v2.0/"
                    };
                });
            
            services.AddMvc(o =>
                {
                    o.EnableEndpointRouting = false;
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            

            services.AddAuthorization();
        }

        private Task AuthenticationFailed(AuthenticationFailedContext arg)
        {
            var s = $"AuthenticationFailed: {arg.Exception.Message}";
            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
            return Task.FromResult(0);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register<IOptions<ConnectionOptions>>(context =>
                {
                    var s = _configuration
                        .GetSection("ConnectionStrings")
                        .Get<ConnectionOptions>();
                    return new OptionsWrapper<ConnectionOptions>(s);
                })
                .SingleInstance();
            
            builder.RegisterModule(new ItanModuleRegistrator(_configuration));
            builder.RegisterModule<ItanApiModule>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ItanExceptionMiddleware>();
            app.UseResponseCompression();

            app.UseAuthentication();
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("itan");
            app.UseMvc();
        }
    }
}