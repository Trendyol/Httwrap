using System.Net;
using System.Net.Http;
using System.Threading;
using Httwrap.Interception;

namespace Httwrap.Tests
{
    public class DummyInterceptor : IHttpInterceptor
    {
        public void OnRequest(HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
        }

        public void OnResponse(HttpClient httpClient, HttpRequestMessage request, HttpResponseMessage response,
            HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            response.StatusCode = HttpStatusCode.Accepted;
        }
    }
}