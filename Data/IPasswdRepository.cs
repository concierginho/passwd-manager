using System.Collections.Generic;
using inz_int.Models;

namespace inz_int
{
    public interface IPasswdRepository
    {
        IEnumerable<Password> GetAllPasswords();
        IEnumerable<Password> GetPasswdsByUserId();
        void CreatePasswd(Password password);
        bool SaveChanges();
    }
}