using System;

namespace TgBotFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command without / at the beginning
        /// </summary>
        public string Text { get; set; }

        public CommandAttribute(string text)
        {
            Text = text;
        }
    }
}