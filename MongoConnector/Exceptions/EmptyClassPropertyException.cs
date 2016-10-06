using System;
using System.Runtime.Serialization;

namespace MongoConnector.Exceptions
{
    [Serializable]
    public class EmptyClassPropertyException : Exception
    {
        public EmptyClassPropertyException()
        {
        }

        public EmptyClassPropertyException(string message) : base(message)
        {
        }

        public EmptyClassPropertyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EmptyClassPropertyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}