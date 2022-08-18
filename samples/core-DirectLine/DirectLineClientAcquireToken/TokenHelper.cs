using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DirectLineSampleClient.Models;

namespace DirectLineSampleClient
{
    public class TokenHelper
    {
        private static HttpClient client = new HttpClient();
        // Your Token source service endpoint.
        private static string tokenRefreshEndpoint = "http://localhost:3000/token/refresh"; 

        public static async Task<string> GetTokenAsync()
        {
            HttpResponseMessage response = await client.GetAsync(tokenRefreshEndpoint);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();

                return tokenResponse.Token.accessToken;
            }

            throw new Exception("Request Failed!");

        }
    }
}
