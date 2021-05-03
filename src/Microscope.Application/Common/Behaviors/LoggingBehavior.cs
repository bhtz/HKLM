using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Microscope.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var name = typeof(TRequest).Name;

            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", name, request);

            var response = await next();
            
            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}", name, response);

            return response;
        }
    }
}
