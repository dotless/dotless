using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Parser.Tree
{
    public class CssFunction : Node
    {
        public string Name { get; set; }

        public Node Value { get; set; }

        protected override Node CloneCore() {
            return new CssFunction() {Name = Name, Value = Value.Clone()};
        }

        public override void AppendCSS(Env env)
        {
            env.Output.Append(string.Format("{0}({1})", Name, Value.ToCSS(env)));
        }
    }

    public class CssFunctionList : NodeList
    {
        public override void AppendCSS(Env env)
        {
            env.Output.AppendMany(Inner, " ");
        }
    }
}
