using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using Mailgun.Core.Messages;
using Mailgun.Exceptions;
using Mailgun.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mailgun.Messages
{
    public class Message : IMessage
    {
        public JObject RecipientVariables { get; set; }
        public IRecipient From { get; set; }
        public ICollection<IRecipient> To { get; set; }
        public ICollection<IRecipient> Cc { get; set; }
        public ICollection<IRecipient> Bcc { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public ICollection<FileInfo> Attachments { get; set; }
        public ICollection<IFileAttachment> FileAttachments { get; set; }
        public ICollection<FileInfo> Inline { get; set; }
        public ICollection<string> Tags { get; set; }
        public string CampaignId { get; set; }
        public bool Dkim { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public bool TestMode { get; set; }
        public bool Tracking { get; set; }
        public bool TrackingClicks { get; set; }
        public bool TrackingOpen { get; set; }
        public IDictionary<string, JObject> CustomData { get; set; }
        public IDictionary<string, string> CustomHeaders { get; set; }
        public IDictionary<string, string> CustomParameters { get; set; }


        public HttpContent AsFormContent()
        {
            //return basic form content
           // if (Attachments == null && Inline == null) return new FormUrlEncodedContent(AsKeyValueCollection());

            var content = new MultipartFormDataContent();
            //there are images or attachments, so process those
            if (Attachments != null && Attachments.Count > 0)
            {
                //add attachments
                foreach (var file in Attachments)
                {
                    content.Add(new ByteArrayContent(File.ReadAllBytes(file.FullName)), "attachment", file.Name);
                }
            }

            if (FileAttachments != null && FileAttachments.Count > 0)
            {
                //add attachments
                foreach (var file in FileAttachments)
                {
                    content.Add(new ByteArrayContent(file.Data), "attachment", file.Name);
                }
            }

            if (Inline != null && Inline.Count > 0)
            {
                //add inline images
                foreach (var image in Inline)
                {
                    content.Add(new StreamContent(image.OpenRead()), "inline", image.Name);
                }
            }


            //add the rest
            foreach (var kvp in AsKeyValueCollection())
            {
                content.Add(new StringContent(kvp.Value), kvp.Key);
            }

            return content;
        }

        public ICollection<KeyValuePair<string, string>> AsKeyValueCollection()
        {
            //check for required properties, and throw if they are missing
            ThrowIf.IsPropertyNull(() => To);
            ThrowIf.IsPropertyNull(() => From);

            //create kv store
            var kvp = new Collection<KeyValuePair<string, string>>();

            //add the to values
            var toSb = new StringBuilder();
            foreach (var to in To)
            {
                toSb.Append(to.ToFormattedString() + ",");
            }
            kvp.Add("to", toSb.ToString());

            //add the from field
            if (From != null)
            {
                kvp.AddIfNotNullOrEmpty("from", From.ToFormattedString());
            }
            //add the subject, test, html and campaign ID
            kvp.AddIfNotNullOrEmpty("subject", Subject);
            kvp.AddIfNotNullOrEmpty("text", Text);
            kvp.AddIfNotNullOrEmpty("html", Html);
            kvp.AddIfNotNullOrEmpty("o:campaign", CampaignId);

            //add date time values
            if (DeliveryTime != null)
            {
                kvp.Add("o:deliverytime", DeliveryTime.Value.ToUnixTime().ToString());
            }


            //add boolean values
            kvp.Add("o:dkim", Dkim.AsYesNo());
            kvp.Add("o:testmode", TestMode.AsYesNo());
            kvp.Add("o:tracking", Tracking.AsYesNo());
            kvp.Add("o:tracking-clicks", TrackingClicks.AsYesNo());
            kvp.Add("o:tracking-opens", TrackingOpen.AsYesNo());


            //tags
            if (Tags != null)
            {
                foreach (var t in Tags)
                {
                    kvp.AddIfNotNullOrEmpty("o:tag", t);
                }
            }

            //add recipient variables
            if (RecipientVariables != null)
            {
                kvp.AddIfNotNullOrEmpty("recipient-variables", RecipientVariables.ToString(Formatting.None));
            }

            //add custom headers
            if (CustomHeaders != null)
            {
                foreach (var h in CustomHeaders)
                {
                    kvp.AddIfNotNullOrEmpty(string.Format("h:{0}", h.Key), h.Value);
                }
            }

            //add custom data
            if (CustomData != null)
            {
                foreach (var cd in CustomData)
                {
                    kvp.Add(string.Format("v:{0}", cd.Key), cd.Value.ToString(Formatting.None));
                }
            }

            if (CustomParameters != null)
            {
                //add custom params
                foreach (var p in CustomParameters)
                {
                    kvp.AddIfNotNullOrEmpty(p.Key, p.Value);
                }
            }

            //TODO attachments and inline
            return kvp;
        }
    }
}