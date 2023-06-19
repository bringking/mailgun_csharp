using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mailgun.Events
{

    /// <summary>
    /// Represents a message event, <see href="https://documentation.mailgun.com/en/latest/api-events.html">MailGun documentation for events</see>
    /// </summary>
    public class MessageEvent
    {
        [JsonProperty("envelope")]
        public Envelope Envelope { get; set; }

        [JsonProperty("storage")]
        public Storage Storage { get; set; }

        [JsonProperty("user-variables")]
        public Dictionary<string, string> UserVariables { get; set; }

        [JsonProperty("tags")]
        public List<object> Tags { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public double Timestamp { get; set; }

        [JsonProperty("log-level")]
        public string LogLevel { get; set; }

        [JsonProperty("delivery-status")]
        public DeliveryStatus DeliveryStatus { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("recipient-domain")]
        public string RecipientDomain { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("campaigns")]
        public List<object> Campaigns { get; set; }
    }

    public class DeliveryStatus
    {
        [JsonProperty("retry-seconds")]
        public int RetrySeconds { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("session-seconds")]
        public double SessionSeconds { get; set; }

        [JsonProperty("attempt-no")]
        public int AttemptNo { get; set; }

        [JsonProperty("utf8")]
        public bool Utf8 { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("certificate-verified")]
        public bool CertificateVerified { get; set; }

        [JsonProperty("mx-host")]
        public string MxHost { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("enhanced-code")]
        public string EnhancedCode { get; set; }

        [JsonProperty("bounce-code")]
        public string BounceCode { get; set; }
    }

    public class Envelope
    {
        [JsonProperty("sending-ip")]
        public string SendingIp { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("targets")]
        public string Targets { get; set; }

        [JsonProperty("transport")]
        public string Transport { get; set; }
    }

    public class Flags
    {
        [JsonProperty("is-system-test")]
        public bool IsSystemTest { get; set; }

        [JsonProperty("is-test-mode")]
        public bool IsTestMode { get; set; }

        [JsonProperty("is-authenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("is-routed")]
        public bool IsRouted { get; set; }

        [JsonProperty("is-delayed-bounce")]
        public bool? IsDelayedBounce { get; set; }
    }

    public class Headers
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("message-id")]
        public string MessageId { get; set; }
    }



    public class Message
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }

    public class Paging
    {
        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }

    public class MessageEvents
    {
        [JsonProperty("items")]
        public List<MessageEvent> Items { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public class Storage
    {
        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("env")]
        public string Env { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }


}