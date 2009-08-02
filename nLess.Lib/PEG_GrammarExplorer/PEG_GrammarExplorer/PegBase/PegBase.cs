/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 *  20080929;UTF16BE;Added UTF16BE read support to <<FileLoader.LoadFile(out string src)>>
 * <</History>>
*/
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text;
using System;
namespace Peg.Base
{
    #region Input File Support
    public enum EncodingClass { unicode, utf8, binary, ascii };
    public enum UnicodeDetection { notApplicable, BOM, FirstCharIsAscii };
    public class FileLoader
    {
        public enum FileEncoding { none, ascii, binary, utf8, unicode, utf16be, utf16le, utf32le, utf32be, uniCodeBOM };
        public FileLoader(EncodingClass encodingClass, UnicodeDetection detection, string path)
        {
            encoding_ = GetEncoding(encodingClass, detection, path);
            path_ = path;
        }
        public bool IsBinaryFile()
        {
            return encoding_ == FileEncoding.binary;
        }
        public bool LoadFile(out byte[] src)
        {
            src = null;
            if (!IsBinaryFile()) return false;
            using (BinaryReader brdr = new BinaryReader(File.Open(path_, FileMode.Open,FileAccess.Read)))
            {
                src = brdr.ReadBytes((int)brdr.BaseStream.Length);
                return true;
            }
        }
        public bool LoadFile(out string src)
        {
            src = null;
            Encoding textEncoding = FileEncodingToTextEncoding();
            if (textEncoding == null)
            {
                if (encoding_ == FileEncoding.binary) return false;
                using (StreamReader rd = new StreamReader(path_, true))
                {
                    src = rd.ReadToEnd();
                    return true;
                }
            }
            else
            {
                if (encoding_ == FileEncoding.utf16be)//UTF16BE
                {
                    using (BinaryReader brdr = new BinaryReader(File.Open(path_, FileMode.Open, FileAccess.Read)))
                    {
                        byte[] bytes = brdr.ReadBytes((int)brdr.BaseStream.Length);
                        StringBuilder s = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i += 2)
                        {
                            char c = (char)(bytes[i] << 8 | bytes[i + 1]);
                            s.Append(c);
                        }
                        src = s.ToString();
                        return true;
                    }
                }
                else
                {
                    using (StreamReader rd = new StreamReader(path_, textEncoding))
                    {
                        src = rd.ReadToEnd();
                        return true;
                    }
                }
            }

        }
        Encoding FileEncodingToTextEncoding()
        {
            switch (encoding_)
            {
                case FileEncoding.utf8: return new UTF8Encoding();
                case FileEncoding.utf32be:
                case FileEncoding.utf32le: return new UTF32Encoding();
                case FileEncoding.unicode:
                case FileEncoding.utf16be:
                case FileEncoding.utf16le: return new UnicodeEncoding();
                case FileEncoding.ascii: return new ASCIIEncoding();
                case FileEncoding.binary:
                case FileEncoding.uniCodeBOM: return null;
                default: Debug.Assert(false);
                    return null;

            }
        }
        static FileEncoding DetermineUnicodeWhenFirstCharIsAscii(string path)
        {
            using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                byte[] startBytes = br.ReadBytes(4);
                if (startBytes.Length == 0) return FileEncoding.none;
                if (startBytes.Length == 1 || startBytes.Length == 3) return FileEncoding.utf8;
                if (startBytes.Length == 2 && startBytes[0] != 0) return FileEncoding.utf16le;
                if (startBytes.Length == 2 && startBytes[0] == 0) return FileEncoding.utf16be;
                if (startBytes[0] == 0 && startBytes[1] == 0) return FileEncoding.utf32be;
                if (startBytes[0] == 0 && startBytes[1] != 0) return FileEncoding.utf16be;
                if (startBytes[0] != 0 && startBytes[1] == 0) return FileEncoding.utf16le;
                return FileEncoding.utf8;
            }
        }
        FileEncoding GetEncoding(EncodingClass encodingClass, UnicodeDetection detection, string path)
        {
            switch (encodingClass)
            {
                case EncodingClass.ascii: return FileEncoding.ascii;
                case EncodingClass.unicode:
                    {
                        if (detection == UnicodeDetection.FirstCharIsAscii)
                        {
                            return DetermineUnicodeWhenFirstCharIsAscii(path);
                        }
                        else if (detection == UnicodeDetection.BOM)
                        {
                            return FileEncoding.uniCodeBOM;
                        }
                        else return FileEncoding.unicode;
                    }
                case EncodingClass.utf8: return FileEncoding.utf8;
                case EncodingClass.binary: return FileEncoding.binary;
            }
            return FileEncoding.none;
        }
        string path_;
        public readonly FileEncoding encoding_;
    }
    #endregion Input File Support
    #region Error handling
    public class PegException : System.Exception
    {
        public PegException()
            : base("Fatal parsing error ocurred")
        {
        }
    }
    public struct PegError
    {
        internal SortedList<int, int> lineStarts;
        void AddLineStarts(string s, int first, int last, ref int lineNo, out int colNo)
        {
            colNo = 2;
            for (int i = first + 1; i <= last; ++i, ++colNo)
            {
                if (s[i - 1] == '\n')
                {
                    lineStarts[i] = ++lineNo;
                    colNo = 1;
                }
            }
            --colNo;
        }
        public void GetLineAndCol(string s, int pos, out int lineNo, out int colNo)
        {
            for (int i = lineStarts.Count(); i > 0; --i)
            {
                KeyValuePair<int, int> curLs = lineStarts.ElementAt(i - 1);
                if (curLs.Key == pos)
                {
                    lineNo = curLs.Value;
                    colNo = 1;
                    return;
                }
                if (curLs.Key < pos)
                {
                    lineNo = curLs.Value;
                    AddLineStarts(s, curLs.Key, pos, ref lineNo, out colNo);
                    return;
                }
            }
            lineNo = 1;
            AddLineStarts(s, 0, pos, ref lineNo, out colNo);
        }
    }
    #endregion Error handling
    #region Syntax/Parse-Tree related classes
    public enum ESpecialNodes { eFatal = -10001, eAnonymNTNode = -1000, eAnonymASTNode = -1001, eAnonymousNode = -100 }
    public enum ECreatorPhase { eCreate, eCreationComplete, eCreateAndComplete }
    public struct PegBegEnd//indices into the source string
    {
        public int Length
        {
            get { return posEnd_ - posBeg_; }
        }
        public string GetAsString(string src)
        {
            Debug.Assert(src.Length >= posEnd_);
            return src.Substring(posBeg_, Length);
        }
        public int posBeg_;
        public int posEnd_;
    }
    public class PegNode : ICloneable
    {
        #region Constructors
        public PegNode(PegNode parent, int id, PegBegEnd match, PegNode child, PegNode next)
        {
            parent_ = parent; id_ = id; child_ = child; next_ = next;
            match_ = match;
        }
        public PegNode(PegNode parent, int id, PegBegEnd match, PegNode child)
            : this(parent, id, match, child, null)
        {
        }
        public PegNode(PegNode parent, int id, PegBegEnd match)
            : this(parent, id, match, null, null)
        { }
        public PegNode(PegNode parent, int id)
            : this(parent, id, new PegBegEnd(), null, null)
        {
        }
        #endregion Constructors
        #region Public Members
        public virtual string GetAsString(string s)
        {
            return match_.GetAsString(s);
        }
        public virtual PegNode Clone()
        {
            PegNode clone= new PegNode(parent_, id_, match_);
            CloneSubTrees(clone);
            return clone;
        }
        #endregion Public Members
        #region Protected Members
        protected void CloneSubTrees(PegNode clone)
        {
            PegNode child = null, next = null;
            if (child_ != null)
            {
                child = child_.Clone();
                child.parent_ = clone;
            }
            if (next_ != null)
            {
                next = next_.Clone();
                next.parent_ = clone;
            }
            clone.child_ = child;
            clone.next_ = next;
        }
        #endregion Protected Members
        #region Data Members
        public int id_;
        public PegNode parent_, child_, next_;
        public PegBegEnd match_;
        #endregion Data Members

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
    internal struct PegTree
    {
        internal enum AddPolicy { eAddAsChild, eAddAsSibling };
        internal PegNode root_;
        internal PegNode cur_;
        internal AddPolicy addPolicy;
    }
    public abstract class PrintNode
    {
        public abstract int LenMaxLine();
        public abstract bool IsLeaf(PegNode p);
        public virtual bool IsSkip(PegNode p) { return false; }
        public abstract void PrintNodeBeg(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel);
        public abstract void PrintNodeEnd(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel);
        public abstract int LenNodeBeg(PegNode p);
        public abstract int LenNodeEnd(PegNode p);
        public abstract void PrintLeaf(PegNode p, ref int nOffsetLineBeg, bool bAlignVertical);
        public abstract int LenLeaf(PegNode p);
        public abstract int LenDistNext(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel);
        public abstract void PrintDistNext(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel);
    }
    public class TreePrint : PrintNode
    {
        #region Data Members
        public delegate string GetNodeName(PegNode node);
        string src_;
        TextWriter treeOut_;
        int nMaxLineLen_;
        bool bVerbose_;
        GetNodeName GetNodeName_;
        #endregion Data Members
        #region Methods
        public TreePrint(TextWriter treeOut, string src, int nMaxLineLen, GetNodeName GetNodeName, bool bVerbose)
        {
            treeOut_ = treeOut;
            nMaxLineLen_ = nMaxLineLen;
            bVerbose_ = bVerbose;
            GetNodeName_ = GetNodeName;
            src_ = src;
        }

        public void PrintTree(PegNode parent, int nOffsetLineBeg, int nLevel)
        {
            if (IsLeaf(parent))
            {
                PrintLeaf(parent, ref nOffsetLineBeg, false);
                treeOut_.Flush();
                return;
            }
            bool bAlignVertical =
                DetermineLineLength(parent, nOffsetLineBeg) > LenMaxLine();
            PrintNodeBeg(parent, bAlignVertical, ref nOffsetLineBeg, nLevel);
            int nOffset = nOffsetLineBeg;
            for (PegNode p = parent.child_; p != null; p = p.next_)
            {
                if (IsSkip(p)) continue;

                if (IsLeaf(p))
                {
                    PrintLeaf(p, ref nOffsetLineBeg, bAlignVertical);
                }
                else
                {
                    PrintTree(p, nOffsetLineBeg, nLevel + 1);
                }
                if (bAlignVertical)
                {
                    nOffsetLineBeg = nOffset;
                }
                while (p.next_ != null && IsSkip(p.next_)) p = p.next_;

                if (p.next_ != null)
                {
                    PrintDistNext(p, bAlignVertical, ref nOffsetLineBeg, nLevel);
                }
            }
            PrintNodeEnd(parent, bAlignVertical, ref  nOffsetLineBeg, nLevel);
            treeOut_.Flush();
        }
        int DetermineLineLength(PegNode parent, int nOffsetLineBeg)
        {
            int nLen = LenNodeBeg(parent);
            PegNode p;
            for (p = parent.child_; p != null; p = p.next_)
            {
                if (IsSkip(p)) continue;
                if (IsLeaf(p))
                {
                    nLen += LenLeaf(p);
                }
                else
                {
                    nLen += DetermineLineLength(p, nOffsetLineBeg);
                }
                if (nLen + nOffsetLineBeg > LenMaxLine())
                {
                    return nLen + nOffsetLineBeg;
                }
            }
            nLen += LenNodeEnd(p);
            return nLen;
        }
        public override int LenMaxLine() { return nMaxLineLen_; }
        public override void
            PrintNodeBeg(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel)
        {
            PrintIdAsName(p);
            treeOut_.Write("<");
            if (bAlignVertical)
            {
                treeOut_.WriteLine();
                treeOut_.Write(new string(' ', nOffsetLineBeg += 2));
            }
            else
            {
                ++nOffsetLineBeg;
            }
        }
        public override void
            PrintNodeEnd(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel)
        {
            if (bAlignVertical)
            {
                treeOut_.WriteLine();
                treeOut_.Write(new string(' ', nOffsetLineBeg -= 2));
            }
            treeOut_.Write('>');
            if (!bAlignVertical)
            {
                ++nOffsetLineBeg;
            }
        }
        public override int LenNodeBeg(PegNode p) { return LenIdAsName(p) + 1; }
        public override int LenNodeEnd(PegNode p) { return 1; }
        public override void PrintLeaf(PegNode p, ref int nOffsetLineBeg, bool bAlignVertical)
        {
            if (bVerbose_)
            {
                PrintIdAsName(p);
                treeOut_.Write('<');
            }
            int len = p.match_.posEnd_ - p.match_.posBeg_;
            treeOut_.Write("'");
            if (len > 0)
            {
                treeOut_.Write(src_.Substring(p.match_.posBeg_, p.match_.posEnd_ - p.match_.posBeg_));
            }
            treeOut_.Write("'");
            if (bVerbose_) treeOut_.Write('>');
        }
        public override int LenLeaf(PegNode p)
        {
            int nLen = p.match_.posEnd_ - p.match_.posBeg_ + 2;
            if (bVerbose_) nLen += LenIdAsName(p) + 2;
            return nLen;
        }
        public override bool IsLeaf(PegNode p)
        {
            return p.child_ == null;
        }

        public override void
            PrintDistNext(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel)
        {
            if (bAlignVertical)
            {
                treeOut_.WriteLine();
                treeOut_.Write(new string(' ', nOffsetLineBeg));
            }
            else
            {
                treeOut_.Write(' ');
                ++nOffsetLineBeg;
            }
        }

        public override int
            LenDistNext(PegNode p, bool bAlignVertical, ref int nOffsetLineBeg, int nLevel)
        {
            return 1;
        }
        int LenIdAsName(PegNode p)
        {
            string name = GetNodeName_(p);
            return name.Length;
        }
        void PrintIdAsName(PegNode p)
        {
            string name = GetNodeName_(p);
            treeOut_.Write(name);
        }
        #endregion Methods
    }
    #endregion Syntax/Parse-Tree related classes
    #region Parsers
    public abstract class PegBaseParser
    {
        #region Data Types
        public delegate bool Matcher();
        public delegate PegNode Creator(ECreatorPhase ePhase, PegNode parentOrCreated, int id);
        #endregion Data Types
        #region Data members
        protected int srcLen_;
        protected int pos_;
        protected bool bMute_;
        protected TextWriter errOut_;
        protected Creator nodeCreator_;
        PegTree tree;
        #endregion Data members
        public virtual string GetRuleNameFromId(int id)
        {//normally overridden
            switch (id)
            {
                case (int)ESpecialNodes.eFatal: return "FATAL";
                case (int)ESpecialNodes.eAnonymNTNode: return "Nonterminal";
                case (int)ESpecialNodes.eAnonymASTNode: return "ASTNode";
                case (int)ESpecialNodes.eAnonymousNode: return "Node";
                default: return id.ToString();
            }
        }
        public virtual void GetProperties(out EncodingClass encoding, out UnicodeDetection detection)
        {
            encoding = EncodingClass.ascii;
            detection = UnicodeDetection.notApplicable;
        }
        protected PegNode DefaultNodeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                return new PegNode(parentOrCreated, id);
            else return null;
        }
        #region Constructors
         public PegBaseParser(TextWriter errOut)
        {
            srcLen_ = pos_ = 0;
            errOut_ = errOut;
            nodeCreator_ = DefaultNodeCreator;
        }
        #endregion Constructors
        #region Reinitialization, TextWriter access,Tree Access
         public void Construct(TextWriter Fout)
         {
             srcLen_ = pos_ = 0;
             bMute_ = false;
             SetErrorDestination(Fout);
             ResetTree();
         }
         public void Rewind() { pos_ = 0; }
         public void SetErrorDestination(TextWriter errOut)
         {
             errOut_ = errOut == null ? new StreamWriter(System.Console.OpenStandardError())
                 : errOut;
         }
        #endregion Reinitialization, TextWriter access,Tree Access
         #region Tree root access, Tree Node generation/display
         public PegNode GetRoot() { return tree.root_; }
         public void ResetTree()
         {
             tree.root_ = null;
             tree.cur_ = null;
             tree.addPolicy = PegTree.AddPolicy.eAddAsChild;
         }
         void AddTreeNode(int nId, PegTree.AddPolicy newAddPolicy, Creator createNode, ECreatorPhase ePhase)
         {
             if (bMute_) return;
             if (tree.root_ == null)
             {
                 tree.root_ = tree.cur_ = createNode(ePhase, tree.cur_, nId);
             }
             else if (tree.addPolicy == PegTree.AddPolicy.eAddAsChild)
             {
                 tree.cur_ = tree.cur_.child_ = createNode(ePhase, tree.cur_, nId);
             }
             else
             {
                 tree.cur_ = tree.cur_.next_ = createNode(ePhase, tree.cur_.parent_, nId);
             }
             tree.addPolicy = newAddPolicy;
         }
         void RestoreTree(PegNode prevCur, PegTree.AddPolicy prevPolicy)
         {
             if (bMute_) return;
             if (prevCur == null)
             {
                 tree.root_ = null;
             }
             else if (prevPolicy == PegTree.AddPolicy.eAddAsChild)
             {
                 prevCur.child_ = null;
             }
             else
             {
                 prevCur.next_ = null;
             }
             tree.cur_ = prevCur;
             tree.addPolicy = prevPolicy;
         }
         public bool TreeChars(Matcher toMatch)
         {
             return TreeCharsWithId((int)ESpecialNodes.eAnonymousNode, toMatch);
         }
         public bool TreeChars(Creator nodeCreator, Matcher toMatch)
         {
             return TreeCharsWithId(nodeCreator, (int)ESpecialNodes.eAnonymousNode, toMatch);
         }
         public bool TreeCharsWithId(int nId, Matcher toMatch)
         {
             return TreeCharsWithId(nodeCreator_, nId, toMatch);
         }
         public bool TreeCharsWithId(Creator nodeCreator, int nId, Matcher toMatch)
         {
             int pos = pos_;
             if (toMatch())
             {
                 if (!bMute_)
                 {
                     AddTreeNode(nId, PegTree.AddPolicy.eAddAsSibling, nodeCreator, ECreatorPhase.eCreateAndComplete);
                     tree.cur_.match_.posBeg_ = pos;
                     tree.cur_.match_.posEnd_ = pos_;
                 }
                 return true;
             }
             return false;
         }
         public bool TreeNT(int nRuleId, Matcher toMatch)
         {
             return TreeNT(nodeCreator_, nRuleId, toMatch);
         }
         public bool TreeNT(Creator nodeCreator, int nRuleId, Matcher toMatch)
         {
             if (bMute_) return toMatch();
             PegNode prevCur = tree.cur_, ruleNode;
             PegTree.AddPolicy prevPolicy = tree.addPolicy;
             int posBeg = pos_;
             AddTreeNode(nRuleId, PegTree.AddPolicy.eAddAsChild, nodeCreator, ECreatorPhase.eCreate);
             ruleNode = tree.cur_;
             bool bMatches = toMatch();
             if (!bMatches) RestoreTree(prevCur, prevPolicy);
             else
             {
                 ruleNode.match_.posBeg_ = posBeg;
                 ruleNode.match_.posEnd_ = pos_;
                 tree.cur_ = ruleNode;
                 tree.addPolicy = PegTree.AddPolicy.eAddAsSibling;
                 nodeCreator(ECreatorPhase.eCreationComplete, ruleNode, nRuleId);
             }
             return bMatches;
         }
         public bool TreeAST(int nRuleId, Matcher toMatch)
         {
             return TreeAST(nodeCreator_, nRuleId, toMatch);
         }
         public bool TreeAST(Creator nodeCreator, int nRuleId, Matcher toMatch)
         {
             if (bMute_) return toMatch();
             bool bMatches = TreeNT(nodeCreator, nRuleId, toMatch);
             if (bMatches)
             {
                 if (tree.cur_.child_ != null && tree.cur_.child_.next_ == null && tree.cur_.parent_ != null)
                 {
                     if (tree.cur_.parent_.child_ == tree.cur_)
                     {
                         tree.cur_.parent_.child_ = tree.cur_.child_;
                         tree.cur_.child_.parent_ = tree.cur_.parent_;
                         tree.cur_ = tree.cur_.child_;
                     }
                     else
                     {
                         PegNode prev;
                         for (prev = tree.cur_.parent_.child_; prev != null && prev.next_ != tree.cur_; prev = prev.next_)
                         {
                         }
                         if (prev != null)
                         {
                             prev.next_ = tree.cur_.child_;
                             tree.cur_.child_.parent_ = tree.cur_.parent_;
                             tree.cur_ = tree.cur_.child_;
                         }
                     }
                 }
             }
             return bMatches;
         }
         public bool TreeNT(Matcher toMatch)
         {
             return TreeNT((int)ESpecialNodes.eAnonymNTNode, toMatch);
         }
         public bool TreeNT(Creator nodeCreator, Matcher toMatch)
         {
             return TreeNT(nodeCreator, (int)ESpecialNodes.eAnonymNTNode, toMatch);
         }
         public bool TreeAST(Matcher toMatch)
         {
             return TreeAST((int)ESpecialNodes.eAnonymASTNode, toMatch);
         }
         public bool TreeAST(Creator nodeCreator, Matcher toMatch)
         {
             return TreeAST(nodeCreator, (int)ESpecialNodes.eAnonymASTNode, toMatch);
         }
         public virtual string TreeNodeToString(PegNode node)
         {
             return GetRuleNameFromId(node.id_);
         }
         public void SetNodeCreator(Creator nodeCreator)
         {
             Debug.Assert(nodeCreator != null);
             nodeCreator_ = nodeCreator;
         }
         #endregion Tree Node generation
         #region PEG  e1 e2 .. ; &e1 ; !e1 ;  e? ; e* ; e+ ; e{a,b} ; .
         public bool And(Matcher pegSequence)
         {
             PegNode prevCur = tree.cur_;
             PegTree.AddPolicy prevPolicy = tree.addPolicy;
             int pos0 = pos_;
             bool bMatches = pegSequence();
             if (!bMatches)
             {
                 pos_ = pos0;
                 RestoreTree(prevCur, prevPolicy);
             }
             return bMatches;
         }
         public bool Peek(Matcher toMatch)
         {
             int pos0 = pos_;
             bool prevMute = bMute_;
             bMute_ = true;
             bool bMatches = toMatch();
             bMute_ = prevMute;
             pos_ = pos0;
             return bMatches;
         }
         public bool Not(Matcher toMatch)
         {
             int pos0 = pos_;
             bool prevMute = bMute_;
             bMute_ = true;
             bool bMatches = toMatch();
             bMute_ = prevMute;
             pos_ = pos0;
             return !bMatches;
         }
         public bool PlusRepeat(Matcher toRepeat)
         {
             int i;
             for (i = 0; ; ++i)
             {
                 int pos0 = pos_;
                 if (!toRepeat())
                 {
                     pos_ = pos0;
                     break;
                 }
             }
             return i > 0;
         }
         public bool OptRepeat(Matcher toRepeat)
         {
             for (; ; )
             {
                 int pos0 = pos_;
                 if (!toRepeat())
                 {
                     pos_ = pos0;
                     return true;
                 }
             }
         }
         public bool Option(Matcher toMatch)
         {
             int pos0 = pos_;
             if (!toMatch()) pos_ = pos0;
             return true;
         }
         public bool ForRepeat(int count, Matcher toRepeat)
         {
             PegNode prevCur = tree.cur_;
             PegTree.AddPolicy prevPolicy = tree.addPolicy;
             int pos0 = pos_;
             int i;
             for (i = 0; i < count; ++i)
             {
                 if (!toRepeat())
                 {
                     pos_ = pos0;
                     RestoreTree(prevCur, prevPolicy);
                     return false;
                 }
             }
             return true;
         }
         public bool ForRepeat(int lower, int upper, Matcher toRepeat)
         {
             PegNode prevCur = tree.cur_;
             PegTree.AddPolicy prevPolicy = tree.addPolicy;
             int pos0 = pos_;
             int i;
             for (i = 0; i < upper; ++i)
             {
                 if (!toRepeat()) break;
             }
             if (i < lower)
             {
                 pos_ = pos0;
                 RestoreTree(prevCur, prevPolicy);
                 return false;
             }
             return true;
         }
         public bool Any()
         {
             if (pos_ < srcLen_)
             {
                 ++pos_;
                 return true;
             }
             return false;
         }
         #endregion PEG  e1 e2 .. ; &e1 ; !e1 ;  e? ; e* ; e+ ; e{a,b} ; .
    }
    public class PegByteParser : PegBaseParser
    {
        #region Data members
        protected byte[] src_;
        PegError errors;
        #endregion Data members
        
        #region PEG optimizations
        public sealed class BytesetData     
        {
            public struct Range
            {
                public Range(byte low, byte high) { this.low = low; this.high = high; }
                public byte low;
                public byte high;
            }
            System.Collections.BitArray charSet_;
            bool bNegated_;
            public BytesetData(System.Collections.BitArray b)
                : this(b, false)
            {
            }
            public BytesetData(System.Collections.BitArray b, bool bNegated)
            {
                charSet_ = new System.Collections.BitArray(b);
                bNegated_ = bNegated;
            }
            public BytesetData(Range[] r, byte[] c)
                : this(r, c, false)
            {
            }
            public BytesetData(Range[] r, byte[] c, bool bNegated)
            {
                int max = 0;
                if (r != null) foreach (Range val in r) if (val.high > max) max = val.high;
                if (c != null) foreach (int val in c) if (val > max) max = val;
                charSet_ = new System.Collections.BitArray(max + 1, false);
                if (r != null)
                {
                    foreach (Range val in r)
                    {
                        for (int i = val.low; i <= val.high; ++i)
                        {
                            charSet_[i] = true;
                        }
                    }
                }
                if (c != null) foreach (int val in c) charSet_[val] = true;
                bNegated_ = bNegated;
            }
            public bool Matches(byte c)
            {
                bool bMatches = c < charSet_.Length && charSet_[(int)c];
                if (bNegated_) return !bMatches;
                else return bMatches;
            }
        }
        /*     public class BytesetData
             {
                 public struct Range
                 {
                     public Range(byte low, byte high) { this.low = low; this.high = high; }
                     public byte low;
                     public byte high;
                 }
                 protected System.Collections.BitArray charSet_;
                 bool bNegated_;
                 public BytesetData(System.Collections.BitArray b, bool bNegated)
                 {
                     charSet_ = new System.Collections.BitArray(b);
                     bNegated_ = bNegated;
                 }
                 public BytesetData(byte[] c, bool bNegated)
                 {
                     int max = 0;
                     foreach (int val in c) if (val > max) max = val;
                     charSet_ = new System.Collections.BitArray(max + 1, false);
                     foreach (int val in c) charSet_[val] = true;
                     bNegated_ = bNegated;
                 }
                 public BytesetData(Range[] r, byte[] c, bool bNegated)
                 {
                     int max = 0;
                     foreach (Range val in r) if (val.high > max) max = val.high;
                     foreach (int val in c) if (val > max) max = val;
                     charSet_ = new System.Collections.BitArray(max + 1, false);
                     foreach (Range val in r)
                     {
                         for (int i = val.low; i <= val.high; ++i)
                         {
                             charSet_[i] = true;
                         }
                     }
                     foreach (int val in c) charSet_[val] = true;
                 }


                 public bool Matches(byte c)
                 {
                     bool bMatches = c < charSet_.Length && charSet_[(int)c];
                     if (bNegated_) return !bMatches;
                     else return bMatches;
                 }
             }*/
        #endregion PEG optimizations
        #region Constructors
        public PegByteParser()
            : this(null)
        {
        }
        public PegByteParser(byte[] src):base(null)
        {
            SetSource(src);
        }
        public PegByteParser(byte[] src, TextWriter errOut):base(errOut)
        {
            SetSource(src);
        }
        #endregion Constructors
        #region Reinitialization, Source Code access, TextWriter access,Tree Access
        public void Construct(byte[] src, TextWriter Fout)
        {
            base.Construct(Fout);
            SetSource(src);
        }
        public void SetSource(byte[] src)
        {
            if (src == null) src = new byte[0];
            src_ = src; srcLen_ = src.Length;
            errors.lineStarts = new SortedList<int, int>();
            errors.lineStarts[0] = 1;
        }
        public byte[] GetSource() { return src_; }
        
        #endregion Reinitialization, Source Code access, TextWriter access,Tree Access
        #region Setting host variables
        public bool Into(Matcher toMatch,out byte[] into)
        {
            int pos = pos_;
            if (toMatch())
            {
                int nLen = pos_ - pos;
                into= new byte[nLen];
                for(int i=0;i<nLen;++i){
                    into[i] = src_[i+pos];
                }
                return true;
            }
            else
            {
                into = null;
                return false;
            }
        }
        public bool Into(Matcher toMatch,out PegBegEnd begEnd)
        {
            begEnd.posBeg_ = pos_;
            bool bMatches = toMatch();
            begEnd.posEnd_ = pos_;
            return bMatches;
        }
        public bool Into(Matcher toMatch,out int into)
        {
            byte[] s;
            into = 0;
            if (!Into(toMatch,out s)) return false;
            into = 0;
            for (int i = 0; i < s.Length; ++i)
            {
                into <<= 8;
                into |= s[i];
            }
            return true;
        }
        public bool Into(Matcher toMatch,out double into)
        {
            byte[] s;
            into = 0.0;
            if (!Into(toMatch,out s)) return false;
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            string sAsString = encoding.GetString(s);
            if (!System.Double.TryParse(sAsString, out into)) return false;
            return true;
        }
        public bool BitsInto(int lowBitNo, int highBitNo,out int into)
        {
            if (pos_ < srcLen_)
            {
                into = (src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1);
                ++pos_;
                return true;
            }
            into = 0;
            return false;
        }
        public bool BitsInto(int lowBitNo, int highBitNo, BytesetData toMatch, out int into)
        {
            if (pos_ < srcLen_)
            {
                byte value = (byte)((src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1));
                ++pos_;
                into = value;
                return toMatch.Matches(value);
            }
            into = 0;
            return false;
        }
        #endregion Setting host variables
        #region Error handling
        void LogOutMsg(string sErrKind, string sMsg)
        {
            errOut_.WriteLine("<{0}>{1}:{2}", pos_, sErrKind, sMsg);
            errOut_.Flush();
        }
        public virtual bool Fatal(string sMsg)
        {

            LogOutMsg("FATAL", sMsg);
            throw new PegException();
        }
        public bool Warning(string sMsg)
        {
            LogOutMsg("WARNING", sMsg);
            return true;
        }
        #endregion Error handling
       #region PEG Bit level equivalents for PEG e1 ; &e1 ; !e1; e1:into ; 
        public bool Bits(int lowBitNo, int highBitNo, byte toMatch)
        {
            if (pos_ < srcLen_ && ((src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch)
            {
                ++pos_;
                return true;
            }
            return false;
        }
        public bool Bits(int lowBitNo, int highBitNo,BytesetData toMatch)
        {
            if( pos_ < srcLen_ )
            {
                byte value= (byte)((src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1));
                ++pos_;
                return toMatch.Matches(value);
            }
            return false;
        }
        public bool PeekBits(int lowBitNo, int highBitNo, byte toMatch)
        {
            return pos_ < srcLen_ && ((src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch;
        }
        public bool NotBits(int lowBitNo, int highBitNo, byte toMatch)
        {
            return !(pos_ < srcLen_ && ((src_[pos_] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch);
        }
        public bool IntoBits(int lowBitNo,int highBitNo,out int val)
        {
            return BitsInto(lowBitNo,highBitNo,out val);
        }
        public bool IntoBits(int lowBitNo, int highBitNo, BytesetData toMatch, out int val)
        {
            return BitsInto(lowBitNo, highBitNo, out val);
        }
        public bool Bit(int bitNo,byte toMatch)
        {
            if (pos_ < srcLen_ && ((src_[pos_]>>(bitNo-1))&1)==toMatch){
                ++pos_;
                return true;
            }
            return false;
        }
        public bool PeekBit(int bitNo, byte toMatch)
        {
            return pos_ < srcLen_ && ((src_[pos_] >> (bitNo - 1)) & 1) == toMatch;
        }
        public bool NotBit(int bitNo, byte toMatch)
        {
            return !(pos_ < srcLen_ && ((src_[pos_] >> (bitNo - 1)) & 1) == toMatch);
        }
        #endregion PEG Bit level equivalents for PEG e1 ; &e1 ; !e1; e1:into ;
        #region PEG '<Literal>' / '<Literal>'/i / [low1-high1,low2-high2..] / [<CharList>]
        public bool Char(byte c1)
        {
            if (pos_ < srcLen_ && src_[pos_] == c1)
            { ++pos_; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2)
        {
            if (pos_ + 1 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2)
            { pos_ += 2; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3)
        {
            if (pos_ + 2 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3)
            { pos_ += 3; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4)
        {
            if (pos_ + 3 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4)
            { pos_ += 4; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (pos_ + 4 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5)
            { pos_ += 5; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (pos_ + 5 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6)
            { pos_ += 6; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (pos_ + 6 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6
                && src_[pos_ + 6] == c7)
            { pos_ += 7; return true; }
            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7, byte c8)
        {
            if (pos_ + 7 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6
                && src_[pos_ + 6] == c7
                && src_[pos_ + 7] == c8)
            { pos_ += 8; return true; }
            return false;
        }
        public bool Char(byte[] s)
        {
            int sLength = s.Length;
            if (pos_ + sLength > srcLen_) return false;
            for (int i = 0; i < sLength; ++i)
            {
                if (s[i] != src_[pos_ + i]) return false;
            }
            pos_ += sLength;
            return true;
        }
        public static byte ToUpper(byte c)
        {
            if (c >= 97 && c <= 122) return (byte)(c - 32); else return c;
        }
        public bool IChar(byte c1)
        {
            if (pos_ < srcLen_ && ToUpper(src_[pos_]) == c1)
            { ++pos_; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2)
        {
            if (pos_ + 1 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2))
            { pos_ += 2; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3)
        {
            if (pos_ + 2 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2)
                && ToUpper(src_[pos_ + 2]) == ToUpper(c3))
            { pos_ += 3; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4)
        {
            if (pos_ + 3 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2)
                && ToUpper(src_[pos_ + 2]) == ToUpper(c3)
                && ToUpper(src_[pos_ + 3]) == ToUpper(c4))
            { pos_ += 4; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (pos_ + 4 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2)
                && ToUpper(src_[pos_ + 2]) == ToUpper(c3)
                && ToUpper(src_[pos_ + 3]) == ToUpper(c4)
                && ToUpper(src_[pos_ + 4]) == ToUpper(c5))
            { pos_ += 5; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (pos_ + 5 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2)
                && ToUpper(src_[pos_ + 2]) == ToUpper(c3)
                && ToUpper(src_[pos_ + 3]) == ToUpper(c4)
                && ToUpper(src_[pos_ + 4]) == ToUpper(c5)
                && ToUpper(src_[pos_ + 5]) == ToUpper(c6))
            { pos_ += 6; return true; }
            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (pos_ + 6 < srcLen_
                && ToUpper(src_[pos_]) == ToUpper(c1)
                && ToUpper(src_[pos_ + 1]) == ToUpper(c2)
                && ToUpper(src_[pos_ + 2]) == ToUpper(c3)
                && ToUpper(src_[pos_ + 3]) == ToUpper(c4)
                && ToUpper(src_[pos_ + 4]) == ToUpper(c5)
                && ToUpper(src_[pos_ + 5]) == ToUpper(c6)
                && ToUpper(src_[pos_ + 6]) == ToUpper(c7))
            { pos_ += 7; return true; }
            return false;
        }
        public bool IChar(byte[] s)
        {
            int sLength = s.Length;
            if (pos_ + sLength > srcLen_) return false;
            for (int i = 0; i < sLength; ++i)
            {
                if (s[i] != ToUpper(src_[pos_ + i])) return false;
            }
            pos_ += sLength;
            return true;
        }
        public bool In(byte c0, byte c1)
        {
            if (pos_ < srcLen_
                && src_[pos_] >= c0 && src_[pos_] <= c1)
            {
                ++pos_;
                return true;
            }
            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3
                    || c >= c4 && c <= c5)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3
                    || c >= c4 && c <= c5
                    || c >= c6 && c <= c7)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(byte[] s)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if (c >= s[i] && c <= s[i + 1])
                    {
                        ++pos_;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool NotIn(byte[] s)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if ( c >= s[i] && c <= s[i + 1] ) return false;
                }
                ++pos_;
                return true;
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1)
        {
            if (pos_ < srcLen_
                && (src_[pos_] == c0 || src_[pos_] == c1))
            {
                ++pos_;
                return true;
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2, byte c3)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6 || c == c7)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(byte[] s)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                for (int i = 0; i < s.Length; ++i)
                {
                    if (c == s[i]) { ++pos_; return true; }
                }
            }
            return false;
        }
        public bool NotOneOf(byte[] s)
        {
            if (pos_ < srcLen_)
            {
                byte c = src_[pos_];
                for (int i = 0; i < s.Length; ++i)
                {
                    if (c == s[i]) { return false; }
                }
                return true;
            }
            return false;
        }
        public bool OneOf(BytesetData bset)
        {
            if(pos_ < srcLen_ && bset.Matches(src_[pos_]))
            {
                ++pos_; return true;
            }
            return false;
        }
        #endregion PEG '<Literal>' / '<Literal>'/i / [low1-high1,low2-high2..] / [<CharList>]
    }
    public class PegCharParser : PegBaseParser
    {
        #region Data members
        protected string src_;
        PegError errors;
        #endregion Data members
        #region PEG optimizations
        public sealed class OptimizedCharset
        {
            public struct Range
            {
                public Range(char low, char high) { this.low = low; this.high = high; }
                public char low;
                public char high;
            }
            System.Collections.BitArray charSet_;
            bool bNegated_;
            public OptimizedCharset(System.Collections.BitArray b)
                : this(b, false)
            {
            }
            public OptimizedCharset(System.Collections.BitArray b, bool bNegated)
            {
                charSet_ = new System.Collections.BitArray(b);
                bNegated_ = bNegated;
            }
            public OptimizedCharset(Range[] r, char[] c)
                : this(r, c, false)
            {
            }
            public OptimizedCharset(Range[] r, char[] c, bool bNegated)
            {
                int max = 0;
                if (r != null) foreach (Range val in r) if (val.high > max) max = val.high;
                if (c != null) foreach (int val in c) if (val > max) max = val;
                charSet_ = new System.Collections.BitArray(max + 1, false);
                if (r != null)
                {
                    foreach (Range val in r)
                    {
                        for (int i = val.low; i <= val.high; ++i)
                        {
                            charSet_[i] = true;
                        }
                    }
                }
                if (c != null) foreach (int val in c) charSet_[val] = true;
                bNegated_ = bNegated;
            }


            public bool Matches(char c)
            {
                bool bMatches = c < charSet_.Length && charSet_[(int)c];
                if (bNegated_) return !bMatches;
                else return bMatches;
            }
        }
        public sealed class OptimizedLiterals
        {
            internal class Trie
            {
                internal Trie(char cThis,int nIndex, string[] literals)
                {
                    cThis_ = cThis;
                    char cMax = char.MinValue;
                    cMin_ = char.MaxValue;
                    HashSet<char> followChars = new HashSet<char>();
                    
                    foreach (string literal in literals)
                    {
                        if (literal==null ||  nIndex > literal.Length ) continue;
                        if (nIndex == literal.Length)
                        {
                            bLitEnd_ = true;
                            continue;
                        }
                        char c = literal[nIndex];
                        followChars.Add(c);
                        if ( c < cMin_) cMin_ = c;
                        if ( c > cMax) cMax = c;
                    }
                    if (followChars.Count == 0)
                    {
                        children_ = null;
                    }
                    else
                    {
                        children_ = new Trie[(cMax - cMin_) + 1];
                        foreach (char c in followChars)
                        {
                            List<string> subLiterals = new List<string>();
                            foreach (string s in literals)
                            {
                                if ( nIndex >= s.Length ) continue;
                                if (c == s[nIndex])
                                {
                                    subLiterals.Add(s);
                                }
                            }
                            children_[c - cMin_] = new Trie(c, nIndex + 1, subLiterals.ToArray());
                        }
                    }

                }
                internal char cThis_;           //character stored in this node
                internal bool bLitEnd_;         //end of literal

                internal char cMin_;            //first valid character in children
                internal Trie[] children_;      //contains the successor node of cThis_;
            }
            internal Trie literalsRoot;
            public OptimizedLiterals(string[] litAlternatives)
            {
                literalsRoot = new Trie('\u0000', 0, litAlternatives);
            }
        }
        #endregion  PEG optimizations
        #region Constructors
        public PegCharParser():this("")
        {
           

        }
        public PegCharParser(string src):base(null)
        {
            SetSource(src);
        }
        public PegCharParser(string src, TextWriter errOut):base(errOut)
        {
            SetSource(src);
            nodeCreator_ = DefaultNodeCreator;
        }
        #endregion Constructors
        #region Overrides
        public override string TreeNodeToString(PegNode node)
        {
            string label = base.TreeNodeToString(node);
            if (node.id_ == (int)ESpecialNodes.eAnonymousNode)
            {
                string value = node.GetAsString(src_);
                if (value.Length < 32) label += " <" + value + ">";
                else label += " <" + value.Substring(0, 29) + "...>";
            }
            return label;
        }
        #endregion Overrides
        #region Reinitialization, Source Code access, TextWriter access,Tree Access
        public void Construct(string src, TextWriter Fout)
        {
            base.Construct(Fout);
            SetSource(src);
        }
        public void SetSource(string src)
        {
            if (src == null) src = "";
            src_ = src; srcLen_ = src.Length; pos_ = 0;
            errors.lineStarts = new SortedList<int, int>();
            errors.lineStarts[0] = 1;
        }
        public string GetSource() { return src_; }
        #endregion Reinitialization, Source Code access, TextWriter access,Tree Access
        #region Setting host variables
        public bool Into(Matcher toMatch,out string into)
        {
            int pos = pos_;
            if (toMatch())
            {
                into = src_.Substring(pos, pos_ - pos);
                return true;
            }
            else
            {
                into = "";
                return false;
            }
        }
        public bool Into(Matcher toMatch,out PegBegEnd begEnd)
        {
            begEnd.posBeg_ = pos_;
            bool bMatches = toMatch();
            begEnd.posEnd_ = pos_;
            return bMatches;
        }
        public bool Into(Matcher toMatch,out int into)
        {
            string s;
            into = 0;
            if (!Into(toMatch,out s)) return false;
            if (!System.Int32.TryParse(s, out into)) return false;
            return true;
        }
        public bool Into(Matcher toMatch,out double into)
        {
            string s;
            into = 0.0;
            if (!Into(toMatch,out s)) return false;
            if (!System.Double.TryParse(s, out into)) return false;
            return true;
        }
        #endregion Setting host variables
        #region Error handling
        void LogOutMsg(string sErrKind, string sMsg)
        {
            int lineNo, colNo;
            errors.GetLineAndCol(src_, pos_, out lineNo, out colNo);
            errOut_.WriteLine("<{0},{1}>{2}:{3}", lineNo, colNo, sErrKind, sMsg);
            errOut_.Flush();
        }
        public virtual bool Fatal(string sMsg)
        {

            LogOutMsg("FATAL", sMsg);
            throw new PegException();
            //return false;
        }
        public bool Warning(string sMsg)
        {
            LogOutMsg("WARNING", sMsg);
            return true;
        }
        #endregion Error handling
        #region PEG  optimized version of  e* ; e+ 
        public bool OptRepeat(OptimizedCharset charset)
        {
            for (; pos_ < srcLen_ && charset.Matches(src_[pos_]); ++pos_) ;
            return true;
        }
        public bool PlusRepeat(OptimizedCharset charset)
        {
            int pos0 = pos_;
            for (; pos_ < srcLen_ && charset.Matches(src_[pos_]); ++pos_) ;
            return pos_ > pos0;
        }
        #endregion PEG  optimized version of  e* ; e+
        #region PEG '<Literal>' / '<Literal>'/i / [low1-high1,low2-high2..] / [<CharList>]
        public bool Char(char c1)
        {
            if (pos_ < srcLen_ && src_[pos_] == c1)
            { ++pos_; return true; }
            return false;
        }
        public bool Char(char c1, char c2)
        {
            if (pos_ + 1 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2)
            { pos_ += 2; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3)
        {
            if (pos_ + 2 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3)
            { pos_ += 3; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3, char c4)
        {
            if (pos_ + 3 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4)
            { pos_ += 4; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3, char c4, char c5)
        {
            if (pos_ + 4 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5)
            { pos_ += 5; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (pos_ + 5 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6)
            { pos_ += 6; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (pos_ + 6 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6
                && src_[pos_ + 6] == c7)
            { pos_ += 7; return true; }
            return false;
        }
        public bool Char(char c1, char c2, char c3, char c4, char c5, char c6, char c7, char c8)
        {
            if (pos_ + 7 < srcLen_
                && src_[pos_] == c1
                && src_[pos_ + 1] == c2
                && src_[pos_ + 2] == c3
                && src_[pos_ + 3] == c4
                && src_[pos_ + 4] == c5
                && src_[pos_ + 5] == c6
                && src_[pos_ + 6] == c7
                && src_[pos_ + 7] == c8)
            { pos_ += 8; return true; }
            return false;
        }
        public bool Char(string s)
        {
            int sLength = s.Length;
            if (pos_ + sLength > srcLen_) return false;
            for (int i = 0; i < sLength; ++i)
            {
                if (s[i] != src_[pos_ + i]) return false;
            }
            pos_ += sLength;
            return true;
        }
        public bool IChar(char c1)
        {
            if (pos_ < srcLen_ && System.Char.ToUpper(src_[pos_]) == c1)
            { ++pos_; return true; }
            return false;
        }
        public bool IChar(char c1, char c2)
        {
            if (pos_ + 1 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2))
            { pos_ += 2; return true; }
            return false;
        }
        public bool IChar(char c1, char c2, char c3)
        {
            if (pos_ + 2 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2)
                && System.Char.ToUpper(src_[pos_ + 2]) == System.Char.ToUpper(c3))
            { pos_ += 3; return true; }
            return false;
        }
        public bool IChar(char c1, char c2, char c3, char c4)
        {
            if (pos_ + 3 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2)
                && System.Char.ToUpper(src_[pos_ + 2]) == System.Char.ToUpper(c3)
                && System.Char.ToUpper(src_[pos_ + 3]) == System.Char.ToUpper(c4))
            { pos_ += 4; return true; }
            return false;
        }
        public bool IChar(char c1, char c2, char c3, char c4, char c5)
        {
            if (pos_ + 4 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2)
                && System.Char.ToUpper(src_[pos_ + 2]) == System.Char.ToUpper(c3)
                && System.Char.ToUpper(src_[pos_ + 3]) == System.Char.ToUpper(c4)
                && System.Char.ToUpper(src_[pos_ + 4]) == System.Char.ToUpper(c5))
            { pos_ += 5; return true; }
            return false;
        }
        public bool IChar(char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (pos_ + 5 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2)
                && System.Char.ToUpper(src_[pos_ + 2]) == System.Char.ToUpper(c3)
                && System.Char.ToUpper(src_[pos_ + 3]) == System.Char.ToUpper(c4)
                && System.Char.ToUpper(src_[pos_ + 4]) == System.Char.ToUpper(c5)
                && System.Char.ToUpper(src_[pos_ + 5]) == System.Char.ToUpper(c6))
            { pos_ += 6; return true; }
            return false;
        }
        public bool IChar(char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (pos_ + 6 < srcLen_
                && System.Char.ToUpper(src_[pos_]) == System.Char.ToUpper(c1)
                && System.Char.ToUpper(src_[pos_ + 1]) == System.Char.ToUpper(c2)
                && System.Char.ToUpper(src_[pos_ + 2]) == System.Char.ToUpper(c3)
                && System.Char.ToUpper(src_[pos_ + 3]) == System.Char.ToUpper(c4)
                && System.Char.ToUpper(src_[pos_ + 4]) == System.Char.ToUpper(c5)
                && System.Char.ToUpper(src_[pos_ + 5]) == System.Char.ToUpper(c6)
                && System.Char.ToUpper(src_[pos_ + 6]) == System.Char.ToUpper(c7))
            { pos_ += 7; return true; }
            return false;
        }
        public bool IChar(string s)
        {
            int sLength = s.Length;
            if (pos_ + sLength > srcLen_) return false;
            for (int i = 0; i < sLength; ++i)
            {
                if (s[i] != System.Char.ToUpper(src_[pos_ + i])) return false;
            }
            pos_ += sLength;
            return true;
        }

        public bool In(char c0, char c1)
        {
            if (pos_ < srcLen_
                && src_[pos_] >= c0 && src_[pos_] <= c1)
            {
                ++pos_;
                return true;
            }
            return false;
        }
        public bool In(char c0, char c1, char c2, char c3)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(char c0, char c1, char c2, char c3, char c4, char c5)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3
                    || c >= c4 && c <= c5)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(char c0, char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c >= c0 && c <= c1
                    || c >= c2 && c <= c3
                    || c >= c4 && c <= c5
                    || c >= c6 && c <= c7)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool In(string s)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if (!(c >= s[i] && c <= s[i + 1])) return false;
                }
                ++pos_;
                return true;
            }
            return false;
        }
        public bool NotIn(string s)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if ( c >= s[i] && c <= s[i + 1]) return false;
                }
                ++pos_;
                return true;
            }
            return false;
        }
        public bool OneOf(char c0, char c1)
        {
            if (pos_ < srcLen_
                && (src_[pos_] == c0 || src_[pos_] == c1))
            {
                ++pos_;
                return true;
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2, char c3)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2, char c3, char c4)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5, char c6)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(char c0, char c1, char c2, char c3, char c4, char c5, char c6, char c7)
        {
            if (pos_ < srcLen_)
            {
                char c = src_[pos_];
                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6 || c == c7)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(string s)
        {
            if (pos_ < srcLen_)
            {
                if (s.IndexOf(src_[pos_]) != -1)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool NotOneOf(string s)
        {
            if (pos_ < srcLen_)
            {
                if (s.IndexOf(src_[pos_]) == -1)
                {
                    ++pos_;
                    return true;
                }
            }
            return false;
        }
        public bool OneOf(OptimizedCharset cset)
        {
            if (pos_ < srcLen_ && cset.Matches(src_[pos_]))
            {
                ++pos_; return true;
            }
            return false;
        }
        public bool OneOfLiterals(OptimizedLiterals litAlt)
        {
            OptimizedLiterals.Trie node = litAlt.literalsRoot;
            int matchPos = pos_-1;
            for (int pos = pos_; pos < srcLen_ ; ++pos)
            {
                if (node.bLitEnd_) matchPos = pos;
                char c = src_[pos];
                if (    node.children_==null 
                    ||  c < node.cMin_ || c > node.cMin_ + node.children_.Length - 1
                    ||  node.children_[c - node.cMin_] == null)
                {
                    break;
                }
                node = node.children_[c - node.cMin_];
            }
            if (matchPos >= pos_)
            {
                pos_= matchPos;
                return true;
            }
            else return false;
        }
        #endregion PEG '<Literal>' / '<Literal>'/i / [low1-high1,low2-high2..] / [<CharList>]
    }
    #endregion Parsers
}
