using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mailgun.Core.Messages;
using Mailgun.Core.Service;
using Mailgun.Exceptions;

namespace Mailgun.Service
{
    /// <summary>
    /// The core mailgun service
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public string ApiKey { get;  }
        public string BaseAddress { get;  }
        public bool UseSSl { get; }

        /// <summary>
        /// Create an instance of the mailgun service with the specified apikey
        /// </summary>
        /// <param name="apikey">Your mailgun API Key</param>
        /// <param name="useSsl">Should the library use SSL for all requests?</param>
        /// <param name="baseAddress">Base address of the mailgun api, excluding the scheme, e.g. api.mailgun.net/v3</param>
        [Obsolete("Consider providing IHttpClientFactory parameter")]
        public MessageService(string apikey, bool useSsl = true, string baseAddress = "api.mailgun.net/v3")
        {
            ApiKey = apikey;
            BaseAddress = baseAddress;
            UseSSl = useSsl;
            this.httpClientFactory = null;
        }

        /// <summary>
        /// Create an instance of the mailgun service with the specified apikey
        /// </summary>
        /// <param name="apikey">Your mailgun API Key</param>
        /// <param name="httpClientFactory">HTTP client factory. See https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests</param>
        /// <param name="baseAddress">Base address of the mailgun api, excluding the scheme, e.g. api.mailgun.net/v3</param>
        /// <remarks>
        ///     SSL parameter was removed, because it makes no sense to disable SSL.
        /// </remarks>
        public MessageService(string apikey, IHttpClientFactory httpClientFactory,  string baseAddress = "api.mailgun.net/v3")
        {
            ApiKey = apikey;
            this.httpClientFactory = httpClientFactory;
            BaseAddress = baseAddress;
            UseSSl = true;
        }

        /// <summary>
        /// Send a message using the Mailgun API service.
        /// </summary>
        /// <param name="workingDomain">The mailgun working domain to use</param>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendMessageAsync(string workingDomain, IMessage message)
        {
            //check for parameters
            ThrowIf.IsArgumentNull(() => workingDomain);
            ThrowIf.IsArgumentNull(() => message);


            //build request
            using (var client = httpClientFactory?.CreateClient("Mailgun.MessageService") ?? new HttpClient())
            {
                var buildUri = new UriBuilder
                {
                    Host = BaseAddress,
                    Scheme = UseSSl ? "https" : "http",
                    Path = string.Format("{0}/messages", workingDomain)
                };


                //add authentication
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + ApiKey)));


                //set the client uri
                return await client.PostAsync(buildUri.ToString(), message.AsFormContent())
                    .ConfigureAwait(false);
                //TODO may be read answer and throw exceptions on errors.
                //or introduce some new API that will do that
            }
        }
    }
}