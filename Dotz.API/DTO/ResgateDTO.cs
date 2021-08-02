using Dotz.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.API.DTO
{
    public class ResgateDTO
    {
        public int Id { get; set; }
        public int TotalPontos { get; set; }
        public EnderecoDTO Endereco { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public ICollection<ProdutoResgateDTO> ProdutosResgateDTO { get; set; }
        public bool Entregue { get; set; }
    }

    public class ProdutoResgateDTO
    {
        public ProdutoResgateProdutoDTO Produto { get; set; }
        public int Quantidade { get; set; }
        public int TotalPontos { get; set; }
    }

    public class ProdutoResgateProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Preco { get; set; }
        public ProdutoResgateProdutoCategoriaDTO Categoria { get; set; }
    }

    public class ProdutoResgateProdutoCategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
