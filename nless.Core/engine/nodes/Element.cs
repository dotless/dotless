#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace nless.Core.engine
{
    public class Element : List<INode>, INode
    {
        public Element(List<INode> rules, List<INode> set, string name, string selector)
        {
            Rules = rules;
            Set = set;
            Name = name;
            Selector = Selector.Get(selector);
        }

        public List<INode> Rules { get; set; }
        public List<INode> Set { get; set; }
        public string Name { get; set; }
        public Selector Selector { get; set; }

        public bool IsTag
        {
            get { return !IsId || !IsClass || !IsUniversal; }
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
            get { return !IsClass && !IsId; }
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
                return Rules.Where(r => r is Property)
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Variable> Variables
        {
            get
            {
                return Rules.Where(r => r is Variable)
                    .Select(r => (Variable) r).ToList();
            }
        }

        public IList<Element> Elements
        {
            get
            {
                return Rules.Where(r => r is Element)
                    .Select(r => (Element) r).ToList();
            }
        }

        #region INode Members

        public INode Parent { get; set; }

        public virtual string ToCss()
        {
            return "";
        }

        public virtual string ToCSharp()
        {
            return "";
        }

        #endregion
    }
}