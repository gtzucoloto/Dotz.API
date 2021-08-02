using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class CategoriaDAO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<ProdutoDAO> Produtos{ get; set; }
    }
}
