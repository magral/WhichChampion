using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Autofac;
using Avalonia;
using Avalonia.Logging.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace ChampionSelection
{
    class Program
    {
        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            //Add services to IoC container to be injected
            var builder = new ContainerBuilder();
            builder.RegisterType<HttpClientFactory>().As<IHttpClientFactory>();
            builder.RegisterType<QuestionsDeserializer>().As<IQuestionDeserializer>();
            builder.RegisterType<APIMessage>().As<IAPIMessages>();
            Container = builder.Build();

            BuildAvaloniaApp().Start<MainWindow>(); ;
        }
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}