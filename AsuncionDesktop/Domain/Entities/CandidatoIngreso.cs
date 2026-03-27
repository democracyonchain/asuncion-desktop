using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    internal class CandidatoIngreso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
        public int Votos { get; set; }
        public string ipfs {  get; set; }
        public string Hash { get; set; }
    }
}
