using Dotz.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Dotz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PontosController : ControllerBase
    {
        private AppDbContext _context;

        public PontosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Saldo")]
        public ActionResult Saldo()
        {
            var id = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var saldo = _context.ExtratoPontos.Where(x => x.UsuarioId == id).OrderByDescending(x => x.Id).First().Saldo;

            return Ok(new { saldo });
        }

        [HttpGet("Extrato")]
        public ActionResult Extrato()
        {
            var id = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            return Ok(_context.ExtratoPontos.Where(x => x.UsuarioId == id).ToList());
        }

        [HttpPost]
        public ActionResult Post([FromBody] int pontos)
        {
            var id = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var ultimo = _context.ExtratoPontos.Where(x => x.UsuarioId == id).OrderByDescending(x => x.Id).FirstOrDefault() ?? new ExtratoPontosDAO();
            var compra = new ExtratoPontosDAO
            {
                UsuarioId = id,
                DataLancamento = DateTime.Now,
                Descricao = "Compras",
                Quantidade = pontos,
                Saldo = ultimo.Saldo + pontos
            };

            _context.ExtratoPontos.Add(compra);
            _context.SaveChanges();

            return Ok(new { Saldo = compra.Saldo });
        }
    }
}
