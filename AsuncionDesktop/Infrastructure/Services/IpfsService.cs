using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System;
using AsuncionDesktop.Domain.Entities;
using System.Configuration;


namespace AsuncionDesktop.Infrastructure.Services
{
    public class IpfsService
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public IpfsService()
        {
            _url = ConfigurationManager.AppSettings["IpfsUploadUrl"];
            _client = new HttpClient { BaseAddress = new Uri(_url) };
        }

        public async Task<(string Hash, string Name)> UploadFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) return default;

            var content = new MultipartFormDataContent();
            var fileBytes = File.ReadAllBytes(filePath); // compatible C# 7.3
            var byteArray = new ByteArrayContent(fileBytes);
            byteArray.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            content.Add(byteArray, "file", Path.GetFileName(filePath));
            var response = await _client.PostAsync("", content);

            if (!response.IsSuccessStatusCode) return default;

            var json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            return (obj.ipfs_hash.ToString(), obj.name.ToString());
        }
    }
}
