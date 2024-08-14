using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Behaviors
{
    public sealed class RequestResponseLogginBehavior<TRequest, TResponse>(ILogger<RequestResponseLogginBehavior<TRequest, TResponse>> _logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, 
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            Guid trackId = Guid.NewGuid();

            // request logging
            var requestObj = JsonSerializer.Serialize(request);
            _logger.LogInformation($"Handle the request for id: {trackId} - {requestObj}");

            // response logging
            var response = await next();
            var responseObj = JsonSerializer.Serialize(response);
            _logger.LogInformation($"Handle the response for id: {trackId} - {responseObj}");

            return response;
        }
    }
}
