/* created on 22.09.2008 08:49:12 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace json_tree
{
      
      enum Ejson_tree{json_text= 1, expect_file_end= 2, @object= 3, members= 4, pair= 5, 
                       array= 6, elements= 7, value= 8, @string= 9, string_content= 10, 
                       number= 11, S= 12, @true= 13, @false= 14, @null= 15};
      class json_tree : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.unicode;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.FirstCharIsAscii;
        #endregion Input Properties
        #region Constructors
        public json_tree()
            : base()
        {
            
        }
        public json_tree(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Ejson_tree ruleEnum = (Ejson_tree)id;
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
        public bool json_text()    /*[1]^^json_text:(object / array	/
		FATAL<"json file must start with '{' or '['">)
                expect_file_end;*/
        {

           return TreeNT((int)Ejson_tree.json_text,()=>
                And(()=>  
                     (    
                         @object()
                      || array()
                      || Fatal("json file must start with '{' or '['"))
                  && expect_file_end() ) );
		}
        public bool expect_file_end()    /*[2]expect_file_end:!./ WARNING<"non-json stuff before end of file">;*/
        {

           return   
                     Not(()=> Any() )
                  || Warning("non-json stuff before end of file");
		}
        public bool @object()    /*[3]^^object:  S '{' S (&'}'/members) S @'}' S	;*/
        {

           return TreeNT((int)Ejson_tree.@object,()=>
                And(()=>  
                     S()
                  && Char('{')
                  && S()
                  && (    Peek(()=> Char('}') ) || members())
                  && S()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool members()    /*[4]members:   pair S (',' S @pair S)*		;*/
        {

           return And(()=>  
                     pair()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    pair() || Fatal("<<pair>> expected"))
                            && S() ) ) );
		}
        public bool pair()    /*[5]^^pair:    @string S ':' S value		;*/
        {

           return TreeNT((int)Ejson_tree.pair,()=>
                And(()=>  
                     (    @string() || Fatal("<<string>> expected"))
                  && S()
                  && Char(':')
                  && S()
                  && value() ) );
		}
        public bool array()    /*[6]^^array:   S '[' S (&']'/elements) S @']' S	;*/
        {

           return TreeNT((int)Ejson_tree.array,()=>
                And(()=>  
                     S()
                  && Char('[')
                  && S()
                  && (    Peek(()=> Char(']') ) || elements())
                  && S()
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S() ) );
		}
        public bool elements()    /*[7]elements:  value S (','S  @value S)*		;*/
        {

           return And(()=>  
                     value()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    value() || Fatal("<<value>> expected"))
                            && S() ) ) );
		}
        public bool value()    /*[8]value:     @(string / number / object / 
				array / true / false / null);*/
        {

           return   
                     (    
                         @string()
                      || number()
                      || @object()
                      || array()
                      || @true()
                      || @false()
                      || @null())
                  || Fatal("<<(string  or  number  or  object  or  array  or  true  or  false  or  null)>> expected");
		}
        public bool @string()    /*[9]string:    '"' string_content '"'	;*/
        {

           return And(()=>    Char('"') && string_content() && Char('"') );
		}
        public bool string_content()    /*[10]^^string_content: ( '\\' 
                           ( 'u'([0-9A-Fa-f]{4}/FATAL<"4 hex digits expected">)
                           / ["\\/bfnrt]/FATAL<"illegal escape"> 
                           ) 
                        / [#x20-#x21#x23-#xFFFF]
                        )*	;*/
        {

           return TreeNT((int)Ejson_tree.string_content,()=>
                OptRepeat(()=>  
                      
                         And(()=>      
                               Char('\\')
                            && (        
                                       And(()=>          
                                                 Char('u')
                                              && (            
                                                             ForRepeat(4,4,()=>              
                                                                        In('0','9', 'A','F', 'a','f') )
                                                          || Fatal("4 hex digits expected")) )
                                    || OneOf(optimizedCharset0)
                                    || Fatal("illegal escape")) )
                      || In('\u0020','\u0021', '\u0023','\uffff') ) );
		}
        public bool number()    /*[11]^^number:  '-'? ('0'/[1-9][0-9]*) ('.' [0-9]+)? ([eE] [-+] [0-9]+)?;*/
        {

           return TreeNT((int)Ejson_tree.number,()=>
                And(()=>  
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
                            && PlusRepeat(()=> In('0','9') ) ) ) ) );
		}
        public bool S()    /*[12]S:         [ \t\r\n]*			;*/
        {

           return OptRepeat(()=> OneOf(" \t\r\n") );
		}
        public bool @true()    /*[13]^^true:    'true'				;*/
        {

           return TreeNT((int)Ejson_tree.@true,()=>
                Char('t','r','u','e') );
		}
        public bool @false()    /*[14]^^false:   'false'				;*/
        {

           return TreeNT((int)Ejson_tree.@false,()=>
                Char('f','a','l','s','e') );
		}
        public bool @null()    /*[15]^^null:    'null'				;*/
        {

           return TreeNT((int)Ejson_tree.@null,()=>
                Char('n','u','l','l') );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        
        static json_tree()
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