using System;
using System.Runtime.Serialization;

namespace SearchAlgorithmsLib
{
    [Serializable]
    public class NotSolvableException : Exception
    {
        public NotSolvableException()
        {
        }

        public NotSolvableException(string message) : base(message)
        {

        }

        public NotSolvableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSolvableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}