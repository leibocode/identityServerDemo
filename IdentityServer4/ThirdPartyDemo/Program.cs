using System;
using System.Net.Http;
using IdentityModel;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var diao = DiscoveryClient.GetAsync("http://localhost:5000").Result;

            //token
            var tokenClient = new TokenClient(diao.TokenEndpoint, "client", "secret");
            var tokenResponese = tokenClient.RequestClientCredentialsAsync("api").Result;
            if (tokenResponese.IsError)
            {
                Console.WriteLine(tokenResponese.Error);
            }

            Console.WriteLine(tokenResponese.Json);
            Console.WriteLine("\n\n");

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponese.AccessToken);

            var response = httpClient.GetAsync("http:localhost:5001/api/values").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsByteArrayAsync().Result);

            }

            Console.ReadLine();

        }
    }
}
