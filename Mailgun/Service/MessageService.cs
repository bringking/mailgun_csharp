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
        public string ApiKey { get; private set; }
        public string BaseAddress { get; private set; }
        public bool UseSSl { get; private set; }

        /// <summary>
        /// Create an instance of the mailgun service with the specified apikey
        /// </summary>
        /// <param name="apikey">Your mailgun API Key</param>
        /// <param name="useSsl">Should the library use SSL for all requests?</param>
        /// <param name="baseAddress">Base address of the mailgun api, excluding the scheme, e.g. api.mailgun.net/v3</param>
        public MessageService(string apikey, bool useSsl = true, string baseAddress = "api.mailgun.net/v3")
        {
            ApiKey = apikey;
            BaseAddress = baseAddress;
            UseSSl = useSsl;
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
            using (var client = new HttpClient())
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
                return await client.PostAsync(buildUri.ToString(), message.AsFormContent()).ConfigureAwait(false);
            }
        }
    }
}