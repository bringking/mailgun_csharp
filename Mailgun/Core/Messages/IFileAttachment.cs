namespace Mailgun.Core.Messages
{
    public interface IFileAttachment
    {
        /// <summary>
        /// The byte data of the file.
        /// </summary>
        byte[] Data { get; set; }

        /// <summary>
        /// The filename of the file to be sent.
        /// </summary>
        string Name { get; set; }
    }
}