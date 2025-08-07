using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class ActaDTO
    {
        public int id { get; set; }
        public int blancos { get; set; }
        public int nulos { get; set; }
        public int sufragantes { get; set; }
        public int votosicr { get; set; }
        public string txicr { get; set; }
        public ImagenActaDTO imagenacta { get; set; }
        public List<ImagenSegmentoDTO> imagensegmento { get; set; }
        public List<VotoReconocidoDTO> votos { get; set; }
    }
}
