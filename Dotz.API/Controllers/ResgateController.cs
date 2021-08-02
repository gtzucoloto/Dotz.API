using Dotz.API.Data;
using Dotz.API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dotz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResgateController : ControllerBase
    {
        private AppDbContext _context;

        public ResgateController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] bool? entregue = null)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);

            var enderecos = _context.Enderecos.Where(x => x.UsuarioId == usuarioId).ToList();
            
            if (enderecos?.Any() == false)
                return NotFound();

            var query = _context.Resgates.Include(k => k.Endereco).Include(x => x.ProdutosResgate).ThenInclude(y => y.Produto).ThenInclude(z => z.Categoria)
                .Where(x => enderecos.Select(e => e.Id).Contains(x.EnderecoId));

            if (entregue != null && entregue.Value)
                query = query.Where(x => x.DataEntrega != null && x.DataEntrega > DateTime.MinValue);
            else if(entregue != null && !entregue.Value)
                query = query.Where(x => x.DataEntrega == null || x.DataEntrega == DateTime.MinValue);

            var resgates = query.ToList();

            var lista = new List<ResgateDTO>();
            foreach (var resgate in resgates)
            {
                var resgateDTO = new ResgateDTO();
                resgateDTO.Id = resgate.Id;
                resgateDTO.DataSolicitacao = resgate.DataSolicitacao;
                resgateDTO.DataEntrega = resgate.DataEntrega != DateTime.MinValue ? resgate.DataEntrega : null;
                resgateDTO.Entregue = resgate.DataEntrega != null && resgate.DataEntrega > DateTime.MinValue;
                resgateDTO.Endereco = new EnderecoDTO
                {
                    Id = resgate.Endereco.Id,
                    Bairro = resgate.Endereco.Bairro,
                    Cidade = resgate.Endereco.Cidade,
                    Logradouro = resgate.Endereco.Logradouro,
                    Uf = resgate.Endereco.Uf,
                    Cep = resgate.Endereco.Cep,
                    Numero = resgate.Endereco.Numero,
                };

                resgateDTO.ProdutosResgateDTO = new List<ProdutoResgateDTO>();
                foreach (var produto in resgate.ProdutosResgate)
                {
                    var produtoResgate = new ProdutoResgateDTO
                    {
                        Quantidade = produto.Quantidade,
                        TotalPontos = produto.TotalPontos,
                        Produto = new ProdutoResgateProdutoDTO
                        {
                            Id = produto.Produto.Id,
                            Nome = produto.Produto.Nome,
                            Preco = produto.Produto.Preco,
                            Categoria = new ProdutoResgateProdutoCategoriaDTO
                            {
                                Id = produto.Produto.Categoria.Id,
                                Nome = produto.Produto.Categoria.Nome
                            }
                        }
                    };
                    resgateDTO.ProdutosResgateDTO.Add(produtoResgate);
                }
                lista.Add(resgateDTO);
            }

            return Ok(lista);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ResgateSolicitacaoDTO solicitacao)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var ultimoPonto = _context.ExtratoPontos.Where(x => x.UsuarioId == usuarioId).OrderByDescending(x => x.Id).FirstOrDefault();

            if (ultimoPonto == null)
                return Forbid("Você não possui saldo suficiente.");

            var endereco = _context.Enderecos.FirstOrDefault(x => x.Id == solicitacao.EnderecoId);

            var resgate = new ResgateDAO();
            resgate.Endereco = endereco;
            resgate.DataSolicitacao = DateTime.Now;
            resgate.ProdutosResgate = new List<ProdutoResgateDAO>();
            foreach (var item in solicitacao.Produtos)
            {
                var produto = _context.Produtos.Include(x => x.Categoria).FirstOrDefault(p => p.Id == item.ProdutoId);
                var produtoResgate = new ProdutoResgateDAO
                {
                    Produto = produto,
                    ProdutoId = produto.Id,
                    Quantidade = item.Quantidade,
                    Resgate = resgate,
                    ResgateId = resgate.Id,
                    TotalPontos = produto.Preco * item.Quantidade
                };
                resgate.ProdutosResgate.Add(produtoResgate);
            }
            resgate.TotalPontos = resgate.ProdutosResgate.Sum(x => x.TotalPontos);

            if (resgate.TotalPontos > ultimoPonto.Saldo)
                return Forbid("Você não possui saldo suficiente.");


            var ponto = new ExtratoPontosDAO();
            ponto.Quantidade = resgate.TotalPontos * -1;
            ponto.Saldo = ultimoPonto.Saldo - resgate.TotalPontos;
            ponto.Descricao = "Troca";
            ponto.DataLancamento = DateTime.Now;
            ponto.UsuarioId = usuarioId;

            _context.ExtratoPontos.Add(ponto);
            _context.Resgates.Add(resgate);
            _context.SaveChanges();

            return Ok($"Solicitado troca id {resgate.Id}");
        }

        [HttpPost("Receber")]
        public ActionResult Receber([FromBody] int resgateId)
        {
            var usuarioId = Convert.ToInt32(User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault()?.Value);
            var resgate = _context.Resgates.Where(x => x.Id == resgateId && x.Endereco.UsuarioId == usuarioId).Include(y => y.Endereco).FirstOrDefault();

            if (resgate == null)
                return NotFound("Troca não encontrada");

            resgate.DataEntrega = DateTime.Now;
            _context.SaveChanges();
            return Ok("Troca entregue");
        }
    }
}
