
using WebApi.interfaces;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.DTOs;
using WebApi.Helpers;

namespace WebApi.Extenstions
{
    public static class  ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config){
            
            Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IphotoService, PhotoService>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);



            var tokenKey = config.GetSection("TokenKey");
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey.Value)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            return  Services;
        }
        
    }
}