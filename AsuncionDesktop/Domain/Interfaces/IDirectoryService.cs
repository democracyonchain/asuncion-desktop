namespace AsuncionDesktop.Domain.Interfaces
{
    public interface IDirectoryService
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        string GetBasePath(); // Agregado si lo usas en ScanForm
    }
}
