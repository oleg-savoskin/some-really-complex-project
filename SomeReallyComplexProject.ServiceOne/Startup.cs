using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeReallyComplexProject.Integration.Extensions;
using SomeReallyComplexProject.ServiceOne.Application.Extensions;
using SomeReallyComplexProject.ServiceOne.Domain.Extensions;

namespace SomeReallyComplexProject.ServiceOne
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
            services.AddIntegrationEvents(configuration);
            services.AddServiceOneDomainServices(configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIntegrationEvents();
            app.UseServiceOneDomainServices();
            app.UseMvc();
        }
    }
}
