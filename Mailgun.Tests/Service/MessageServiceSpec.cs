using System;
using System.IO;
using System.Threading.Tasks;
using Mailgun.Messages;
using Mailgun.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Should;

namespace Mailgun.Tests.Service
{
    [TestClass]
    public class MessageServiceSpec
    {
        private const string ApiKey = "[apikeyHere]";
        private const string Domain = "sandbox9adbe277a51a430daeb12aaa652af7f1.mailgun.org";

        [TestMethod]
        public void TestDefaults()
        {
            var mg = new MessageService(ApiKey, true,"api.mailgun.net/v3");
            mg.ApiKey.ShouldEqual(ApiKey);
            mg.UseSSl.ShouldBeTrue();
            mg.BaseAddress.ShouldEqual("api.mailgun.net/v3");
        }

        [TestMethod]
        public async Task TestSendBatchMessage()
        {
             var mg = new MessageService(ApiKey);

            //build a message
            var builder = new MessageBuilder()
                .SetTestMode(true)
                .SetSubject("Plain text test")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetTextBody("This is a test");

            //add 1000 users
            for (var i = 0; i < 1000; i++)
            {
                builder.AddToRecipient(new Recipient() {Email = string.Format("test{0}@test.com", i)},JObject.Parse("{\"id\":"+i+"}"));
            }

            var content = await mg.SendMessageAsync(Domain, builder.GetMessage());
            content.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task TestSendMessage()
        {
            var mg = new MessageService(ApiKey);

            //build a message
            var message = new MessageBuilder()
                .AddToRecipient(new Recipient
                {
                    Email = "bringking@gmail.com",
                    DisplayName = "Charlie King"
                })
                .SetTestMode(true)
                .SetSubject("Plain text test")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetTextBody("This is a test")
                .GetMessage();

            var content = await mg.SendMessageAsync(Domain, message);
            content.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task TestSendHtmlMessage()
        {
            var mg = new MessageService(ApiKey);

            //build a message
            var message = new MessageBuilder()
                .AddToRecipient(new Recipient
                {
                    Email = "bringking@gmail.com",
                    DisplayName = "Charlie King"
                })
                .SetTestMode(true)
                .SetSubject("Html test")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetHtmlBody("<html><h1>Hello from the Mailgun C# library</h1></html>")
                .GetMessage();

            var content = await mg.SendMessageAsync(Domain, message);
            content.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task TestSendAttachments()
        {
            var mg = new MessageService(ApiKey);

            //build a message
            var message = new MessageBuilder()
                .AddToRecipient(new Recipient
                {
                    Email = "bringking@gmail.com",
                    DisplayName = "Charlie King"
                })
                .SetTestMode(true)
                .SetSubject("Attachment test")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetHtmlBody("<html><h1>I have an attachment</h1></html>")
                .AddAttachment(new FileInfo(Consts.PictureFileName))
                .GetMessage();

            var content = await mg.SendMessageAsync(Domain, message);
            content.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task TestInlineImage()
        {
            var mg = new MessageService(ApiKey);

            //build a message
            var message = new MessageBuilder()
                .AddToRecipient(new Recipient
                {
                    Email = "bringking@gmail.com",
                    DisplayName = "Charlie King"
                })
                .SetTestMode(true)
                .SetSubject("Inline image test")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetHtmlBody("<html>Inline image here: <img src=\"cid:Desert.jpg\"></html>")
                .AddInlineImage(new FileInfo(Consts.PictureFileName))
                .GetMessage();

            var content = await mg.SendMessageAsync(Domain, message);
            content.ShouldNotBeNull();
        }

        [TestMethod]
        public async Task TestKitchenSink()
        {
            var mg = new MessageService(ApiKey);

            //build a message
            var picturesDesert = Consts.PictureFileName;
            var message = new MessageBuilder()
                .SetTestMode(true)
                .AddToRecipient(new Recipient
                {
                    Email = "bringking@gmail.com",
                    DisplayName = "Charlie King"
                })
                .AddCcRecipient(new Recipient
                {
                    Email = "test@test.com",
                    DisplayName = "Tester"
                })
                .AddBccRecipient(new Recipient
                {
                    Email = "test1@test.com",
                    DisplayName = "Tester"
                })
                .SetReplyToAddress(new Recipient
                {
                    Email = "test_reply@test.com",
                    DisplayName = "Tester Reply"
                })
                .AddCustomData("Some Data", JObject.Parse("{\"test\":\"A test json object\"}"))
                .AddCustomHeader("X-My-Custom-Header", "Custom Header")
                .AddCustomParameter("CustomParam", "A custom parameter")
                .AddTag("Kitchen sink Tag")
                .AddCampaignId("fake_campaign_id")
                .SetClickTracking(true)
                .SetDkim(true)
                .SetOpenTracking(true)
                .SetDeliveryTime(DateTime.Now + TimeSpan.FromDays(1))
                .SetSubject("Kitchen Sink")
                .SetFromAddress(new Recipient {Email = "bringking@gmail.com", DisplayName = "Mailgun C#"})
                .SetTextBody("This is the text body")
                .SetHtmlBody("<html>Inline image here: <img src=\"cid:Desert.jpg\"></html>")
                .AddInlineImage(new FileInfo(picturesDesert))
                .AddAttachment(new FileInfo(picturesDesert))
                .GetMessage();

            var content = await mg.SendMessageAsync(Domain, message);
            content.ShouldNotBeNull();
        }
    }
}