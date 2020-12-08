using System.Collections.Generic;
using inz_int.Models;

namespace inz_int
{
    public interface IPasswdRepository
    {
        IEnumerable<Password> GetAllPasswords();
        Password GetPasswdsByUserId(int i );
        void CreatePasswd(Password password);
        bool SaveChanges();
    }
}