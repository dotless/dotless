﻿namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Media : Ruleset
    {
        public Node Features { get; set; }
        public Ruleset Ruleset { get; set; }

        public Media(Node features, NodeList rules)
        {
            Features = features;
            Ruleset = new Ruleset(GetEmptySelector(), rules);
        }

        public Media(Node features, Ruleset ruleset)
        {
            Features = features;
            Ruleset = ruleset;
        }

        public static NodeList<Selector> GetEmptySelector()
        {
            return new NodeList<Selector>() { new Selector(new NodeList<Element>() { new Element(new Combinator(""), "&") }) };
        }

        public override void Accept(Plugins.IVisitor visitor)
        {
            Features = VisitAndReplace(Features, visitor);

            Ruleset = VisitAndReplace(Ruleset, visitor);
        }

        public override Node Evaluate(Env env)
        {
            int blockIndex = env.MediaBlocks.Count;
            env.MediaBlocks.Add(this);
            env.MediaPath.Push(this);

            var features = Features.Evaluate(env);
            var ruleset = Ruleset.Evaluate(env) as Ruleset;

            var media = new Media(features, ruleset).ReducedFrom<Media>(this);

            env.MediaPath.Pop();
            env.MediaBlocks[blockIndex] = media;

            if (env.MediaPath.Count == 0)
            {
                return media.EvalTop(env);
            }
            else
            {
                return media.EvalNested(env, features, ruleset);
            }
        }

        /// <summary>
        ///  Simple case for the top media node being evaluated
        /// </summary>
        protected Node EvalTop(Env env)
        {
            Node result;

            // Render all dependent Media blocks.
            if (env.MediaBlocks.Count > 1)
            {
                result = new Ruleset(GetEmptySelector(), new NodeList(env.MediaBlocks.Cast<Node>())) { MultiMedia = true }
                    .ReducedFrom<Ruleset>(this);
            }
            else
            {
                result = env.MediaBlocks[0];
            }

            env.MediaPath.Clear();
            env.MediaBlocks.Clear();

            return result;
        }

        /// <summary>
        ///  Evaluate when you have a media inside another media
        /// </summary>
        protected Node EvalNested(Env env, Node features, Ruleset ruleset)
        {
            var path = new NodeList<Media>(env.MediaPath.ToList());
            path.Add(this);

            NodeList<NodeList> derivedPath = new NodeList<NodeList>();

            // Extract the media-query conditions separated with `,` (OR).
            for (int i = 0; i < path.Count; i++)
            {
                Node pathComponent;
                Value value = path[i].Features as Value;
                if (value != null)
                {
                    pathComponent = value.Values;
                }
                else
                {
                    pathComponent = path[i].Features;
                }

                derivedPath.Add((pathComponent as NodeList) ?? new NodeList() { pathComponent });
            }

            // Trace all permutations to generate the resulting media-query.
            //
            // (a, b and c) with nested (d, e) ->
            //    a and d
            //    a and e
            //    b and c and d
            //    b and c and e

            NodeList<NodeList> pathWithAnds = new NodeList<NodeList>();

            foreach (NodeList node in derivedPath)
            {
                pathWithAnds.Add(node);
                pathWithAnds.Add(new NodeList() { new TextNode("and") });
            }

            pathWithAnds.RemoveAt(pathWithAnds.Count - 1);

            Features = new Value(Permute(pathWithAnds), null);

            // Fake a tree-node that doesn't output anything.
            return new Ruleset(new NodeList<Selector>(), new NodeList());
        }

        /// <summary>
        ///  Flattens a list of nodes seperated by comma so that all the conditions are on the bottom.
        ///  e.g.
        ///  (A) and (B) and (C) => A and B and C
        ///  (A, B) and (D) and (C) => A and D and C, B and D and C
        ///  (A) and (B) and (C, D) => A and B and C, A and B and D
        /// 
        ///  It does this by generating a list of permutations for the last n-1 then n-2
        ///  and with each call it multiplies out the OR'd elements
        /// </summary>
        private NodeList Permute(NodeList<NodeList> arr)
        {
            // in simple cases return
            if (arr.Count == 0)
                return new NodeList();

            if (arr.Count == 1)
            {
                return arr[0];
            }

            NodeList returner = new NodeList();

            // run permute on the next n-1
            NodeList<NodeList> sliced = new NodeList<NodeList>(arr.Skip(1));
            NodeList rest = Permute(sliced);

            //now multiply
            for (int i = 0; i < rest.Count; i++)
            {
                NodeList inner = arr[0];
                for (int j = 0; j < inner.Count; j++)
                {
                    NodeList newl = new NodeList();
                    newl.Add(inner[j]);

                    NodeList addition = rest[i] as NodeList;
                    if (addition)
                    {
                        newl.AddRange(addition);
                    }
                    else
                    {
                        newl.Add(rest[i]);
                    }

                    //add an expression so it seperated by spaces
                    returner.Add(new Expression(newl));
                }
            }

            return returner;
        }

        public override void AppendCSS(Env env, Context ctx)
        {
            if (env.Compress && !Ruleset.Rules.Any())
                return;

            // first deal with the contents, in case its empty
            env.Output.Push();

            Ruleset.IsRoot = ctx.Count == 0;

            Ruleset.AppendCSS(env, ctx);

            if (!env.Compress)
                env.Output.Trim().Indent(2);

            var contents = env.Output.Pop();

            // if we have no contents, skip
            if (env.Compress && contents.Length == 0)
                return;

            // go ahead and output
            env.Output.Append("@media");

            if (Features)
            {
                env.Output.Append(' ');
                env.Output.Append(Features);
            }

            if (env.Compress)
                env.Output.Append('{');
            else
                env.Output.Append(" {\n");


            env.Output.Append(contents);

            if (env.Compress)
                env.Output.Append('}');
            else
                env.Output.Append("\n}\n");
        }
    }
}