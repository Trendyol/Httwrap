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

        private static readonly object SyncLock = new object();

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;
        private readonly string _scope;
        private Token _token;
        private bool _isTokenExpired = true;

        public OAuth2ClientCredentialsInterceptor(string clientId, string clientSecret, string tokenEndpoint,
            List<string> scopes = null)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;

            if (scopes != null && scopes.Count > 0)
            {
                _scope = String.Join(" ", scopes);
            }
        }

        public void OnRequest(HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            RefreshCurrentToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);
        }

        public void OnResponse(HttpClient httpClient, HttpRequestMessage request, HttpResponseMessage response,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _isTokenExpired = true;
                RefreshCurrentToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

                response = httpClient.SendAsync(request, completionOption, cancellationToken).Result;
            }
        }

        private void RefreshCurrentToken()
        {
            if (_isTokenExpired)
            {
                lock (SyncLock)
                {
                    if (_isTokenExpired)
                    {
                        var content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            {"client_id", _clientId},
                            {"client_secret", _clientSecret},
                            {"grant_type", GrantType},
                            {"scope", _scope}
                        });

                        string additionalInfo = String.Empty;

                        try
                        {
                            var httpClient = new HttpClient();
                            var tokenResponse = httpClient.PostAsync(_tokenEndpoint, content).Result;

                            if (tokenResponse.IsSuccessStatusCode)
                            {
                                _token = new HttwrapResponse(tokenResponse).ReadAs<Token>();
                                _isTokenExpired = false;
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            additionalInfo = ex.Message;
                        }
                        
                        throw new HttwrapHttpException(HttpStatusCode.Unauthorized, $"Httpwrap cannot get a new token for client:{_clientId}. {additionalInfo}");
                    }
                }
            }
        }
    }
}