namespace dotless.Core.Parser.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Functions;
    using Nodes;
    using Plugins;
    using Tree;
    using dotless.Core.Loggers;

    public class Env
    {
        private readonly Dictionary<string, Type> _functionTypes;
        private readonly List<IPlugin> _plugins;
        private readonly List<Extender> _extensions;

        public Stack<Ruleset> Frames { get; protected set; }
        public bool Compress { get; set; }
        public bool Debug { get; set; }
        public Node Rule { get; set; }
        public ILogger Logger { get; set; }
        public Output Output { get; private set; }
        public Stack<Media> MediaPath { get; private set; }
        public List<Media> MediaBlocks { get; private set; }
        public bool DisableVariableRedefines { get; set; }
        public bool DisableColorCompression { get; set; }
        public bool KeepFirstSpecialComment { get; set; }
        public bool IsFirstSpecialCommentOutput { get; set; }

        public Env() : this(null, null)
        {
        }

        protected Env(Stack<Ruleset> frames, Dictionary<string, Type> functions)
        {
            Frames = frames ?? new Stack<Ruleset>();
            Output = new Output(this);
            MediaPath = new Stack<Media>();
            MediaBlocks = new List<Media>();
            Logger = new NullLogger(LogLevel.Info);

            _plugins = new List<IPlugin>();
            _functionTypes = functions ?? new Dictionary<string, Type>();
            _extensions = new List<Extender>();
            ExtendMediaScope = new Stack<Media>();

            if (_functionTypes.Count == 0)
                AddCoreFunctions();
        }

        /// <summary>
        ///  Creates a new Env variable for the purposes of scope
        /// </summary>
        public virtual Env CreateChildEnv(Stack<Ruleset> frames)
        {
            return new Env(frames, _functionTypes)
            {
                Parent = this,
                Debug = Debug,
                Compress = Compress,
                DisableColorCompression = DisableColorCompression,
                DisableVariableRedefines = DisableVariableRedefines
            };
        }

        private Env Parent { get; set; }

        /// <summary>
        ///  Creates a new Env variable for the purposes of scope
        /// </summary>
        public virtual Env CreateVariableEvaluationEnv(string variableName, Stack<Ruleset> frames) {
            var env = CreateChildEnv(frames);
            env.EvaluatingVariable = variableName;
            return env;
        }

        private string EvaluatingVariable { get; set; }

        public bool IsEvaluatingVariable(string variableName) {
            if (string.Equals(variableName, EvaluatingVariable, StringComparison.InvariantCulture)) {
                return true;
            }

            if (Parent != null) {
                return Parent.IsEvaluatingVariable(variableName);
            }

            return false;
        }

        /// <summary>
        ///  Adds a plugin to this Env
        /// </summary>
        public void AddPlugin(IPlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            _plugins.Add(plugin);

            IFunctionPlugin functionPlugin = plugin as IFunctionPlugin;
            if (functionPlugin != null)
            {
                foreach(KeyValuePair<string, Type> function in functionPlugin.GetFunctions())
                {
                    string functionName = function.Key.ToLowerInvariant();

                    if (_functionTypes.ContainsKey(functionName))
                    {
                        string message = string.Format("Function '{0}' already exists in environment but is added by plugin {1}",
                            functionName, plugin.GetName());
                        throw new InvalidOperationException(message);
                    }

                    AddFunction(functionName, function.Value);
                 }
            }
        }

        /// <summary>
        ///  All the visitor plugins to use
        /// </summary>
        public IEnumerable<IVisitorPlugin> VisitorPlugins
        {
            get
            {
                return _plugins.OfType<IVisitorPlugin>();
            }
        }

        //Keep track of media scoping for extenders
        public Stack<Media> ExtendMediaScope { get; set; }

        /// <summary>
        ///  Returns whether the comment should be silent
        /// </summary>
        /// <param name="isDoubleStarComment"></param>
        /// <returns></returns>
        public bool IsCommentSilent(bool isValidCss, bool isCssHack, bool isSpecialComment)
        {
            if (!isValidCss)
                return true;

            if (isCssHack)
                return false;

            if (Compress && KeepFirstSpecialComment && !IsFirstSpecialCommentOutput && isSpecialComment)
            {
                IsFirstSpecialCommentOutput = true;
                return false;
            }

            return Compress;
        }

        /// <summary>
        ///  Finds the first scoped variable with this name
        /// </summary>
        public Rule FindVariable(string name)
        {
            return FindVariable(name, Rule);
        }

        /// <summary>
        ///  Finds the first scoped variable matching the name, using Rule as the current rule to work backwards from
        /// </summary>
        public Rule FindVariable(string name, Node rule)
        {
            var previousNode = rule;
            foreach (var frame in Frames)
            {
                var v = frame.Variable(name, DisableVariableRedefines ? null : previousNode);
                if (v)
                    return v;
                previousNode = frame;
            }
            return null;
        }

        /// <summary>
        ///  Finds the first Ruleset matching the selector argument that inherits from or is of type TRuleset (pass this as Ruleset if
        ///  you are trying to find ANY Ruleset that matches the selector)
        /// </summary>
        public IEnumerable<Closure> FindRulesets<TRuleset>(Selector selector) where TRuleset : Ruleset
        {
            return Frames
                .Select(frame => frame.Find<TRuleset>(this, selector, null))
                .Select(
                    matchedClosuresList => matchedClosuresList.Where(
                            matchedClosure => {
                                if (!Frames.Any(frame => frame.IsEqualOrClonedFrom(matchedClosure.Ruleset)))
                                    return true;

                                var mixinDef = matchedClosure.Ruleset as MixinDefinition;
                                if (mixinDef != null)
                                    return mixinDef.Condition != null;

                                return false;
                        }
                    )
                )
                .FirstOrDefault(matchedClosuresList => matchedClosuresList.Count() != 0);
        }

        /// <summary>
        ///  Adds a Function to this Env object
        /// </summary>
        public void AddFunction(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");

            _functionTypes[name] = type;
        }

        /// <summary>
        ///  Given an assembly, adds all the dotless Functions in that assembly into this Env.
        /// </summary>
        public void AddFunctionsFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var functionType = typeof (Function);

            foreach (var func in assembly
                .GetTypes()
                .Where(t => functionType.IsAssignableFrom(t) && t != functionType)
                .Where(t => !t.IsAbstract)
                .SelectMany<Type, KeyValuePair<string, Type>>(GetFunctionNames))
            {
                AddFunction(func.Key, func.Value);
            }
        }

        private void AddCoreFunctions()
        {
            AddFunctionsFromAssembly(Assembly.GetExecutingAssembly());
            AddFunction("%", typeof (CFormatString));
        }

        /// <summary>
        ///  Given a function name, returns a new Function matching that name.
        /// </summary>
        public virtual Function GetFunction(string name)
        {
            Function function = null;
            name = name.ToLowerInvariant();

            if (_functionTypes.ContainsKey(name))
            {
                function = (Function)Activator.CreateInstance(_functionTypes[name]);
                function.Logger = Logger;
            }

            return function;
        }

        private static IEnumerable<KeyValuePair<string, Type>> GetFunctionNames(Type t)
        {
            var name = t.Name;

            if (name.EndsWith("function", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0, name.Length - 8);

            name = Regex.Replace(name, @"\B[A-Z]", "-$0");

            name = name.ToLowerInvariant();

            yield return new KeyValuePair<string, Type>(name, t);

            if(name.Contains("-"))
                yield return new KeyValuePair<string, Type>(name.Replace("-", ""), t);
        }

        public void AddExtension(Selector selector, Extend extends, Env env)
        {
            foreach (var extending in extends.Exact)
            {
                Extender match = null;
                if ((match = _extensions.OfType<ExactExtender>().FirstOrDefault(e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim())) == null)
                {
                    match = new ExactExtender(extending);
                    _extensions.Add(match);
                }

                match.AddExtension(selector,env);
            }

            foreach (var extending in extends.Partial)
            {
                Extender match = null;
                if ((match = _extensions.OfType<PartialExtender>().FirstOrDefault(e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim())) == null)
                {
                    match = new PartialExtender(extending);
                    _extensions.Add(match);
                }

                match.AddExtension(selector,env);
            }
        }

        public ExactExtender FindExactExtension(string selection)
        {
            if (ExtendMediaScope.Any())
            {
                return ExtendMediaScope.Select(media => media.FindExactExtension(selection)).FirstOrDefault(result => result != null);
            }

            return _extensions.OfType<ExactExtender>().FirstOrDefault(e => e.BaseSelector.ToString().Trim() == selection);
        }

        public PartialExtender[] FindPartialExtensions(string selection)
        {
            if (ExtendMediaScope.Any())
            {
                return ExtendMediaScope.Select(media => media.FindPartialExtensions(selection)).FirstOrDefault(result => result.Any());
            }

            return _extensions.OfType<PartialExtender>().Where(e => selection.Contains(e.BaseSelector.ToString().Trim())).ToArray();
        }
    }
}