using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChampionSelector
{
    public class ChampionDto 
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public Dictionary<string, Champ> Data { get; set; }
    }

    public class Champ
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, int> Info { get; set; } 
    }
    public class APIMessage
    {
        private const string URL = "https://na1.api.riotgames.com/lol/static-data/v3/champions?locale=en_US&champListData=info&champListData=tags&dataById=false&api_key=RGAPI-2259d8e4-28f5-4369-a462-97becff1a046";

        public ChampionDto MakeRequest()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpWebRequest request = HttpWebRequest.Create(URL) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse response = request.GetResponse();

            var resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var data = JsonConvert.DeserializeObject<ChampionDto>(resp);

            return data;
        }
    }
}