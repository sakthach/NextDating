
using WebApi.interfaces;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace WebApi.Extenstions
{
    public static class  ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config){
 
            Services.AddScoped<ITokenService, TokenService>();
            var myString = config.GetSection("TokenKey");
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(myString.Value)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            return  Services;
        }
        
    }
}