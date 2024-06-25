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
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            {
                var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
                if (connectionString == null)
                {
                    connectionString = "Server=hyperssql-cakhosolo2003-325a.e.aivencloud.com;Port=17997;Database=authdb;User=avnadmin;Password=AVNS_EBxOtAQ6lHdDe2fbQEh;SslMode=Required;";
                }
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
            }
        }
    }
}
