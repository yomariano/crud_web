using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewWeb.Handlers
{
    public class RequireMagicHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!request.Headers.TryGetValues("x-magicheader", out var magicHeaders) 
                || !int.TryParse(magicHeaders.First(), out int magicValue) 
                || magicValue < 100 
                || magicValue > 200)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Sorry, you need to supply the x-magicheader header with a valid value")
                };
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                tcs.SetResult(response);
                return tcs.Task;
            };
            
            return base.SendAsync(request, cancellationToken);
        }
    }
}