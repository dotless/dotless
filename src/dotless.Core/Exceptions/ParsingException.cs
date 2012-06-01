namespace dotless.Core.Exceptions
{
    using System;
    using dotless.Core.Parser;

    public class ParsingException : Exception
    {
        public NodeLocation Location { get; set; }
        public NodeLocation CallLocation { get; set; }

        public ParsingException(string message, NodeLocation location) : this(message, null, location, null) { }

        public ParsingException(string message, NodeLocation location, NodeLocation callLocation) : this(message, null, location, callLocation) { }

        public ParsingException(Exception innerException, NodeLocation location) : this(innerException, location, null) { }

        public ParsingException(Exception innerException, NodeLocation location, NodeLocation callLocation) : this(innerException.Message, innerException, location, callLocation) { }

        public ParsingException(string message, Exception innerException, NodeLocation location, NodeLocation callLocation)
            : base(message, innerException)
        {
            Location = location;
            CallLocation = callLocation;
        }
    }
}