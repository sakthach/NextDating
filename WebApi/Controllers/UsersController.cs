using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Data;

namespace WebApi.Controllers
{
    public class UsersController:BaseApiController
    {
        private WebContext _webContext;
        public UsersController(WebContext webContext)
        {
            _webContext = webContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(){
            var users = await _webContext.Users.ToListAsync();
            if(users == null){
                return NotFound();
            }

            return Ok(users);
                
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _webContext.Users.FindAsync(id);
            if (user == null){
                return NotFound();
            }
            return Ok(user);
                
        }
        
    }
}