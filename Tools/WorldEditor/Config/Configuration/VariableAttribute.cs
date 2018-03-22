using System;

namespace WorldEditor.Config.Configuration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class VariableAttribute : Attribute
    {
        public bool DefinableRunning { get; set; }

        public int Priority { get; set; }

        public VariableAttribute()
        {
            Priority = 1;
        }

        public VariableAttribute(bool definableByConfig = false)
        {
            DefinableRunning = definableByConfig;
            Priority = 1;
        }
    }
}