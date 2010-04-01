namespace nLess
{
    internal class LessPegRootNode : LessPegNode
    {
        public string Source { get; set; }

        public LessPegRootNode(int id, string source) : base(null, id, null)
        {
            Source = source;
            Root = this;
        }
    }
}