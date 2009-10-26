namespace dotless.Core
{
    using System;
    using System.IO;
    using engine;

    public class LessEngine : ILessEngine
    {
        public string TransformToCss(string filename)
        {
            var engine = new Engine(File.ReadAllText(filename), Console.Out);
            return engine.Parse().Css;
        }
    }
}