using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Httwrap.Interception
{
    public class BasicAuthInterceptor : IHttpInterceptor
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthInterceptor(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public void OnRequest(HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
        }

        public void OnResponse(HttpClient httpClient, HttpRequestMessage request, HttpResponseMessage response,
            HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
        }
    }
}