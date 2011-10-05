using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.Core.Parser.Infrastructure
{
    public interface IExtension
    {
        void Setup(Env environment);
    }
}
