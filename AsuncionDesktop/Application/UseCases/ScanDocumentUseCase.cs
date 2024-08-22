using AsuncionDesktop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Application.UseCases
{
    public class ScanDocumentUseCase
    {
        private readonly IScannerService _scannerService;
        private readonly IQRCodeService _qrCodeService;

        public ScanDocumentUseCase(IScannerService scannerService, IQRCodeService qrCodeService)
        {
            _scannerService = scannerService;
            _qrCodeService = qrCodeService;
        }

        public (Image, string) Execute()
        {
            var image = _scannerService.ScanDocument();
            var qrCode = _qrCodeService.DecodeQRCode(image);
            return (image, qrCode);
        }
    }
}
