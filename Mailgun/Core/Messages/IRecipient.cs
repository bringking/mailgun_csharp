namespace Mailgun.Core.Messages
{
    public interface IRecipient
    {
        /// <summary>
        /// The display name of the recipient
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// The email address of the recipient
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Get this recipients value as a formatted string. For example, if a display name is provided, it will return 'displayName <address>'
        /// but if no display name is provided, it will return 'address'
        /// </summary>
        /// <returns></returns>
        string ToFormattedString();
    }
}
