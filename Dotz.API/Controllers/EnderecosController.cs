using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotz.API.Data;
using System.Security.Claims;

namespace Dotz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnderecosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Enderecos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnderecoDAO>>> GetEnderecos()
        {
            var id = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            return await _context.Enderecos.Where(x => x.UsuarioId == id).ToListAsync();
        }

        // GET: api/Enderecos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoDAO>> GetEndereco(int id)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var endereco = await _context.Enderecos.Where(x => x.UsuarioId == usuarioId && x.Id == id).FirstOrDefaultAsync();

            if (endereco == null)
            {
                return NotFound();
            }

            return endereco;
        }

        // PUT: api/Enderecos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEndereco(int id, EnderecoDAO endereco)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            if (id != endereco.Id)
            {
                return BadRequest();
            }
            endereco.UsuarioId = usuarioId;
            _context.Entry(endereco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoExists(id, usuarioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Enderecos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EnderecoDAO>> PostEndereco(EnderecoDAO endereco)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            endereco.UsuarioId = usuarioId;
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEndereco", new { id = endereco.Id }, endereco);
        }

        // DELETE: api/Enderecos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var endereco = await _context.Enderecos.FirstOrDefaultAsync(x => x.Id == id  && x.UsuarioId == usuarioId);
            if (endereco == null)
            {
                return NotFound();
            }

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnderecoExists(int id, int usuarioID)
        {
            return _context.Enderecos.Any(e => e.Id == id && e.UsuarioId == usuarioID);
        }
    }
}
