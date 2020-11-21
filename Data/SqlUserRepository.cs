using System.Collections.Generic;
using System.Linq;
using inz_int.Models;

namespace inz_int.Data
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public SqlUserRepository(UserContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            if(user == null)
                throw new System.ArgumentNullException(nameof(user));

            _context.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}