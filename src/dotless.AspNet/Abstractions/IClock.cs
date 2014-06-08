using System;

namespace dotless.Core.Abstractions
{
    public interface IClock
    {
        DateTime GetUtcNow();
    }
}
