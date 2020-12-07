namespace inz_int.Authentication
{
    public interface IJwtAuthenticationManager
    {
        public string Authenticate(string login, string passwd);
    }
}