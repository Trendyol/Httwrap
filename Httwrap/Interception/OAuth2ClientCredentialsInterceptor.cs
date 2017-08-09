using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Httwrap.Auth;

namespace Httwrap.Interception
{
    public class OAuth2ClientCredentialsInterceptor : IHttpInterceptor
    {
        private const string GrantType = "client_credentials";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;
        private Token _token;

        public OAuth2ClientCredentialsInterceptor(string clientId, string clientSecret, string tokenEndpoint)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;
        }

        public void OnRequest(HttpRequestMessage request)
        {
            if (_token == null)
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", _clientId},
                    {"client_secret", _clientSecret},
                    {"grant_type", GrantType}
                });

                var httpClient = new HttpClient();
                var response = httpClient.PostAsync(_tokenEndpoint, content).Result;
                _token = new HttwrapResponse(response).ReadAs<Token>();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);
        }

        public void OnResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"client_id", _clientId},
                    {"client_secret", _clientSecret},
                    {"grant_type", GrantType}
                });

                var httpClient = new HttpClient();
                var tokenResponse = httpClient.PostAsync(_tokenEndpoint, content).Result;
                _token = new HttwrapResponse(tokenResponse).ReadAs<Token>();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

                response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).Result;
            }
        }
    }
}