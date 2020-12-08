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
            if(password == null)
                throw new System.ArgumentNullException(nameof(password));

            _context.Add(password);
        }

        public IEnumerable<Password> GetAllPasswords()
        {
            return _context.Passwords.ToList();
        }

        public Password GetPasswdsByUserId(int i)
        {
            return _context.Passwords.FirstOrDefault(p => p.Id == i);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}