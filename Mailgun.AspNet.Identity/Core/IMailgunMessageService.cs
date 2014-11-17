using Mailgun.Core.Messages;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailgun.AspNet.Identity.Core
{
    public interface IMailgunMessageService : IIdentityMessageService
    {
        /// <summary>
        /// Send an IdentityMessage but from a particular address and display name
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fromAddress"></param>
        /// <returns></returns>
        Task SendAsync(IdentityMessage message, IRecipient fromAddress);

        /// <summary>
        /// Send an IdentityMessage but from a particular address and display name, with a particular reply-to address
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fromAddress"></param>
        /// <param name="replyTo"></param>
        /// <returns></returns>
        Task SendAsync(IdentityMessage message, IRecipient fromAddress, IRecipient replyTo);
    }
}
