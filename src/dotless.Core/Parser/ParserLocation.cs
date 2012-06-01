namespace dotless.Core.Parser
{
    using System;

    public class NodeLocation
    {
        public int Index { get; set; }
        public string Source { get; set; }
        public string FileName { get; set; }

        public NodeLocation(int index, string source, string filename)
        {
            Index = index;
            Source = source;
            FileName = filename;
        }
    }
}
