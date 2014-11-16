using Mailgun.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Should;

namespace Mailgun.Tests.Messages
{
    [TestClass]
    public class MessageBuilderSpec
    {
        [TestMethod]
        public void MessageBuilderRecipientVariables()
        {
            var builder = new MessageBuilder();

            var message =
                builder.AddToRecipient(new Recipient {DisplayName = "Charles King", Email = "bringking@gmail.com"},
                    JObject.Parse("{\"id\":\"123\"}"))
                    .GetMessage();

            message.RecipientVariables.ShouldNotBeNull();
            message.RecipientVariables["bringking@gmail.com"].ShouldNotBeNull();
        }
    }
}