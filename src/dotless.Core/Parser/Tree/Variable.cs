namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Variable : Node
    {
        public string Name { get; set; }

        public Variable(string name)
        {
            Name = name;
        }

        public override Node Evaluate(Env env)
        {
            var name = Name;
            if (name.StartsWith("@@"))
            {
                var v = new Variable(name.Substring(1)).Evaluate(env);
                name = '@' + (v is TextNode ? (v as TextNode).Value : v.ToCSS(env));
            }

            if (env.IsEvaluatingVariable(name)) {
                throw new ParsingException("Recursive variable definition for " + name, Location);
            }

            var variable = env.FindVariable(name);

            if (variable) {
                return variable.Value.Evaluate(env.CreateVariableEvaluationEnv(name));
            }

            throw new ParsingException("variable " + name + " is undefined", Location);
        }
    }
}