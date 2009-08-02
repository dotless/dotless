/* created on 22.09.2008 10:59:43 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace EMail
{
      
      enum EEMail{email_address= 1, checked_local_part= 2, checked_domain_part= 3, 
                   local_part= 4, quoted_local_part= 5, unquoted_local_part= 6, 
                   atext= 7, print_char= 8, quoted_char= 9, domain_part= 10, dot_atom= 11, 
                   domain_literal= 12, dtext= 13, FWS= 14, CFWS= 15, comment= 16, 
                   ccontent= 17, ctext= 18, quoted_pair= 19, label= 20, top_level_domain= 21, 
                   local_part_char= 22, domain_char= 23};
      class EMail : PegCharParser 
      {
        class _Top{
	internal PegBegEnd lpart_, dpart_;
	internal bool check_len64_()  {return lpart_.posEnd_ - lpart_.posBeg_ <=64;}
	internal bool check_len_255_(){return dpart_.posEnd_ - dpart_.posBeg_ <=255;}
}
_Top _top;

         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public EMail()
            : base()
        {
            _top= new _Top();

        }
        public EMail(string src,TextWriter FerrOut)
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
                   EEMail ruleEnum = (EEMail)id;
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
        public bool email_address()    /*[1]  ^^email_address:   checked_local_part @'@' checked_domain_part;*/
        {

           return TreeNT((int)EEMail.email_address,()=>
                And(()=>  
                     checked_local_part()
                  && (    Char('@') || Fatal("<<'@'>> expected"))
                  && checked_domain_part() ) );
		}
        public bool checked_local_part()    /*[2] checked_local_part: local_part:lpart_  
			(check_len64_ /FATAL<"at most 64 characters before @">);*/
        {

           return And(()=>  
                     Into(()=> local_part(),out _top.lpart_)
                  && (    
                         _top.check_len64_()
                      || Fatal("at most 64 characters before @")) );
		}
        public bool checked_domain_part()    /*[3] checked_domain_part:domain_part:dpart_ 
			(check_len_255_/FATAL<"at most 255 characters after @">);*/
        {

           return And(()=>  
                     Into(()=> domain_part(),out _top.dpart_)
                  && (    
                         _top.check_len_255_()
                      || Fatal("at most 255 characters after @")) );
		}
        public bool local_part()    /*[4] local_part: 	@('"' quoted_local_part @'"' / unquoted_local_part);*/
        {

           return   
                     (    
                         And(()=>      
                               Char('"')
                            && quoted_local_part()
                            && (    Char('"') || Fatal("<<'\"'>> expected")) )
                      || unquoted_local_part())
                  || Fatal("<<('\"' quoted_local_part @'\"'  or  unquoted_local_part)>> expected");
		}
        public bool quoted_local_part()    /*[5] ^^quoted_local_part:  (!["\\] print_char / quoted_pair)+ ;*/
        {

           return TreeNT((int)EEMail.quoted_local_part,()=>
                PlusRepeat(()=>  
                      
                         And(()=>    Not(()=> OneOf("\"\\") ) && print_char() )
                      || quoted_pair() ) );
		}
        public bool unquoted_local_part()    /*[6] ^^unquoted_local_part:  CFWS? atext+ ('.' atext+)* CFWS? ;*/
        {

           return TreeNT((int)EEMail.unquoted_local_part,()=>
                And(()=>  
                     Option(()=> CFWS() )
                  && PlusRepeat(()=> atext() )
                  && OptRepeat(()=>    
                      And(()=>    Char('.') && PlusRepeat(()=> atext() ) ) )
                  && Option(()=> CFWS() ) ) );
		}
        public bool atext()    /*[7] atext:		local_part_char / quoted_pair ;*/
        {

           return     local_part_char() || quoted_pair();
		}
        public bool print_char()    /*[8] print_char:		[-A-Za-z0-9.!#$%&'*+/=?^_`{|}~@,[\] ];*/
        {

           return OneOf(optimizedCharset0);
		}
        public bool quoted_char()    /*[9] quoted_char: 	'\\' [A-Za-z0-9.!#$%&'*+-/=?^_`{|}~@,"\\[\]];*/
        {

           return And(()=>    Char('\\') && OneOf(optimizedCharset1) );
		}
        public bool domain_part()    /*[10] ^^domain_part:	@(dot_atom / domain_literal);*/
        {

           return TreeNT((int)EEMail.domain_part,()=>
                  
                     (    dot_atom() || domain_literal())
                  || Fatal("<<(dot_atom  or  domain_literal)>> expected") );
		}
        public bool dot_atom()    /*[11] ^^dot_atom:	(label ('.'/FATAL<"at least one dot expected">))+ top_level_domain;*/
        {

           return TreeNT((int)EEMail.dot_atom,()=>
                And(()=>  
                     PlusRepeat(()=>    
                      And(()=>      
                               label()
                            && (    Char('.') || Fatal("at least one dot expected")) ) )
                  && top_level_domain() ) );
		}
        public bool domain_literal()    /*[12] ^^domain_literal:	CFWS? '[' (FWS? (dtext/quoted_pair))* FWS? ']' CFWS?;*/
        {

           return TreeNT((int)EEMail.domain_literal,()=>
                And(()=>  
                     Option(()=> CFWS() )
                  && Char('[')
                  && OptRepeat(()=>    
                      And(()=>      
                               Option(()=> FWS() )
                            && (    dtext() || quoted_pair()) ) )
                  && Option(()=> FWS() )
                  && Char(']')
                  && Option(()=> CFWS() ) ) );
		}
        public bool dtext()    /*[13]dtext:		[#x1-#x8#xB#xC#xE-#x1F#x7F] / [#x21-#x5A#x5E-#x7E] ;*/
        {

           return (In('\u0001','\u0008', '\u000e','\u001f', '\u0021','\u005a', '\u005e','\u007e')||OneOf("\u000b\u000c\u007f"));
		}
        public bool FWS()    /*[14]FWS:		([ \t]* '\r\n')? [ \t]+;*/
        {

           return And(()=>  
                     Option(()=>    
                      And(()=>      
                               OptRepeat(()=> OneOf(" \t") )
                            && Char('\r','\n') ) )
                  && PlusRepeat(()=> OneOf(" \t") ) );
		}
        public bool CFWS()    /*[15]CFWS:		(FWS? comment)+/FWS;*/
        {

           return   
                     PlusRepeat(()=>    
                      And(()=>    Option(()=> FWS() ) && comment() ) )
                  || FWS();
		}
        public bool comment()    /*[16]^^comment:       	'(' (FWS? ccontent)* FWS? ')';*/
        {

           return TreeNT((int)EEMail.comment,()=>
                And(()=>  
                     Char('(')
                  && OptRepeat(()=>    
                      And(()=>    Option(()=> FWS() ) && ccontent() ) )
                  && Option(()=> FWS() )
                  && Char(')') ) );
		}
        public bool ccontent()    /*[17]ccontent:		ctext/quoted_pair/comment;*/
        {

           return     ctext() || quoted_pair() || comment();
		}
        public bool ctext()    /*[18]ctext:		[#x1-#x8#xB#xC#xE-#x1F#x7F] / [#x21-#x27#x2A-#x5B#x5D-#x7E];*/
        {

           return OneOf(optimizedCharset2);
		}
        public bool quoted_pair()    /*[19]quoted_pair:	'\\' [#x1-#x9#xB#xC#xE-#x7F];*/
        {

           return And(()=>  
                     Char('\\')
                  && (In('\u0001','\u0009', '\u000e','\u007f')||OneOf("\u000b\u000c")) );
		}
        public bool label()    /*[20] ^^label:		!top_level_domain [A-Za-z] (!('-' !domain_char) domain_char)*;*/
        {

           return TreeNT((int)EEMail.label,()=>
                And(()=>  
                     Not(()=> top_level_domain() )
                  && In('A','Z', 'a','z')
                  && OptRepeat(()=>    
                      And(()=>      
                               Not(()=>        
                                    And(()=>          
                                                 Char('-')
                                              && Not(()=> domain_char() ) ) )
                            && domain_char() ) ) ) );
		}
        public bool top_level_domain()    /*[21] ^^top_level_domain:[a-zA-Z]{2,}![-0-9.];*/
        {

           return TreeNT((int)EEMail.top_level_domain,()=>
                And(()=>  
                     ForRepeat(2,2147483647,()=> In('a','z', 'A','Z') )
                  && Not(()=> (In('0','9')||OneOf("-.")) ) ) );
		}
        public bool local_part_char()    /*[22] local_part_char: 	[-A-Za-z0-9!#$%&'*+/=?^_`{|}~];*/
        {

           return OneOf(optimizedCharset3);
		}
        public bool domain_char()    /*[23] domain_char:	[A-Za-z0-9-];*/
        {

           return (In('A','Z', 'a','z', '0','9')||OneOf("-"));
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedCharset optimizedCharset1;
        internal static OptimizedCharset optimizedCharset2;
        internal static OptimizedCharset optimizedCharset3;
        
        
        static EMail()
        {
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('0','9'),
                   };
               char[] oneOfChars = new char[]    {'-','.','!','#','$'
                                                  ,'%','&','\'','*','+'
                                                  ,'/','=','?','^','_'
                                                  ,'`','{','|','}','~'
                                                  ,'@',',','[',']',' '
                                                  };
               optimizedCharset0= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('0','9'),
                   new OptimizedCharset.Range('+','/'),
                   };
               char[] oneOfChars = new char[]    {'.','!','#','$','%'
                                                  ,'&','\'','*','=','?'
                                                  ,'^','_','`','{','|'
                                                  ,'}','~','@',',','"'
                                                  ,'\\','[',']'};
               optimizedCharset1= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0001','\u0008'),
                   new OptimizedCharset.Range('\u000e','\u001f'),
                   new OptimizedCharset.Range('\u0021','\u0027'),
                   new OptimizedCharset.Range('\u002a','\u005b'),
                   new OptimizedCharset.Range('\u005d','\u007e'),
                   };
               char[] oneOfChars = new char[]    {'\u000b','\u000c','\u007f'};
               optimizedCharset2= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('0','9'),
                   };
               char[] oneOfChars = new char[]    {'-','!','#','$','%'
                                                  ,'&','\'','*','+','/'
                                                  ,'=','?','^','_','`'
                                                  ,'{','|','}','~'};
               optimizedCharset3= new OptimizedCharset(ranges,oneOfChars);
            }
            
            
            
        }
        #endregion Optimization Data 
           }
}