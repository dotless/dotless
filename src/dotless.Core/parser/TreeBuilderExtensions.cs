using nLess;
using Peg.Base;

namespace dotless.Core.parser
{
    internal static class TreeBuilderExtensions
    {
        internal static EnLess ToEnLess(this int i)
        {
            return (EnLess)i;
        }

        internal static EnLess ToEnLess(this PegNode node)
        {
            return node.id_.ToEnLess();
        }
    }
}