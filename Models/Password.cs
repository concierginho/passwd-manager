using System.ComponentModel.DataAnnotations;

namespace inz_int.Models
{
    public class Password
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        [MaxLength(99)]
        public string Site { get; set; }
        [Required]
        [MaxLength(32)]
        public string Login { get; set; }
        [Required]
        [MaxLength(64)]
        public string Passwd { get; set; }
    }
}