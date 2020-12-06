using AutoMapper;
using inz_int.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace inz_int
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswdController : ControllerBase
    {
        private readonly IPasswdRepository _passwdRepository;
        private readonly IMapper _mapper;

        public PasswdController(IPasswdRepository passwdRepository, IMapper mapper)
        {
            _passwdRepository = passwdRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<PasswdReadDTO> GetPasswds()
        {
            var passwds = _passwdRepository.GetAllPasswords();
            return Ok(passwds);
        }
    }
}