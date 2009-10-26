using Peg.Base;

namespace dotless.Core.parser
{
    internal class NodePrinter
    {
        private readonly PegBaseParser parser_;

        internal NodePrinter(PegBaseParser parser)
        {
            parser_ = parser;
        }

        internal string GetNodeName(PegNode n)
        {
            return parser_.GetRuleNameFromId(n.id_);
        }
    }
}