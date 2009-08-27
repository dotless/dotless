using System.Collections.Generic;
using System.Linq;
using System.Text;
using nless.Core.Exceptions;
using nless.Core.utils;

namespace nless.Core.engine
{
    public class Element : INode, INearestResolver
    {
        public Element(string name, string selector)
        {
            Rules = new List<INode>();
            Set = new List<Element>();
            Name = name;
            Selector = selector!=null ? Selector.Get(selector) : Selector.Get("");
        }

        public Element(string name) : this(name, null)
        {
        }

        public Element() : this(string.Empty)
        {
        }

        public List<INode> Rules { get; set; }
        public List<Element> Set { get; set; }
        public string Name { get; set; }
        public Selector Selector { get; set; }
        public Element Last
        {
            get
            {
                return Elements.LastOrDefault();
            }
        }
       public Element First
        {
            get
            {
                return Elements.FirstOrDefault();
            }
        }
        public bool IsTag
        {
            get { return !IsId || !IsClass || !IsUniversal; }
        }

      //def << obj
      //  if obj.kind_of? Node::Entity
      //    obj.parent = self
      //    @rules << obj
      //  else
      //    raise ArgumentError, "argument can't be a #{obj.class}"
      //  end
      //end
        public void Add(INode token)
        {
            token.Parent = this;
            Rules.Add(token);
        }

        //def class?;     name =~ /^\./ end
        //def id?;        name =~ /^#/  end
        //def universal?; name == '*'   end
        public bool IsClass
        {
            get { return Name.StartsWith("."); }
        }

        public bool IsId
        {
            get { return Name.StartsWith("#"); }
        }

        public bool IsUniversal
        {
            get { return Name == "*"; }
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        public bool IsEmpty
        {
            get { return Rules.Count() == 0; }
        }

        public bool IsLeaf
        {
            get { return Rules.Count() == 0; }
        }


        public Element GetRoot()
        {
            var els = this;
            while(!els.IsRoot)
            {
                els = (Element)els.Parent;
            }
            return els;
        }
        public IList<Property> Identifiers
        {
            get
            {
                return Rules.Where(r => r is Property)
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Property> Properties
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Property))
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Variable> Variables
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Variable))
                    .Select(r => (Variable) r).ToList();
            }
        }

        public IList<Element> Elements
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Element))
                    .Select(r => (Element) r).ToList();
            }
        }

        public INode Get(string key)
        {
            var rules = All().SelectMany(e => e.Rules);
            foreach(var rule in rules){
                if (rule.ToString() == key)
                    return rule;
            }
            return null;
        }
         //@rules.find {|i| i.eql? key 

        public INode Get(INode key)
        {
            return Rules.Where(r => r.Equals(key)).FirstOrDefault();
        }
        public T GetAs<T>(string key)
        {
            return (T)Get(key);
        }
        public T GetAs<T>(INode key)
        {
            return (T)Get(key);
        }
        #region INode Members

        public INode Parent { get; set; }

        public virtual string ToCSharp()
        {
            return "";
        }

        public IList<INode> Path(INode node)
        {
            var path = new List<INode>();
            if (node == null) node = this;
            while (node != null){
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }
        public IList<INode> Path()
        {
            return Path(this);
        }
        #endregion

        public IList<Element> All()
        {
            var path = new List<Element>();
            if(!path.Contains(this)) path.Add(this);
            foreach(var element in Elements)
            {
                path.Add(element);
                path.AddRange(element.All());
            }
            return path;
        }

        public struct ElementPath
        {
            public ElementPath(Element element, Stack<INode> path)
                : this()
            {
                Element = element;
                Path = path;
            }

            public Element Element { get; set; }
            public Stack<INode> Path { get; set; }
        }


        public virtual string ToCss()
        {
            return ToCss(new List<string>());
        }

        public virtual string ToCss(IList<string> path)
        {
            if (!IsRoot){
                path.Add(Selector.ToCss());
                path.Add(Name);
            }
            var properties = new StringBuilder();
            foreach (var prop in Properties) properties.AppendLine(string.Format("  {0}", prop.ToCss()));

            var setArray = Set.Select(s => s.Name).ToArray();
            var pathContent =  string.Join(string.Empty, path.Where(p => !string.IsNullOrEmpty(p)).ToArray());
            var setContent = new StringBuilder(pathContent);

            foreach (var setItem in setArray)
                setContent.Append(setItem);

            var propContent = string.Format("{{\n{0}}}\n", properties);
            var ruleset = properties.Length != 0 ? (setContent + propContent) : "";
            return ruleset + GetChildCss(path);
        }

        private string GetChildCss(IEnumerable<string> path)
        {
            var css = new StringBuilder();
            foreach(var element in Elements)
            {
                css.Append(element.ToCss(new List<string>(path)));
            }
            return css.ToString();
        }

        public Element Group()
        {
            var matched = false;
            var stack = new Stack<Element>(Elements.Reverse());
            var result = new List<Element>();

            if(Elements.Count() == 0) return this;

            foreach (var element in Elements){
                var e = stack.First();
                
                if(!matched) result.Add(e);
                matched = true;

                if (stack.Count() > 1){
                    for (var i = 1; i < stack.Count; i++ ){
                        var ee = stack.ToArray()[i];
                        if (ee.IsEquiv(e) && e.Elements.Count == 0){
                            GetAs<Element>(e).Set.Add(ee);
                            //Set.Add(ee);
                            stack.Pop();
                        }
                        else{
                            stack.Pop();
                            matched = false;
                            break;
                        }
                    }
                }
            }
            var grpEls = Elements.Where(e => !result.Contains(e)).ToList();
            if (grpEls.Count > 0){
                foreach (var gpEl in grpEls)
                    Rules.Remove(gpEl);
            }
            return this;
        }
  
        private bool IsEquiv(Element other)
        {
            var equiv = Rules.Count() == other.Rules.Count();
            var differentToCss =
               Rules.SelectMany(a => other.Rules, (a, b) => new {a, b})
                    .Where(@t => @t.a.ToCss() != @t.b.ToCss())
                    .Select(@t => @t.a);
            return equiv && differentToCss.Count() == 0;
        }

        internal Element Descend(Selector selector, Element element)
        {
            Selector s = null;
            if (selector is Child)
            {
                s = GetAs<Element>(element.Name).Selector;
                if (s is Child || s is Descendant) return GetAs<Element>(element.Name);
            }
            else if(selector is Descendant)
                return GetAs<Element>(element.Name);
            else
            {
                s = GetAs<Element>(element.Name).Selector;
                if (s.GetType() == selector.GetType()) return GetAs<Element>(element.Name);
            }
            return null;
        }

        public INode Nearest(string ident)
        {
            INode node = null; 
            foreach(var el in Path().Where(n => n is Element).Select(n => (Element)n)){
                var ary = ident.IsIdent() ? el.Elements.Select(n => (INode)n).ToList() : el.Variables.Select(n => (INode)n).ToList();
                node = ary.Where(i => i.ToString() == ident).FirstOrDefault();
                if(node!=null) break;
            }
            if (node == null)  throw new VariableNameException(ident);
            return node;
        }

        public T NearestAs<T>(string ident)
        {
            return (T) Nearest(ident);
        }
        public override string ToString()
        {
            return IsRoot ? "*" : Name;
        }

/*      def nearest ident
        ary = ident =~ /^[.#]/ ? :elements : :variables
        path.map do |node|
          node.send(ary).find {|i| i.to_s == ident }
        end.compact.first.tap do |result|
          raise VariableNameError, ident unless result
        end
      end*/
    }
}