using System;

namespace Mailgun.Exceptions
{
    public class RequiredPropertyNullException : ArgumentNullException
    {
        public RequiredPropertyNullException()
        {
        }

        public RequiredPropertyNullException(string paramName) : base(paramName)
        {
        }

        public RequiredPropertyNullException(string paramName, string message) : base(paramName, message)
        {
        }

        public RequiredPropertyNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}