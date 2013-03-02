namespace dotless.Core
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public interface ILessEngine
    {
        string TransformToCss(string source, string fileName, StringBuilder sourceMap);
        string TransformToCss(string source, string fileName);
        void ResetImports();
        IEnumerable<string> GetImports();
        bool LastTransformationSuccessful { get; }
    }
}