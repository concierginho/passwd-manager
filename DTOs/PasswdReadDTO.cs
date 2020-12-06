using System.ComponentModel.DataAnnotations;

namespace inz_int.DTOs
{
    public class PasswdReadDTO
    {
        public string Site { get; set; }
        public string Login { get; set; }
        public string Passwd { get; set; }
    }
}