using System;
using System.Runtime.Serialization;

namespace Kubera.Business.Exceptions
{
    public class AlphaVantageException : Exception
    {
        internal AlphaVantageException()
        {
        }

        internal AlphaVantageException(string message)
            : base(message)
        {
        }

        internal AlphaVantageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal AlphaVantageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
