using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Interfaces
{
    public interface IScannerService
    {
        Image ScanDocument();
    }

    // IQRCodeService.cs
    public interface IQRCodeService
    {
        string DecodeQRCode(Image image);
    }
}
