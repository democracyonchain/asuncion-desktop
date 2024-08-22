using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces;
using System.Threading.Tasks;
using GraphQL;

namespace AsuncionDesktop.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly GraphQLHttpClient _client;

        public AuthService()
        {
            _client = new GraphQLHttpClient("http://localhost:3090/graphql", new NewtonsoftJsonSerializer());
        }

        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query Authlogin($password: String!, $username: String!) {
                    authlogin(password: $password, username: $username) {
                        provincia
                        token
                        username
                    }
                }",
                Variables = new { username, password }
            };

            var response = await _client.SendQueryAsync<AuthLoginResponse>(query);

            return response.Data.Authlogin;
        }

        // Eliminar 'private' de la declaración de clase.
        class AuthLoginResponse
        {
            public AuthResponse Authlogin { get; set; }
        }
    }
}
