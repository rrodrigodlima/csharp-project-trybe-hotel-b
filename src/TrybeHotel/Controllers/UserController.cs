using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            var existingUser = _repository.GetUserByEmail(user.Email);
            if (existingUser != null) return Conflict(new { message = "User email already exists" });

            return Created("", _repository.Add(user));
        }
    }
}