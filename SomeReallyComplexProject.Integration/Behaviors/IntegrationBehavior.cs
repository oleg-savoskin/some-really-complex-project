using MediatR;
using SomeReallyComplexProject.Core.Integration;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Integration.Behaviors
{
    public class IntegrationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IIntegrationService integrationService;

        public IntegrationBehavior(IIntegrationService integrationService)
        {
            this.integrationService = integrationService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            await integrationService.PublishPendingEvents();
            return response;
        }
    }
}
