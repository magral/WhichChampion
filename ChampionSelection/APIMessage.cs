using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Avalonia.Controls;
using Newtonsoft.Json;

namespace ChampionSelection
{
    //Data objects for serializing from Riot API
    //-------------------------------------------
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
    //--------------------------------------------

    public interface IAPIMessages
    {
        Task<string> GetChampionResult(string summonerName, List<IControl> _radioButtons);
    }
    public class APIMessage : IAPIMessages
    {
        //INSERT API KEY VVVV
        private string ApiKey = "";
        private readonly IHttpClientFactory _httpClientFactory;

        public APIMessage(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetChampionResult(string summonerName, List<IControl> _radioButtons)
        {
            var summonerId = await MakeSummonerRequestAsync(summonerName);
            var mastery = await MakeMasteryRequestAsync(summonerId);
            var championData = await MakeChampionRequestAsync();

            List<Champion> champions = new List<Champion>();
            foreach (var c in championData.Data)
            {
                champions.Add(new Champion(c.Value.Name, c.Value.Key, c.Value.Tags, c.Value.Info, c.Value.Stats, mastery));
            }
            foreach (RadioButton rbtn in _radioButtons)
            {
                if ((bool)rbtn.IsChecked)
                {
                    champions = ChampionUtil.FilterChampions(rbtn.Name.Split(' ')[0], rbtn.Name.Split(' ')[1], champions);
                }
            }
            if(champions.Count == 0)
            {
                return "Sorry, no champions match your query :( Please adjust your inputs and try again";
            }
            Random rand = new Random();
            string champName = champions[rand.Next(champions.Count)].Name;
            return champName;
        }

        public async Task<ChampionDto> MakeChampionRequestAsync()
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
        
        public async Task<List<Mastery>> MakeMasteryRequestAsync(int summonerid)
        {
            HttpClient riotApiClient = _httpClientFactory.CreateClient("https://na1.api.riotgames.com/lol/");
            riotApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await riotApiClient.GetAsync($"champion-mastery/v3/champion-masteries/by-summoner/{summonerid}?api_key={ApiKey}");

            var data = await response.Content.ReadAsStringAsync();
            var mastery = JsonConvert.DeserializeObject<List<Mastery>>(data);
            return mastery;
        }

        public async Task<int> MakeSummonerRequestAsync(string summonerName)
        {
            HttpClient riotApiClient = _httpClientFactory.CreateClient("https://na1.api.riotgames.com/lol/");
            riotApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = await riotApiClient.GetAsync($"summoner/v3/summoners/by-name/{summonerName}?api_key={ApiKey}");

            var data = await response.Content.ReadAsStringAsync();
            var summoner = JsonConvert.DeserializeObject<SummonerDto>(data);
            return summoner.Id;
        }
    }
}