using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

        public void OnRequest(HttpRequestMessage request)
        {
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authString);
        }

        public void OnResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
        }
    }
}