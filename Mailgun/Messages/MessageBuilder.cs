using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Mailgun.Core.Messages;
using Mailgun.Exceptions;
using Newtonsoft.Json.Linq;

namespace Mailgun.Messages
{
    public class MessageBuilder : IMessageBuilder
    {
        private IMessage _message;
        private int _recipientCount;
        private int _ccCount;
        private int _bccCount;

        public MessageBuilder()
        {
            _message = new Message()
            {
                Dkim = true, //That's a sane default
            };
        }

        public IMessageBuilder AddToRecipientList(IEnumerable<IRecipient> recipients)
        {
            foreach (IRecipient recipient in recipients)
            {
                AddToRecipient(recipient);
            }
            return this;
        }


        public IMessageBuilder AddToRecipient(IRecipient recipient, JObject recipientVariables = null)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => recipient);

            if (_recipientCount == Constants.MaximumAllowedRecipients)
            {
                throw new Exception("Messages cannot contain more than To 1000 recipients");
            }

            //set the to value
            if (_message.To == null)
            {
                _message.To = new Collection<IRecipient>();
            }

            //set the recipient variables
            if (recipientVariables != null)
            {
                if (_message.RecipientVariables == null)
                {
                    _message.RecipientVariables = new JObject();
                }
                _message.RecipientVariables[recipient.Email] = recipientVariables;
            }

            //add to the message
            _message.To.Add(recipient);
            _recipientCount++;

            return this;
        }

        public IMessageBuilder AddCcRecipient(IRecipient recipient)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => recipient);

            if (_ccCount == Constants.MaximumAllowedRecipients)
            {
                throw new Exception("Messages cannot contain more than 1000 CC recipients");
            }

            //set the Cc value
            if (_message.Cc == null)
            {
                _message.Cc = new Collection<IRecipient>();
            }

            //add to the message
            _message.To.Add(recipient);
            _ccCount++;

            return this;
        }

        public IMessageBuilder AddBccRecipient(IRecipient recipient)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => recipient);

            if (_bccCount == Constants.MaximumAllowedRecipients)
            {
                throw new Exception("Messages cannot contain more than 1000 Bcc recipients");
            }

            //set the Bcc value
            if (_message.Bcc == null)
            {
                _message.Bcc = new Collection<IRecipient>();
            }
            //add to the message
            _message.To.Add(recipient);
            _bccCount++;

            return this;
        }

        public IMessageBuilder SetFromAddress(IRecipient sender)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => sender);

            //add to the message
            _message.From = sender;

            return this;
        }

        public IMessageBuilder SetReplyToAddress(IRecipient recipient)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => recipient);

            if (_message.CustomHeaders == null)
            {
                _message.CustomHeaders = new Dictionary<string, string>();
            }

            //add the reply to header
            _message.CustomHeaders.Add("reply-to",
                string.IsNullOrEmpty(recipient.DisplayName)
                    ? recipient.Email
                    : string.Format("{0} <{1}>", recipient.DisplayName, recipient.Email));

            return this;
        }

        public IMessageBuilder SetSubject(string subject)
        {
            //check for recipient
            ThrowIf.IsArgumentNull(() => subject);

            _message.Subject = subject;

            return this;
        }

        public IMessageBuilder AddCustomHeader(string headerName, string headerData)
        {
            ThrowIf.IsArgumentNull(() => headerName);
            ThrowIf.IsArgumentNull(() => headerData);

            if (_message.CustomHeaders == null)
            {
                _message.CustomHeaders = new Dictionary<string, string>();
            }

            //add the custom header
            _message.CustomHeaders.Add(headerName, headerData);

            return this;
        }

        public IMessageBuilder SetTextBody(string textBody)
        {
            ThrowIf.IsArgumentNull(() => textBody);

            _message.Text = textBody;

            return this;
        }

        public IMessageBuilder SetHtmlBody(string htmlBody)
        {
            ThrowIf.IsArgumentNull(() => htmlBody);

            _message.Html = htmlBody;

            return this;
        }


        public IMessageBuilder AddAttachment(FileInfo file)
        {
            ThrowIf.IsArgumentNull(() => file);


            if (_message.Attachments == null)
            {
                _message.Attachments = new Collection<FileInfo>();
            }
            //Add
            _message.Attachments.Add(file);

            return this;
        }

        public IMessageBuilder AddAttachment(IFileAttachment file)
        {
            ThrowIf.IsArgumentNull(() => file);

            if (_message.FileAttachments == null)
            {
                _message.FileAttachments = new Collection<IFileAttachment>();
            }
            //Add
            _message.FileAttachments.Add(file);

            return this;
        }

        public IMessageBuilder AddInlineImage(FileInfo file)
        {
            ThrowIf.IsArgumentNull(() => file);

            if (_message.Inline == null)
            {
                _message.Inline = new Collection<FileInfo>();
            }

            //Add
            _message.Inline.Add(file);

            return this;
        }

        public IMessageBuilder SetTestMode(bool testMode)
        {
            _message.TestMode = testMode;
            return this;
        }

        public IMessageBuilder AddCampaignId(string campaignId)
        {
            _message.CampaignId = campaignId;
            return this;
        }

        public IMessageBuilder AddTag(string tag)
        {
            ThrowIf.IsArgumentNull(() => tag);

            //create if not exist
            if (_message.Tags == null)
            {
                _message.Tags = new Collection<string>();
            }

            //Add
            _message.Tags.Add(tag);

            return this;
        }

        public IMessageBuilder SetDkim(bool enabled)
        {
            _message.Dkim = enabled;
            return this;
        }

        public IMessageBuilder SetOpenTracking(bool enabled)
        {
            _message.TrackingOpen = enabled;
            return this;
        }

        public IMessageBuilder SetClickTracking(bool enabled)
        {
            _message.TrackingClicks = enabled;
            return this;
        }

        public IMessageBuilder SetTracking(bool enabled)
        {
            _message.Tracking = enabled;
            return this;
        }


        public IMessageBuilder SetDeliveryTime(DateTime dateTime, TimeZone zone = null)
        {
            _message.DeliveryTime = zone == null ? dateTime.ToUniversalTime() : zone.ToUniversalTime(dateTime);
            return this;
        }

        public IMessageBuilder ScheduleDeliveryTime(DateTimeOffset dateTime)
        {
            _message.DeliveryTime = dateTime.UtcDateTime;
            return this;
        }

        public IMessageBuilder AddCustomData(string customName, JObject data)
        {
            ThrowIf.IsArgumentNull(() => customName);
            ThrowIf.IsArgumentNull(() => data);

            //create if not exist
            if (_message.CustomData == null)
            {
                _message.CustomData = new Dictionary<string, JObject>();
            }

            //Add
            _message.CustomData.Add(customName, data);

            return this;
        }

        public IMessageBuilder AddCustomParameter(string parameterName, string data)
        {
            ThrowIf.IsArgumentNull(() => parameterName);
            ThrowIf.IsArgumentNull(() => data);


            //create if not exist
            if (_message.CustomParameters == null)
            {
                _message.CustomParameters = new Dictionary<string, string>();
            }

            //Add
            _message.CustomParameters.Add(parameterName, data);

            return this;
        }

        public IMessageBuilder SetMessage(IMessage message)
        {
            ThrowIf.IsArgumentNull(() => message);

            _message = message;
            return this;
        }

        public IMessage GetMessage()
        {
            return _message;
        }

        public ICollection<FileInfo> GetFiles()
        {
            return _message.Attachments;
        }
    }
}