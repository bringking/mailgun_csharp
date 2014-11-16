using System;
using System.Linq;
using System.Threading.Tasks;
using Mailgun.AspNet.Identity.Core;
using Mailgun.AspNet.Identity.Exceptions;
using Mailgun.Messages;
using Mailgun.Service;
using Microsoft.AspNet.Identity;

namespace Mailgun.AspNet.Identity
{
    public class MailgunMessageService : IIdentityMessageService
    {
        private readonly MessageService _messageService;
        private readonly string _domain;
        private readonly IMailgunMessageServiceOptions _options;

        /// <summary>
        /// Simple constructor with only the required domain and api key options
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="apikey"></param>
        public MailgunMessageService(string domain, string apikey)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(domain);
            }
            if (string.IsNullOrEmpty(apikey))
            {
                throw new ArgumentNullException(apikey);
            }

            _domain = domain;
            _messageService = new MessageService(apikey);
        }

        /// <summary>
        /// Contstructor that accepts a more robust IMailgunMessageServiceOptions object
        /// </summary>
        /// <param name="options"></param>
        public MailgunMessageService(IMailgunMessageServiceOptions options)
        {
            _options = options;

            if (string.IsNullOrEmpty(options.ApiKey))
            {
                throw new RequiredOptionMissingException("ApiKey", Constants.ApiOrDomainKeyMissingMessage);
            }
            if (string.IsNullOrEmpty(options.Domain))
            {
                throw new RequiredOptionMissingException("Domain", Constants.ApiOrDomainKeyMissingMessage);
            }
            //set the message service
            _messageService = new MessageService(options.ApiKey, options.UseSsl);
        }

        /// <summary>
        /// Send an Identity message using the Mailgun mailer
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            return _options != null ? SendWithOptions(message) : SendWithSimpleParameters(message);
        }

        private Task SendWithOptions(IdentityMessage message)
        {
            var builder = new MessageBuilder();
            builder.
                SetSubject(message.Subject).
                AddToRecipient(new Recipient() {Email = message.Destination});

            //do customization based on service options
            builder.SetClickTracking(_options.TrackingClicks);
            builder.SetDkim(_options.UseDkim);
            builder.SetOpenTracking(_options.TrackingOpen);
            builder.SetTestMode(_options.TestMode);

            //add body
            if (_options.UseHtmlBody)
            {
                builder.SetHtmlBody(message.Body);
            }
            else
            {
                builder.SetTextBody(message.Body);
            }
            //add tags and headers
            if (_options.DefaultHeaders != null && _options.DefaultHeaders.Count > 0)
            {
                foreach (var kvp in _options.DefaultHeaders)
                {
                    builder.AddCustomHeader(kvp.Key, kvp.Value);
                }
            }
            if (_options.DefaultTags != null && _options.DefaultTags.Count > 0)
            {
                _options.DefaultTags.ToList().ForEach(t => builder.AddTag(t));
            }
            //send the message
            return _messageService.SendMessageAsync(_options.Domain, builder.GetMessage());
        }

        private Task SendWithSimpleParameters(IdentityMessage message)
        {
            //Build a mailgun message from the message object
            var msg = new MessageBuilder()
                .AddToRecipient(new Recipient {Email = message.Destination})
                .SetSubject(message.Subject)
                .SetTextBody(message.Body)
                .GetMessage();

            return _messageService.SendMessageAsync(_domain, msg);
        }
    }
}