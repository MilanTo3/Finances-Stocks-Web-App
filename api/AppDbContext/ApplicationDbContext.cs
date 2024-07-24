using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using api.Models;

namespace api.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions){

        }

        public DbSet<Stock> Stocks{get;set;}
        public DbSet<Comment> Comments{get;set;}

        protected override void OnModelCreating(ModelBuilder mb){
            base.OnModelCreating(mb);
            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{ Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole{ Name = "User", NormalizedName = "USER" }

            };
            mb.Entity<IdentityRole>().HasData(roles);

        }
    }
}