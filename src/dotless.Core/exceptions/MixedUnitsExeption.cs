using System;
using System.Runtime.Serialization;

namespace dotless.Core.exceptions
{
    public class MixedUnitsExeption : Exception
    {
        public MixedUnitsExeption()
        {
        }

        public MixedUnitsExeption(string message) : base(message)
        {
        }

        public MixedUnitsExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MixedUnitsExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}