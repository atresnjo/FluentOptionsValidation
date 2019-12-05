using System;
using System.Runtime.Serialization;

namespace FluentOptionsValidation
{
    public class AbortStartupException : Exception
    {
        public AbortStartupException()
        {
        }

        public AbortStartupException(string message) : base(message)
        {
        }

        public AbortStartupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AbortStartupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}