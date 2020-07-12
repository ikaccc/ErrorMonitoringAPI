using System.Net;
using System.Net.Http;

namespace ExceptionHandler.API.Common
{
    public static class HttpClientUtil
    {
        public static HttpClient CreateHttpClient(string proxyAddress = null, string proxyUsername = null, string proxyPassword = null)
        {
            if (string.IsNullOrWhiteSpace(proxyAddress))
                return new HttpClient();
            ICredentials Credentials = (ICredentials)null;
            if (!string.IsNullOrWhiteSpace(proxyUsername) && !string.IsNullOrWhiteSpace(proxyPassword))
                Credentials = (ICredentials)new NetworkCredential(proxyUsername, proxyPassword);
            WebProxy webProxy = Credentials != null ? new WebProxy(proxyAddress, true, (string[])null, Credentials) : new WebProxy(proxyAddress);
            return new HttpClient((HttpMessageHandler)new HttpClientHandler()
            {
                Proxy = (IWebProxy)webProxy,
                PreAuthenticate = true,
                UseDefaultCredentials = false
            });
        }
    }
}
