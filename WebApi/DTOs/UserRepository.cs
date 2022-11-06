using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Entities;
using WebApi.interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DTOs
{
    public class UserRepository : IUserRepository
    {

        private WebContext _webContext;
        public UserRepository(WebContext webcontext)
        {
            _webContext = webcontext;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _webContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           return await _webContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _webContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _webContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _webContext.Entry(user).State = EntityState.Modified;
        }
    }
}