using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotz.API.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dotz.API.DTO;

namespace Dotz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDAO>>> GetProductos()
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var ponto = _context.ExtratoPontos.Where(x => x.UsuarioId == usuarioId).OrderByDescending(y => y.Id).FirstOrDefault();

            if (ponto == null)
                return Ok("Você não possui ponto sufucientes para resgatar nenhum produto");

            var produtos = await _context.Produtos.Where(p => p.Preco < ponto.Saldo).Include(x => x.Categoria).ToListAsync();
            var lista = new List<ProdutoDTO>();

            foreach (var produto in produtos)
            {
                lista.Add(new ProdutoDTO
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Categoria = new CategoriaDTO
                    {
                        Id = produto.Categoria.Id,
                        Nome = produto.Categoria.Nome
                    }
                });
            }

            return Ok(lista);
        }
    }
}
