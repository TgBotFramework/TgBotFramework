using System;

namespace TgBotFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class StageAttribute : System.Attribute
    {
        public string Stage { get; set; }
    }
}