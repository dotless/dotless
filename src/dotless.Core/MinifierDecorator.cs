namespace dotless.Core
{
    using System.Text;
    using minifier;

    public class MinifierDecorator : ILessEngine
    {
        private readonly ILessEngine engine;

        public MinifierDecorator(ILessEngine engine)
        {
            this.engine = engine;
        }

        public string TransformToCss(string filename)
        {
            string buffer = engine.TransformToCss(filename);
            var processor = new Processor(buffer);
            return new StringBuilder().Append(processor.Output).ToString();
        }
    }
}