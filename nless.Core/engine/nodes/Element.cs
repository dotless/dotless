using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return Rules.Where(r => r.ToString() == key).FirstOrDefault();
        }
        public INode Get(INode key)
        {
            return Rules.Where(r => r.Equals(key)).FirstOrDefault();
        }
        public T GetAs<T>(INode key)
        {
            return (T)Rules.Where(r => r.Equals(key)).FirstOrDefault();
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


        //TODO: Dont understand why this is called each and not Leafs or what rge &blk all about
        //TODO: This isnt working, I need to determine wtf is supposed to happen
        public IEnumerable<ElementPath> Each(Stack<INode> path)
        {
            foreach(var element in Elements)
            {
                path.Push(element);
                if(element.IsLeaf) yield return new ElementPath(element, path);
                element.Each(path);
                path.Pop();
            }
        }

        public IEnumerable<ElementPath> Each()
        {
            return Each(new Stack<INode>());
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
            if(!IsRoot)
            {
                path.Add(Selector.ToCss());
                path.Add(Name);
            }
            var properties = new StringBuilder();            
            foreach(var prop in Properties){
                properties.AppendLine(string.Format("  {0}", prop.ToCss()));
            }
            var setContent = GetElementContent(path);
            var ruleset = properties.Length != 0 ?
                              (setContent + 
                              string.Format("{{{0}}}\n", properties)).Substring(2)
                              : "";
            return ruleset + GetChildCss(path);
        }

        private string GetElementContent(IEnumerable<string> path)
        {
            var pathList = path.Where(e => e != string.Empty).ToList().Distinct().ToArray();
            var setList = Set.Select(s=> s.Name).Distinct()
                .Where(r => !pathList.Contains(r)).Distinct().ToArray();
         
            //N.B. Im sure this isnt right but im doing it this way after fucking around with IRB and getting simmilar results as this would cause
            var elementContent = string.Join(" ", pathList) + string.Join("", setList.Select(s => string.Format(",{0}", s)).ToArray());
            return elementContent;
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


        //def to_css path = []
      //  path << @selector.to_css << name unless root?

      //  content = properties.map do |i|
      //    ' ' * 2 + i.to_css
      //  end.compact.reject(&:empty?) * "\n"

      //  content = content.include?("\n") ?
      //    "\n#{content}\n" : " #{content.strip} "
      //  ruleset = !content.strip.empty??
      //    "#{[path.reject(&:empty?).join.strip,
      //    *@set.map(&:name)].uniq * ', '} {#{content}}\n" : ""

      //  css = ruleset + elements.map do |i|
      //    i.to_css(path)
      //  end.reject(&:empty?).join
      //  path.pop; path.pop
      //  css
      //end

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

        //# Group similar rulesets together
        //# This is horrible, horrible code,
        //# but it'll have to do until I find
        //# a proper way to do it.
        //def group
        //  matched = false
        //  stack, result = elements.dup, []                #CO: dup seems to reverse as well as clone
        //  return self unless elements.size > 1

        //  elements.each do
        //    e = stack.first                               #CO: First of ones removed
        //    result << e unless matched                    #CO: Add to result if all elements aren't matched

        //    matched = stack[1..-1].each do |ee|           #CO: get from 1 > end (so every element except the first 
        //      if e.equiv? ee and e.elements.size == 0     #CO: if css matches and there are no other ELEMENTS below it (other elements below will group later on it)
        //        self[e].set << ee                         #CO: 
        //        stack.shift                               #CO: Removes first element of array
        //      else
        //        stack.shift                               #CO: Removes first element of array
        //        break false                               #CO: break loop and return false to matched (else all elements matched)
        //      end
        //    end if stack.size > 1                         #CO: Only do if there are items in stack to match against
        //  end
        //  @rules -= (elements - result)                   
        //  self
        //end
  
        private bool IsEquiv(Element other)
        {
            var equiv = Rules.Count() == other.Rules.Count();
            var differentToCss =
               Rules.SelectMany(a => other.Rules, (a, b) => new {a, b})
                    .Where(@t => @t.a.ToCss() != @t.b.ToCss())
                    .Select(@t => @t.a);
            return equiv && differentToCss.Count() == 0;
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