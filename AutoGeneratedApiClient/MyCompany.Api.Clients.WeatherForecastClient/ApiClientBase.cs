using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.Api.Clients.WeatherForecastClient
{
	public class ApiClientBase: IOpenApiAuth
    {
        public string BaseUrl { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string UserToken { get; set; }



        protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
        {
            if (UserToken == null && BaseUrl != null && Username != null)
            {
                UserToken = await GetUserTokenAsync();
            }

            HttpSecurityClientHandler handler = new HttpSecurityClientHandler()
            {
                UserToken = UserToken,
            };



            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;



            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.ExpectContinue = false;

            return client;
        }

        public void UseUserAuth(string userName, string password)
        {
            Username = userName;
            Password = password;
        }

        protected async Task<string> GetUserTokenAsync()
        {
            var builder = new UriBuilder(BaseUrl)
            {
                Path = ""
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = builder.Uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var content = JsonContent.Create(new { userName = Username, password = Password });
                var response = await client.PostAsync($"login", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var token = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    return token["token"].Value<string>();


                }
            }
            return "";
        }

        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpRequestMessage());
        }
    }

    public class HttpSecurityClientHandler : HttpClientHandler
    {
        public const string AUTH_TOKEN = "Authorization";
        public int TenantId { get; set; }
        public string UserToken { get; set; }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (!string.IsNullOrEmpty(UserToken))
            {
                request.Headers.Add(AUTH_TOKEN, "Bearer " + UserToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
