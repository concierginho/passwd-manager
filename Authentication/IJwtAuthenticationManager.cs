using inz_int.Models;

namespace inz_int.Authentication
{
    public interface IJwtAuthenticationManager
    {
        public User Authenticate(string login, string passwd);
    }
}