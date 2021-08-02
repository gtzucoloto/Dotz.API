using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class ProdutoResgateDAO
    {
        public int ProdutoId { get; set; }
        [JsonIgnore]
        public ProdutoDAO Produto { get; set; }
        public int ResgateId { get; set; }
        [JsonIgnore]
        public ResgateDAO Resgate { get; set; }
        public int Quantidade { get; set; }
        public int TotalPontos { get; set; }
    }
}
