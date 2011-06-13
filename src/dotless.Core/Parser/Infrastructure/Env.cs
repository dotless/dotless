﻿namespace dotless.Core.Parser.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Functions;
    using Nodes;
    using Tree;

    public class Env
    {
        private Dictionary<string, Type> _functionTypes;

        public Stack<Ruleset> Frames { get; set; }
        public bool Compress { get; set; }
        public Node Rule { get; set; }
        public Output Output { get; private set; }

        public Env()
        {
            Frames = new Stack<Ruleset>();
            Output = new Output(this);
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
            return Frames.Select(frame => frame.Find(this, selector, null))
                .Where(f => !f.Any(c => Frames.Contains(c.Ruleset)))
                .FirstOrDefault(r => r.Count != 0);
        }

        public virtual Function GetFunction(string name)
        {
            if (_functionTypes == null)
            {
                var functionType = typeof (Function);

                _functionTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => functionType.IsAssignableFrom(t) && t != functionType)
                    .Where(t => !t.IsAbstract)
                    .SelectMany<Type, KeyValuePair<string, Type>>(GetFunctionNames)
                    .ToDictionary(p => p.Key, p => p.Value);

                _functionTypes["%"] = typeof (CFormatString);
            }

            name = name.ToLowerInvariant();

            if (_functionTypes.ContainsKey(name))
                return (Function) Activator.CreateInstance(_functionTypes[name]);

            return null;
        }

        private static IEnumerable<KeyValuePair<string, Type>> GetFunctionNames(Type t)
        {
            var name = t.Name;

            if (name.EndsWith("function", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0, name.Length - 8);

            name = Regex.Replace(name, @"\B[A-Z]", "-$0");

            name = name.ToLowerInvariant();

            yield return new KeyValuePair<string, Type>(name, t);

            if(name.Contains("-"))
                yield return new KeyValuePair<string, Type>(name.Replace("-", ""), t);
        }
    }
}