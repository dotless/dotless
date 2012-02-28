#if CSS3EXPERIMENTAL
namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class RepeatEntity : Node
    {
        public Node Value { get; set; }
        public Node RepeatCount { get; set; }

        public RepeatEntity(Node value, Node repeatCount)
        {
            Value = value;
            RepeatCount = repeatCount;
        }

        public override Node Evaluate(Env env)
        {
            var value = Value.Evaluate(env);
            var repeatCount = RepeatCount.Evaluate(env);
            return new RepeatEntity(value, repeatCount);
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append(Value)
                .Append('[')
                .Append(RepeatCount)
                .Append(']');
        }

        public override void Accept(IVisitor visitor)
        {
            Value = VisitAndReplace(Value, visitor);
            RepeatCount = VisitAndReplace(RepeatCount, visitor);
        }
    }
}
#endif