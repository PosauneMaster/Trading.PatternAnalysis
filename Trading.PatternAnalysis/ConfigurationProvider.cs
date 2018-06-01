using Microsoft.Extensions.Configuration;
using System.IO;

namespace Trading.PatternAnalysis
{
    public static class ConfigurationProvider
    {
        private static readonly IConfigurationRoot Configuration;

        static ConfigurationProvider()
        {
             var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

        }

        public static string AlphaVantageKey => Configuration["alphavantagekey"];


    }
}
