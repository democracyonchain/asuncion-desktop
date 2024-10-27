using AsuncionDesktop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public string DecodeQRCode(Image image)
        {
            var reader = new BarcodeReader();
            var result = reader.Decode((Bitmap)image);
            return result?.Text;
        }
    }
}
