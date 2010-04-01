using System;
using System.Collections.Generic;
using dotless.Core.parser;
using Peg.Base;

namespace nLess
{
    internal class LessPegNode : PegNode
    {
        public LessPegRootNode Root { get; set; }
        public LessPegNode Child
        {
            get { return (LessPegNode)child_; }
            set { child_ = value; }
        }
        public LessPegNode Next { get { return (LessPegNode)next_; } }

        public LessPegNode(PegNode parent, int id, LessPegRootNode root) : base(parent, id)
        {
            Root = root;
        }

        public override string ToString()
        {
            return Type().ToString();
        }

        public string TextValue
        {
            get { return GetAsString(Root.Source); }
        }

        public EnLess Type()
        {
            return (EnLess)id_;
        }

        public bool IsEmpty()
        {
            return match_.Length == 0;
        }

        public IEnumerable<LessPegNode> Siblings()
        {
            return Siblings(x => true);
        }

        public IEnumerable<LessPegNode> Siblings(Func<LessPegNode, bool> condition)
        {
            var cursor = this;
            while (cursor != null)
            {
                if (!condition(cursor))
                {
                    cursor = cursor.Next;
                    continue;
                }
                yield return cursor;
                cursor = cursor.Next;
            }
        }

        public IEnumerable<LessPegNode> Children()
        {
            return Children(x => true);
        }

        public IEnumerable<LessPegNode> Children(Func<LessPegNode, bool> condition)
        {
            if (Child == null)
                return new LessPegNode[] {};

            return Child.Siblings(condition);
        }
    }
}