using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class ImagenActaDTO
    {
        public string hash { get; set; }
        public byte[] imagen { get; set; } // base64 o IPFS URL
        public string nombre { get; set; }
        public string pagina { get; set; }
        public string pathipfs { get; set; }        // Hash SHA256 o similar de la imagen
    }
}
