using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class ResgateDAO
    {
        public int Id { get; set; }
        public int TotalPontos { get; set; }
        public int EnderecoId { get; set; }
        public EnderecoDAO Endereco{ get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public ICollection<ProdutoResgateDAO> ProdutosResgate { get; set; }
    }
}
