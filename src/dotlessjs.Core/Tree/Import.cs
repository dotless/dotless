using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Import : Directive
  {
    public string Path { get; set; }
    protected Node OriginalPath { get; set; }
    protected bool Css { get; set; }
    public Ruleset InnerRoot { get; set; }

    public Import(Quoted path, Importer importer)
      : this(path.Contents, importer)
    {
      OriginalPath = path;
    }

    public Import(Url path, Importer importer)
      : this(path.GetUrl(), importer)
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
        importer.Import(this); // TODO: move this into Evaluate()
    }

    protected override string ToCSS(List<IEnumerable<Selector>> context)
    {
      return base.ToCSS(); // should throw InvalidOperationException
    }

    public override Node Evaluate(Env env)
    {
      if (Css)
        return new NodeList(new TextNode("@import " + OriginalPath.ToCSS() + ";\n"));

      NodeHelper.ExpandNodes<Import>(env, InnerRoot);

      return new NodeList(InnerRoot.Rules);
    }
  }
}