/* created on 21.09.2008 15:46:56 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace BERTree
{
      
      enum EBERTree{ProtocolDataUnit= 1, TLV= 2, ConstructedLengthValue= 30, Tag= 3, 
                     TagWithConstructedFlag= 4, OneOctetTag= 5, MultiOctetTag= 6, 
                     Length= 7, OneOctetLength= 8, NoLength= 9, MultiOctetLength= 10, 
                     PrimitiveValue= 11, ConstructedDelimValue= 12, ConstructedValue= 13};
      class BERTree : PegByteParser 
      {
        class Top
{
    internal int @byte;
    internal int tag, length, n,tagclass;
    internal bool init_()       { tag = 0; length = 0; return true; }
    internal bool add_Tag_()    { tag *= 128; tag += n; return true; }
    internal bool addLength_()  { length *= 256; length+= @byte;return true; }
}

Top top;
#region CREATE

	#region overrides
	public override string TreeNodeToString(PegNode node)
        {
            string s= GetRuleNameFromId(node.id_);
            BERTreeNode berNode= node as BERTreeNode;
            if( berNode!=null ) s+= ": " + berNode.TreeNodeToString(src_);
            return s;
        }
	#endregion overrides
	 #region PegNode subclasses for BERTree
        abstract class BERTreeNode : PegNode
        {
            internal BERTreeNode(PegNode parent, int id): base(parent, id){}
            internal abstract string TreeNodeToString(byte[] src);
        }
        class TagNode : BERTreeNode
        {
            internal TagNode(PegNode parent, int id) : base(parent, id)  {tagValue_ = -1;}
            internal override string TreeNodeToString(byte[] src)        {return tagValue_.ToString();}
           
            internal int tagValue_;

        }
        class LengthNode : BERTreeNode
        {
            internal LengthNode(PegNode parent, int id) : base(parent, id) { lengthValue_ = 0;}
            internal override string TreeNodeToString(byte[] src)
            {
                if (lengthValue_ <= 0) return "\u221E";
                return lengthValue_.ToString();
            }
            
            internal int lengthValue_;
        }
        class PrimitiveValueNode : BERTreeNode
        {
            const int maxShow = 16;
            internal PrimitiveValueNode(PegNode parent, int id)
                : base(parent, id)
            { }
             bool GetAsInteger(byte[] src, out string sInt)
            {
                int len = match_.Length, pos0 = match_.posBeg_, pos1 = match_.posEnd_;
                sInt = "";
                if (len == 0 || src[pos0] == 0 && len > 1 && (src[pos0 + 1] & 0x80) == 0) return false;
                long val = (src[pos0] & 0x80) != 0 ? -1 : 0;
                for (; pos0 != pos1; ++pos0)
                {
                    val <<= 8;
                    val |= src[pos0];
                }
                sInt = val.ToString();
                return true;
            }
            string GetAsAsciiString(byte[] src)
            {
                int pos0 = match_.posBeg_, pos1 = match_.posEnd_;
                if (pos1 - pos0 > 16) pos1 = pos0 + 16;
                StringBuilder sb= new StringBuilder();
                for (; pos0 < pos1; ++pos0){
                    if (src[pos0] <= 0x7F && !char.IsControl(((char)src[pos0]))){
                        sb.Append((char)src[pos0]);
                    }
                    else{
                        sb.Append('.');
                    }
                }
                if (match_.posEnd_ > pos1) sb.Append("...");
                return sb.ToString();
            }
            internal override string TreeNodeToString(byte[] src)
            {
                string display="";
                if (match_.Length <= 8 && GetAsInteger(src, out display)){
                    display += " / ";
                }
                display += GetAsAsciiString(src);
                return display;
            }
        }
        #endregion PegNode subclasses for BERTree	

	    PegNode TagNodeCreator(ECreatorPhase phase,PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete){
                return new TagNode(parentOrCreated, id);
            }else{
                ((TagNode)parentOrCreated).tagValue_ = top.tag;
                return null;
            }
        }
	    PegNode LengthNodeCreator(ECreatorPhase phase,PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete){
                return new LengthNode(parentOrCreated, id);
            }else{
                ((LengthNode)parentOrCreated).lengthValue_ = top.length;
                return null;
            }
        }
        PegNode PrimitiveValueNodeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
           if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
           {
               return new PrimitiveValueNode(parentOrCreated, id);
           }
           else
          {
            return null;
          }
    }
#endregion CREATE

         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.binary;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public BERTree()
            : base()
        {
            top= new Top();

        }
        public BERTree(byte[] src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            top= new Top();

        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   EBERTree ruleEnum = (EBERTree)id;
                    string s= ruleEnum.ToString();
                    int val;
                    if( int.TryParse(s,out val) ){
                        return base.GetRuleNameFromId(id);
                    }else{
                        return s;
                    }
            }
            catch (Exception)
            {
                return base.GetRuleNameFromId(id);
            }
        }
        public override void GetProperties(out EncodingClass encoding, out UnicodeDetection detection)
        {
            encoding = encodingClass;
            detection = unicodeDetection;
        } 
        #endregion Overrides
		#region Grammar Rules
        public bool ProtocolDataUnit()    /*[1] ProtocolDataUnit:   TLV;*/
        {

           return TLV();
		}
        public bool TLV()    /*[2] ^^TLV:              init_
                        (  TagWithConstructedFlag ConstructedLengthValue 
		        / Tag Length PrimitiveValue
                        );*/
        {

           return TreeNT((int)EBERTree.TLV,()=>
                And(()=>  
                     top.init_()
                  && (    
                         And(()=>      
                               TagWithConstructedFlag()
                            && ConstructedLengthValue() )
                      || And(()=>    Tag() && Length() && PrimitiveValue() )) ) );
		}
        public bool ConstructedLengthValue()    /*[30]ConstructedLengthValue:
                           NoLength  ConstructedDelimValue  @(#0#0) 
                        /  Length ConstructedValue;*/
        {

           return   
                     And(()=>    
                         NoLength()
                      && ConstructedDelimValue()
                      && (      
                               And(()=>    Char(0x0000) && Char(0x0000) )
                            || Fatal("<<(#0#0)>> expected")) )
                  || And(()=>    Length() && ConstructedValue() );
		}
        public bool Tag()    /*[3]Tag:                 &BITS<7-8,.,:tagclass>
                        (OneOctetTag / MultiOctetTag / FATAL<"illegal TAG">);*/
        {

           return And(()=>  
                     Peek(()=> BitsInto(7,8,out top.tagclass) )
                  && (    
                         OneOctetTag()
                      || MultiOctetTag()
                      || Fatal("illegal TAG")) );
		}
        public bool TagWithConstructedFlag()    /*[4] TagWithConstructedFlag: 
                        &BITS<6,#1> Tag;*/
        {

           return And(()=>    PeekBit(6,0x0001) && Tag() );
		}
        public bool OneOctetTag()    /*[5] ^^CREATE<TagNodeCreator> OneOctetTag:   
                        !BITS<1-5,#b11111> BITS<1-5,.,:tag>;*/
        {

           return TreeNT(TagNodeCreator,(int)EBERTree.OneOctetTag,()=>
                And(()=>  
                     NotBits(1,5,0x001f)
                  && BitsInto(1,5,out top.tag) ) );
		}
        public bool MultiOctetTag()    /*[6] ^^CREATE<TagNodeCreator> MultiOctetTag: 
                        . 
                        (&BITS<8,#1> BITS<1-7,.,:n> add_Tag_)* 
                        BITS<1-7,.,:n> add_Tag_;*/
        {

           return TreeNT(TagNodeCreator,(int)EBERTree.MultiOctetTag,()=>
                And(()=>  
                     Any()
                  && OptRepeat(()=>    
                      And(()=>      
                               PeekBit(8,0x0001)
                            && BitsInto(1,7,out top.n)
                            && top.add_Tag_() ) )
                  && BitsInto(1,7,out top.n)
                  && top.add_Tag_() ) );
		}
        public bool Length()    /*[7] Length :              OneOctetLength 
                        / NoLength 
                        / MultiOctetLength 
                        / FATAL<"illegal LENGTH">;*/
        {

           return   
                     OneOctetLength()
                  || NoLength()
                  || MultiOctetLength()
                  || Fatal("illegal LENGTH");
		}
        public bool OneOctetLength()    /*[8]^^CREATE<LengthNodeCreator> OneOctetLength: 
                        &BITS<8,#0> BITS<1-7,.,:length>;*/
        {

           return TreeNT(LengthNodeCreator,(int)EBERTree.OneOctetLength,()=>
                And(()=>  
                     PeekBit(8,0x0000)
                  && BitsInto(1,7,out top.length) ) );
		}
        public bool NoLength()    /*[9]^^CREATE<LengthNodeCreator> NoLength: 
                        #x80;*/
        {

           return TreeNT(LengthNodeCreator,(int)EBERTree.NoLength,()=>
                Char(0x0080) );
		}
        public bool MultiOctetLength()    /*[10]^^CREATE<LengthNodeCreator> MultiOctetLength: 
                        &BITS<8,#1> BITS<1-7,.,:n> 
                        (( .:byte addLength_){:n}/FATAL<"illegal Length">) ;*/
        {

           return TreeNT(LengthNodeCreator,(int)EBERTree.MultiOctetLength,()=>
                And(()=>  
                     PeekBit(8,0x0001)
                  && BitsInto(1,7,out top.n)
                  && (    
                         ForRepeat(top.n,top.n,()=>      
                            And(()=>        
                                       Into(()=> Any(),out top.@byte)
                                    && top.addLength_() ) )
                      || Fatal("illegal Length")) ) );
		}
        public bool PrimitiveValue()    /*[11]^^CREATE<PrimitiveValueNodeCreator> PrimitiveValue: 
	                (.{:length} / FATAL<"BER input ends before VALUE ends">);*/
        {

           return TreeNT(PrimitiveValueNodeCreator,(int)EBERTree.PrimitiveValue,()=>
                  
                     ForRepeat(top.length,top.length,()=> Any() )
                  || Fatal("BER input ends before VALUE ends") );
		}
        public bool ConstructedDelimValue()    /*[12]^^ConstructedDelimValue:
                        (!(#0#0) TLV)*;*/
        {

           return TreeNT((int)EBERTree.ConstructedDelimValue,()=>
                OptRepeat(()=>  
                  And(()=>    
                         Not(()=> And(()=>    Char(0x0000) && Char(0x0000) ) )
                      && TLV() ) ) );
		}
   class _ConstructedValue{
    internal _ConstructedValue(BERTree ber)
    {
        parent = ber; len = 0; begEnd.posBeg_ = 0; begEnd.posEnd_ = 0;
    }
    BERTree parent;
    int len;
    internal PegBegEnd  begEnd;
    internal bool save_() { len = parent.top.length; return true; }
    internal bool at_end_() { return len <= 0; }
    internal bool decr_()
    {
        len -= begEnd.posEnd_ - begEnd.posBeg_;
        return len >= 0;
    } 
}
        public bool ConstructedValue()    /*[13]^^ConstructedValue
{
    internal _ConstructedValue(BERTree ber)
    {
        parent = ber; len = 0; begEnd.posBeg_ = 0; begEnd.posEnd_ = 0;
    }
    BERTree parent;
    int len;
    PegBegEnd  begEnd;
    bool save_() { len = parent.top.length; return true; }
    bool at_end_() { return len <= 0; }
    bool decr_()
    {
        len -= begEnd.posEnd_ - begEnd.posBeg_;
        return len >= 0;
    } 
}:                      save_
                        (!at_end_ TLV:begEnd (decr_/FATAL<"illegal length">))*;*/
        {

             var _sem= new _ConstructedValue(this);

           return TreeNT((int)EBERTree.ConstructedValue,()=>
                And(()=>  
                     _sem.save_()
                  && OptRepeat(()=>    
                      And(()=>      
                               Not(()=> _sem.at_end_() )
                            && Into(()=> TLV(),out _sem.begEnd)
                            && (    _sem.decr_() || Fatal("illegal length")) ) ) ) );
		}
		#endregion Grammar Rules
   }
}