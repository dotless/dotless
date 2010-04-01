using System;
using System.Collections.Generic;
using System.Linq;
using dotless.Core.exceptions;
using dotless.Core.utils;

namespace dotless.Core.engine
{
    public class MixinBlock : ElementBlock
    {
        public MixinBlock(string name, string selector) : base(name, selector)
        {
            Arguments = new List<Variable>();
        }

        public MixinBlock(string name) : base(name)
        {
            Arguments = new List<Variable>();
        }

        public List<Variable> Arguments { get; set; }

        public IEnumerable<INode> GetClonedChildren(ElementBlock newParent, IEnumerable<Variable> arguments)
        {
            Guard.ExpectMaxArguments(Arguments.Count, arguments.Count(), GetMixinString(arguments));

            var argumentBlock = new ElementBlock(Name) {Parent = newParent};
            var rulesBlock = new ElementBlock(Name + "_rules") {Parent = argumentBlock};

            var rules = new List<INode>();

            var clonedChildren = Children.Select(n => n.AdoptClone(rulesBlock, this)).ToList();
            rules.AddRange(clonedChildren);

            var clonedArguments = Arguments.Select(n => n.AdoptClone(argumentBlock)).Cast<Variable>().ToList();

            var overridenArguments = new List<Variable>();
            var foundNamedArgument = false;
            foreach (var arg in arguments)
            {
                int position;
                Variable newArgument = null;
                if (int.TryParse(arg.Key, out position))
                {
                    if (foundNamedArgument)
                        throw new ParsingException(string.Format("Positional arguments must appear before all named arguments. in {0}", GetMixinString(arguments)));

                    if (position < Arguments.Count)
                        newArgument = clonedArguments[position];
                }
                else
                {
                    foundNamedArgument = true;
                    newArgument = clonedArguments.FirstOrDefault(v => v.Key == arg.Key);

                    if (newArgument == null)
                        throw new ParsingException(string.Format("Argument '{0}' not found. in {1}", arg.Name, GetMixinString(arguments)));
                }

                if (newArgument != null) // should not be null here, should throw if null above.
                {
                    newArgument.Value = (Expression) arg.Value.AdoptClone(newParent);
                    overridenArguments.Add(newArgument);
                }
            }

            // avoid recursion in the case where you give an argument the same name as it's value
            foreach (var argument in clonedArguments.Except(overridenArguments))
                argument.Value = (Expression) argument.Value.AdoptClone(newParent.Parent);

            argumentBlock.Children.AddRange(clonedArguments.Cast<INode>());

            return rules;
        }

        private string GetMixinString(IEnumerable<Variable> arguments)
        {
            return string.Format("'{0}({1})'", Name, GetArgumentString(arguments));
        }

        private static string GetArgumentString(IEnumerable<Variable> arguments)
        {
            Func<Variable, string> argString = a => char.IsNumber(a.Key[0]) ? a.Value.ToString() : a.ToString();

            return arguments.Select(argString).JoinStrings(", ");
        }
    }
}