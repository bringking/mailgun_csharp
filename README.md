mailgun_csharp
==============

A Mailgun API library for C#

Made with inspiration from the [Mailgun-php](https://github.com/mailgun/mailgun-php) implementation, this library wraps the
Mailgun HTTP API for easy use in C# applications. 

##Basic usage
The current implementation supports creating a MessageService and sending Messages. A Message can be created manually or 
you can use the recommended MessageBuilder.

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
            
The current Message object supports all the options listed in the Mailgun documentation [here](http://documentation.mailgun.com/api-sending.html#sending)

##TODO
There is much more to do, but on the plate next are-

* Testing for Batch sending
* ASP.net Microsoft.AspNet.Identity.IIdentityMessageService that allows for plugging Mailgun into the new ASP.net Identity mailing system
* Stored Messages
* Events
