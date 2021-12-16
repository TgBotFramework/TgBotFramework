using System;

namespace TgBotFramework.Exceptions
{
    public class PipelineException : FrameworkException
    {
        
        public PipelineException(string message) : base(message)
        { }
        public PipelineException(string message, params object[] obj) : base(string.Format(message, obj))
        { }
    }
}