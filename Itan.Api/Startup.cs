using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itan.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(this.Configuration);

            services.AddAuthentication(o => { o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(o =>
                {
                    o.Authority = $"https://login.microsoftonline.com/tfp/{this.Configuration["AzureAdB2C:Tenant"]}/{this.Configuration["AzureAdB2C:Policy"]}/v2.0/";
                    o.Audience = this.Configuration["AzureAdB2C:ClientId"];
                    o.Events = new JwtBearerEvents
                    {
                        // OnAuthenticationFailed = this.AuthenticationFailed,
                        // OnTokenValidated = this.TokenValidated,
                        // OnChallenge = this.Challenge,
                        // OnForbidden = this.Forbidden
                        // OnMessageReceived = this.MessageReceived
                    };
                });


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

        private Task MessageReceived(MessageReceivedContext arg)
        {
            throw new System.NotImplementedException();
        }

        private Task Forbidden(ForbiddenContext arg)
        {
            throw new System.NotImplementedException();
        }

        private Task Challenge(JwtBearerChallengeContext arg)
        {
            throw new System.NotImplementedException();
        }

        private Task TokenValidated(TokenValidatedContext arg)
        {
            throw new System.NotImplementedException();
        }

        private Task AuthenticationFailed(AuthenticationFailedContext arg)
        {
            throw new System.NotImplementedException();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

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
