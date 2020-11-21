using System.Collections.Generic;
using inz_int.Models;

namespace inz_int.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
    }
}