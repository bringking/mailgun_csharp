using System.Collections.Generic;
using Mailgun.AspNet.Identity.Core;

namespace Mailgun.AspNet.Identity
{
    public class MailgunMessageServiceOptions : IMailgunMessageServiceOptions
    {
        public bool UseSsl { get; set; }
        public string ApiKey { get; set; }
        public string Domain { get; set; }
        public bool UseHtmlBody { get; set; }
        public bool UseDkim { get; set; }
        public bool TestMode { get; set; }
        public bool Tracking { get; set; }
        public bool TrackingClicks { get; set; }
        public bool TrackingOpen { get; set; }
        public ICollection<string> DefaultTags { get; set; }
        public IDictionary<string, string> DefaultHeaders { get; set; }
        public Mailgun.Core.Messages.IRecipient DefaultFrom
        {
            get;
            set;
        }
        public Mailgun.Core.Messages.IRecipient DefaultReplyTo { get; set; }


    }
}