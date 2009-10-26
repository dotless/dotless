namespace dotless.Core.engine
{
    using System.Collections.Generic;

    public interface INode
    {
        INode Parent { get; set; }
        string ToCss();
        string ToCSharp();
        IList<INode> Path(INode node);
    }
}