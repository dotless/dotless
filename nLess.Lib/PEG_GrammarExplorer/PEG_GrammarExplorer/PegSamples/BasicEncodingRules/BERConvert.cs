using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peg.Samples;
using Peg.Base;
using System.IO;
using System.Diagnostics;

namespace BERTree
{
    class BERConvertDefiniteLength : IParserPostProcessor
    {
        #region Data members
        TreeContext context_;
        Dictionary<PegNode, uint> dictLength= new Dictionary<PegNode,uint>();
        byte[] lengthBuffer = new byte[5];
        #endregion Data members
        #region Helper classes
        class TreeContext
        {
            internal PegNode root_;
            internal byte[] byteSrc_;
            internal TextWriter errOut_;
            internal ParserPostProcessParams generatorParams_;
            internal string sErrorPrefix;

            internal TreeContext(ParserPostProcessParams generatorParams)
            {
                generatorParams_ = generatorParams;
                root_ = generatorParams.root_;
                byteSrc_ = generatorParams.byteSrc_;
                errOut_ = generatorParams.errOut_;
                sErrorPrefix = "<BER_DEFINITE_CONVERTER> FILE:'" + generatorParams_.grammarFileName_ + "' ";
            }
        }
        #endregion Helper classes
        #region IParserPostProcessor Members

        string IParserPostProcessor.ShortDesc
        {
            get { return "-> Definite Length Form"; }
        }
        string IParserPostProcessor.ShortestDesc
        {
            get { return "Definite"; }
        }
        string IParserPostProcessor.DetailDesc
        {
            get
            {
                return "Convert to definite length form (usally smaller files)";
            }
        }

        void IParserPostProcessor.Postprocess(ParserPostProcessParams postProcessorParams)
        {
            context_ = new TreeContext(postProcessorParams);
            string outDir =PUtils.MakeFileName("", context_.generatorParams_.outputDirectory_, "DefiniteLengthForm");
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            string outFile= PUtils.MakeFileName(context_.generatorParams_.sourceFileTitle_,outDir);
            using (BinaryWriter rw = new BinaryWriter(File.Open(outFile, FileMode.Create)))
            {
                WriteDefinite(rw, context_.generatorParams_.root_);
                context_.generatorParams_.errOut_.WriteLine("INFO  from <BER_DEFINITE_ENCODER> {0} bytes written to '{1}'",
                                             rw.BaseStream.Position,outFile);
            }
            
        }

       
        #endregion IParserPostProcessor Members
        #region WriteDefinite method and helpers
        private void WriteDefinite(BinaryWriter rw, PegNode pegNode)
        {
            PegNode child;
           if (pegNode == null) return;
           WriteTag(rw, pegNode);
           WriteLength(rw, pegNode);
           if (IsComposite(pegNode, out child))
           {
               WriteDefinite(rw, child);
           }
           else
           {
               Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null && pegNode.child_.next_.next_ != null);
               WriteContent(rw, pegNode.child_.next_.next_);
           }
           WriteDefinite(rw, pegNode.next_);
        }

        private bool IsComposite(PegNode pegNode,out PegNode child)
        {
            Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null && pegNode.child_.next_.next_!=null);
            
            child = pegNode.child_.next_.next_;
            bool bIsComposite=  child.id_ == (int)EBERTree.ConstructedDelimValue ||
                                child.id_ == (int)EBERTree.ConstructedValue;
            if (bIsComposite) child = child.child_;
            return bIsComposite;
        }

        private void WriteTag(BinaryWriter rw, PegNode pegNode)
        {
            Debug.Assert(pegNode.child_ != null);
            PegNode tagNode = pegNode.child_;
            Debug.Assert(tagNode.id_== (int)EBERTree.OneOctetTag || tagNode.id_==(int)EBERTree.MultiOctetTag);
            rw.Write(context_.byteSrc_, tagNode.match_.posBeg_, tagNode.match_.posEnd_ - tagNode.match_.posBeg_);
            
        }
        private void WriteLength(BinaryWriter rw, PegNode pegNode)
        {
            Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null);
            PegNode lengthNode = pegNode.child_.next_;
            uint length = GetLength(lengthNode.next_);
            int bytesToWrite= GetLengthEncodingLength(length);
            if (bytesToWrite == 1)
            {
                lengthBuffer[0] = (byte)length;
            }
            else
            {
                lengthBuffer[0] = (byte)((bytesToWrite - 1) | 0x80);
                int j=1;
                for (int i =  bytesToWrite - 1; i>0; i--)
                {
                    byte @byte = (byte)((length & (0xFF << 8 * (i - 1) )) >>  8 * (i - 1));
                    lengthBuffer[j++] = @byte;
                }
            }
            rw.Write(lengthBuffer, 0,bytesToWrite);
        }

        private uint GetLength(PegNode pegNode)
        {
            if (dictLength.ContainsKey(pegNode)) return dictLength[pegNode];
            if (pegNode.id_ == (int)EBERTree.ConstructedDelimValue ||
                pegNode.id_ == (int)EBERTree.ConstructedValue)
            {
                uint length = 0;
                for (PegNode child = pegNode.child_; child != null; child = child.next_)
                {
                    length += GetLength(child);
                }
                dictLength.Add(pegNode, length);
                return length;
            }
            else if (pegNode.id_ == (int)EBERTree.PrimitiveValue)
            {
                uint length= (uint)(pegNode.match_.posEnd_ - pegNode.match_.posBeg_);
                dictLength.Add(pegNode, length);
                return length;
            }
            else if (pegNode.id_ == (int)EBERTree.TLV)
            {
                Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null);
                PegNode tag = pegNode.child_, content = tag.next_.next_;
                uint length= (uint)(pegNode.child_.match_.posEnd_ - pegNode.child_.match_.posBeg_)
                    + (uint)GetLengthEncodingLength(GetLength(content))
                    + GetLength(content);
                dictLength.Add(pegNode, length);
                return length;
            }
            else
            {
                Debug.Assert(false);
                return 0;
            }
        }

        private int GetLengthEncodingLength(uint length)
        {
            if (length < 127) return 1;
            else
            {
                if ((length & 0xFF000000) != 0) return 5;
                else if ((length & 0xFFFF0000) != 0) return 4;
                else if ((length & 0xFFFFFF00) != 0) return 3;
                else return 2;
            }
        }
        private void WriteContent(BinaryWriter rw, PegNode pegNode)
        {
            Debug.Assert(pegNode.id_ == (int)EBERTree.PrimitiveValue);
            rw.Write(context_.byteSrc_, pegNode.match_.posBeg_, pegNode.match_.posEnd_ - pegNode.match_.posBeg_);
        }
        #endregion  WriteDefinite method and helpers
    }
    class BERConvertIndefiniteLength : IParserPostProcessor
    {
        #region Data members
        TreeContext context_;
        Dictionary<PegNode, uint> dictLength = new Dictionary<PegNode, uint>();
        byte[] lengthBuffer = new byte[5];
        #endregion Data members
        #region Helper classes
        class TreeContext
        {
            internal PegNode root_;
            internal byte[] byteSrc_;
            internal TextWriter errOut_;
            internal ParserPostProcessParams generatorParams_;
            internal string sErrorPrefix;

            internal TreeContext(ParserPostProcessParams generatorParams)
            {
                generatorParams_ = generatorParams;
                root_ = generatorParams.root_;
                byteSrc_ = generatorParams.byteSrc_;
                errOut_ = generatorParams.errOut_;
                sErrorPrefix = "<BER_INDEFINITE_CONVERTER> FILE:'" + generatorParams_.grammarFileName_ + "' ";
            }
        }
        #endregion Helper classes
        #region IParserPostProcessor Members

        string IParserPostProcessor.ShortDesc
        {
            get { return "-> Indefinite Length Form"; }
        }
        string IParserPostProcessor.ShortestDesc
        {
            get { return "Indefinite"; }
        }

        string IParserPostProcessor.DetailDesc
        {
            get
            {
                return "Convert to indefinite length form (usally bigger files)";
            }
        }

        void IParserPostProcessor.Postprocess(ParserPostProcessParams postProcessorParams)
        {
            context_ = new TreeContext(postProcessorParams);
            string outDir = PUtils.MakeFileName("", context_.generatorParams_.outputDirectory_, "IndefiniteLengthForm");
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            string outFile = PUtils.MakeFileName(context_.generatorParams_.sourceFileTitle_, outDir);
            using (BinaryWriter rw = new BinaryWriter(File.Open(outFile, FileMode.Create)))
            {
                WriteInDefinite(rw, context_.generatorParams_.root_);
                context_.generatorParams_.errOut_.WriteLine("INFO  from <BER_INDEFINITE_ENCODER> {0} bytes written to '{1}'",
                                             rw.BaseStream.Position, outFile);
            }

        }


        #endregion IParserPostProcessor Members
        #region WriteDefinite method and helpers
        private void WriteInDefinite(BinaryWriter rw, PegNode pegNode)
        {
            PegNode child;
            if (pegNode == null) return;
            bool bIsComposite = IsComposite(pegNode, out child);
            WriteTag(rw, pegNode);
            WriteLength(rw, pegNode,bIsComposite);
            if (bIsComposite)
            {
                for(;child!=null;child= child.next_)
                {
                    WriteInDefinite(rw, child);
                }
                rw.Write((ushort)0x0000);
            }
            else
            {
                Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null && pegNode.child_.next_.next_ != null);
                WriteContent(rw, pegNode.child_.next_.next_);
            }
        }

        private bool IsComposite(PegNode pegNode, out PegNode child)
        {
            Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null && pegNode.child_.next_.next_ != null);

            child = pegNode.child_.next_.next_;
            bool bIsComposite = child.id_ == (int)EBERTree.ConstructedDelimValue ||
                                child.id_ == (int)EBERTree.ConstructedValue;
            if (bIsComposite) child = child.child_;
            return bIsComposite;
        }

        private void WriteTag(BinaryWriter rw, PegNode pegNode)
        {
            Debug.Assert(pegNode.child_ != null);
            PegNode tagNode = pegNode.child_;
            Debug.Assert(tagNode.id_ == (int)EBERTree.OneOctetTag || tagNode.id_ == (int)EBERTree.MultiOctetTag);
            rw.Write(context_.byteSrc_, tagNode.match_.posBeg_, tagNode.match_.posEnd_ - tagNode.match_.posBeg_);

        }
        private void WriteLength(BinaryWriter rw, PegNode pegNode,bool bIsComposite)
        {
            Debug.Assert(pegNode.child_ != null && pegNode.child_.next_ != null);
            if (bIsComposite)
            {
                rw.Write((byte)0x80);
            }
            else {
                PegNode lengthNode = pegNode.child_.next_;
                uint length = GetLength(lengthNode.next_);
                int bytesToWrite = GetLengthEncodingLength(length);
                if (bytesToWrite == 1)
                {
                    lengthBuffer[0] = (byte)length;
                }
                else
                {
                    lengthBuffer[0] = (byte)((bytesToWrite - 1) | 0x80);
                    int j = 1;
                    for (int i = bytesToWrite - 1; i > 0; i--)
                    {
                        byte @byte = (byte)((length & (0xFF << 8 * (i - 1))) >> 8 * (i - 1));
                        lengthBuffer[j++] = @byte;
                    }
                }
                rw.Write(lengthBuffer, 0, bytesToWrite);
            }
        }

        private uint GetLength(PegNode pegNode)
        {
            
            uint length = (uint)(pegNode.match_.posEnd_ - pegNode.match_.posBeg_);
            return length;
        }

        private int GetLengthEncodingLength(uint length)
        {
            if (length < 127) return 1;
            else
            {
                if ((length & 0xFF000000) != 0) return 5;
                else if ((length & 0xFFFF0000) != 0) return 4;
                else if ((length & 0xFFFFFF00) != 0) return 3;
                else return 2;
            }
        }
        private void WriteContent(BinaryWriter rw, PegNode pegNode)
        {
            Debug.Assert(pegNode.id_ == (int)EBERTree.PrimitiveValue);
            rw.Write(context_.byteSrc_, pegNode.match_.posBeg_, pegNode.match_.posEnd_ - pegNode.match_.posBeg_);
        }
        #endregion  WriteDefinite method and helpers
    }
}
