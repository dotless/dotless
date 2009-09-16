using System.Collections.Generic;

namespace nless.Core.engine
{
    public interface INode
    {
        INode Parent { get; set; }
        string ToCss();
        string ToCSharp();
        IList<INode> Path(INode node);
    }
}