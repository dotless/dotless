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

namespace dotless.Core.utils
{
    using System;
    using System.CodeDom.Compiler;
    using System.Text;
    using Microsoft.CSharp;

    public static class CsEval
    {
        public static object Eval(string injectedCode)
        {
            var comp = (new CSharpCodeProvider().CreateCompiler());
            var cp = new CompilerParameters();
            //cp.ReferencedAssemblies.Add("system.dll");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()){
                try{
                    var location = assembly.Location;
                    if (!String.IsNullOrEmpty(location)) cp.ReferencedAssemblies.Add(location);
                }
                catch (NotSupportedException){
                    // this happens for dynamic assemblies, so just ignore it.
                }
            }
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
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

            var cr = comp.CompileAssemblyFromSource(cp, code.ToString());
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
    }
}