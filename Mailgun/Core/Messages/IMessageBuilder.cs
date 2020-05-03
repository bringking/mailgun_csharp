using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Mailgun.Core.Messages
{
    public interface IMessageBuilder
    {
        /// <summary>
        /// Add a recipient 'to' the to field of the message being built
        /// </summary>
        /// <param name="recipient">The recipient details</param>
        /// <param name="recipientVariables">The variables to asscociate with this recipient</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddToRecipient(IRecipient recipient, JObject recipientVariables = null);

        /// <summary>
        /// Add a recipient list 'to' the to field of the message being built
        /// </summary>
        /// <param name="recipients"></param>
        /// <returns></returns>
        IMessageBuilder AddToRecipientList(IEnumerable<IRecipient> recipients);

        /// <summary>
        /// Add a recipient to the 'cc' field of the message being built
        /// </summary>
        /// <param name="recipient">The recipient details</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddCcRecipient(IRecipient recipient);

        /// <summary>
        /// Add a recipient to the 'bcc' field of the message being built
        /// </summary>
        /// <param name="recipient">The recipient details</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddBccRecipient(IRecipient recipient);

        /// <summary>
        /// Set the senders address and name for this message
        /// </summary>
        /// <param name="recipient">The recipient details</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetFromAddress(IRecipient recipient);

        /// <summary>
        /// Set the senders reply-to address
        /// </summary>
        /// <param name="recipient">The recipient details</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetReplyToAddress(IRecipient recipient);

        /// <summary>
        /// Set the message subject
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetSubject(string subject);

        /// <summary>
        /// Add a custom header to the mail message
        /// </summary>
        /// <param name="headerName">The name of the custom header</param>
        /// <param name="headerData">The data for the custom header</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddCustomHeader(string headerName, string headerData);

        /// <summary>
        /// Set the text body of the email
        /// </summary>
        /// <param name="textBody">The plain text message body</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetTextBody(string textBody);

        /// <summary>
        /// Set the html body of the email
        /// </summary>
        /// <param name="htmlBody">The html text message body</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetHtmlBody(string htmlBody);

        /// <summary>
        /// Add an attachment to the mail message
        /// </summary>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddAttachment(FileInfo file);

        /// <summary>
        /// Add an attachment to the mail message
        /// </summary>
        /// <returns></returns>
        IMessageBuilder AddAttachment(IFileAttachment file);

        /// <summary>
        /// Add an image inline into the message body
        /// </summary>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddInlineImage(FileInfo file);

        /// <summary>
        /// Set the message to be in test mode
        /// </summary>
        /// <param name="testMode"></param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetTestMode(bool testMode);

        /// <summary>
        /// Set the Campaign ID of the mail message
        /// </summary>
        /// <param name="campaignId">The Mailgun campaign ID</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddCampaignId(string campaignId);

        /// <summary>
        /// Add a tag to the mail message
        /// </summary>
        /// <param name="tag">The tag to add</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder AddTag(string tag);

        /// <summary>
        /// Set Dkim to enabled
        /// </summary>
        /// <param name="enabled">Flag</param>
        /// <returns>IMessageBuilder</returns>
        IMessageBuilder SetDkim(bool enabled);

        /// <summary>
        /// Set tracking to be enabled for this mail message
        /// </summary>
        /// <param name="enabled">Flag</param>
        /// <returns></returns>
        IMessageBuilder SetTracking(bool enabled);

        /// <summary>
        /// Set open tracking to be enabled for this mail message
        /// </summary>
        /// <param name="enabled">Flag</param>
        /// <returns></returns>
        IMessageBuilder SetOpenTracking(bool enabled);

        /// <summary>
        /// Set click tracking for this message to be on or off
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        IMessageBuilder SetClickTracking(bool enabled);

        /// <summary>
        /// Set the delivery time for this message.
        /// </summary>
        /// <param name="dateTime">The delivery time</param>
        /// <param name="zone">The target timezone. (Optional)</param>
        /// <returns></returns>
        [Obsolete("Use ScheduleDeliveryTime")]
        IMessageBuilder SetDeliveryTime(DateTime dateTime, TimeZone zone = null);

        /// <summary>
        /// Set the delivery time for this message.
        /// </summary>
        /// <param name="dateTimeOffset">The delivery time</param>
        IMessageBuilder ScheduleDeliveryTime(DateTimeOffset dateTimeOffset);

        /// <summary>
        /// Add a custom array of data to the mail message
        /// </summary>
        /// <param name="customName">The name of the custom data</param>
        /// <param name="data">The custom data</param>
        /// <returns></returns>
        IMessageBuilder AddCustomData(string customName, JObject data);

        /// <summary>
        /// Add a custom parameter to the mail message
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="data">The data to add</param>
        /// <returns></returns>
        IMessageBuilder AddCustomParameter(string parameterName, string data);

        /// <summary>
        /// Set the message to build from
        /// </summary>
        /// <param name="message">The mail message to build on</param>
        /// <returns></returns>
        IMessageBuilder SetMessage(IMessage message);

        /// <summary>
        /// The built IMessage
        /// </summary>
        /// <returns></returns>
        IMessage GetMessage();

        /// <summary>
        /// Get the attachments set
        /// </summary>
        /// <returns></returns>
        ICollection<FileInfo> GetFiles();
    }
}