using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Parser.Tree
{
    public class SelectorExtend : Node
    {
        public SelectorExtend(List<Selector> selectors)
        {
            Selectors = selectors;
        }

        public List<Selector> Selectors { get; set; }

        public override Node Evaluate(Env env)
        {
            return this;
        }

        public override void AppendCSS(Env env)
        {
            
        }
    }
}
