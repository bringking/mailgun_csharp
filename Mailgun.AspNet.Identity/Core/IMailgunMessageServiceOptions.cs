using Mailgun.Core.Messages;
using System.Collections.Generic;

namespace Mailgun.AspNet.Identity.Core
{
    public interface IMailgunMessageServiceOptions
    {
        /// <summary>
        /// The sender name and address
        /// </summary>
        IRecipient DefaultFrom { get; set; }
        /// <summary>
        /// The reply-to name and address
        /// </summary>
        IRecipient DefaultReplyTo { get; set; }

        bool UseSsl { get; set; }
        /// <summary>
        /// The mailgun API key
        /// </summary>
        string ApiKey { get; set; }
        /// <summary>
        /// The mailgun domain to use for this service
        /// </summary>
        string Domain { get; set; }
        /// <summary>
        /// Should emails be sent with an Html body
        /// </summary>
        bool UseHtmlBody { get; set; }
        /// <summary>
        /// Is Dkim enabled
        /// </summary>
        bool UseDkim { get; set; }
        /// <summary>
        /// Is test mode on? (Messages won't leave the mailgun service)
        /// </summary>
        bool TestMode { get; set; }
        /// <summary>
        /// Should emails sent with this service use tracking
        /// </summary>
        bool Tracking { get; set; }
        /// <summary>
        /// Should emails sent with this service track clicks
        /// </summary>
        bool TrackingClicks { get; set; }
        /// <summary>
        /// Should emails sent with this service track opens
        /// </summary>
        bool TrackingOpen { get; set; }
        /// <summary>
        /// A set of tags to include in every message 
        /// </summary>
        ICollection<string> DefaultTags { get; set; }
        /// <summary>
        /// Name and string values to add as a custom header. e.g. {"X-My-Header","SomeCustomerHeaderValue"}
        /// </summary>
        IDictionary<string, string> DefaultHeaders { get; set; }

    }
}
