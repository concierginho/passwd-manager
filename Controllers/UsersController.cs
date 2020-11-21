using System.Collections.Generic;
using AutoMapper;
using inz_int.Data;
using inz_int.DTOs;
using inz_int.Models;
using Microsoft.AspNetCore.Mvc;

namespace inz_int.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDTO>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        [HttpGet("{id}")] 
        public ActionResult<UserReadDTO> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if(user == null)
                return NotFound(user);
            return Ok(_mapper.Map<UserReadDTO>(user));
        }
    }
}