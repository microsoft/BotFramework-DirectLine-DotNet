using System;
#if FEATURE_SYSTEM_CONFIGURATION
using System.Configuration;
#endif
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;

namespace Microsoft.Bot.Connector.DirectLine
{
    /// <summary>
    /// Use credentials from AppSetting "DirectLineSecret"
    /// </summary>
    public class DirectLineClientCredentials : ServiceClientCredentials
    {
#if FEATURE_SYSTEM_CONFIGURATION
        private static Lazy<string> _secret = new Lazy<string>(() => ConfigurationManager.AppSettings["DirectLineSecret"]);
        private static Lazy<string> _endpoint = new Lazy<string>(() => ConfigurationManager.AppSettings["DirectLineEndpoint"]);
#else
        // .NET Core does not support System.Configuration API's.
        private static Lazy<string> _secret = new Lazy<string>(() => null);
        private static Lazy<string> _endpoint = new Lazy<string>(() => null);
#endif

        public string Secret { get; private set; }

        public string Authorization { get; private set; }

        public string Endpoint { get; protected set; }

        /// <summary>
        /// Create a new instance of the DirectLineClientCredentials class
        /// </summary>
        /// <param name="secret">default will come from Settings["DirectLineSecret"]</param>
        public DirectLineClientCredentials(string secret = null, string endpoint = null)
        {
            this.Secret = secret ?? _secret.Value;
            this.Authorization = this.Secret;
            this.Endpoint = endpoint ?? _endpoint.Value ?? "https://directline.botframework.com/";
        }

        /// <summary>
        /// Apply the credentials to the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param><param name="cancellationToken">Cancellation token.</param>
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.Authorization);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}