using System;

namespace dotless.Core.Abstractions
{
    class Clock : IClock
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}