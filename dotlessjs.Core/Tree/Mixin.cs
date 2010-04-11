using System.Collections.Generic;
using System.Linq;
using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Utils;

// ReSharper disable RedundantNameQualifier

namespace dotless.Tree
{
  public class Mixin
  {
    public class Definition : Ruleset
    {
      public string Name { get; set; }
      public NodeList<Rule> Params { get; set; }

      public Definition(string name, NodeList<Rule> parameters, List<Node> rules)
      {
        Name = name;
        Params = parameters;
        Rules = rules;
        Selectors = new NodeList<Selector> {new Selector(new NodeList<Element>(new Element(null, name)))};
      }

      public Ruleset Evaluate(NodeList<Expression> args, Env env)
      {
        if(args)
          Guard.ExpectMaxArguments(Params.Count, args.Count, string.Format("'{0}'", Name));
        
        var frame = new Ruleset(null, new List<Node>());

        for (var i = 0; i < Params.Count; i++)
        {
          Node val;
          if (args && i < args.Count)
            val = args[i];
          else
            val = Params[i].Value;

          if (val)
            frame.Rules.Add(new Rule(Params[i].Name, val));
          else
            throw new ParsingException("wrong number of arguments for " + Name);
        }

        var frames = new[] {this, frame}.Concat(env.Frames).Reverse();
        var context = new Env{ Frames = new Stack<Ruleset>(frames) };

        var newRules = new List<Node>();

        foreach (var rule in Rules)
        {
          if (rule is Mixin.Definition)
          {
            var mixin = rule as Mixin.Definition;
            var parameters = mixin.Params.Concat(frame.Rules.Cast<Rule>());
            newRules.Add(new Mixin.Definition(mixin.Name, new NodeList<Rule>(parameters), mixin.Rules));
          }
          else if (rule is Ruleset)
          {
            var ruleset = (rule as Ruleset);
            var rules = ruleset.Rules
              .Select(r => r.Evaluate(context))
              .ToList();

            newRules.Add(new Ruleset(ruleset.Selectors, rules));
          }
          else if (rule is Mixin.Call)
          {
            newRules.AddRange((NodeList)rule.Evaluate(context));
          }
          else
          {
            newRules.Add(rule.Evaluate(context));
          }
        }

        return new Ruleset(null, newRules);
      }

      public override string ToCSS(List<IEnumerable<Selector>> list, Env env)
      {
        return "";
      }
    }

    public class Call : Node, IEvaluatable
    {
      public NodeList<Expression> Arguments { get; set; }
      protected Selector Selector { get; set; }

      public Call(NodeList<Element> elements, NodeList<Expression> arguments)
      {
        Selector = new Selector(elements);
        Arguments = arguments;
      }

      public override Node Evaluate(Env env)
      {
        foreach (var frame in env.Frames)
        {
          NodeList mixins;
          if ((mixins = frame.Find(Selector, null)).Count == 0) 
            continue;

          var rules = new NodeList();
          foreach (var node in mixins)
          {
            if (node is Mixin.Definition)
            {
              var mixin = node as Mixin.Definition;
              rules.AddRange(mixin.Evaluate(Arguments, env).Rules);
            }
            else if (node is Ruleset)
            {
              var ruleset = node as Ruleset;
              if (ruleset.Rules != null)
                rules.AddRange(ruleset.Rules);
            }
            // todo fix for other Ruleset types?
          }
          return rules;
        }
        throw new ParsingException(Selector.ToCSS(env).Trim() + " is undefined");
      }
    }
  }
}