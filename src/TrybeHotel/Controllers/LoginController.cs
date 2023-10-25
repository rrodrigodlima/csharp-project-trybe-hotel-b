using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            // Autenticar o usuário
            var userDto = _repository.Login(login);

            if (userDto == null)
            {
                return Unauthorized(new { message = "Incorrect e-mail or password" });
            }

            // Gerar o token de acesso
            var token = new TokenGenerator().Generate(userDto);

            return Ok(new { token });
        }
    }
}