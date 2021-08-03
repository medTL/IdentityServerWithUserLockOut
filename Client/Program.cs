using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var  discoveryDocument = await httpClient.GetDiscoveryDocumentAsync("http://127.0.0.1:5000");
            if (discoveryDocument.IsError)
            {
                Console.WriteLine(discoveryDocument.Error);
                return;
            }
            var passwordTokenRequest = new PasswordTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "resourceOwnerClient",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                GrantType = OidcConstants.GrantTypes.Password,
                Scope = "api1",
                UserName = "bob",
                Password = "Pass123"
            };
            
            var tokenResponse =   await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.ErrorDescription);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
        }
    }
}