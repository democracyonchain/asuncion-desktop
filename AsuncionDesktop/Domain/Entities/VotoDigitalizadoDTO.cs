using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class VotoDigitalizadoDTO
    {
        public int candidato_id { get; set; }
        public int votosdigitacion { get; set; }
        public string cifrado { get; set; }
    }
}
