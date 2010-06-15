namespace dotless.Core
{
    using System.Collections.Generic;

    public interface ILessEngine
    {
        string TransformToCss(string source, string fileName);
        IEnumerable<string> GetImports();
    }
}