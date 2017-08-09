using System.Collections.Generic;
using System.Net.Http;
using Httwrap.Auth;
using Httwrap.Interception;

namespace Httwrap.Interface
{
    public interface IHttwrapConfiguration
    {
        string BasePath { get; }
        ISerializer Serializer { get; }
        Credentials Credentials { get; set; }
        HttpClient GetHttpClient();
        List<IHttpInterceptor> GetInterceptors();
    }
}