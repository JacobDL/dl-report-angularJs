using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamLogisticsSqlSPA.ControllerLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using Repository.Models;
using Repository.Repositories.Query_and_QueryParam;
using Repository.Database.Query_and_QueryParam.Interfaces;
using Repository.Database.Query_and_QueryParam;
using Repository.Database;
using Repository.Repositories;

namespace DreamLogisticsSqlSPA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = appSettings.Secret;

            services.AddAuthentication(au =>
            {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                jwt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = AdditionalValidation
                };
            });
            services.AddScoped<IDelete, Delete>();
            services.AddScoped<IInsert, Insert>();
            services.AddScoped<ISelectQuery, SelectQuery>();
            services.AddScoped<ISelectQueryParam, SelectQueryParam>();
            services.AddScoped<IUpdate, Update>();

            services.AddScoped<IQueryRequestDb, QueryRequestDb>();
            services.AddScoped<ISqlListDb, SqlListDb>();
            services.AddScoped<IUserDb, UserDb>();

            services.AddScoped<IQueryRequestRepository, QueryRequestRepository>();
            services.AddScoped<ISqlListRepository, SqlListRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQueryRepository, QueryRepository>();

            services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();
        }

        private static Task AdditionalValidation(TokenValidatedContext context)
        {
            var accessToken = context.SecurityToken as JwtSecurityToken;
            var jsonTokenString = accessToken.Payload;
            string role = jsonTokenString["role"].ToString();
            if (role == "2")
            {
                string query = "/api/query";
                string search = "/api/search/";
                var path = context.Request.Path.Value;
                if (path == query + "/QueryParams" || path.Contains(query + "/List/") || path == search + "Excel" || path == search || path == query)
                {
                    return Task.CompletedTask;
                }
                else
                {
                    context.Fail("Failed additional validation");
                }
            }

            return Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
