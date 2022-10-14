using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data
{
    public class WebContext:DbContext
    {
        public WebContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<AppUser> Users {get; set;}


        
    }
}