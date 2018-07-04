using System;
using System.Net.Http;

namespace ChampionSelection
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string uri);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string uri)
        {
            var client = new HttpClient();
            SetupClientDefaults(client, uri);
            return client;
        }

        protected virtual void SetupClientDefaults(HttpClient client, string uri)
        {
            client.Timeout = TimeSpan.FromSeconds(30); 
            client.BaseAddress = new Uri(uri);
        }

    }
}