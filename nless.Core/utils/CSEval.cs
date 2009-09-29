using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace nless.Core.utils
{
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
            code.Append("using nless.Core.engine; \n");
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