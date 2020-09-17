using Microsoft.Extensions.Configuration;
using System;

namespace ISUCorp.Infra.Configurations
{
    public class DbConfiguration
    {
        private static string DataConnectionKey => "Data";

        public static IConfigurationRoot Configuration => new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        public static string DataConnectionString => Configuration.GetConnectionString(DataConnectionKey);
    }
}
