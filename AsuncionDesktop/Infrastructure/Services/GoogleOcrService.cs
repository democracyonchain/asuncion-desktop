using System;
using System.Collections.Generic;
using System.IO;
using Google.Cloud.Vision.V1;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class GoogleOcrService
    {
        private readonly ImageAnnotatorClient _client;

        public GoogleOcrService(string credentialsPath)
        {
            // Configurar la variable de entorno con las credenciales
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

            // Crear cliente para Vision API
            _client = ImageAnnotatorClient.Create();
        }

        public string ProcesarSegmento(string imagePath)
        {
            try
            {
                var image = Image.FromFile(imagePath);
                var response = _client.DetectText(image);

                if (response.Count == 0)
                    return "No se detectó texto";

                // Unir todo el texto detectado
                return response[0].Description;
            }
            catch (Exception ex)
            {
                return $"Error en OCR: {ex.Message}";
            }
        }
    }
}
