using System.Net.Http;
using Httwrap.Interception;

namespace Httwrap.Auth
{
    public class OAuth2ClientCredentials : Credentials
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;

        public OAuth2ClientCredentials(string clientId, string clientSecret, string tokenEndpoint)
        {
            Check.NotNullOrEmpty(clientId, "clientId");
            Check.NotNullOrEmpty(clientSecret, "clientSecret");
            Check.NotNullOrEmpty(tokenEndpoint, "tokenEndpoint");
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;
        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            var httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();
            return httpClient;
        }

        public override IHttpInterceptor GetInterceptor()
        {
            var interceptor = new OAuth2ClientCredentialsInterceptor(_clientId, _clientSecret, _tokenEndpoint);
            return interceptor;
        }

        public override bool IsTlsCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}