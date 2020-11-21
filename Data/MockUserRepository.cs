using System.Collections.Generic;
using inz_int.Models;

namespace inz_int.Data
{
    public class MockUserRepository : IUserRepository
    {
        public void CreateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var commands = new List<User>
            {
                new User{Id=0, FirstName="Sean", LastName="Bean", Login="dies_in_every_film", Passwd="strongPasswd"},
                new User{Id=1, FirstName="Leaonardo", LastName="DiCaprio", Login="finallyGotOscar", Passwd="9filmsofQT"},
                new User{Id=2, FirstName="Django", LastName="Freeman", Login="fastestGun", Passwd="brunhilda123"}
            };
            return commands;
        }
        public User GetUserById(int id)
        {
            return new User{Id=id, FirstName="Sean", LastName="Bean", Login="dies_in_every_film", Passwd="strongPasswd"};
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}