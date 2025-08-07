using System.IO;
using AsuncionDesktop.Domain.Interfaces;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public void MoveFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destinationPath);
            }
        }

        public bool FileExists(string path) => File.Exists(path);

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
        }

        public string ReadAllText(string path) => File.Exists(path) ? File.ReadAllText(path) : string.Empty;

        public void WriteAllText(string path, string content) => File.WriteAllText(path, content);
    }
}
