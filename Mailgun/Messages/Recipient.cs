using Mailgun.Core.Messages;
using Mailgun.Exceptions;

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
            ValidateEmail();
            var displayName = DisplayName ?? "";
            //Escape quotes in display name
            displayName = displayName.Replace("\"", "\\\"");
            return displayName == ""
                ? Email
                : $"\"{displayName}\" <{Email}>";
        }

        /// <summary>
        /// That's just some basic checks to handle already happened situations. Mailgun doesn't signal you
        /// specific failing emails (and it can be hundreds of them in one batch in some scenarios).
        /// Better check something on client side
        /// </summary>
        private void ValidateEmail()
        {
            if (!Email.Contains("@"))
            {
                throw new InvalidEmailException(Email);
            }
            if (Email.Contains(".@"))
            {
                throw new InvalidEmailException(Email);
            }
        }
    }
}