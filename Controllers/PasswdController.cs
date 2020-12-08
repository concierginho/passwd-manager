using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using AutoMapper;
using inz_int.Data;
using inz_int.DTOs;
using inz_int.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inz_int
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswdController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswdRepository _passwdRepository;
        private readonly IMapper _mapper;

        public PasswdController(IPasswdRepository passwdRepository,
            IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwdRepository = passwdRepository;
            _mapper = mapper;
        }
        
        [Authorize(Policy = Policies.Admin)]
        [HttpGet("{id}", Name = "GetPasswdById")]
        public ActionResult<PasswdReadDTO> GetPasswdById(int id)
        {
            var passwd = _passwdRepository.GetPasswdsByUserId(id);
            if(passwd == null)
                return NotFound();
            return Ok(passwd);
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<PasswdReadDTO>> GetAllPasswds(PasswdReadDTO password)
        {
            var passwds = _passwdRepository.GetAllPasswords();
            if(passwds.Count() == 0)
                return NotFound();
            return Ok(passwds);
        }

        [Authorize(Policy = Policies.User)]
        [HttpPost]
        public ActionResult<PasswordCreateDTO> CreatePasswd(PasswordCreateDTO password)
        {
            var token = password.Token;
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var login = decodedToken.Claims.ElementAt(0);

            var user = _userRepository.GetAllUsers().FirstOrDefault(usr => usr.Login == login.Value);
            if(user == null)
                Conflict("user does not exist");
            
            var passwdModel = _mapper.Map<Password>(password);
            passwdModel.OwnerId = user.Id.ToString();
            _passwdRepository.CreatePasswd(passwdModel);
            _passwdRepository.SaveChanges();

            var passwdReadDTO = _mapper.Map<PasswdReadDTO>(passwdModel);
            return CreatedAtRoute(nameof(GetPasswdById),
                new {Id = passwdModel.Id},
            passwdReadDTO);
        }

        [Authorize(Policy = Policies.User)]
        [HttpPost("mypasswds")]
        public ActionResult<IEnumerable<PasswdReadDTO>> GetAllPasswdsByOwnerId(Access access)
        {
            var token = access.Token;
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var login = decodedToken.Claims.ElementAt(0);

            var user = _userRepository.GetAllUsers().FirstOrDefault(usr => usr.Login == login.Value);
            if(user == null)
                Conflict("user doest not exist");
            
            var id = user.Id;
            List<PasswdReadDTO> passwds = new List<PasswdReadDTO>();
            foreach(var passwd in _passwdRepository.GetAllPasswords())
            {
                if(passwd.OwnerId == id.ToString())
                {
                    var passwdReadDTO = _mapper.Map<PasswdReadDTO>(passwd);
                    passwds.Add(passwdReadDTO);
                }
            }
            return Ok(passwds);
        }
    }
}