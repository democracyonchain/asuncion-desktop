using System;
using System.Configuration;
using AsuncionDesktop.Domain.Interfaces;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class DirectoryService : IDirectoryService
    {
        public bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public string GetBasePath()
        {
            string basePath = ConfigurationManager.AppSettings["BasePath"];
            if (string.IsNullOrEmpty(basePath))
            {
                throw new InvalidOperationException("BasePath no está configurado en App.config.");
            }
            return basePath;
        }
    }
}
