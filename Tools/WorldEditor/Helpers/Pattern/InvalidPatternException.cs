using System;
using System.Runtime.Serialization;

namespace WorldEditor.Helpers.Pattern
{
    [Serializable]
    public class InvalidPatternException : Exception
    {
        public int Index { get; set; }

        public InvalidPatternException(string message)
            : base(message)
        {
        }

        public InvalidPatternException(string message, int index)
            : base(message)
        {
            this.Index = index;
        }

        public InvalidPatternException(string message, Exception inner, int index)
            : base(message, inner)
        {
            this.Index = index;
        }

        protected InvalidPatternException(SerializationInfo info, StreamingContext context, int index)
            : base(info, context)
        {
            this.Index = index;
        }
    }
}
