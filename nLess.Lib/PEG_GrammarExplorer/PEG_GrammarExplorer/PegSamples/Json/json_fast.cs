/* created on 15.09.2008 10:44:50 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace json_fast
{
      
      enum Ejson_fast{json_text= 1, expect_file_end= 2, top_element= 3, @object= 4, 
                       members= 5, pair= 6, array= 7, elements= 8, value= 9, @string= 10, 
                       @char= 11, number= 13};
      class json_fast : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.unicode;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.FirstCharIsAscii;
        #endregion Input Properties
        #region Constructors
        public json_fast()
            : base()
        {
            
        }
        public json_fast(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Ejson_fast ruleEnum = (Ejson_fast)id;
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
        public bool json_text()    /*[1]json_text:		top_element expect_file_end			;*/
        {

           return And(()=>    top_element() && expect_file_end() );
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
        public bool @object()    /*[4]object:  		[ \t\r\n]* 
                        '{' [ \t\r\n]* (&'}'/members) [ \t\r\n]* @'}' [ \t\r\n]*;*/
        {

           return And(()=>  
                     OptRepeat(()=> OneOf(" \t\r\n") )
                  && Char('{')
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && (    Peek(()=> Char('}') ) || members())
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && OptRepeat(()=> OneOf(" \t\r\n") ) );
		}
        public bool members()    /*[5]members: 		pair [ \t\r\n]* (',' [ \t\r\n]* @pair [ \t\r\n]*)*;*/
        {

           return And(()=>  
                     pair()
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && OptRepeat(()=> OneOf(" \t\r\n") )
                            && (    pair() || Fatal("<<pair>> expected"))
                            && OptRepeat(()=> OneOf(" \t\r\n") ) ) ) );
		}
        public bool pair()    /*[6]pair:		@string [ \t\r\n]* ':' [ \t\r\n]* @value		;*/
        {

           return And(()=>  
                     (    @string() || Fatal("<<string>> expected"))
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && Char(':')
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && (    value() || Fatal("<<value>> expected")) );
		}
        public bool array()    /*[7]array:  		[ \t\r\n]* 
                        '[' [ \t\r\n]* (&']'/elements) [ \t\r\n]* @']' [ \t\r\n]*;*/
        {

           return And(()=>  
                     OptRepeat(()=> OneOf(" \t\r\n") )
                  && Char('[')
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && (    Peek(()=> Char(']') ) || elements())
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && OptRepeat(()=> OneOf(" \t\r\n") ) );
		}
        public bool elements()    /*[8]elements: 		value [ \t\r\n]* (','[ \t\r\n]*  @value [ \t\r\n]*)*;*/
        {

           return And(()=>  
                     value()
                  && OptRepeat(()=> OneOf(" \t\r\n") )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && OptRepeat(()=> OneOf(" \t\r\n") )
                            && (    value() || Fatal("<<value>> expected"))
                            && OptRepeat(()=> OneOf(" \t\r\n") ) ) ) );
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
        public bool @char()    /*[11]char:  		'\\' (["\\/bfnrt] / 
			'u' ([0-9A-Fa-f]{4}/FATAL<"4 hex digits expected">)/
			     FATAL<"illegal escape">) / 
			[#x20-#x21#x23-#x5B#x5D-#xFFFF]			;*/
        {

           return   
                     And(()=>    
                         Char('\\')
                      && (      
                               OneOf(optimizedCharset0)
                            || And(()=>        
                                       Char('u')
                                    && (          
                                                 ForRepeat(4,4,()=>            
                                                          In('0','9', 'A','F', 'a','f') )
                                              || Fatal("4 hex digits expected")) )
                            || Fatal("illegal escape")) )
                  || In('\u0020','\u0021', '\u0023','\u005b', '\u005d','\uffff');
		}
        public bool number()    /*[13]number:		'-'? ('0'/ [1-9][0-9]*) ('.' [0-9]+)? ([eE] [-+] [0-9]+)?;*/
        {

           return And(()=>  
                     Option(()=> Char('-') )
                  && (    
                         Char('0')
                      || And(()=>      
                               In('1','9')
                            && OptRepeat(()=> In('0','9') ) ))
                  && Option(()=>    
                      And(()=>      
                               Char('.')
                            && PlusRepeat(()=> In('0','9') ) ) )
                  && Option(()=>    
                      And(()=>      
                               OneOf("eE")
                            && OneOf("-+")
                            && PlusRepeat(()=> In('0','9') ) ) ) );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        
        static json_fast()
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