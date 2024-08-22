using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class Candidato
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public int PatidoId { get; set; }
        public int VotosIa { get; set; }
        public string Path { get; set; }
        public int Estado { get; set; }

    }
}
