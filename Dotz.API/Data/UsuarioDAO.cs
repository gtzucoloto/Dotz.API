using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class UsuarioDAO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<ExtratoPontosDAO> Pontos { get; internal set; }
        public List<EnderecoDAO> Enderecos { get; internal set; }
    }
}
