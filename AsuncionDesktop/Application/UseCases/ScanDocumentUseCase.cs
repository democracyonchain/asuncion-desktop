using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces; // ✅ Importamos las interfaces
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsuncionDesktop.Application.UseCases
{
    public class ScanDocumentUseCase
    {
        private readonly IImageService _imageService;
        private readonly IQRCodeService _qrCodeService;

        public ScanDocumentUseCase(IImageService imageService, IQRCodeService qrCodeService) // ✅ Usamos interfaces
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _qrCodeService = qrCodeService ?? throw new ArgumentNullException(nameof(qrCodeService));
        }
       
        public async Task<ScannedDocument> ExecuteAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("El archivo no puede estar vacío.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo especificado no existe: {filePath}");

            var image = _imageService.LoadImage(filePath);
            if (image == null)
                throw new Exception($"No se pudo cargar la imagen: {filePath}");

            var qrCode = "";

            return await Task.FromResult(new ScannedDocument
            {
                Image = image,
                QRCode = qrCode,
                FilePath = filePath,
                ScannedAt = DateTime.UtcNow
            });
        }
    }
}
