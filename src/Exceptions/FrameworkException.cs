using System;
using System.Runtime.Serialization;

namespace TgBotFramework.Exceptions
{
    public class FrameworkException : Exception
    {
        public FrameworkException() : base()
        { }

        public FrameworkException(string? message) : base(message)
        { }
        
        public FrameworkException(string? message, Exception? innerException) : base(message, innerException) 
        { }

        protected FrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {  }
    }
}