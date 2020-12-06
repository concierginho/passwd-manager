using System.Collections.Generic;
using inz_int.Models;
using Microsoft.EntityFrameworkCore;

namespace inz_int.Data
{
    public class PasswdContext : DbContext
    {
        public PasswdContext(DbContextOptions<PasswdContext> opt) : base(opt)
        {
        }

        public DbSet<Password> Passwords { get; set; }
    }
}