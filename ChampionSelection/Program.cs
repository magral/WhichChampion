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

            //Add Client service to DI container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            var container = serviceCollection.BuildServiceProvider();

            //Build API message class with client service
            APIMessage apiMessage = new APIMessage((IHttpClientFactory) container.GetService(typeof(IHttpClientFactory)));
            
            //Make API calls
            int summonerId = apiMessage.GetSummonerInfo("bearlyleah");
            championList = apiMessage.MakeChampionRequest(summonerId);
            
            //Get Question Doc
            //TODO: Support mac and windows files
            string questionDocument = File.ReadAllText("E:\\Documents\\RiderProjects\\ChampionSelector\\ChampionSelection\\Data.yaml");

            //Construct question list
            QuestionsDeserializer questionsDeserializer = new QuestionsDeserializer(questionDocument);
            questionList = questionsDeserializer.ParseDocument();

            BuildAvaloniaApp().Start<MainWindow>();;
        }
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}