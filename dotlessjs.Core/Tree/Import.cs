using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Import : Node, IEvaluatable
  {
    public string Path { get; set; }
    protected Node OriginalPath { get; set; }
    protected bool Css { get; set; }
    public Ruleset Root { get; set; }

    public Import(Quoted path, Importer importer)
      : this(path.Contents, importer)
    {
      OriginalPath = path;
    }

    public Import(Url path, Importer importer)
      : this(path.GetUrl(null), importer)
    {
      OriginalPath = path;
    }

    private Import(string path, Importer importer)
    {
      var regex = new Regex(@"\.(le|c)ss$");

      Path = regex.IsMatch(path) ? path : path + ".less";

      Css = Path.EndsWith("css");

      // Only pre-compile .less files
      if (!Css)
        importer.Import(this);
    }

    public override string  ToCSS(Env env)
    {
      if (Css)
        return "@import " + OriginalPath.ToCSS(env) + ";\n";

      return "";
    }

    public override Node Evaluate(Env env)
    {
      if (Css)
        return new NodeList(this);

      var rules = Root.Rules
        .SelectMany(r => r is Import ? r.Evaluate(env) as IEnumerable<Node> : new[] {r});

      Root.Rules = new NodeList(rules);

      return Root.Rules;
    }
  }
}