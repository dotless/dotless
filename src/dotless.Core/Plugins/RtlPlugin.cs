namespace dotless.Core.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using dotless.Core.Parser.Infrastructure.Nodes;
    using dotless.Core.Parser.Tree;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.ComponentModel;

    [DisplayName("Rtl"), Description("Reverses some css when in rtl mode")]
    public class RtlPlugin : VisitorPlugin
    {
        public RtlPlugin(bool onlyReversePrefixedRules, bool forceRtlTransform) : this()
        {
            OnlyReversePrefixedRules = onlyReversePrefixedRules;
            ForceRtlTransform = forceRtlTransform;
        }

        public RtlPlugin()
        {
            PropertiesToReverse = new List<string>()
            {
                "border-left",
                "border-right",
                "border-width",
                "margin",
                "padding",
                "float",
                "right",
                "left",
                "text-align"
            };
        }

        public bool OnlyReversePrefixedRules
        {
            get;
            set;
        }

        public bool ForceRtlTransform
        {
            get;
            set;
        }

        public IEnumerable<string> PropertiesToReverse
        {
            get;
            set;
        }

        public override VisitorPluginType AppliesTo
        {
            get { return VisitorPluginType.AfterEvaluation; }
        }

        public override void OnPreVisiting(Parser.Infrastructure.Env env)
        {
            base.OnPreVisiting(env);

            bool isRtl = ForceRtlTransform || CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;

            PrefixesToProcess = new List<Prefix>();

            if (!OnlyReversePrefixedRules && isRtl)
            {
                foreach (string property in PropertiesToReverse)
                {
                    PrefixesToProcess.Add(new Prefix()
                    {
                        KeepRule = true,
                        PrefixString = property,
                        RemovePrefix = false,
                        Reverse = true
                    });
                }
            }

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-rtl-reverse-",
                RemovePrefix = true,
                KeepRule = true,
                Reverse = isRtl
            });

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-ltr-reverse-",
                RemovePrefix = true,
                KeepRule = true,
                Reverse = !isRtl
            });

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-rtl-ltr-",
                RemovePrefix = true,
                KeepRule = true,
                Reverse = false
            });

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-ltr-rtl-",
                RemovePrefix = true,
                KeepRule = true,
                Reverse = false
            });

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-rtl-",
                RemovePrefix = true,
                KeepRule = isRtl
            });

            PrefixesToProcess.Add(new Prefix()
            {
                PrefixString = "-ltr-",
                RemovePrefix = true,
                KeepRule = !isRtl
            });
        }

        public override Node Execute(Node node, out bool visitDeeper)
        {
            Rule rule = node as Rule;
            if (rule != null)
            {
                visitDeeper = false;

                string ruleName = rule.Name.ToLowerInvariant();

                foreach (Prefix prefix in PrefixesToProcess)
                {
                    if (ruleName.StartsWith(prefix.PrefixString))
                    {
                        if (!prefix.KeepRule)
                        {
                            return null;
                        }

                        if (prefix.RemovePrefix)
                        {
                            rule.Name = rule.Name.Substring(prefix.PrefixString.Length);
                        }

                        if (prefix.Reverse)
                        {
                            if (rule.Name.IndexOf("right", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                rule.Name = Replace(rule.Name, "right", "left", StringComparison.InvariantCultureIgnoreCase);
                                return rule;
                            }

                            if (rule.Name.IndexOf("left", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                rule.Name = Replace(rule.Name, "left", "right", StringComparison.InvariantCultureIgnoreCase);
                                return rule;
                            }

                            if (rule.Name.IndexOf("top", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                rule.Name.IndexOf("bottom", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                return rule;
                            }

                            ValuesReverserVisitor reverser = new ValuesReverserVisitor();
                            return reverser.ReverseRule(rule);
                        }
                        else
                        {
                            return rule;
                        }
                    }
                }
            }
            visitDeeper = true;
            return node;
        }

        private string Replace(string haystack, string needle, string replacement, StringComparison comparisonType)
        {
            int index = haystack.IndexOf(needle, comparisonType);
            if (index < 0)
            {
                return haystack;
            }
            return haystack.Substring(0, index) + replacement + haystack.Substring(index + needle.Length);

        }

        private List<Prefix> PrefixesToProcess { get; set; }

        private class Prefix
        {
            public string PrefixString { get; set; }
            public bool KeepRule { get; set; }
            public bool Reverse { get; set; }
            public bool RemovePrefix { get; set; }
        }

        private class ValuesReverserVisitor : IVisitor
        {
            private StringBuilder _textContent = new StringBuilder();
            private List<Node> _nodeContent = new List<Node>();

            public Rule ReverseRule(Rule rule)
            {
                rule.Accept(this);

                // we have the values.. now we can reverse it
                string content = _textContent.ToString();
                string important = "";

                var value = rule.Value as Value;
                if (value != null)
                {
                    important = value.Important;
                }

                bool valueChanged = false;

                if (_nodeContent.Count > 1)
                {
                    if (_nodeContent.Count == 4)
                    {
                        Node tmp = _nodeContent[1];
                        _nodeContent[1] = _nodeContent[3];
                        _nodeContent[3] = tmp;
                        return new Rule(rule.Name, new Expression(_nodeContent)).ReducedFrom<Rule>(rule);
                    }
                }
                else
                {
                    if (content == "left")
                    {
                        content = ("right " + important).TrimEnd();
                        valueChanged = true;
                    }
                    else if (content == "right")
                    {
                        content = ("left " + important).TrimEnd();
                        valueChanged = true;
                    }
                    else
                    {
                        string[] items = content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items.Length == 4)
                        {
                            string temp = items[1];
                            items[1] = items[3];
                            items[3] = temp;
                            content = String.Join(" ", items);
                            valueChanged = true;
                        }
                    }
            
                    if (valueChanged)
                    {
                        return new Rule(rule.Name, new TextNode(content)).ReducedFrom<Rule>(rule);
                    }
                }

                return rule;
            }

            #region IVisitor Members

            public Node Visit(Node node)
            {
                TextNode tn = node as TextNode;

                if (tn != null)
                {
                    _textContent.Append(tn.Value);
                    _nodeContent.Add(tn);
                    return node;
                }

                Number number = node as Number;

                if (number != null)
                {
                    _nodeContent.Add(number);
                    return node;
                }

                Keyword keyword = node as Keyword;

                if (keyword != null)
                {
                    _nodeContent.Add(keyword);
                    _textContent.Append(keyword.Value);
                    return node;
                }

                node.Accept(this);

                return node;
            }

            #endregion
        }
    }
}
