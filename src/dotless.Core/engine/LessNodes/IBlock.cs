using System.Collections.Generic;

namespace dotless.Core.engine
{
    public interface IBlock : INode
    {
        List<INode> Children { get; set; }
        List<IBlock> SubBlocks { get; }
        IList<Property> Properties { get; }
        IList<Variable> Variables { get; }
        IList<ElementBlock> Elements { get; }
        IList<Insert> Inserts { get;  }
        void Add(INode token);
    }
}