using System.Net.Http;
using Httwrap.Interception;

namespace Httwrap.Auth
{
    public class BasicAuthCredentials : Credentials
    {
        private readonly bool _isTls;
        private readonly string _password;
        private readonly string _username;

        public BasicAuthCredentials(string username, string password, bool isTls = false)
        {
            Check.NotNullOrEmpty(username, "username");
            Check.NotNullOrEmpty(password, "password");

            _username = username;
            _password = password;
            _isTls = isTls;
        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            var httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();
            return httpClient;
        }

        public override IHttpInterceptor GetInterceptor()
        {
            var interceptor = new BasicAuthInterceptor(_username, _password);
            return interceptor;
        }

        public override bool IsTlsCredentials()
        {
            return _isTls;
        }
    }
}