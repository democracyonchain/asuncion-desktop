using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class CardanoApiService: ICardanoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public CardanoApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrlCardano"];
        }

        public async Task<string> EnviarActaAsync(Acta acta)
        {
            try
            {
                var url = $"{_baseUrl}Acta/{acta.Codigo}/escaneo";

                var json = JsonConvert.SerializeObject(acta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al enviar acta: {response.StatusCode}");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<BlockchainResponse>(responseString);

                // ✅ Extraer solo el hash (último token en la cadena TransactionId)
                var tokens = result?.TransactionId?.Split(' ');
                var txHash = tokens?.LastOrDefault();

                if (string.IsNullOrWhiteSpace(txHash))
                    throw new Exception("Error: No se pudo extraer el TxHash de la respuesta.");

                return txHash;

            }
            catch
            (Exception ex)
            {
                return "Error:  al ejecutar transacción";
            }
            
        }
    }
}
