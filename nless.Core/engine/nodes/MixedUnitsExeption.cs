using System;
using System.Runtime.Serialization;

namespace nless.Core.engine
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