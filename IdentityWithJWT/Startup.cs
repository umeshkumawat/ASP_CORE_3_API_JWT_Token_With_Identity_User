using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IdentityWithJWT.Contracts;
using IdentityWithJWT.Data;
using IdentityWithJWT.Handlers;
using IdentityWithJWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityWithJWT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextHelper(Configuration);

            // Configure Identity
            services.AddIdentityHelper();

            //Configure JWT Authentication
            services.AddAuthenticationJWTBearerHelper(Configuration);

            services.AddAuthorizationHelper();

            services.AddSingleton<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.Configure<JwtSettings>(Configuration.GetSection("JwtConfig"));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceHelper
    {
        public static void AddDbContextHelper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("default"));
            });
        }
        public static void AddIdentityHelper(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void AddAuthenticationJWTBearerHelper(this IServiceCollection services, IConfiguration configuration)
        {
            var JwtSettings = configuration.GetSection("JwtConfig");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = "jwtbearer";
                opt.DefaultChallengeScheme = "jwtbearer";
            })
                .AddJwtBearer("jwtbearer", opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = JwtSettings.GetSection("Issuer").Value,
                        ValidAudience = JwtSettings.GetSection("Audience").Value,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.GetSection("Secret").Value))
                    };

                });
        }

        public static void AddAuthorizationHelper(this IServiceCollection services)
        {
            services.AddAuthorization(conf =>
            {
                conf.AddPolicy("Fake", policy => policy.Requirements.Add(new CustomRequireClaim("FakeClaim")));
            });
        }
    }
}
