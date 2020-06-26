using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Itan.Api.Controllers;
using Itan.Api.Middleware;
using Itan.Common;
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
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.environment = environment;
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<IConfiguration>(this.configuration);

            services.AddResponseCompression(o =>
            {
                o.Providers.Add<GzipCompressionProvider>();
                o.EnableForHttps = true;
            });
            
            services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);

            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => { this.configuration.Bind("AzureAdB2C", options); });

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority =
                        $"https://login.microsoftonline.com/tfp/{this.configuration["AzureAdB2C:Tenant"]}/{this.configuration["AzureAdB2C:Policy"]}/v2.0/";
                    jwtOptions.Audience = this.configuration["AzureAdB2C:ClientId"];
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = this.AuthenticationFailed,
                    };
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new[]
                        {
                            "05cd7635-e6f4-47c9-a5ce-8ec04368b297",
                            "f1ab593c-f0b4-44da-85dc-d89a457745a9",
                        },
                        ValidIssuer =
                            "https://isthereanynewscodeblast.b2clogin.com/3408b585-a1ca-41d4-ae2f-ea3ea685223f/v2.0/"
                    };
                });

            // services.AddControllers(o =>
            //     {
            //         o.EnableEndpointRouting = false;
            //         var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //         o.Filters.Add(new AuthorizeFilter(policy));
            //     })
            //     .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //     .AddNewtonsoftJson();

            services.AddMvc(o =>
                {
                    o.EnableEndpointRouting = false;
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddCors();

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
            var path = AppContext.BaseDirectory;
            var files = Directory.GetFiles(path, "itan.*.dll", SearchOption.TopDirectoryOnly);
            var x = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            builder.Register<ConnectionOptions>(context => this.configuration
                .GetSection("ConnectionStrings")
                .Get<ConnectionOptions>())
                .SingleInstance();

            builder.RegisterModule<ItanApiModule>();
            
            // builder.RegisterType<IOptions<ConnectionOptions>>()
            // {
            //     this.configuration
            //         .GetSection("ConnectionStrings")
            //         .Get<ConnectionOptions>();
            // });
            
            foreach (var assembly in x)
            {
                if (assembly.FullName.ToLower().Contains("itan"))
                {
                    var a = Assembly.Load(assembly);
                    builder.RegisterAssemblyModules(a);
                }
            }
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

            app.UseCors(b =>
                b.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}