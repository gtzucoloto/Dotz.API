using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class ProdutoDAO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Preco {  get; set; }
        public int CategoriaId { get; set; }
        public CategoriaDAO Categoria { get; set; }
        public ICollection<ProdutoResgateDAO> ProdutosResgate { get; set; }
    }
}
