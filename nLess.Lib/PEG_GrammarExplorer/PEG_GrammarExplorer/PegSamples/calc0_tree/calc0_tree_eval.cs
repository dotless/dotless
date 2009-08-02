using Peg.Samples;
using System.Collections.Generic;
using Peg.Base;
using System.Diagnostics;
using System;
using System.IO;

namespace calc0_tree
{
    class Calc0TreeEval : IParserPostProcessor
    {
        #region IParserPostProcessor functions
        string IParserPostProcessor.ShortDesc { get { return "Evaluate"; } }
        string IParserPostProcessor.ShortestDesc { get { return "Eval"; } }
        string IParserPostProcessor.DetailDesc
        {
            get
            {
                return "Evaluates the syntax tree and ouputs the result to the console";
            }
        }
        void IParserPostProcessor.Postprocess(ParserPostProcessParams postProcessorParams)
        {
            Session.Eval(postProcessorParams.root_, postProcessorParams.src_, postProcessorParams.errOut_);
        }
        #endregion IParserPostProcessor functions
        #region Helper classes
        static class Session
        {
            #region Data Members
            internal static Dictionary<string, double> variables= new Dictionary<string,double>();
            #endregion Data Members
            internal static double Eval(PegNode node, string src, TextWriter errOut)
            {
                switch ((Ecalc0_tree)node.id_)
                {
                    case Ecalc0_tree.Calc:
                        {
                            double res=0;
                            for (node = node.child_; node != null; node = node.next_)
                            {
                                res = Eval(node, src, errOut);
                            }
                            Console.WriteLine("-->{0}<--", res);
                            return res;
                        }
                    case Ecalc0_tree.Assign:
                        {
                            double res = Eval(node.child_.next_, src, errOut);
                            string ident = node.child_.GetAsString(src);
                            variables[ident] = res;
                            Console.WriteLine("{0}={1}", ident, res);
                            return res;
                        }
                    case Ecalc0_tree.Sum:
                    case Ecalc0_tree.Prod:
                        {
                            double res = Eval(node.child_,src,errOut);
                            for (PegNode op = node.child_.next_; op != null; op = node.next_)
                            {
                                node = op.next_;
                                switch (op.GetAsString(src))
                                {
                                    case "+": res += Eval(node, src, errOut); break;
                                    case "-": res -= Eval(node, src, errOut); break;
                                    case "*": res *= Eval(node, src, errOut); break;
                                    case "/": res /= Eval(node, src, errOut); break;
                                    default: Debug.Assert(false); break;
                                }
                            }
                            return res;
                        }
                    case Ecalc0_tree.Number:
                        {
                            double res;
                            double.TryParse(node.GetAsString(src),out res);
                            return res;
                        }
                    case Ecalc0_tree.Call:
                        {
                            double res = Eval(node.child_.next_, src, errOut);
                            string operation=node.child_.GetAsString(src).ToLower();
                            switch (operation)
                            {
                                case "sin":  return Math.Sin(res);
                                case "cos":  return Math.Cos(res);
                                case "tan":  return Math.Tan(res);
                                case "sqrt": return Math.Sqrt(res);
                                case "abs":  return Math.Abs(res);
                                case "acos": return Math.Acos(res);
                                case "asin": return Math.Asin(res);
                                case "atan": return Math.Atan(res);
                                case "ceiling": return Math.Ceiling(res);
                                case "cosh": return Math.Cosh(res);
                                case "exp": return Math.Exp(res);
                                case "floor":return Math.Floor(res);
                                case "log": return Math.Log10(res);
                                case "sinh": return Math.Sinh(res);
                                case "tanh": return Math.Tanh(res);
                                default: errOut.WriteLine(
                                                "ERROR from CALCOTreeEval: function '{0}' not supported", operation);
                                    return 0;
                            }
                        }
                    case Ecalc0_tree.ident:
                        {
                            string ident = node.GetAsString(src);
                            if (variables.ContainsKey(ident)) return variables[ident];
                            else return 0;
                        }
                    case (Ecalc0_tree)ESpecialNodes.eAnonymousNode:
                        {
                            if( node.GetAsString(src)=="print" )
                            {
                                foreach(var v in variables)
                                {
                                    Console.WriteLine("{0,-20}{1}",v.Key,v.Value);
                                }
                            }
                            return 0;
                        }
                }
                return 0;
            }

        }
        #endregion Helper classes
    }
}