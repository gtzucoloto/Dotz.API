using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.API.Data
{
    public class ExtratoPontosDAO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataLancamento { get; set; }
        public int Saldo {  get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDAO Usuario { get; set; }
    }
}
