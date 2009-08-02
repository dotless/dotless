/* created on 21.09.2008 19:02:14 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace json_check
{
      
      enum Ejson_check{json_text= 1, expect_file_end= 2, top_element= 3, @object= 4, 
                        members= 5, pair= 6, array= 7, elements= 8, value= 9, @string= 10, 
                        @char= 11, escape= 12, number= 13, @int= 14, frac= 15, exp= 16, 
                        control_chars= 17, unicode_char= 18, S= 19};
      class json_check : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.unicode;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.FirstCharIsAscii;
        #endregion Input Properties
        #region Constructors
        public json_check()
            : base()
        {
            
        }
        public json_check(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Ejson_check ruleEnum = (Ejson_check)id;
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
        public bool json_text()    /*[1]json_text:		S top_element expect_file_end			;*/
        {

           return And(()=>    S() && top_element() && expect_file_end() );
		}
        public bool expect_file_end()    /*[2]expect_file_end:	!./ WARNING<"non-json stuff before end of file">;*/
        {

           return   
                     Not(()=> Any() )
                  || Warning("non-json stuff before end of file");
		}
        public bool top_element()    /*[3]top_element:		object / array  /
			FATAL<"json file must start with '{' or '['">	;*/
        {

           return   
                     @object()
                  || array()
                  || Fatal("json file must start with '{' or '['");
		}
        public bool @object()    /*[4]object:  		'{' S (&'}'/members)  @'}' S			;*/
        {

           return And(()=>  
                     Char('{')
                  && S()
                  && (    Peek(()=> Char('}') ) || members())
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() );
		}
        public bool members()    /*[5]members: 		pair S (',' S pair S)*				;*/
        {

           return And(()=>  
                     pair()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && pair() && S() ) ) );
		}
        public bool pair()    /*[6]pair:		@string S @':' S value				;*/
        {

           return And(()=>  
                     (    @string() || Fatal("<<string>> expected"))
                  && S()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && value() );
		}
        public bool array()    /*[7]array:  		'[' S (&']'/elements)  @']' S			;*/
        {

           return And(()=>  
                     Char('[')
                  && S()
                  && (    Peek(()=> Char(']') ) || elements())
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S() );
		}
        public bool elements()    /*[8]elements: 		value S (','S  value S)*			;*/
        {

           return And(()=>  
                     value()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && value() && S() ) ) );
		}
        public bool value()    /*[9]value:    		@(string / number / object / 
				array / 'true' / 'false' / 'null')	;*/
        {

           return   
                     (    
                         @string()
                      || number()
                      || @object()
                      || array()
                      || Char('t','r','u','e')
                      || Char('f','a','l','s','e')
                      || Char('n','u','l','l'))
                  || Fatal("<<(string  or  number  or  object  or  array  or  'true'  or  'false'  or  'null')>> expected");
		}
        public bool @string()    /*[10]string:	 	'"' char* @'"'					;*/
        {

           return And(()=>  
                     Char('"')
                  && OptRepeat(()=> @char() )
                  && (    Char('"') || Fatal("<<'\"'>> expected")) );
		}
        public bool @char()    /*[11]char:  		escape / !(["\\]/control_chars)unicode_char	;*/
        {

           return   
                     escape()
                  || And(()=>    
                         Not(()=>     OneOf("\"\\") || control_chars() )
                      && unicode_char() );
		}
        public bool escape()    /*[12]escape:		'\\' ( ["\\/bfnrt] / 
			'u' ([0-9A-Fa-f]{4}/FATAL<"4 hex digits expected">)/
			     FATAL<"illegal escape">);*/
        {

           return And(()=>  
                     Char('\\')
                  && (    
                         OneOf(optimizedCharset0)
                      || And(()=>      
                               Char('u')
                            && (        
                                       ForRepeat(4,4,()=> In('0','9', 'A','F', 'a','f') )
                                    || Fatal("4 hex digits expected")) )
                      || Fatal("illegal escape")) );
		}
        public bool number()    /*[13]number:		'-'? int frac? exp?				;*/
        {

           return And(()=>  
                     Option(()=> Char('-') )
                  && @int()
                  && Option(()=> frac() )
                  && Option(()=> exp() ) );
		}
        public bool @int()    /*[14]int:		'0'/ [1-9][0-9]*				;*/
        {

           return   
                     Char('0')
                  || And(()=>    
                         In('1','9')
                      && OptRepeat(()=> In('0','9') ) );
		}
        public bool frac()    /*[15]frac:		'.' [0-9]+					;*/
        {

           return And(()=>    Char('.') && PlusRepeat(()=> In('0','9') ) );
		}
        public bool exp()    /*[16]exp:		[eE] [-+] [0-9]+				;*/
        {

           return And(()=>  
                     OneOf("eE")
                  && OneOf("-+")
                  && PlusRepeat(()=> In('0','9') ) );
		}
        public bool control_chars()    /*[17]control_chars:	[#x0-#x1F]					;*/
        {

           return In('\u0000','\u001f');
		}
        public bool unicode_char()    /*[18]unicode_char:	[#x0-#xFFFF]					;*/
        {

           return In('\u0000','\uffff');
		}
        public bool S()    /*[19]S:  		[ \t\r\n]*					;*/
        {

           return OptRepeat(()=> OneOf(" \t\r\n") );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        
        static json_check()
        {
            {
               char[] oneOfChars = new char[]    {'"','\\','/','b','f'
                                                  ,'n','r','t'};
               optimizedCharset0= new OptimizedCharset(null,oneOfChars);
            }
            
            
            
        }
        #endregion Optimization Data 
           }
}