using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskAPI.BusinessLogic.AuthServices;
using TaskAPI.BusinessLogic.Repositories;
using TaskAPI.BusinessLogic.Services;
using TaskAPI.Core.Interfaces;
using TaskAPI.Infrastructure.Data;
using TaskAPI.Infrastructure.Services;
using TaskAPI.Web.Configurations.Middlewares;

namespace TaskAPI.Web.Configurations {
    public static class Extensions {

        public static AuthenticationBuilder AddJwtAuthConfiguration(this IServiceCollection service, string isuer, string audience,string key) {

            return service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = isuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))

                    };
                
                });

        }

        public static void AddAllServices(this IServiceCollection service) {

            service.AddScoped<IDbContext, DbService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IPasswordEncryptor, PasswordEncryptor>();

        }

        public static void ConfigureCustomCORS(this IServiceCollection service) {

            service.AddCors(cors => {
                cors.AddDefaultPolicy(policy => {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                });
            });

        }

        public static void ConfigureCustomMiddlewares(this IApplicationBuilder builder) {

            builder.UseMiddleware<NotFoundMiddleware>();
            builder.UseMiddleware<ExceptionMiddleware>();

        }
    }
}
