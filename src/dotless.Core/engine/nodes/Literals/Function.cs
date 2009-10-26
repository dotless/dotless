using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.Core.engine
{
    public class Function : Literal
    {
        public IList<INode> Args
        {
            get; set;
        }

        public Function(string value, IList<INode> args)
            : base(value)
        {
            Args = args;
        }
        public override string ToCss()
        {
            return Evaluate().ToCss();
        }

        private INode Evaluate()
        {
            //TODO: Evaluate function instead of just printing
            return new Anonymous(string.Format("{0}{1}", Value, ArgsString));
        }

        protected string ArgsString
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var arg in Args)
                    sb.AppendFormat("{0},", arg);
                var args = sb.ToString();
                return args.Substring(0, args.Length - 1);
            }
        }
    }
}