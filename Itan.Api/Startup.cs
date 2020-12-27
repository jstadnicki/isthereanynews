﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
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

            services.AddOptions();

            services.AddResponseCompression(o =>
            {
                o.Providers.Add<GzipCompressionProvider>();
                o.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Optimal);

            services
                .AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
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
                            "9181bdde-959f-42a6-a253-b10a6f05d883",
                            "67e91a82-4dbe-4d74-9c8e-68873f4a5e16"
                        },
                        ValidIssuer =
                            $"https://isthereanynewscodeblast.b2clogin.com/{this.configuration["AzureAdB2C:Tenant"]}/v2.0/"
                    };
                });

            services.AddMvc(o =>
                {
                    o.EnableEndpointRouting = false;
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    o.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddCors(a => a.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));

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
                    var s = this.configuration
                        .GetSection("ConnectionStrings")
                        .Get<ConnectionOptions>();
                    return new OptionsWrapper<ConnectionOptions>(s);
                })
                .SingleInstance();
            var assemblies = GetItanReferencedAssemblies(Assembly.GetExecutingAssembly());
            RegisterAssemblyModules(builder, assemblies);
            builder.RegisterModule<ItanApiModule>();
        }

        private void RegisterAssemblyModules(ContainerBuilder builder, List<AssemblyName> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var a = Assembly.Load(assembly);
                builder.RegisterAssemblyModules(a);
                RegisterAssemblyModules(builder, GetItanReferencedAssemblies(a));
            }
        }

        private List<AssemblyName> GetItanReferencedAssemblies(Assembly assembly)
        {
            return assembly
                .GetReferencedAssemblies()
                .Where(f => f.FullName.ToLowerInvariant().Contains("itan"))
                .ToList();
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

            app.UseCors();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}