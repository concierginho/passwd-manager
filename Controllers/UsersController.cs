using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using inz_int.Authentication;
using inz_int.Data;
using inz_int.DTOs;
using inz_int.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inz_int.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository, IMapper mapper, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<UserReadDTO>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        [HttpGet("{id}", Name="GetUserById")] 
        public ActionResult<UserReadDTO> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if(user == null)
                return NotFound(user);
            return Ok(_mapper.Map<UserReadDTO>(user));
        }

        [HttpPost]
        public ActionResult<UserReadDTO> CreateUser(UserCreateDTO userCreateDTO)
        {
            var duplicates = _repository.GetAllUsers().Where(suspect => suspect.Login == userCreateDTO.Login).ToList();
            if(duplicates.Count > 0)
                return Conflict("User with given login already exists.");

            var userModel = _mapper.Map<User>(userCreateDTO);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDTO = _mapper.Map<UserReadDTO>(userModel);
            return CreatedAtRoute(nameof(GetUserById), new {Id = userModel.Id}, userReadDTO);
        }

        [HttpPost("login")]
        public IActionResult Authenticate(UserLoginReadDTO userLogin)
        {
            var token = _jwtAuthenticationManager.Authenticate(userLogin.Login, userLogin.Passwd);
            if(token == null)
                return Unauthorized();
            return Ok(new UserLoginAnswerDTO{ Token = token });
        }
    }
}