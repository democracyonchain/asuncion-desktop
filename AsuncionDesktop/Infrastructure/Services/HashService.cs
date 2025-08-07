using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class HashService
    {
        /// <summary>
        /// Genera un hash SHA-256 de un archivo
        /// </summary>
        public static string GenerateFileHash(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }
    }
}
