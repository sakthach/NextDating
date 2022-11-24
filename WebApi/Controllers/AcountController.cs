using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;
using WebApi.interfaces;

namespace WebApi.Controllers
{
    public class AcountController: BaseApiController
    {
        private readonly WebContext _webContext;
        private ITokenService _tokenService;
        public AcountController(WebContext webContext, ITokenService tokenService )
        {
            _webContext = webContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
       
        public async Task<ActionResult<UserDto>> Register(RegisterDto request)
        {

            if(await UserExist(request.Username)) return BadRequest("username exist");

            using var hmac = new HMACSHA512();
            var user = new AppUser {
                UserName = request.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key

            };

            _webContext.Users.Add(user);
            await _webContext.SaveChangesAsync();

            return Ok(new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            });
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(RegisterDto request){

        // find a user by username and include the photo as well
            
            var user = await _webContext.Users.Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == request.Username );
        // check if username exist or not
            if(user == null) return Unauthorized("invalid");
        // validate user by password    
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));

            for(int i = 0; i < computedHash.Length; i++ ){
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid");
            }

        // Once user is successfully logged in
        // We return back  username, token and photoUrl

            return new UserDto {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url // we return a photo isMain is true.
            };
        }



        private async Task<bool> UserExist(string username){
            return await _webContext.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        
    }
}