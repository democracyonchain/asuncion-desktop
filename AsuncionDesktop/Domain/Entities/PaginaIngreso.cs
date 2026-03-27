using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    internal class PaginaIngreso
    {
        public int ActaId { get; set; }
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public List<CandidatoIngreso> candidatos { get; set; }
        public int Estado { get; set; }
    }
}
