using Dotz.API.Data;
using Dotz.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Dotz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult GetToken([FromBody] LoginModelDTO login)
        {
            var user = ValidateLogin(login);

            if (user == null)
                return Unauthorized("Try again");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, user.Nome),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Settings.ASCII_KEY)
                    , SecurityAlgorithms.HmacSha256Signature)
                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });

        }

        private UsuarioDAO ValidateLogin(LoginModelDTO login)
        {
            var emailValido = ValidacaoService.EmailEValido(login?.Email);
            if (login == null || !emailValido || string.IsNullOrWhiteSpace(login.Password))
                return null;

            return _context.Usuarios.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Password);
        }
    }
}
