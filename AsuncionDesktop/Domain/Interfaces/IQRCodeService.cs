using System.Drawing;

namespace AsuncionDesktop.Domain.Interfaces
{
    public interface IQRCodeService
    {
        (string qrData, Point referencia) DecodeQRCodeWithReference(Bitmap image);
    }
}
