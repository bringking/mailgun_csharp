Maintained but not developed
==============
This project now is not actively developed. But it works, used for production projects. I'll do my best to fix issues if they appear (they rarely do), and if somebody will contribute some improvements, I'll review them.
@leotsarev

![Nuget](https://img.shields.io/nuget/v/mailgun_csharp)

mailgun_csharp
==============

A Mailgun API library for C#

Made with inspiration from the [Mailgun-php](https://github.com/mailgun/mailgun-php) implementation, this library wraps the
Mailgun HTTP API for easy use in C# applications. 

## Download
You can download the source and build it using VS2013, or use Nuget
     ```
          Install-Package mailgun_csharp
     ```
     and the ASP.net package (if needed)
     ```
         Install-Package mailgun_csharp.AspNet.Identity
     ```

## Basic usage
The current implementation supports creating a MessageService and sending Messages. A Message can be created manually or 
you can use the recommended MessageBuilder.
```csharp
     var mg = new MessageService(ApiKey);
     //var mg = new MessageService(ApiKey,false); //you can specify to use SSL or not, which determines the url API scheme to use
     //var mg = new MessageService(ApiKey,false,"api.mailgun.net/v3"); //you can also override the base URL, which defaults to v2

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

## ASP.net Identity Usage
The new ASP.net Identity system supports the addition of an IIdentityMessageService for sending authorization emails. The Mailgun.AspNet.Identity package has an implementation for use with your Mailgun account. Usage would be something like this-
```csharp
     //wherever you initialize your user manager
    _userManager = new UserManager<IdentityUser, string>(store);
    
    //simple usage
    _userManager.EmailService = new MailgunMessageService("domain","apiKey");
```
The above configuration will send plain text emails using the specified domain and apiKey over SSL. For more options, you can pass in an IMailgunMessageServiceOptions object, to specify any custom rackspace configuration options you might have.
```csharp
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
                DefaultTags = new Collection<string>{"AuthorizationEmails"},
                BaseUrlOverride = "api.mailgun.net/v3" //use a different base URL
            });
```     

## Event retrieval

Message events (logs) can be retrieved from MailGun using

```csharp

        internal async Task<List<string>> GetFailuresAsync()
        {
            var mg = new MessageService(ApiKey);
	    //the filter structure is defined here: https://documentation.mailgun.com/en/latest/api-events.html#events
            var events = await mg.GetMessageEventsAsync("mg.conveyor.cloud", "event=rejected OR failed&limit=5");
            var failures = new List<string>();

            //extract email addresses with permanent failures
            do
            {
                failures.AddRange(events.Items.Where(e=>e.Severity=="permanent").Select(e => e.Recipient));
            } while ((events = await mg.GetMessageEventsPageAsync(events.Paging.Next)).Items.Count > 0);

            return failures;
        }
```    


## TODO
There is much more to do, but on the plate next are-

* Stored Messages
* Improve events
