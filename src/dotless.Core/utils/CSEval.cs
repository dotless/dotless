/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System.CodeDom;
using System.Threading;

namespace dotless.Core.utils
{
    using System;
    using System.CodeDom.Compiler;
    using System.Text;
    using Microsoft.CSharp;

    public static class OldCsEval
    {
        private static int counter = 0;
        public static object Eval(string injectedCode)
        {
            Console.WriteLine(counter);
            counter++;
            var c = new CSharpCodeProvider();
            
            var comp = (new CSharpCodeProvider().CreateCompiler());
            var cr = comp.CompileAssemblyFromSource(Params, GetCode(injectedCode));
            if (cr.Errors.HasErrors)
            {
                var error = new StringBuilder();
                foreach (CompilerError err in cr.Errors)
                {
                    error.AppendFormat("{0}\n", err.ErrorText);
                }
                throw new Exception(error.ToString());
            }

            var a = cr.CompiledAssembly;
            var compiled = a.CreateInstance("CsEvaluation._Evaluator");
            var mi = compiled.GetType().GetMethod("_Eval");
            return mi.Invoke(compiled, null);
        }

        private static string GetCode(string injectedCode)
        {
            var code = new StringBuilder();
            code.Append("using System; \n");
            code.Append("using dotless.Core.engine; \n");
            code.Append("namespace CsEvaluation { \n");
            code.Append("  public class _Evaluator { \n");
            code.Append("       public object _Eval() { \n");
            code.AppendFormat("             return {0}; ", injectedCode);
            code.Append("               }\n");
            code.Append("       }\n");
            code.Append("  }\n");
            return code.ToString();
        }

        private static CompilerParameters _params;
        private static CompilerParameters Params
        {
            get
            {
                if(_params==null){
                    _params = new CompilerParameters {GenerateExecutable = false, GenerateInMemory = true};

                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        try
                        {
                            var location = assembly.Location;
                            if (!String.IsNullOrEmpty(location)) _params.ReferencedAssemblies.Add(location);
                        }
                        catch (NotSupportedException)
                        {
                            // this happens for dynamic assemblies, so just ignore it.
                        }
                    }
                }
                return _params;
            }
        }
    }


    public static class CsEval
    {
        static void LogTime(string Message)
        {
           if (Last == 0)
                Last = DateTime.Now.Ticks;
            else
            {

                Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.Ticks - Last, Message));
                Last = DateTime.Now.Ticks;
            }
        }

        private static long Last = 132;
        public static object Eval(string injectedCode)
        {

            //LogTime("starting");
            var code = GetCode(injectedCode);
            //LogTime("getting code");
            var compileUnit = new CodeSnippetCompileUnit(code);
            //LogTime("CodeSnippetCompileUnit");
            var provider = new CSharpCodeProvider();
            var cr = provider.CompileAssemblyFromDom(Params, compileUnit);
            //LogTime("CompileAssemblyFromDom");
            if (cr.Errors.HasErrors)
            {
                var error = new StringBuilder();
                foreach (CompilerError err in cr.Errors)
                    error.AppendFormat("{0}\n", err.ErrorText);

                throw new Exception(error.ToString());
            }

            var a = cr.CompiledAssembly;
            var compiled = a.CreateInstance("CsEvaluation._Evaluator");
            var mi = compiled.GetType().GetMethod("_Eval");
            var returnObj =  mi.Invoke(compiled, null);
            //LogTime("Reflection results");
            return returnObj;
        }

        private static string GetCode(string injectedCode)
        {
            var code = new StringBuilder();
            code.Append("using System; \n");
            code.Append("using dotless.Core.engine; \n");
            code.Append("namespace CsEvaluation { \n");
            code.Append("  public class _Evaluator { \n");
            code.Append("       public object _Eval() { \n");
            code.AppendFormat("             return {0}; ", injectedCode);
            code.Append("               }\n");
            code.Append("       }\n");
            code.Append("  }\n");
            return code.ToString();
        }

        private static CompilerParameters _params;
        private static CompilerParameters Params
        {
            get
            {
                if (_params == null)
                {
                    _params = new CompilerParameters { GenerateExecutable = false, GenerateInMemory = true };

                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        try
                        {
                            var location = assembly.Location;
                            if (!String.IsNullOrEmpty(location)) _params.ReferencedAssemblies.Add(location);
                        }
                        catch (NotSupportedException)
                        {
                            // this happens for dynamic assemblies, so just ignore it.
                        }
                    }
                }
                return _params;
            }
        }
    }

}