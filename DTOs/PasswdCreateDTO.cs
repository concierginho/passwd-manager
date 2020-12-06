using System.ComponentModel.DataAnnotations;

namespace inz_int.DTOs
{
    public class PasswordCreateDTO
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Site { get; set; }
        public string Login { get; set; }
        public string Passwd { get; set; }
    }
}