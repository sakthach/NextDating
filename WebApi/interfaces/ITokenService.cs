using WebApi.Entities;

namespace WebApi.interfaces
{
    public interface ITokenService
    {
      public  string CreateToken(AppUser user);

    }
}