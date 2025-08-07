namespace AsuncionDesktop.Domain.Interfaces
{
    public interface IFileService
    {
        void MoveFile(string sourcePath, string destinationPath);
        bool FileExists(string path);
        void DeleteFile(string path);
        void CopyFile(string sourcePath, string destinationPath);
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
    }
}
