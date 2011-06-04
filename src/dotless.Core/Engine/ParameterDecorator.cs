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
            foreach (var parameter in parameters)
            {
                sb.AppendFormat("@{0}: {1};\n", parameter.Key, parameter.Value);
            }
            sb.Append(source);
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

        private static bool ValueIsNotNullOrEmpty(KeyValuePair<string, string> kvp)
        {
            return !string.IsNullOrEmpty(kvp.Value);
        }
    }
}