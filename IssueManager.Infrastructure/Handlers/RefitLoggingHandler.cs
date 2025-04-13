using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Infrastructure.Handlers
{
    public class RefitLoggingHandler(ILogger<RefitLoggingHandler> logger) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Refit Request: {Method} {Url}", request.Method, request.RequestUri);

            if (request.Content != null)
            {
                var body = await request.Content.ReadAsStringAsync();
                logger.LogDebug("Request Body: {Body}", body);
            }

            var response = await base.SendAsync(request, cancellationToken);

            logger.LogInformation("Refit Response: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                logger.LogError("Response Error: {Error}", error);
            }

            return response;
        }
    }
}