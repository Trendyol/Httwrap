using System.Net.Http;
using System.Threading;

namespace Httwrap.Interception
{
    public interface IHttpInterceptor
    {
        void OnRequest(HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        void OnResponse(HttpClient httpClient, HttpRequestMessage request, HttpResponseMessage response, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}