using System.Net.Http;
using Httwrap.Interception;

namespace Httwrap.Auth
{
    public abstract class Credentials
    {
        public abstract HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null);
        public abstract IHttpInterceptor GetInterceptor();
        public abstract bool IsTlsCredentials();
    }
}