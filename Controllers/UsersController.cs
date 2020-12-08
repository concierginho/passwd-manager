using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using inz_int.Authentication;
using inz_int.Data;
using inz_int.DTOs;
using inz_int.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace inz_int.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository,
            IMapper mapper,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IConfiguration configuration)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        public ActionResult<IEnumerable<UserReadDTO>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        [HttpGet("{id}", Name="GetUserById")]
        [Authorize(Policy = Policies.Admin)]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate(UserLoginReadDTO userLogin)
        {
            var user = _jwtAuthenticationManager.Authenticate(userLogin.Login, userLogin.Passwd);
            if(user == null)
                return Unauthorized();
            
            var token = GenerateJwtToken(user);
            return Ok(new UserLoginAnswerDTO{ Token = token, Role = user.Role });
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim("first name", user.FirstName),
                new Claim("last name", user.LastName),
                new Claim("role", user.Role),
                new Claim("passwd", user.Passwd),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}