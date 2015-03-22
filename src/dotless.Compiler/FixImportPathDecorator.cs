namespace dotless.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;

    class FixImportPathDecorator : ILessEngine
    {
        private readonly ILessEngine underlying;

        public FixImportPathDecorator(ILessEngine underlying)
        {
            this.underlying = underlying;
        }

        public string TransformToCss(string source, string fileName)
        {
            return underlying.TransformToCss(source, fileName);
        }

        public void ResetImports()
        {
            underlying.ResetImports();
        }

        public IEnumerable<string> GetImports()
        {
            return underlying.GetImports().Select(import => import.Replace("/", "\\"));
        }

        public bool LastTransformationSuccessful
        {
            get
            {
                return underlying.LastTransformationSuccessful;
            }
        }

        public string CurrentDirectory
        {
            get { return underlying.CurrentDirectory; }
            set { underlying.CurrentDirectory = value; }
        }
    }
}