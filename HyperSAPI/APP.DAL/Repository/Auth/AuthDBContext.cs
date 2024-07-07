using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.DAL.Repository.Auth
{
    public class AuthDBContext : IdentityDbContext
    {
        public AuthDBContext() { }
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            {
                var connectionString = Environment.GetEnvironmentVariable("MYSQLAUTH_CONNECTION_STRING");

                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
            }
        }
    }
}
