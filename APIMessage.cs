using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChampionSelector
{
    public class ChampionDto
    {
        public Dictionary<string, Champ> Data { get; set; }
    }

    public class Champ
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public Dictionary<string, float> Stats { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<string, int> Info { get; set; }    
    }

    public class SummonerDto
    {
        public int Id { get; set; }
    }

    public class Mastery
    {
        public int ChampionLevel { get; set; }
        public int ChampionId { get; set; }
    }

    public class APIMessage
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public APIMessage(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }
        
        static string ApiKey = "RGAPI-2596d208-2400-4ddc-bf4e-29484a952d34";

        public List<Champion> MakeChampionRequest(int summonerid)
        {
            var data = MakeChampionRequestAsync();
            var champData = data.Result;
            var masteries = GetChampionNewness(summonerid);
            List<Champion> champions = new List<Champion>();
            foreach (var c in champData.Data)
            {
                champions.Add(new Champion(c.Value.Name, c.Value.Key, c.Value.Tags, c.Value.Info, c.Value.Stats, masteries));
            }

            return champions;
        }

        async Task<ChampionDto> MakeChampionRequestAsync()
        {
            HttpClient ddragonClient = _httpClientFactory.CreateClient("http://ddragon.leagueoflegends.com/");

            // Add an Accept header for JSON format.
            ddragonClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await ddragonClient.GetAsync("cdn/8.12.1/data/en_US/champion.json");

            var data = await response.Content.ReadAsStringAsync();
            var champs = JsonConvert.DeserializeObject<ChampionDto>(data);
            return champs;
        }


        public List<Mastery> GetChampionNewness(int summonerid)
        {
            var resp = MakeMasteryRequestAsync(summonerid);
            var data = resp.Result;
            return data;
        }
        
        private async Task<List<Mastery>> MakeMasteryRequestAsync(int summonerid)
        {
            // List data response.
            HttpClient riotApiClient = _httpClientFactory.CreateClient("https://na1.api.riotgames.com/lol/");
            // Add an Accept header for JSON format.
            riotApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await riotApiClient.GetAsync($"champion-mastery/v3/champion-masteries/by-summoner/{summonerid}?api_key={ApiKey}");

            var data = await response.Content.ReadAsStringAsync();
            var mastery = JsonConvert.DeserializeObject<List<Mastery>>(data);
            return mastery;
        }

        public int GetSummonerInfo(string summonerName)
        {
            var data = MakeSummonerRequestAsync(summonerName).Result;

            return data.Id;
        }
        
        private async Task<SummonerDto> MakeSummonerRequestAsync(string summonerName)
        {
            HttpClient riotApiClient = _httpClientFactory.CreateClient("https://na1.api.riotgames.com/lol/");
            
            // Add an Accept header for JSON format.
            riotApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            // List data response.
            HttpResponseMessage response = await riotApiClient.GetAsync($"summoner/v3/summoners/by-name/{summonerName}?api_key={ApiKey}");

            var data = await response.Content.ReadAsStringAsync();
            var summoner = JsonConvert.DeserializeObject<SummonerDto>(data);
            return summoner;
        }
    }
}