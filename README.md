mailgun_csharp
==============

A Mailgun API library for C#

Made with inspiration from the [Mailgun-php](https://github.com/mailgun/mailgun-php) implementation, this library wraps the
Mailgun HTTP API for easy use in C# applications. 

##Basic usage
The current implementation supports creating a MessageService and sending Messages. A Message can be created manually or 
you can use the recommended MessageBuilder.
```csharp
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
```          
The current Message object supports all the options listed in the Mailgun documentation [here](http://documentation.mailgun.com/api-sending.html#sending)

##ASP.net Identity Usage
The new ASP.net Identity system supports the addition of an IIdentityMessageService for sending authorization emails. The Mailgun.AspNet.Identity package has an implementation for use with your Mailgun account. Usage would be something like this-
```csharp
     //wherever you initialize your user manager
    _userManager = new UserManager<IdentityUser, string>(store);
    
    //simple usage
    _userManager.EmailService = new MailgunMessageService("domain","apiKey");
```
The above configuration will send plain text emails using the specified domain and apiKey over SSL. For more options, you can pass in an IMailgunMessageServiceOptions object, to specify any custom rackspace configuration options you might have.

     //wherever you initialize your user manager
    _userManager = new UserManager<IdentityUser, string>(store);
    //advanced usage
    _userManager.EmailService = new MailgunMessageService(new MailgunMessageServiceOptions
            {
                ApiKey = "",
                Domain = "",
                TestMode = true,
                Tracking = true,
                TrackingClicks = true,
                TrackingOpen = true,
                UseDkim = true,
                DefaultHeaders = new Dictionary<string, string>{{"X-Some-Custom-Header","Custom"}},
                DefaultTags = new Collection<string>{"AuthorizationEmails"}
            });
     

##TODO
There is much more to do, but on the plate next are-

* Testing for Batch sending
* Stored Messages
* Events
