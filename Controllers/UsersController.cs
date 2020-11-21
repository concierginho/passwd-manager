using System.Collections.Generic;
using inz_int.Data;
using inz_int.Models;
using Microsoft.AspNetCore.Mvc;

namespace inz_int.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MockUserRepository _repository = new MockUserRepository();

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")] 
        public ActionResult<User> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            return Ok(user);
        }
    }
}