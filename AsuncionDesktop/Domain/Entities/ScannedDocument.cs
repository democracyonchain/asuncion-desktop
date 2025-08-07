using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class ScannedDocument  // ✅ Asegurar que sea pública
    {
        public Image Image { get; set; }    // ✅ Debe existir esta propiedad
        public string QRCode { get; set; }  // ✅ Debe existir esta propiedad
        public string FilePath { get; set; }
        public DateTime ScannedAt { get; set; }
    }
}
