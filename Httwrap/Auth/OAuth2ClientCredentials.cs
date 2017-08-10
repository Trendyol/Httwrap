using System.Collections.Generic;
using System.Net.Http;
using Httwrap.Interception;

namespace Httwrap.Auth
{
    public class OAuth2ClientCredentials : Credentials
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;
        private readonly List<string> _scopes;

        public OAuth2ClientCredentials(string clientId, string clientSecret, string tokenEndpoint, List<string> scopes)
        {
            Check.NotNullOrEmpty(clientId, "clientId");
            Check.NotNullOrEmpty(clientSecret, "clientSecret");
            Check.NotNullOrEmpty(tokenEndpoint, "tokenEndpoint");
            Check.NotNull(scopes, "scopes");
            Check.NotZeroLength(scopes, "scopes");
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;
            _scopes = scopes;
        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            var httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();
            return httpClient;
        }

        public override IHttpInterceptor GetInterceptor()
        {
            var interceptor = new OAuth2ClientCredentialsInterceptor(_clientId, _clientSecret, _tokenEndpoint, _scopes);
            return interceptor;
        }

        public override bool IsTlsCredentials()
        {
            throw new System.NotImplementedException();
        }
    }
}