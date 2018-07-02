using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChampionSelector
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory, IConfiguration config) { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHttpClientFactory, HttpClientFactory>();
        }
    }
}