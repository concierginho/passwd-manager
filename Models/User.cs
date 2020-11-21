namespace inz_int.Models
{
    public class User
    {
        public User(int id, string firstName, string lastName, string login, string passwd) 
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Passwd = passwd;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Passwd { get; set; }
    }
}