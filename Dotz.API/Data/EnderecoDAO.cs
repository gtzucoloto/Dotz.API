using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class EnderecoDAO
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public int? Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public ICollection<ResgateDAO> Resgates { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDAO Usuario { get; set; }
    }
}
