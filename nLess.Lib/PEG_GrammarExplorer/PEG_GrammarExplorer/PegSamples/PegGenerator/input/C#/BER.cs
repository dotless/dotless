/* created on 01.10.2008 17:09:31 from peg generator V1.0 using 'BER_peg.txt' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace BER
{
      
      enum EBER{ProtocolDataUnit= 1, TLV= 2, Tag= 3, OneOctetTag= 4, MultiOctetTag= 5, 
                 Length= 6, OneOctetLength= 7, MultiOctetLength= 8, PrimitiveValue= 9, 
                 CompositeDelimValue= 10, CompositeValue= 11};
      class BER : PegByteParser 
      {
        class _Top{
           internal int tag,length,n,@byte;
           internal bool init_()     {tag=0;length=0;           return true;}
           internal bool add_Tag_()  {tag*=128;tag+=n;          return true;}
           internal bool addLength_(){length*=256;length+=@byte;return true;}
        }
        
        _Top _top;
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.binary;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public BER()
            : base()
        {
            _top= new _Top();

        }
        public BER(byte[] src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            _top= new _Top();

        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   EBER ruleEnum = (EBER)id;
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
        public bool ProtocolDataUnit()    /*[1] ProtocolDataUnit: TLV;*/
        {

           return TLV();
		}
        public bool TLV()    /*[2] ^^TLV:  init_
     ( &BITS<6,#1> Tag ( #x80  CompositeDelimValue  #0#0 / Length CompositeValue )
     / Tag Length PrimitiveValue
     );*/
        {

           return TreeNT((int)EBER.TLV,()=>
                And(()=>  
                     _top.init_()
                  && (    
                         And(()=>      
                               PeekBit(6,0x0001)
                            && Tag()
                            && (        
                                       And(()=>          
                                                 Char(0x0080)
                                              && CompositeDelimValue()
                                              && Char(0x0000)
                                              && Char(0x0000) )
                                    || And(()=>    Length() && CompositeValue() )) )
                      || And(()=>    Tag() && Length() && PrimitiveValue() )) ) );
		}
        public bool Tag()    /*[3] Tag: OneOctetTag / MultiOctetTag / FATAL<"illegal TAG">;*/
        {

           return   
                     OneOctetTag()
                  || MultiOctetTag()
                  || Fatal("illegal TAG");
		}
        public bool OneOctetTag()    /*[4] ^^OneOctetTag:   !BITS<1-5,#b11111> BITS<1-5,.,:tag>;*/
        {

           return TreeNT((int)EBER.OneOctetTag,()=>
                And(()=>  
                     NotBits(1,5,0x001f)
                  && BitsInto(1,5,out _top.tag) ) );
		}
        public bool MultiOctetTag()    /*[5] ^^MultiOctetTag: . (&BITS<8,#1> BITS<1-7,.,:n> add_Tag_)* BITS<1-7,.,:n> add_Tag_;*/
        {

           return TreeNT((int)EBER.MultiOctetTag,()=>
                And(()=>  
                     Any()
                  && OptRepeat(()=>    
                      And(()=>      
                               PeekBit(8,0x0001)
                            && BitsInto(1,7,out _top.n)
                            && _top.add_Tag_() ) )
                  && BitsInto(1,7,out _top.n)
                  && _top.add_Tag_() ) );
		}
        public bool Length()    /*[6] Length :   OneOctetLength / MultiOctetLength 
             / FATAL<"illegal LENGTH">;*/
        {

           return   
                     OneOctetLength()
                  || MultiOctetLength()
                  || Fatal("illegal LENGTH");
		}
        public bool OneOctetLength()    /*[7] ^^OneOctetLength: &BITS<8,#0> BITS<1-7,.,:length>;*/
        {

           return TreeNT((int)EBER.OneOctetLength,()=>
                And(()=>  
                     PeekBit(8,0x0000)
                  && BitsInto(1,7,out _top.length) ) );
		}
        public bool MultiOctetLength()    /*[8]^^MultiOctetLength: &BITS<8,#1> BITS<1-7,.,:n> ( .:byte addLength_){:n};*/
        {

           return TreeNT((int)EBER.MultiOctetLength,()=>
                And(()=>  
                     PeekBit(8,0x0001)
                  && BitsInto(1,7,out _top.n)
                  && ForRepeat(_top.n,_top.n,()=>    
                      And(()=>      
                               Into(()=> Any(),out _top.@byte)
                            && _top.addLength_() ) ) ) );
		}
        public bool PrimitiveValue()    /*[9]^^PrimitiveValue: .{:length} / FATAL<"BER input ends before VALUE ends">;*/
        {

           return TreeNT((int)EBER.PrimitiveValue,()=>
                  
                     ForRepeat(_top.length,_top.length,()=> Any() )
                  || Fatal("BER input ends before VALUE ends") );
		}
        public bool CompositeDelimValue()    /*[10]^^CompositeDelimValue: (!(#0#0) TLV)*;*/
        {

           return TreeNT((int)EBER.CompositeDelimValue,()=>
                OptRepeat(()=>  
                  And(()=>    
                         Not(()=> And(()=>    Char(0x0000) && Char(0x0000) ) )
                      && TLV() ) ) );
		}
   class _CompositeValue{
        int len;
        internal PegBegEnd begEnd;
        internal bool save_()  {len= parent_._top.length;return true;}
        internal bool at_end_(){return len<=0;}
        internal bool decr_()  {len-= begEnd.posEnd_-begEnd.posBeg_;return len>=0;} 
        internal _CompositeValue(BER grammarClass){ parent_ = grammarClass; }
        BER parent_;
        }   
        public bool CompositeValue()    /*[11]^^CompositeValue
{
     int len;
     PegBegEnd begEnd;
     bool save_()  {len= length;return true;}
     bool at_end_(){return len<=0;}
     bool decr_()  {len-= begEnd.posEnd_-begEnd.posBeg_;return len>=0;} 
}   :  save_ (!at_end_ TLV:begEnd 
              (decr_/FATAL<"illegal length">))*;*/
        {

             var _sem= new _CompositeValue(this);

           return TreeNT((int)EBER.CompositeValue,()=>
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