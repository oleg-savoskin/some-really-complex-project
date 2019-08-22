using Microsoft.Extensions.Configuration;

namespace SomeReallyComplexProject.Common.Extensions
{
    public class RabbitMqConfiguration
    {
        private readonly IConfiguration configuration;

        private string ServiceUID => configuration.GetServiceUID();

        public string RabbitHost => configuration["RABBIT_HOST"];

        public string RabbitVHost => configuration["RABBIT_VHOST"];

        public ushort RabbitPort => ushort.Parse(configuration["RABBIT_PORT"]);

        public string RabbitQueue => $"integration-events-{ServiceUID}";

        public string RabbitUser => configuration["RABBIT_USERNAME"];

        public string RabbitPass => configuration["RABBIT_PASSWORD"];

        public RabbitMqConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}