using System.Linq;
using inz_int.Data;
using inz_int.Models;

namespace inz_int.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IUserRepository _userRepository;
        private readonly ValidUsersContext _validUsersContext;

        public JwtAuthenticationManager(IUserRepository userRepository, ValidUsersContext validUsersContext)
        {
            _userRepository = userRepository;
            _validUsersContext = validUsersContext;
        }

        public User Authenticate(string login, string passwd)
        {
            if(!_userRepository.GetAllUsers()
                .Any(user => user.Login == login && user.Passwd == passwd))
                return null;

            var userdb = _userRepository.GetAllUsers()
                .Where(user => user.Login == login && user.Passwd == passwd)
                .FirstOrDefault();

            var user = new User 
            {
                Login = userdb.Login,
                Passwd = userdb.Passwd,
                FirstName = userdb.FirstName,
                LastName = userdb.LastName,
                Role = userdb.Role
            };

            return user;
        }
    }
}