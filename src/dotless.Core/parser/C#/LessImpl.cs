using System;
using System.IO;
using Peg.Base;

namespace nLess
{
    internal class LessImpl : nLess
    {
        public LessImpl(string source, TextWriter writer)
            : base(source, writer)
        {
            SetNodeCreator(LessNodeCreator);
        }

        protected PegNode LessNodeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                if (parentOrCreated == null)
                    return new LessPegRootNode(id, src_);
                else
                    return new LessPegNode(parentOrCreated, id, GetRoot());
            
            return null;
        }

        new public LessPegRootNode GetRoot()
        {
            return (LessPegRootNode) base.GetRoot();
        }
    }
}