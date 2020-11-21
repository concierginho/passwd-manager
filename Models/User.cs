using System.ComponentModel.DataAnnotations;

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

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(99)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(99)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(32)]
        public string Login { get; set; }
        [Required]
        [MaxLength(64)]
        public string Passwd { get; set; }
    }
}