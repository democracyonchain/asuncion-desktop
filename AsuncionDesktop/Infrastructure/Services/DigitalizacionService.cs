using AsuncionDesktop.Domain.Entities;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class DigitalizacionService
    {
        private readonly GraphQLHttpClient _client;
        public DigitalizacionService(string token)
        {
            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] + "graphql";
            _client = new GraphQLHttpClient(baseUrl, new NewtonsoftJsonSerializer());
            _client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        public async Task<string> EnviarActaDigitalizadaAsync(ActaDTO dto)
        {
            var mutation = new GraphQLRequest
            {
                Query = @"
                mutation ($dataInput: ActaUpdateInput!) {
                    digtActaUpdate(dataInput: $dataInput) {
                        message
                        status                        
                    }
                }",

                Variables = new { dataInput = dto }
            };

            var response = await _client.SendMutationAsync<dynamic>(mutation);

            // ✅ Verifica si hubo errores estructurales en GraphQL
            if (response.Errors != null && response.Errors.Length > 0)
            {
                var errores = string.Join("\n", response.Errors.Select(e => e.Message));
                return $"❌ Error en GraphQL:\n{errores}";
            }

            // ✅ Verifica que la data devuelta tenga contenido
            if (response.Data == null || response.Data.digtActaUpdate == null)
            {
                return "❌ El servidor no devolvió datos válidos para digtActaUpdate.";
            }

            // ✅ Extrae respuesta del backend
            var result = response.Data.digtActaUpdate;

            var jResult = result as JObject;
            if (jResult == null)
                return "❌ No se pudo interpretar la respuesta del servidor.";

            bool status = jResult["status"]?.Value<bool>() ?? false;
            string message = jResult["message"]?.Value<string>() ?? "Sin mensaje";
          
            return status
                ? $"✅ Acta enviada correctamente: {result.error ?? "Sin errores"}"
                : $"❌ Falló el envío del acta: {result.error}";
        }



        private class DigtActaUpdateResponse
        {
            public DigtActaUpdateResult digtActaUpdate { get; set; }
        }

        private class DigtActaUpdateResult
        {
            public bool success { get; set; }
            public string message { get; set; }
        }

        public async Task<string> EnviarVotosDigitalizadosAsync(List<VotoDigitalizadoDTO> votos)
        {
            var mutation = new GraphQLRequest
            {
                Query = @"
        mutation ($votos: [VotosDigitacionUpdateBasicInput!]!) {
            digtVotosUpdate(votos: $votos) {
                ok
                error
            }
        }",
                Variables = new { votos = votos }
            };

            var response = await _client.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null && response.Errors.Length > 0)
            {
                var errores = string.Join("\n", response.Errors.Select(e => e.Message));
                return $"❌ Error en votos:\n{errores}";
            }

            var data = response.Data.digtVotosUpdate;
            return data.ok ? $"✅ Votos enviados: {data.error}" : $"❌ Falló envío votos: {data.error}";
        }

    }
}
