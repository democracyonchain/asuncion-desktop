using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AsuncionDesktop.Domain.Interfaces;

namespace AsuncionDesktop.Application.UseCases
{
    public class CropImageUseCase
    {
        private readonly IDirectoryService _directoryService;

        public CropImageUseCase(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        public void Execute(string imagePath, string outputDirectory, Rectangle cropArea, string outputFileName)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("El archivo de imagen no existe.", imagePath);

            if (!_directoryService.DirectoryExists(outputDirectory))
                _directoryService.CreateDirectory(outputDirectory);

            try
            {
                using (Bitmap originalImage = new Bitmap(imagePath))
                {
                    using (Bitmap croppedImage = originalImage.Clone(cropArea, originalImage.PixelFormat))
                    {
                        string outputPath = Path.Combine(outputDirectory, outputFileName);
                        croppedImage.Save(outputPath, ImageFormat.Tiff);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cortar la imagen: {ex.Message}");
            }
        }
    }
}
