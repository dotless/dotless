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

    public class Env
    {
        private readonly Dictionary<string, Type> _functionTypes;
        private readonly List<IPlugin> _plugins;

        public Stack<Ruleset> Frames { get; protected set; }
        public bool Compress { get; set; }
        public Node Rule { get; set; }
        public Output Output { get; private set; }
        public Stack<Media> MediaPath { get; private set; }
        public List<Media> MediaBlocks { get; private set; }

        public Env() : this(null, null)
        {
        }

        protected Env(Stack<Ruleset> frames, Dictionary<string, Type> functions)
        {
            Frames = frames ?? new Stack<Ruleset>();
            Output = new Output(this);
            MediaPath = new Stack<Media>();
            MediaBlocks = new List<Media>();

            _plugins = new List<IPlugin>();
            _functionTypes = functions ?? new Dictionary<string, Type>();

            if (_functionTypes.Count == 0)
                AddCoreFunctions();
        }

        /// <summary>
        ///  Creates a new Env variable for the purposes of scope
        /// </summary>
        public virtual Env CreateChildEnv(Stack<Ruleset> frames)
        {
            return new Env(frames, _functionTypes);
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

        public IEnumerable<IVisitorPlugin> VisitorPlugins
        {
            get
            {
                return _plugins.OfType<IVisitorPlugin>();
            }
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
                var v = frame.Variable(name, previousNode);
                if (v)
                    return v;
                previousNode = frame;
            }
            return null;
        }

        /// <summary>
        ///  Finds the first Ruleset matching the selector argument
        /// </summary>
        public IEnumerable<Closure> FindRulesets(Selector selector)
        {
            return Frames.Select(frame => frame.Find(this, selector, null))
                .Select(matchedClosuresList => matchedClosuresList.Where(
                            matchedClosure => {
                                if (!Frames.Any(frame => frame.IsEqualOrClonedFrom(matchedClosure.Ruleset)))
                                    return true;

                                var mixinDef = matchedClosure.Ruleset as MixinDefinition;
                                if (mixinDef != null)
                                {
                                    return mixinDef.Condition != null;
                                }

                                return false;
                            }))
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
            name = name.ToLowerInvariant();

            if (_functionTypes.ContainsKey(name))
                return (Function) Activator.CreateInstance(_functionTypes[name]);

            return null;
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
    }
}