using inz_int.Models;
using Microsoft.EntityFrameworkCore;

namespace inz_int.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}