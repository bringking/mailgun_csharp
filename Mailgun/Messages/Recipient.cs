using Mailgun.Core.Messages;

namespace Mailgun.Messages
{
    /// <summary>
    /// A structure that represents an email recipient with a DisplayName and an e-mail address
    /// </summary>
    public struct Recipient : IRecipient
    {
        /// <summary>
        /// The display name for the recipient
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The email address for the recipient
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get this recipients value as a formatted string. For example, if a display name is provided, it will return 'displayName <address>'
        /// but if no display name is provided, it will return 'address'
        /// </summary>
        /// <returns></returns>
        public string ToFormattedString()
        {
            return string.IsNullOrEmpty(DisplayName)
                ? Email
                : string.Format("\"{0}\" <{1}>", DisplayName, Email);
            ;
        }
    }
}
