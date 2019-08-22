using Microsoft.Extensions.Configuration;
using System;

namespace SomeReallyComplexProject.Common.Extensions
{
    public static class IConfigurationExtensions
    {
        public static string GetServiceUID(this IConfiguration configuration)
        {
            return Environment.GetEnvironmentVariable("SERVICE_IDENTIFIER");
        }

        public static RabbitMqConfiguration GetRabbitMqConfiguration(this IConfiguration configuration)
        {
            return new RabbitMqConfiguration(configuration);
        }

        public static string GetSqlConnectionString(this IConfiguration configuration)
        {
            return $"data source={configuration["SQL_SERVER"]};" +
                   $"initial catalog={configuration["SQL_DATABASE"]};" +
                   $"user id={configuration["SQL_USERNAME"]};" +
                   $"password={configuration["SQL_PASSWORD"]};" +
                   $"MultipleActiveResultSets=true";
        }
    }
}
