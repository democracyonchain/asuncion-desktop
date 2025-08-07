using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class ImagenSegmentoDTO
    {
        public int candidato_id { get; set; }
        public string hash { get; set; }
        public byte[] imagen { get; set; }
        public string nombre { get; set; }
        public string pathipfs { get; set; }
    }
}
