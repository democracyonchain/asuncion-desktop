using System.Drawing;

namespace AsuncionDesktop.Domain.Interfaces
{
    public interface IImageService
    {
        Bitmap LoadImage(string filePath); // ✅ Debe coincidir con ImageService
    }
}
