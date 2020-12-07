using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace inz_int.Data
{
    public class ValidUsersContext
    {
        private IDictionary<string, SecurityToken> _validatedUsers;
        public ValidUsersContext()
        {
            _validatedUsers = new Dictionary<string, SecurityToken>();
        }

        public IEnumerable<KeyValuePair<string, SecurityToken>> GetValidatedUsers()
        {
            return _validatedUsers;
        }

        public SecurityToken GetTokenByLogin(string login)
        {
            SecurityToken result;
            _validatedUsers.TryGetValue(login, out result);
            return result;
        }

        public void AddValidatedUser(string login, SecurityToken token)
        {
            _validatedUsers.Add(login, token);
        }
    }
}