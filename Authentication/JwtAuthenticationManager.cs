using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using inz_int.Data;
using Microsoft.IdentityModel.Tokens;

namespace inz_int.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IUserRepository _userRepository;
        private readonly ValidUsersContext _validUsersContext;

        public JwtAuthenticationManager(IUserRepository userRepository, ValidUsersContext validUsersContext)
        {
            _userRepository = userRepository;
            _validUsersContext = validUsersContext;
        }

        public string Authenticate(string login, string passwd)
        {
            if(!_userRepository.GetAllUsers()
                .Any(user => user.Login == login && user.Passwd == passwd))
                return null;

            var key = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _validUsersContext.AddValidatedUser(login, token);

            return tokenHandler.WriteToken(token);
        }
    }
}