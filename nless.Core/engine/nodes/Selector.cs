using System;
using System.Collections.Generic;

namespace nless.Core.engine.nodes
{
    public class Selectors : Dictionary<string, Func<Selector>> 
    {
        public Selectors()
        {
            Add("", () => new Descendant());
            Add(">", () => new Child());
            Add("+", () => new Adjacent());
            Add(":", () => new PseudoClass());
            Add("::", () => new PseudoElement());
            //Add("~", () => new Sibling());
        }
    }


    public class Selector : Entity
    {
        private static readonly Selectors _selectors = new Selectors();

        public static Selector Get(string key)
        {
            return _selectors[key].Invoke();
        }
    }

    public class Descendant : Selector
    {
        public override string ToCss()
        {
            return " ";
        }
    }

    public class Child : Selector
    {
        public override string ToCss()
        {
            return string.Format(" {0} ", this);
        }
    }

    public class Adjacent : Selector
    {
        public override string ToCss()
        {
            return string.Format(" {0} ", this);
        }
    }

    public class PseudoClass : Selector
    {
        public override string ToCss()
        {
            return ToString();
        }
    }

    public class PseudoElement : Selector
    {
        public override string ToCss()
        {
            return ToString();
        }
    }
}