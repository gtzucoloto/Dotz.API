using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.API.DTO
{
    public class ResgateSolicitacaoDTO
    {
        public int EnderecoId { get; set; }
        public ICollection<ProdutoResgateSolicitacaoDTO> Produtos { get; set; }
    }

    public class ProdutoResgateSolicitacaoDTO
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
