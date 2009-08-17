/* created on 17/08/2009 20:11:06 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace nLess
{
      
      enum EnLess{Parse= 1, primary= 2, comment= 3, declaration= 4, ident= 5, variable= 6, 
                   expressions= 7, expression= 8, @operator= 9, ruleset= 10, selectors= 11, 
                   selector= 12, arguments= 13, argument= 14, element= 15, class_id= 16, 
                   attribute= 17, @class= 18, id= 19, tag= 20, select= 21, function= 22, 
                   entity= 23, fonts= 24, font= 25, literal= 26, keyword= 27, @string= 28, 
                   dimension= 29, number= 30, unit= 31, color= 32, rgb= 33, hex= 34, 
                   WS= 35, ws= 36, s= 37, S= 38, ns= 39};
      class nLess : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public nLess()
            : base()
        {
            
        }
        public nLess(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   EnLess ruleEnum = (EnLess)id;
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
        public bool Parse()    /*Parse:  primary;*/
        {

           return primary();
		}
        public bool primary()    /*^^primary: (comment/ ruleset /declaration)*;*/
        {

           return TreeNT((int)EnLess.primary,()=>
                OptRepeat(()=>  
                      comment() || ruleset() || declaration() ) );
		}
        public bool comment()    /*^^comment: ws '/*' (!'* /' . )* '* /' ws / ws '//' (![\n] .)* [\n] ws;*/
        {

           return TreeNT((int)EnLess.comment,()=>
                  
                     And(()=>    
                         ws()
                      && Char('/','*')
                      && OptRepeat(()=>      
                            And(()=>    Not(()=> Char('*','/') ) && Any() ) )
                      && Char('*','/')
                      && ws() )
                  || And(()=>    
                         ws()
                      && Char('/','/')
                      && OptRepeat(()=>      
                            And(()=>    Not(()=> OneOf("\n") ) && Any() ) )
                      && OneOf("\n")
                      && ws() ) );
		}
        public bool declaration()    /*^^declaration:  ws (ident / variable)  s ':' s expressions  s (';'/ ws &'}') ws  ;*/
        {

           return TreeNT((int)EnLess.declaration,()=>
                And(()=>  
                     ws()
                  && (    ident() || variable())
                  && s()
                  && Char(':')
                  && s()
                  && expressions()
                  && s()
                  && (    
                         Char(';')
                      || And(()=>    ws() && Peek(()=> Char('}') ) ))
                  && ws() ) );
		}
        public bool ident()    /*^^ident: ('*'/'-'/[-a-z0-9_]+);*/
        {

           return TreeNT((int)EnLess.ident,()=>
                  
                     Char('*')
                  || Char('-')
                  || PlusRepeat(()=> (In('a','z', '0','9')||OneOf("-_")) ) );
		}
        public bool variable()    /*^^variable: '@' [-a-zA-Z0-9_]+;*/
        {

           return TreeNT((int)EnLess.variable,()=>
                And(()=>  
                     Char('@')
                  && PlusRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool expressions()    /*^^expressions: expression (operator expression)+ / expression (WS expression)* / [-a-zA-Z0-9_%* /.&=:,#+? \[\]()]+;*/
        {

           return TreeNT((int)EnLess.expressions,()=>
                  
                     And(()=>    
                         expression()
                      && PlusRepeat(()=>      
                            And(()=>    @operator() && expression() ) ) )
                  || And(()=>    
                         expression()
                      && OptRepeat(()=> And(()=>    WS() && expression() ) ) )
                  || PlusRepeat(()=> OneOf(optimizedCharset0) ) );
		}
        public bool expression()    /*^^expression: '(' s expressions s ')' / entity ;*/
        {

           return TreeNT((int)EnLess.expression,()=>
                  
                     And(()=>    
                         Char('(')
                      && s()
                      && expressions()
                      && s()
                      && Char(')') )
                  || entity() );
		}
        public bool @operator()    /*^^operator: S [-+* /] S / [-+* /] ;*/
        {

           return TreeNT((int)EnLess.@operator,()=>
                  
                     And(()=>    S() && OneOf("-+*/") && S() )
                  || OneOf("-+*/") );
		}
        public bool ruleset()    /*^^ruleset : selectors [{] ws primary ws [}] ws /  ws selectors ';' ws;*/
        {

           return TreeNT((int)EnLess.ruleset,()=>
                  
                     And(()=>    
                         selectors()
                      && OneOf("{")
                      && ws()
                      && primary()
                      && ws()
                      && OneOf("}")
                      && ws() )
                  || And(()=>    ws() && selectors() && Char(';') && ws() ) );
		}
        public bool selectors()    /*^^selectors :  ws selector (s ',' ws selector)* ws ;*/
        {

           return TreeNT((int)EnLess.selectors,()=>
                And(()=>  
                     ws()
                  && selector()
                  && OptRepeat(()=>    
                      And(()=>    s() && Char(',') && ws() && selector() ) )
                  && ws() ) );
		}
        public bool selector()    /*^^selector : (s select element s)+ arguments? ;*/
        {

           return TreeNT((int)EnLess.selector,()=>
                And(()=>  
                     PlusRepeat(()=>    
                      And(()=>    s() && select() && element() && s() ) )
                  && Option(()=> arguments() ) ) );
		}
        public bool arguments()    /*^^arguments : '(' s argument s (',' s argument s)* ')';*/
        {

           return TreeNT((int)EnLess.arguments,()=>
                And(()=>  
                     Char('(')
                  && s()
                  && argument()
                  && s()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && s() && argument() && s() ) )
                  && Char(')') ) );
		}
        public bool argument()    /*^^argument : color / number unit / string / [a-zA-Z]+ '=' dimension / [-a-zA-Z0-9_%$/.&=:;#+?]+ / function / keyword (S keyword)*;*/
        {

           return TreeNT((int)EnLess.argument,()=>
                  
                     color()
                  || And(()=>    number() && unit() )
                  || @string()
                  || And(()=>    
                         PlusRepeat(()=> In('a','z', 'A','Z') )
                      && Char('=')
                      && dimension() )
                  || PlusRepeat(()=> OneOf(optimizedCharset1) )
                  || function()
                  || And(()=>    
                         keyword()
                      && OptRepeat(()=> And(()=>    S() && keyword() ) ) ) );
		}
        public bool element()    /*^^element : (class_id / tag / ident) attribute* ('(' ident? attribute* ')')? / attribute+ / '@media' / '@font-face';*/
        {

           return TreeNT((int)EnLess.element,()=>
                  
                     And(()=>    
                         (    class_id() || tag() || ident())
                      && OptRepeat(()=> attribute() )
                      && Option(()=>      
                            And(()=>        
                                       Char('(')
                                    && Option(()=> ident() )
                                    && OptRepeat(()=> attribute() )
                                    && Char(')') ) ) )
                  || PlusRepeat(()=> attribute() )
                  || Char('@','m','e','d','i','a')
                  || Char("@font-face") );
		}
        public bool class_id()    /*^^class_id : tag? (class / id)+;*/
        {

           return TreeNT((int)EnLess.class_id,()=>
                And(()=>  
                     Option(()=> tag() )
                  && PlusRepeat(()=>     @class() || id() ) ) );
		}
        public bool attribute()    /*^^attribute :  '[' tag ([|~*$^]? '=') (tag / string) ']' / '[' (tag / string) ']';*/
        {

           return TreeNT((int)EnLess.attribute,()=>
                  
                     And(()=>    
                         Char('[')
                      && tag()
                      && And(()=>      
                               Option(()=> OneOf("|~*$^") )
                            && Char('=') )
                      && (    tag() || @string())
                      && Char(']') )
                  || And(()=>    
                         Char('[')
                      && (    tag() || @string())
                      && Char(']') ) );
		}
        public bool @class()    /*^^class:  '.' [_a-zA-Z] [-a-zA-Z0-9_]*;*/
        {

           return TreeNT((int)EnLess.@class,()=>
                And(()=>  
                     Char('.')
                  && (In('a','z', 'A','Z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool id()    /*^^id: '#' [_a-zA-Z] [-a-zA-Z0-9_]*;*/
        {

           return TreeNT((int)EnLess.id,()=>
                And(()=>  
                     Char('#')
                  && (In('a','z', 'A','Z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool tag()    /*^^tag : [a-zA-Z] [-a-zA-Z]* [0-9]? / '*';*/
        {

           return TreeNT((int)EnLess.tag,()=>
                  
                     And(()=>    
                         In('a','z', 'A','Z')
                      && OptRepeat(()=> (In('a','z', 'A','Z')||OneOf("-")) )
                      && Option(()=> In('0','9') ) )
                  || Char('*') );
		}
        public bool select()    /*^^select : (s [+>~] s / '::' / s ':' / S)?;*/
        {

           return TreeNT((int)EnLess.select,()=>
                Option(()=>  
                      
                         And(()=>    s() && OneOf("+>~") && s() )
                      || Char(':',':')
                      || And(()=>    s() && Char(':') )
                      || S() ) );
		}
        public bool function()    /*^^function: ([-a-zA-Z_]+) arguments ;

//******************************************** Entity*/
        {

           return TreeNT((int)EnLess.function,()=>
                And(()=>  
                     PlusRepeat(()=> (In('a','z', 'A','Z')||OneOf("-_")) )
                  && arguments() ) );
		}
        public bool entity()    /*^^entity :  fonts / keyword  / variable / literal ; //accessor & function missing*/
        {

           return TreeNT((int)EnLess.entity,()=>
                    fonts() || keyword() || variable() || literal() );
		}
        public bool fonts()    /*^^fonts : font (s ',' s font)+  ;*/
        {

           return TreeNT((int)EnLess.fonts,()=>
                And(()=>  
                     font()
                  && PlusRepeat(()=>    
                      And(()=>    s() && Char(',') && s() && font() ) ) ) );
		}
        public bool font()    /*^^font: [a-zA-Z] [-a-zA-Z0-9]* ;*/
        {

           return TreeNT((int)EnLess.font,()=>
                And(()=>  
                     In('a','z', 'A','Z')
                  && OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-")) ) ) );
		}
        public bool literal()    /*^^literal: color / (dimension / [-a-z]+) '/' dimension / number unit / string ;*/
        {

           return TreeNT((int)EnLess.literal,()=>
                  
                     color()
                  || And(()=>    
                         (      
                               dimension()
                            || PlusRepeat(()=> (In('a','z')||OneOf("-")) ))
                      && Char('/')
                      && dimension() )
                  || And(()=>    number() && unit() )
                  || @string() );
		}
        public bool keyword()    /*^^keyword: [-a-zA-Z]+ !ns;*/
        {

           return TreeNT((int)EnLess.keyword,()=>
                And(()=>  
                     PlusRepeat(()=> (In('a','z', 'A','Z')||OneOf("-")) )
                  && Not(()=> ns() ) ) );
		}
        public bool @string()    /*^^string: ['] (!['] . )* ['] / ["] (!["] . )* ["] ;*/
        {

           return TreeNT((int)EnLess.@string,()=>
                  
                     And(()=>    
                         OneOf("'")
                      && OptRepeat(()=>      
                            And(()=>    Not(()=> OneOf("'") ) && Any() ) )
                      && OneOf("'") )
                  || And(()=>    
                         OneOf("\"")
                      && OptRepeat(()=>      
                            And(()=>    Not(()=> OneOf("\"") ) && Any() ) )
                      && OneOf("\"") ) );
		}
        public bool dimension()    /*^^dimension: number unit;*/
        {

           return TreeNT((int)EnLess.dimension,()=>
                And(()=>    number() && unit() ) );
		}
        public bool number()    /*^^number: '-'? [0-9]* '.' [0-9]+ / '-'? [0-9]+;*/
        {

           return TreeNT((int)EnLess.number,()=>
                  
                     And(()=>    
                         Option(()=> Char('-') )
                      && OptRepeat(()=> In('0','9') )
                      && Char('.')
                      && PlusRepeat(()=> In('0','9') ) )
                  || And(()=>    
                         Option(()=> Char('-') )
                      && PlusRepeat(()=> In('0','9') ) ) );
		}
        public bool unit()    /*^^unit: ('px'/'em'/'pc'/'%'/'ex'/'s'/'pt'/'cm'/'mm')?;*/
        {

           return TreeNT((int)EnLess.unit,()=>
                Option(()=> OneOfLiterals(optimizedLiterals0) ) );
		}
        public bool color()    /*^^color: '#' rgb;*/
        {

           return TreeNT((int)EnLess.color,()=>
                And(()=>    Char('#') && rgb() ) );
		}
        public bool rgb()    /*^^rgb:(hex hex)(hex hex)(hex hex);*/
        {

           return TreeNT((int)EnLess.rgb,()=>
                And(()=>  
                     And(()=>    hex() && hex() )
                  && And(()=>    hex() && hex() )
                  && And(()=>    hex() && hex() ) ) );
		}
        public bool hex()    /*^^hex: [a-fA-F0-9];

//******************************************** Common*/
        {

           return TreeNT((int)EnLess.hex,()=>
                In('a','f', 'A','F', '0','9') );
		}
        public bool WS()    /*WS: [ \r\n]+;*/
        {

           return PlusRepeat(()=> OneOf(" \r\n") );
		}
        public bool ws()    /*ws: [ \r\n]*;*/
        {

           return OptRepeat(()=> OneOf(" \r\n") );
		}
        public bool s()    /*s:  [ ]*;*/
        {

           return OptRepeat(()=> OneOf(" ") );
		}
        public bool S()    /*S:  [ ]+;*/
        {

           return PlusRepeat(()=> OneOf(" ") );
		}
        public bool ns()    /*ns: ![ ;\n] .;*/
        {

           return And(()=>    Not(()=> OneOf(" ;\n") ) && Any() );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedCharset optimizedCharset1;
        
        internal static OptimizedLiterals optimizedLiterals0;
        
        static nLess()
        {
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('0','9'),
                   };
               char[] oneOfChars = new char[]    {'-','_','%','*','/'
                                                  ,'.','&','=',':',','
                                                  ,'#','+','?',' ','\\'
                                                  ,'[',']','(',')'};
               optimizedCharset0= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('0','9'),
                   };
               char[] oneOfChars = new char[]    {'-','_','%','$','/'
                                                  ,'.','&','=',':',';'
                                                  ,'#','+','?'};
               optimizedCharset1= new OptimizedCharset(ranges,oneOfChars);
            }
            
            
            {
               string[] literals=
               { "px","em","pc","%","ex","s","pt","cm",
                  "mm" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            
        }
        #endregion Optimization Data 
           }
}