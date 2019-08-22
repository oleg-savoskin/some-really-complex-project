using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Events;
using SomeReallyComplexProject.Integration.Extensions;
using SomeReallyComplexProject.ServiceTwo.Application.Extensions;
using SomeReallyComplexProject.ServiceTwo.Application.IntegrationEventHandlers;

namespace SomeReallyComplexProject.ServiceTwo
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationRegistrations();
            services.AddIntegrationEvents(configuration, ConfigureSubscriptions);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIntegrationEvents();
            app.UseMvc();
        }

        private void ConfigureSubscriptions(ISubscriptionsManager manager)
        {
            manager.AddSubscription<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();
        }
    }
}
