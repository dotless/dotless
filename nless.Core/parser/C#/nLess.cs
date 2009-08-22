/* created on 22/08/2009 00:47:25 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace nLess
{
      
      enum EnLess{Parse= 1, primary= 2, comment= 3, declaration= 4, standard_declaration= 5, 
                   catchall_declaration= 6, ident= 7, variable= 8, expressions= 9, 
                   operation_expressions= 10, space_delimited_expressions= 11, expression= 12, 
                   @operator= 13, ruleset= 14, standard_ruleset= 15, mixin_ruleset= 16, 
                   selectors= 17, selector= 18, arguments= 19, argument= 20, element= 21, 
                   class_id= 22, attribute= 23, @class= 24, id= 25, tag= 26, select= 27, 
                   function= 28, entity= 29, fonts= 30, font= 31, literal= 32, keyword= 33, 
                   @string= 34, dimension= 35, number= 36, unit= 37, color= 38, 
                   rgb= 39, hex= 40, WS= 41, ws= 42, s= 43, S= 44, ns= 45};
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
        public bool Parse()    /*^Parse:  primary;*/
        {

           return TreeAST((int)EnLess.Parse,()=> primary() );
		}
        public bool primary()    /*^^primary: (comment/ declaration/ ruleset)*;*/
        {

           return TreeNT((int)EnLess.primary,()=>
                OptRepeat(()=>  
                      comment() || declaration() || ruleset() ) );
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
        public bool declaration()    /*^^declaration:  standard_declaration / catchall_declaration ;*/
        {

           return TreeNT((int)EnLess.declaration,()=>
                    standard_declaration() || catchall_declaration() );
		}
        public bool standard_declaration()    /*^^standard_declaration: ws (ident / variable)  s ':' s expressions  s (';'/ ws &'}') ws ;*/
        {

           return TreeNT((int)EnLess.standard_declaration,()=>
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
        public bool catchall_declaration()    /*^^catchall_declaration:  ws ident s ':' s ';' ws ;*/
        {

           return TreeNT((int)EnLess.catchall_declaration,()=>
                And(()=>  
                     ws()
                  && ident()
                  && s()
                  && Char(':')
                  && s()
                  && Char(';')
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
        public bool expressions()    /*^^expressions: operation_expressions / space_delimited_expressions / [-a-zA-Z0-9_%* /.&=:,#+? \[\]()]+;*/
        {

           return TreeNT((int)EnLess.expressions,()=>
                  
                     operation_expressions()
                  || space_delimited_expressions()
                  || PlusRepeat(()=> OneOf(optimizedCharset0) ) );
		}
        public bool operation_expressions()    /*^^operation_expressions:  expression (operator expression)+;*/
        {

           return TreeNT((int)EnLess.operation_expressions,()=>
                And(()=>  
                     expression()
                  && PlusRepeat(()=>    
                      And(()=>    @operator() && expression() ) ) ) );
		}
        public bool space_delimited_expressions()    /*^^space_delimited_expressions: expression (WS expression)*;*/
        {

           return TreeNT((int)EnLess.space_delimited_expressions,()=>
                And(()=>  
                     expression()
                  && OptRepeat(()=> And(()=>    WS() && expression() ) ) ) );
		}
        public bool expression()    /*^expression: '(' s expressions s ')' / entity ;*/
        {

           return TreeAST((int)EnLess.expression,()=>
                  
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
        public bool ruleset()    /*^^ruleset : standard_ruleset / mixin_ruleset;*/
        {

           return TreeNT((int)EnLess.ruleset,()=>
                    standard_ruleset() || mixin_ruleset() );
		}
        public bool standard_ruleset()    /*^^standard_ruleset: ws selectors [{] ws primary ws [}] ws;*/
        {

           return TreeNT((int)EnLess.standard_ruleset,()=>
                And(()=>  
                     ws()
                  && selectors()
                  && OneOf("{")
                  && ws()
                  && primary()
                  && ws()
                  && OneOf("}")
                  && ws() ) );
		}
        public bool mixin_ruleset()    /*^^mixin_ruleset :  ws selectors ';' ws;*/
        {

           return TreeNT((int)EnLess.mixin_ruleset,()=>
                And(()=>    ws() && selectors() && Char(';') && ws() ) );
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
        public bool element()    /*^element : (class_id / tag / ident) attribute* ('(' ident? attribute* ')')? / attribute+ / '@media' / '@font-face';*/
        {

           return TreeAST((int)EnLess.element,()=>
                  
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
        public bool class_id()    /*^class_id : tag? (class / id)+;*/
        {

           return TreeAST((int)EnLess.class_id,()=>
                And(()=>  
                     Option(()=> tag() )
                  && PlusRepeat(()=>     @class() || id() ) ) );
		}
        public bool attribute()    /*^attribute :  '[' tag ([|~*$^]? '=') (tag / string) ']' / '[' (tag / string) ']';*/
        {

           return TreeAST((int)EnLess.attribute,()=>
                  
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
        public bool @class()    /*^class:  '.' [_a-zA-Z] [-a-zA-Z0-9_]*;*/
        {

           return TreeAST((int)EnLess.@class,()=>
                And(()=>  
                     Char('.')
                  && (In('a','z', 'A','Z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool id()    /*^id: '#' [_a-zA-Z] [-a-zA-Z0-9_]*;*/
        {

           return TreeAST((int)EnLess.id,()=>
                And(()=>  
                     Char('#')
                  && (In('a','z', 'A','Z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool tag()    /*^tag : [a-zA-Z] [-a-zA-Z]* [0-9]? / '*';*/
        {

           return TreeAST((int)EnLess.tag,()=>
                  
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
        public bool dimension()    /*^^dimension: number WS unit;*/
        {

           return TreeNT((int)EnLess.dimension,()=>
                And(()=>    number() && WS() && unit() ) );
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