using System;

namespace TgBotFramework.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true, Inherited = false )]
    public class ScopeAttribute : Attribute
    {
        public ScopeEnum Scope { get; }

        public ScopeAttribute(ScopeEnum scope)
        {
            this.Scope = scope;
        }
    }
    public enum ScopeEnum
    {
        Singleton,
        Scoped,
        Transient
    }
}