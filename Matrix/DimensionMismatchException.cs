using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Molytho.Matrix
{
    public class DimensionMismatchException : Exception
    {
        public DimensionMismatchException()
        {
        }

        public DimensionMismatchException(string message) : base(message)
        {
        }

        public DimensionMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DimensionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
