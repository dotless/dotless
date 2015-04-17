using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using dotless.Core.Utils;

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
        private Dictionary<string, Type> _functionTypes;
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
        public Parser Parser { get; set; }

        public Env() : this(new Parser())
        {
        }

        public Env(Parser parser) : this(parser, null, null)
        {
        }

        protected Env(Parser parser, Stack<Ruleset> frames, Dictionary<string, Type> functions) : this(frames, functions) {
            Parser = parser;
        }

        protected Env(Stack<Ruleset> frames, Dictionary<string, Type> functions) {
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
        [Obsolete("Argument is ignored as of version 1.4.3.0. Use the parameterless overload of CreateChildEnv instead.", false)]
        public virtual Env CreateChildEnv(Stack<Ruleset> ruleset)
        {
            return CreateChildEnv();
        }

        /// <summary>
        ///  Creates a new Env variable for the purposes of scope
        /// </summary>
        public virtual Env CreateChildEnv()
        {
            return new Env(null, _functionTypes)
            {
                Parser = Parser,
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
        public virtual Env CreateVariableEvaluationEnv(string variableName) {
            var env = CreateChildEnv();
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

        public virtual Env CreateChildEnvWithClosure(Closure closure) {
            var env = CreateChildEnv();
            env.Rule = Rule;
            env.ClosureEnvironment = CreateChildEnv();
            env.ClosureEnvironment.Frames = new Stack<Ruleset>(closure.Context);
            return env;
        }

        private Env ClosureEnvironment { get; set; }

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
                var v = frame.Variable(name, null);
                if (v)
                    return v;
                previousNode = frame;
            }

            Rule result = null;
            if (Parent != null) {
                result = Parent.FindVariable(name, rule);
            }

            if (result != null) {
                return result;
            }

            if (ClosureEnvironment != null) {
                return ClosureEnvironment.FindVariable(name, rule);
            }

            return null;
        }

        [Obsolete("This method will be removed in a future release.", false)]
        public IEnumerable<Closure> FindRulesets<TRuleset>(Selector selector) where TRuleset : Ruleset
        {
            return FindRulesets(selector).Where(c => c.Ruleset is TRuleset);
        }

        /// <summary>
        ///  Finds the first Ruleset matching the selector argument that inherits from or is of type TRuleset (pass this as Ruleset if
        ///  you are trying to find ANY Ruleset that matches the selector)
        /// </summary>
        public IEnumerable<Closure> FindRulesets(Selector selector)
        {
            var matchingRuleSets = Frames
                .Reverse()
                .SelectMany(frame => frame.Find<Ruleset>(this, selector, null))
                .Where(matchedClosure => {
                        if (!Frames.Any(frame => frame.IsEqualOrClonedFrom(matchedClosure.Ruleset)))
                            return true;

                        var mixinDef = matchedClosure.Ruleset as MixinDefinition;
                        if (mixinDef != null)
                            return mixinDef.Condition != null;

                        return false;
                    }).ToList();

            if (matchingRuleSets.Any())
            {
                return matchingRuleSets;
            }

            if (Parent != null)
            {
                var parentRulesets = Parent.FindRulesets(selector);
                if (parentRulesets != null)
                {
                    return parentRulesets;
                }
            }

            if (ClosureEnvironment != null)
            {
                return ClosureEnvironment.FindRulesets(selector);
            }

            return null;
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

            var functions = GetFunctionsFromAssembly(assembly);

            AddFunctionsToRegistry(functions);
        }

        private void AddFunctionsToRegistry(IEnumerable<KeyValuePair<string, Type>> functions) {
            foreach (var func in functions) {
                AddFunction(func.Key, func.Value);
            }
        }

        private static Dictionary<string, Type> GetFunctionsFromAssembly(Assembly assembly) {
            var functionType = typeof (Function);

            return assembly
                .GetTypes()
                .Where(t => functionType.IsAssignableFrom(t) && t != functionType)
                .Where(t => !t.IsAbstract)
                .SelectMany(GetFunctionNames)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private static Dictionary<string, Type> GetCoreFunctions() {
            var functions = GetFunctionsFromAssembly(Assembly.GetExecutingAssembly());
            functions["%"] = typeof (CFormatString);
            return functions;
        }

        private static readonly Dictionary<string, Type> CoreFunctions = GetCoreFunctions();

        private void AddCoreFunctions() {
            _functionTypes = CoreFunctions;
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
                    match = new ExactExtender(extending, extends);
                    _extensions.Add(match);
                }

                match.AddExtension(selector,env);
            }

            foreach (var extending in extends.Partial)
            {
                Extender match = null;
                if ((match = _extensions.OfType<PartialExtender>().FirstOrDefault(e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim())) == null)
                {
                    match = new PartialExtender(extending, extends);
                    _extensions.Add(match);
                }

                match.AddExtension(selector,env);
            }

            if (Parent != null) {
                Parent.AddExtension(selector, extends, env);
            }
        }

        public void RegisterExtensionsFrom(Env child) {
            _extensions.AddRange(child._extensions);
        }

        public IEnumerable<Extender> FindUnmatchedExtensions() {
            return _extensions.Where(e => !e.IsMatched);
        }

        public ExactExtender FindExactExtension(string selection)
        {
            if (ExtendMediaScope.Any())
            {
                var mediaScopedExtensions = ExtendMediaScope.Select(media => media.FindExactExtension(selection)).FirstOrDefault(result => result != null);
                if (mediaScopedExtensions != null) {
                    return mediaScopedExtensions;
                }
            }

            return _extensions.OfType<ExactExtender>().FirstOrDefault(e => e.BaseSelector.ToString().Trim() == selection);
        }

        public PartialExtender[] FindPartialExtensions(Context selection)
        {
            if (ExtendMediaScope.Any())
            {
                var mediaScopedExtensions = ExtendMediaScope.Select(media => media.FindPartialExtensions(selection)).FirstOrDefault(result => result.Any());
                if (mediaScopedExtensions != null) {
                    return mediaScopedExtensions;
                }
            }

            return _extensions.OfType<PartialExtender>()
                .WhereExtenderMatches(selection)
                .ToArray();
        }

        [Obsolete("This method doesn't return the correct results. Use FindPartialExtensions(Context) instead.", false)]
        public PartialExtender[] FindPartialExtensions(string selection)
        {
            if (ExtendMediaScope.Any())
            {
                var mediaScopedExtensions = ExtendMediaScope.Select(media => media.FindPartialExtensions(selection)).FirstOrDefault(result => result.Any());
                if (mediaScopedExtensions != null) {
                    return mediaScopedExtensions;
                }
            }

            return _extensions.OfType<PartialExtender>().Where(e => selection.Contains(e.BaseSelector.ToString().Trim())).ToArray();
        }

        public override string ToString()
        {
            return Frames.Select(f => f.ToString()).JoinStrings(" <- ");
        }
    }
}