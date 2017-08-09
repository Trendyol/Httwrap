using System.Collections.Generic;
using System.Net.Http;
using Httwrap.Auth;
using Httwrap.Interception;
using Httwrap.Interface;
using Httwrap.Serialization;

namespace Httwrap
{
    public class HttwrapConfiguration : IHttwrapConfiguration
    {
        private readonly HttpMessageHandler _httpHandler;
        private ISerializer _serializer;

        public HttwrapConfiguration(string basePath)
            : this(basePath, null)
        {
        }

        public HttwrapConfiguration(string basePath, HttpMessageHandler httpHandler = null)
            : this(basePath, httpHandler, null)
        {
        }

        public HttwrapConfiguration(string basePath, HttpMessageHandler httpHandler = null, Credentials credentials = null)
        {
            Check.NotNullOrEmpty(basePath, "The basePath may not be null or empty.");
            BasePath = basePath;
            _httpHandler = httpHandler;
            Credentials = credentials;
        }

        public string BasePath { get; protected set; }

        public Credentials Credentials { get; set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new NewtonsoftJsonSerializer()); }
            set { _serializer = value; }
        }

        public HttpClient GetHttpClient()
        {
            Credentials = Credentials ?? new AnonymousCredentials();
            var client = Credentials.BuildHttpClient(_httpHandler);

            return client;
        }

        public List<IHttpInterceptor> GetInterceptors()
        {
            Credentials = Credentials ?? new AnonymousCredentials();
            var interceptors = new List<IHttpInterceptor>();
            var interceptor = Credentials.GetInterceptor();

            if (interceptor != null)
            {
                interceptors.Add(interceptor);
            }

            return interceptors;
        }
    }
}