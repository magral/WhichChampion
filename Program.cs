using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionSelector
{    
    class Program 
    {
        
        static void Main(string[] args)
        {
            //Construct API call and get champion list
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            var container = serviceCollection.BuildServiceProvider();
            APIMessage apiMessage = new APIMessage((IHttpClientFactory) container.GetService(typeof(IHttpClientFactory)));
            int summonerId = apiMessage.GetSummonerInfo("bearlyleah");
            List<Champion> champData = apiMessage.MakeChampionRequest(summonerId);
            
            string questionDocument = File.ReadAllText(Directory.GetCurrentDirectory() + "/Data.yaml");

            //Construct question list
            Questions questionList = new Questions(questionDocument);

            //Ask questions
            foreach (QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.Question);
                foreach (AnswerObj answer in q.Answers)
                {
                    Console.WriteLine(answer.Text);
                }
                //Get Answer
                string input = (Console.ReadLine());
                string answerValue = q.Answers[Int32.Parse(input)].Value;
                //Filter list of available champions
                champData = ChampionUtil.FilterChampions(answerValue, q.Symbol, champData);
            }
            foreach (Champion c in champData)
            {
                Console.WriteLine(c.Name);
            }
        }
    }
}