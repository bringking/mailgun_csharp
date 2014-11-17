using System.Linq;
using System.Threading.Tasks;
using Mailgun.AspNet.Identity.Core;
using Mailgun.AspNet.Identity.Exceptions;
using Mailgun.Core.Messages;
using Mailgun.Exceptions;
using Mailgun.Messages;
using Mailgun.Service;
using Microsoft.AspNet.Identity;

namespace Mailgun.AspNet.Identity
{
    public class MailgunMessageService : IMailgunMessageService
    {
        private readonly MessageService _messageService;
        private readonly string _domain;
        private readonly IMailgunMessageServiceOptions _options;
        private IRecipient _from;
        private IRecipient _replyTo;

        /// <summary>
        /// Simple constructor with only the required domain and api key options
        /// </summary>
        /// <param name="domain">The Mailgun domain to use</param>
        /// <param name="apikey">The Mailgun Apikey to use</param>
        /// <param name="from">The from address to send messages from</param>
        public MailgunMessageService(string domain, string apikey, string from)
        {
            ThrowIf.IsArgumentNull(() => domain);
            ThrowIf.IsArgumentNull(() => apikey);
            ThrowIf.IsArgumentNull(() => from);

            _from = new Recipient {Email = from};
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
            if (options.DefaultFrom == null)
            {
                throw new RequiredOptionMissingException("DefaultFrom", Constants.FromAddressMissing);
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
            ThrowIf.IsArgumentNull(() => message);

            return _options != null ? SendWithOptions(message) : SendWithSimpleParameters(message);
        }

        private Task SendWithOptions(IdentityMessage message)
        {
            ThrowIf.IsArgumentNull(() => message);

            var builder = new MessageBuilder();
            builder.
                SetSubject(message.Subject).
                AddToRecipient(new Recipient {Email = message.Destination});

            //set the default from
            if (_options.DefaultFrom != null)
            {
                builder.SetFromAddress(_options.DefaultFrom);
            }

            //override replyTo if it was set by SendAsync
            if (_from != null)
            {
                builder.SetFromAddress(_from);
            }

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

            //set the default replyTo
            if (_options.DefaultReplyTo != null)
            {
                builder.SetReplyToAddress(_options.DefaultReplyTo);
            }

            //override replyTo if it was set by SendAsync
            if (_replyTo != null)
            {
                builder.SetReplyToAddress(_replyTo);
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
            ThrowIf.IsArgumentNull(() => message);

            //Build a mailgun message from the message object
            var builder = new MessageBuilder()
                .AddToRecipient(new Recipient {Email = message.Destination})
                .SetFromAddress(_from)
                .SetSubject(message.Subject)
                .SetTextBody(message.Body);

            if (_replyTo != null)
            {
                builder.SetReplyToAddress(_replyTo);
            }


            return _messageService.SendMessageAsync(_domain, builder.GetMessage());
        }

        public Task SendAsync(IdentityMessage message, IRecipient fromAddress)
        {
            ThrowIf.IsArgumentNull(() => message);
            ThrowIf.IsArgumentNull(() => fromAddress);

            //set from address
            _from = fromAddress;

            return _options != null ? SendWithOptions(message) : SendWithSimpleParameters(message);
        }


        public Task SendAsync(IdentityMessage message, IRecipient fromAddress, IRecipient replyTo)
        {
            ThrowIf.IsArgumentNull(() => message);
            ThrowIf.IsArgumentNull(() => fromAddress);
            ThrowIf.IsArgumentNull(() => replyTo);


            //set from address
            _from = fromAddress;

            //set the reply to
            _replyTo = replyTo;

            return _options != null ? SendWithOptions(message) : SendWithSimpleParameters(message);
        }
    }
}