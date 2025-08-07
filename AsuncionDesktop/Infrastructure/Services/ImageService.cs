using System;
using System.Drawing;
using System.IO;
using AsuncionDesktop.Domain.Interfaces; // ✅ Importa la interfaz

namespace AsuncionDesktop.Infrastructure.Services
{
    public class ImageService : IImageService // ✅ Implementa la interfaz
    {
        public Bitmap LoadImage(string filePath) // ✅ Coincide con la interfaz
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("El archivo de imagen no existe.", filePath);

            return new Bitmap(filePath); // ✅ Devuelve un Bitmap
        }
    }
}
