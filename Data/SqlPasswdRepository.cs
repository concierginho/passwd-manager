using System.Collections.Generic;
using System.Linq;
using inz_int.Models;

namespace inz_int.Data
{
    public class SqlPasswdRepository : IPasswdRepository
    {
        private readonly PasswdContext _context;

        public SqlPasswdRepository(PasswdContext context)
        {
            _context = context;
        }

        public void CreatePasswd(Password password)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Password> GetAllPasswords()
        {
            return _context.Passwords.ToList();
        }

        public IEnumerable<Password> GetPasswdsByUserId()
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }
    }
}