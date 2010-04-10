using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Ruleset : Node
  {
    public NodeList<Selector> Selectors { get; set; }
    public NodeList Rules { get; set; }
    public bool Root { get; set; }

    private Dictionary<string, NodeList> _lookups;
    private List<Rule> _variables;
    private List<Ruleset> _rulesets;

    public Ruleset(NodeList<Selector> selectors, NodeList rules)
    {
      Selectors = selectors;
      Rules = rules;
      _lookups = new Dictionary<string, NodeList>();
    }

    protected Ruleset()
    {}

    public List<Rule> Variables()
    {
      if (_variables != null)
        return _variables;

      return _variables = Rules.Where(r => r is Rule).Cast<Rule>().Where(r => r.Variable).ToList();
    }

    public List<Ruleset> Rulesets()
    {
      if (_rulesets != null)
        return _rulesets;

      return _rulesets = Rules.Where(r => r is Ruleset).Cast<Ruleset>().ToList();
    }
    
    public NodeList Find(Selector selector, Ruleset self) {
      self = self ?? this;
      var rules = new NodeList();
      var key = selector.ToCSS(null);

      if (_lookups.ContainsKey(key))
        return _lookups[key];

      foreach (var rule in Rulesets().Where(rule => rule != self))
      {
        if (rule.Selectors && rule.Selectors.Any(selector.Match))
        {
          if (selector.Elements.Count > 1)
          {
            var remainingSelectors = new Selector(new NodeList<Element>(selector.Elements.Skip(1)));
            rules.AddRange(rule.Find(remainingSelectors, self));
          }
          else
            rules.Add(rule);
        }
      }
      return _lookups[key] = rules;
    }

    //
    // Entry point for code generation
    //
    //     `context` holds an array of arrays.
    //
    public override string ToCSS(Env env)
    {
      return ToCSS(new List<IEnumerable<Selector>>(), env);
    }

    public virtual string ToCSS(List<IEnumerable<Selector>> context, Env env)
    {
      if(Rules.Count == 0)
        return "";

      var css = new List<string>();                   // The CSS output
      var rules = new List<string>();                 // node.Rule instances
      var rulesets = new List<string>();              // node.Ruleset instances
      var paths = new List<IEnumerable<Selector>>();  // Current selectors

      if (!Root)
      {
        if (context.Count == 0)
          paths = Selectors.Select(s => (IEnumerable<Selector>) new List<Selector> {s}).ToList();
        else
        {
          foreach (var sel in Selectors)
          {
            paths.AddRange(context.Select(t => t.Concat(new[] {sel})));
          }
        }
      }
      else
      {
        env = env ?? new Env();

        var importRules = Rules.SelectMany(r => r is Import ? (IEnumerable<Node>) r.Evaluate(env) : new[] {r});
        Rules = new NodeList(importRules);
      }

      // push the current ruleset to the frames stack
      env.Frames.Push(this);

      // Evaluate mixins
      var mixinRules = Rules.SelectMany(r => r is Mixin.Call ? (IEnumerable<Node>) r.Evaluate(env) : new[] {r});
      Rules = new NodeList(mixinRules);

      // Evaluate rules and rulesets
      foreach (var rule in Rules)
      {
        if (rule is Ruleset)
        {
          var ruleset = (Ruleset) rule;
          rulesets.Add(ruleset.ToCSS(paths, env));
        }
        else if (rule is Comment)
        {
          if (Root)
            rulesets.Add(rule.ToCSS(env));
          else
            rules.Add(rule.ToCSS(env));
        }
        else if (rule is Rule)
        {
          var r = (rule as Rule);

          if (!r.Variable)
            rules.Add(r.ToCSS(env));
        }
        else if(!(rule is TextNode))
        {
          rules.Add(rule.ToCSS(env));
        }
      }


      var rulesetsStr = rulesets.JoinStrings("");

      // If this is the root node, we don't render
      // a selector, or {}.
      // Otherwise, only output if this ruleset has rules.
      if (Root)
        css.Add(rules.JoinStrings("\n"));
      else
      {
        if (rules.Count > 0)
        {
          var selector = paths.Select(p => p.Select(s => s.ToCSS(null)).JoinStrings("").Trim()).
            JoinStrings(paths.Count > 3 ? ",\n" : ", "); // The fully rendered selector
          css.Add(selector);
          css.Add(" {\n  " + rules.JoinStrings("\n  ") + "\n}\n");
        }
      }
      css.Add(rulesetsStr);

      // Pop the stack
      env.Frames.Pop();

      return css.JoinStrings("");
    }
  }
}