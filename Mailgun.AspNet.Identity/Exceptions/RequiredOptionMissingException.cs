using System;

namespace Mailgun.AspNet.Identity.Exceptions
{
    public class RequiredOptionMissingException : ArgumentNullException
    {
         public RequiredOptionMissingException()
        {
        }

        public RequiredOptionMissingException(string paramName) : base(paramName)
        {
        }

        public RequiredOptionMissingException(string paramName, string message) : base(paramName, message)
        {
        }

        public RequiredOptionMissingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
