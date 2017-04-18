using System;
using System.Runtime.Serialization;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// unique Exception type that will return 
    /// when the Seacher alagoritm failed to solve the graph.
    /// 
    /// doesn't do any spaicel thing, just for distinguish between errors.
    /// </summary>
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