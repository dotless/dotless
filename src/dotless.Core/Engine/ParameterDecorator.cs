using System;
using dotless.Core.Exceptions;
using dotless.Core.Parser.Infrastructure;

namespace dotless.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Parameters;

    public class ParameterDecorator : ILessEngine
    {
        public readonly ILessEngine Underlying;
        private readonly IParameterSource parameterSource;

        public ParameterDecorator(ILessEngine underlying, IParameterSource parameterSource)
        {
            this.Underlying = underlying;
            this.parameterSource = parameterSource;
        }

        public string TransformToCss(string source, string fileName)
        {
            var sb = new StringBuilder();
            var parameters = parameterSource.GetParameters()
                .Where(ValueIsNotNullOrEmpty);

            var parser = new Parser.Parser();
            sb.Append(source);
            foreach (var parameter in parameters)
            {
                sb.AppendLine();
                var variableDeclaration = string.Format("@{0}: {1};", parameter.Key, parameter.Value);

                try
                {
                    // Attempt to evaluate the generated variable to see if it's OK
                    parser.Parse(variableDeclaration, "").ToCSS(new Env());
                    sb.Append(variableDeclaration);
                }
                catch (ParserException)
                {
                    // Result wasn't valid LESS, output a comment instead
                    sb.AppendFormat("/* Omitting variable '{0}'. The expression '{1}' is not valid. */", parameter.Key,
                        parameter.Value);
                }
            }
            return Underlying.TransformToCss(sb.ToString(), fileName);
        }

        public IEnumerable<string> GetImports()
        {
            return Underlying.GetImports();
        }

        public void ResetImports()
        {
            Underlying.ResetImports();
        }

        public bool LastTransformationSuccessful
        {
            get
            {
                return Underlying.LastTransformationSuccessful;
            }
        }

        private static bool ValueIsNotNullOrEmpty(KeyValuePair<string, string> kvp)
        {
            return !string.IsNullOrEmpty(kvp.Value);
        }
    }
}