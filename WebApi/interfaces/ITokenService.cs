using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.interfaces
{
    public interface ITokenService
    {
      public  string CreateToken(AppUser user);
    }
}