using System;

namespace dotless.Core.engine.Functions
{
    public abstract class FunctionBase
    {
        protected INode[] Arguments { get; set; }

        public abstract INode Evaluate();

        public void SetArguments(INode[] arguments)
        {
            Arguments = arguments;
        }
    }
}