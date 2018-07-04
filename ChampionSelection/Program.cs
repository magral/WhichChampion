using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Logging.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionSelection
{    
    class Program
    {
        public static QuestionList questionList;
        public static List<Champion> championList;
        static void Main(string[] args)
        {

            //Construct API call and get champion list
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            var container = serviceCollection.BuildServiceProvider();
            APIMessage apiMessage = new APIMessage((IHttpClientFactory) container.GetService(typeof(IHttpClientFactory)));
            int summonerId = apiMessage.GetSummonerInfo("bearlyleah");
            championList = apiMessage.MakeChampionRequest(summonerId);
            
            string questionDocument = File.ReadAllText("E:\\Documents\\RiderProjects\\ChampionSelector\\ChampionSelection\\Data.yaml");

            //Construct question list
            QuestionsDeserializer questionsDeserializer = new QuestionsDeserializer(questionDocument);
            questionList = questionsDeserializer.ParseDocument();

            BuildAvaloniaApp().Start<MainWindow>();;
            //Ask questions
            foreach (QuestionObj q in questionList.questions)
            {
                //MainWindowViewModel.instance.LoadUIForQuestion(q);
               // Console.WriteLine(q.question);
                //MainWindow.InstantiateAnswers(q.answers);
                //Get Answer
                //string input = (Console.ReadLine());
                //string answerValue = q.answers[Int32.Parse(input)].answer.value;
                //Filter list of available champions
                //champData = ChampionUtil.FilterChampions(answerValue, q.symbol, champData);
            }
            
        }
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}