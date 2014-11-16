using System.Net.Http;
using System.Threading.Tasks;
using Mailgun.Core.Messages;

namespace Mailgun.Core.Service
{
    public interface IMessageService
    {
        /// <summary>
        /// Send a message async
        /// </summary>
        /// <param name="workingDomain">The mailgun working domain to use</param>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        Task<HttpResponseMessage> SendMessageAsync(string workingDomain, IMessage message);
    }
}