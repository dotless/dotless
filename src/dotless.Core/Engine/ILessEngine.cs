namespace dotless.Core
{
    using System.Collections.Generic;
    using System;

    public interface ILessEngine
    {
        string TransformToCss(string source, string fileName);
        void ResetImports();
        IEnumerable<string> GetImports();
        bool LastTransformationSuccessful { get; }

        string CurrentDirectory { get; set; }
    }
}