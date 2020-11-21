using System.Collections.Generic;
using inz_int.Models;

namespace inz_int.Data
{
    public class MockUserRepository : IUserRepository
    {
        public IEnumerable<User> GetAllUsers()
        {
            var commands = new List<User>
            {
                new User(0, "Sean", "Bean", "dies_in_every_film", "strongPasswd"),
                new User(1, "Leaonardo", "DiCaprio", "finallyGotOscar", "9filmsofQT"),
                new User(2, "Django", "Freeman", "fastestGun", "brunhilda123")
            };
            return commands;
        }
        public User GetUserById(int id)
        {
            return new User(id, "Sean", "Bean", "dies_in_every_film", "strongPasswd");
        }
    }
}