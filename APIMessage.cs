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

    public class SummonerDto
    {
        public int Id { get; set; }
    }

    public class MasteryDto
    {
        public int ChampionLevel { get; set; }
    }
    
    public class APIMessage
    {
        public static List<Champion> MakeRequest()
        {
            const string URL = "https://na1.api.riotgames.com/lol/static-data/v3/champions?locale=en_US&champListData=info&champListData=tags&dataById=false&api_key=RGAPI-a52b1955-4df2-43e9-9d35-08df57d52144";

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
            
            List<Champion> champions = new List<Champion>();
            foreach (var c in data.Data)
            {
                champions.Add(new Champion(c.Value.Name, c.Value.Tags, c.Value.Info));
            }

            return champions;
        }

        public static bool GetChampionNewness(int summonerid, int championid)
        {
            string URL = String.Format("https://na1.api.riotgames.com/lol/champion-mastery/v3/champion-masteries/by-summoner/{0}/by-champion/{1}&api_key=RGAPI-a52b1955-4df2-43e9-9d35-08df57d52144", summonerid, championid);

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
            var data = JsonConvert.DeserializeObject<MasteryDto>(resp);

            bool isNew = data.ChampionLevel > 4;
            return isNew;
        }

        public static int GetSummonerInfo(string summonerName)
        {
            string URL = String.Format("https://na1.api.riotgames.com/lol/summoner/v3/summoners/by-name/{0}&api_key=RGAPI-a52b1955-4df2-43e9-9d35-08df57d52144", summonerName);

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
            var data = JsonConvert.DeserializeObject<SummonerDto>(resp);

            return data.id;
        }
    }
}