namespace dotless.Core.Parser.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Functions;
    using Nodes;
    using Tree;
    using Utils;

    public class Env
    {
        private Dictionary<string, Type> _functionTypes;

        public Stack<Ruleset> Frames { get; set; }
        public bool Compress { get; set; }
        public Node Rule { get; set; }

        public Env()
        {
            Frames = new Stack<Ruleset>();
        }

        public Rule FindVariable(string name)
        {
            return FindVariable(name, Rule);
        }

        public Rule FindVariable(string name, Node rule)
        {
            var previousNode = rule;
            foreach (var frame in Frames)
            {
                var v = frame.Variable(name, previousNode);
                if (v)
                    return v;
                previousNode = frame;
            }
            return null;
        }

        public List<Closure> FindRulesets(Selector selector)
        {
            return Frames.Select(frame => frame.Find(this, selector, null)).FirstOrDefault(r => r.Count != 0);
        }

        public virtual Function GetFunction(string name)
        {
            if (_functionTypes == null)
            {
                var functionType = typeof (Function);

                _functionTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => functionType.IsAssignableFrom(t) && t != functionType)
                    .ToDictionary(t => GetFunctionName(t));
            }

            name = name.ToLowerInvariant();

            if (_functionTypes.ContainsKey(name))
                return (Function) Activator.CreateInstance(_functionTypes[name]);

            return null;
        }

        private static string GetFunctionName(Type t)
        {
            var name = t.Name.ToLowerInvariant();
            return name.EndsWith("function") ? name.Substring(0, name.Length - 8) : name;
        }
    }
}