/* created on 26/08/2009 15:53:17 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace nLess
{
      
      enum EnLess{Parse= 1, primary= 2, comment= 3, declaration= 4, standard_declaration= 5, 
                   catchall_declaration= 6, ident= 7, variable= 8, expressions= 9, 
                   operation_expressions= 10, space_delimited_expressions= 11, important= 12, 
                   expression= 13, @operator= 14, ruleset= 15, standard_ruleset= 16, 
                   mixin_ruleset= 17, selectors= 18, selector= 19, arguments= 20, 
                   argument= 21, element= 22, class_id= 23, attribute= 24, @class= 25, 
                   id= 26, tag= 27, select= 28, function= 29, function_name= 30, 
                   entity= 31, fonts= 32, font= 33, literal= 34, keyword= 35, @string= 36, 
                   dimension= 37, number= 38, unit= 39, color= 40, rgb= 41, rgb_node= 42, 
                   hex= 43, WS= 44, ws= 45, s= 46, S= 47, ns= 48};
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
        public bool Parse()    /*^Parse:  primary ;*/
        {

           return TreeAST((int)EnLess.Parse,()=> primary() );
		}
        public bool primary()    /*^^primary: (comment/ declaration/ ruleset)* ;*/
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
        public bool ident()    /*^^ident: '*'? '-'? [-_a-zA-Z0-9]+;*/
        {

           return TreeNT((int)EnLess.ident,()=>
                And(()=>  
                     Option(()=> Char('*') )
                  && Option(()=> Char('-') )
                  && PlusRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool variable()    /*^^variable: '@' [-_a-zA-Z0-9]+;*/
        {

           return TreeNT((int)EnLess.variable,()=>
                And(()=>  
                     Char('@')
                  && PlusRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-_")) ) ) );
		}
        public bool expressions()    /*^^expressions: operation_expressions / space_delimited_expressions / [-a-zA-Z0-9_%* /.&=:,#+? \[\]()]+ ;*/
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
        public bool space_delimited_expressions()    /*^^space_delimited_expressions: expression (WS expression)* important? ;*/
        {

           return TreeNT((int)EnLess.space_delimited_expressions,()=>
                And(()=>  
                     expression()
                  && OptRepeat(()=> And(()=>    WS() && expression() ) )
                  && Option(()=> important() ) ) );
		}
        public bool important()    /*^^ important:      s '!' s 'important' ;*/
        {

           return TreeNT((int)EnLess.important,()=>
                And(()=>    s() && Char('!') && s() && Char("important") ) );
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
        public bool function()    /*^^function: function_name arguments ;*/
        {

           return TreeNT((int)EnLess.function,()=>
                And(()=>    function_name() && arguments() ) );
		}
        public bool function_name()    /*^^function_name: [-a-zA-Z_]+;

//******************************************** Entity*/
        {

           return TreeNT((int)EnLess.function_name,()=>
                PlusRepeat(()=> (In('a','z', 'A','Z')||OneOf("-_")) ) );
		}
        public bool entity()    /*^^entity :  function / fonts / keyword  / variable / literal ; //accessor missing*/
        {

           return TreeNT((int)EnLess.entity,()=>
                  
                     function()
                  || fonts()
                  || keyword()
                  || variable()
                  || literal() );
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
        public bool rgb()    /*^rgb:(rgb_node)(rgb_node)(rgb_node) / hex hex hex ;*/
        {

           return TreeAST((int)EnLess.rgb,()=>
                  
                     And(()=>    rgb_node() && rgb_node() && rgb_node() )
                  || And(()=>    hex() && hex() && hex() ) );
		}
        public bool rgb_node()    /*^rgb_node : hex hex;*/
        {

           return TreeAST((int)EnLess.rgb_node,()=>
                And(()=>    hex() && hex() ) );
		}
        public bool hex()    /*hex: [a-fA-F0-9];

//******************************************** Common*/
        {

           return In('a','f', 'A','F', '0','9');
		}
        public bool WS()    /*WS: [ \r\n\t]+;*/
        {

           return PlusRepeat(()=> OneOf(" \r\n\t") );
		}
        public bool ws()    /*ws: [ \r\n\t]*;*/
        {

           return OptRepeat(()=> OneOf(" \r\n\t") );
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