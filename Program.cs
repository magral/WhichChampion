using System;
using System.Collections.Generic;
using System.IO;
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
            QuestionsDeserializer questionsDeserializer = new QuestionsDeserializer(questionDocument);
            QuestionList questionList = questionsDeserializer.ParseDocument();

            //Ask questions
            foreach (QuestionObj q in questionList.questions)
            {
                Console.WriteLine(q.question);
                foreach (AnswerList answer in q.answers)
                {
                    Console.WriteLine(answer.answer.text);
                }
                //Get Answer
                string input = (Console.ReadLine());
                string answerValue = q.answers[Int32.Parse(input)].answer.value;
                //Filter list of available champions
                champData = ChampionUtil.FilterChampions(answerValue, q.symbol, champData);
            }
            foreach (Champion c in champData)
            {
                Console.WriteLine(c.Name);
            }
        }
    }
}