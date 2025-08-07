using System.Drawing;
using System.Collections.Generic;
using ZXing;
using ZXing.Common;
using AsuncionDesktop.Domain.Interfaces;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public (string qrData, Point referencia) DecodeQRCodeWithReference(Bitmap image)
        {
            var reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                    TryHarder = true
                }
            };

            var result = reader.Decode(image);

            // Inicializar la referencia con (0,0) en caso de error
            Point referencia = new Point(0, 0);

            if (result != null)
            {
                // Usar el tercer punto de los ResultPoints como referencia (según tu implementación previa)
                referencia.X = (int)result.ResultPoints[2].X;
                referencia.Y = (int)result.ResultPoints[2].Y;
            }

            return (result?.Text ?? string.Empty, referencia);
        }
    }
}
