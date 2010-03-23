/* created on 23/03/2010 21:57:06 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace nLess
{
      
      enum EnLess{Parse= 1, primary= 2, import= 3, insert= 4, import_url= 5, medias= 6, 
                   url= 7, url_path= 8, comment= 9, declaration= 10, standard_declaration= 11, 
                   catchall_declaration= 12, ident= 13, variable= 14, expressions= 15, 
                   operation_expressions= 16, space_delimited_expressions= 17, important= 18, 
                   expression= 19, @operator= 20, ruleset= 21, standard_ruleset= 22, 
                   mixin_ruleset= 23, mixin_descendant= 24, selectors= 25, selector= 26, 
                   arguments= 27, argument= 28, element= 29, class_id= 30, attribute= 31, 
                   @class= 32, id= 33, tag= 34, select= 35, function= 36, function_name= 37, 
                   entity= 38, accessor= 39, accessor_name= 40, accessor_key= 41, 
                   cursors= 42, cursor= 43, fonts= 44, font= 45, literal= 46, dimension_list= 47, 
                   keyword= 48, @string= 49, dimension= 50, number= 51, unit= 52, 
                   color= 53, rgb= 54, rgb_node= 55, hex= 56, WS= 57, ws= 58, s= 59, 
                   S= 60, ns= 61, na= 62};
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
        public bool primary()    /*^^primary: (import / insert/ declaration / ruleset / comment)* ;*/
        {

           return TreeNT((int)EnLess.primary,()=>
                OptRepeat(()=>  
                      
                         import()
                      || insert()
                      || declaration()
                      || ruleset()
                      || comment() ) );
		}
        public bool import()    /*^^import :  ws '@import'  S import_url medias? s ';' ;*/
        {

           return TreeNT((int)EnLess.import,()=>
                And(()=>  
                     ws()
                  && Char('@','i','m','p','o','r','t')
                  && S()
                  && import_url()
                  && Option(()=> medias() )
                  && s()
                  && Char(';') ) );
		}
        public bool insert()    /*^^insert :  ws '@insert'  S import_url medias? s ';' ;*/
        {

           return TreeNT((int)EnLess.insert,()=>
                And(()=>  
                     ws()
                  && Char('@','i','n','s','e','r','t')
                  && S()
                  && import_url()
                  && Option(()=> medias() )
                  && s()
                  && Char(';') ) );
		}
        public bool import_url()    /*^^import_url : ( string / url  ) ;*/
        {

           return TreeNT((int)EnLess.import_url,()=>
                    @string() || url() );
		}
        public bool medias()    /*^^medias : [-a-z]+ (s ',' s [a-z]+)*;*/
        {

           return TreeNT((int)EnLess.medias,()=>
                And(()=>  
                     PlusRepeat(()=> (In('a','z')||OneOf("-")) )
                  && OptRepeat(()=>    
                      And(()=>      
                               s()
                            && Char(',')
                            && s()
                            && PlusRepeat(()=> In('a','z') ) ) ) ) );
		}
        public bool url()    /*^url: 'url(' url_path ')';*/
        {

           return TreeAST((int)EnLess.url,()=>
                And(()=>  
                     Char('u','r','l','(')
                  && url_path()
                  && Char(')') ) );
		}
        public bool url_path()    /*^url_path: (string / [-a-zA-Z0-9_%$/.&=:;#+?]+ );*/
        {

           return TreeAST((int)EnLess.url_path,()=>
                  
                     @string()
                  || PlusRepeat(()=> OneOf(optimizedCharset0) ) );
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
        public bool standard_declaration()    /*standard_declaration: ws (ident / variable)  ws (comment)* ':' ws (comment)* expressions (comment)* ws (';'/ ws &'}') ws ;*/
        {

           return And(()=>  
                     ws()
                  && (    ident() || variable())
                  && ws()
                  && OptRepeat(()=> comment() )
                  && Char(':')
                  && ws()
                  && OptRepeat(()=> comment() )
                  && expressions()
                  && OptRepeat(()=> comment() )
                  && ws()
                  && (    
                         Char(';')
                      || And(()=>    ws() && Peek(()=> Char('}') ) ))
                  && ws() );
		}
        public bool catchall_declaration()    /*catchall_declaration:  ws ident s ':' s ';' ws ;*/
        {

           return And(()=>  
                     ws()
                  && ident()
                  && s()
                  && Char(':')
                  && s()
                  && Char(';')
                  && ws() );
		}
        public bool ident()    /*^^ident: '*'? '-'? [-_a-zA-Z] [-_a-zA-Z0-9]*;*/
        {

           return TreeNT((int)EnLess.ident,()=>
                And(()=>  
                     Option(()=> Char('*') )
                  && Option(()=> Char('-') )
                  && (In('a','z', 'A','Z')||OneOf("-_"))
                  && OptRepeat(()=>    
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
                  || PlusRepeat(()=> OneOf(optimizedCharset1) ) );
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
        public bool important()    /*^^important: s '!' s 'important' ;*/
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
        public bool ruleset()    /*ruleset : standard_ruleset / mixin_ruleset;*/
        {

           return     standard_ruleset() || mixin_ruleset();
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
        public bool mixin_ruleset()    /*^^mixin_ruleset :  ws mixin_descendant (s ',' ws mixin_descendant)* ws ';' ws;*/
        {

           return TreeNT((int)EnLess.mixin_ruleset,()=>
                And(()=>  
                     ws()
                  && mixin_descendant()
                  && OptRepeat(()=>    
                      And(()=>      
                               s()
                            && Char(',')
                            && ws()
                            && mixin_descendant() ) )
                  && ws()
                  && Char(';')
                  && ws() ) );
		}
        public bool mixin_descendant()    /*^^mixin_descendant: ws selector (WS selector)* ws ;*/
        {

           return TreeNT((int)EnLess.mixin_descendant,()=>
                And(()=>  
                     ws()
                  && selector()
                  && OptRepeat(()=> And(()=>    WS() && selector() ) )
                  && ws() ) );
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
        public bool selector()    /*^^selector : (s select element s)+ ws arguments? ;*/
        {

           return TreeNT((int)EnLess.selector,()=>
                And(()=>  
                     PlusRepeat(()=>    
                      And(()=>    s() && select() && element() && s() ) )
                  && ws()
                  && Option(()=> arguments() ) ) );
		}
        public bool arguments()    /*^^arguments : '(' s (argument s (',' s argument s)*)? ')';*/
        {

           return TreeNT((int)EnLess.arguments,()=>
                And(()=>  
                     Char('(')
                  && s()
                  && Option(()=>    
                      And(()=>      
                               argument()
                            && s()
                            && OptRepeat(()=>        
                                    And(()=>          
                                                 Char(',')
                                              && s()
                                              && argument()
                                              && s() ) ) ) )
                  && Char(')') ) );
		}
        public bool argument()    /*^^argument : color &na / number unit &na / string &na / [a-zA-Z]+ '=' dimension &na / function &na / expressions &na / keyword (S keyword)* &na / variable s ':' s expressions &na / [-a-zA-Z0-9_%$/.&=:;#+?]+ &na;*/
        {

           return TreeNT((int)EnLess.argument,()=>
                  
                     And(()=>    color() && Peek(()=> na() ) )
                  || And(()=>    number() && unit() && Peek(()=> na() ) )
                  || And(()=>    @string() && Peek(()=> na() ) )
                  || And(()=>    
                         PlusRepeat(()=> In('a','z', 'A','Z') )
                      && Char('=')
                      && dimension()
                      && Peek(()=> na() ) )
                  || And(()=>    function() && Peek(()=> na() ) )
                  || And(()=>    expressions() && Peek(()=> na() ) )
                  || And(()=>    
                         keyword()
                      && OptRepeat(()=> And(()=>    S() && keyword() ) )
                      && Peek(()=> na() ) )
                  || And(()=>    
                         variable()
                      && s()
                      && Char(':')
                      && s()
                      && expressions()
                      && Peek(()=> na() ) )
                  || And(()=>    
                         PlusRepeat(()=> OneOf(optimizedCharset2) )
                      && Peek(()=> na() ) ) );
		}
        public bool element()    /*^^element : (class_id / tag / ident) attribute* ('(' (ident attribute* / attribute+) ')')? / attribute+ / '@media' / '@font-face';*/
        {

           return TreeNT((int)EnLess.element,()=>
                  
                     And(()=>    
                         (    class_id() || tag() || ident())
                      && OptRepeat(()=> attribute() )
                      && Option(()=>      
                            And(()=>        
                                       Char('(')
                                    && (          
                                                 And(()=>            
                                                             ident()
                                                          && OptRepeat(()=> attribute() ) )
                                              || PlusRepeat(()=> attribute() ))
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
        public bool entity()    /*^^entity :  fonts / function / accessor / keyword / variable / literal / cursors ;*/
        {

           return TreeNT((int)EnLess.entity,()=>
                  
                     fonts()
                  || function()
                  || accessor()
                  || keyword()
                  || variable()
                  || literal()
                  || cursors() );
		}
        public bool accessor()    /*^^accessor: accessor_name '[' accessor_key ']';*/
        {

           return TreeNT((int)EnLess.accessor,()=>
                And(()=>  
                     accessor_name()
                  && Char('[')
                  && accessor_key()
                  && Char(']') ) );
		}
        public bool accessor_name()    /*^^accessor_name: (class_id / tag) ;*/
        {

           return TreeNT((int)EnLess.accessor_name,()=>
                    class_id() || tag() );
		}
        public bool accessor_key()    /*^^accessor_key: (string / variable) ;*/
        {

           return TreeNT((int)EnLess.accessor_key,()=>
                    @string() || variable() );
		}
        public bool cursors()    /*^^cursors : cursor (s ',' s cursor)+  ;*/
        {

           return TreeNT((int)EnLess.cursors,()=>
                And(()=>  
                     cursor()
                  && PlusRepeat(()=>    
                      And(()=>    s() && Char(',') && s() && cursor() ) ) ) );
		}
        public bool cursor()    /*^^cursor : [-a-zA-Z0-9]*  / url ;*/
        {

           return TreeNT((int)EnLess.cursor,()=>
                  
                     OptRepeat(()=>    
                      (In('a','z', 'A','Z', '0','9')||OneOf("-")) )
                  || url() );
		}
        public bool fonts()    /*^^fonts : font (s ',' s font)+  ;*/
        {

           return TreeNT((int)EnLess.fonts,()=>
                And(()=>  
                     font()
                  && PlusRepeat(()=>    
                      And(()=>    s() && Char(',') && s() && font() ) ) ) );
		}
        public bool font()    /*^^font: [a-zA-Z] [-a-zA-Z0-9]* / string  ;*/
        {

           return TreeNT((int)EnLess.font,()=>
                  
                     And(()=>    
                         In('a','z', 'A','Z')
                      && OptRepeat(()=>      
                            (In('a','z', 'A','Z', '0','9')||OneOf("-")) ) )
                  || @string() );
		}
        public bool literal()    /*^^literal: color / dimension_list / number unit / string ;*/
        {

           return TreeNT((int)EnLess.literal,()=>
                  
                     color()
                  || dimension_list()
                  || And(()=>    number() && unit() )
                  || @string() );
		}
        public bool dimension_list()    /*^^dimension_list:  (dimension / [-a-z]+) '/' dimension;*/
        {

           return TreeNT((int)EnLess.dimension_list,()=>
                And(()=>  
                     (    
                         dimension()
                      || PlusRepeat(()=> (In('a','z')||OneOf("-")) ))
                  && Char('/')
                  && dimension() ) );
		}
        public bool keyword()    /*^^keyword: [-a-zA-Z]+ !ns;*/
        {

           return TreeNT((int)EnLess.keyword,()=>
                And(()=>  
                     PlusRepeat(()=> (In('a','z', 'A','Z')||OneOf("-")) )
                  && Not(()=> ns() ) ) );
		}
        public bool @string()    /*^string: ['] ( [\\]['] / !['] . )* ['] / ["] ( [\\]["] / !["] . )* ["] ;*/
        {

           return TreeAST((int)EnLess.@string,()=>
                  
                     And(()=>    
                         OneOf("'")
                      && OptRepeat(()=>      
                                    
                                       And(()=>    OneOf("\\") && OneOf("'") )
                                    || And(()=>    Not(()=> OneOf("'") ) && Any() ) )
                      && OneOf("'") )
                  || And(()=>    
                         OneOf("\"")
                      && OptRepeat(()=>      
                                    
                                       And(()=>    OneOf("\\") && OneOf("\"") )
                                    || And(()=>    Not(()=> OneOf("\"") ) && Any() ) )
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
        public bool hex()    /*^hex: [a-fA-F0-9];

//******************************************** Common*/
        {

           return TreeAST((int)EnLess.hex,()=>
                In('a','f', 'A','F', '0','9') );
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
        public bool ns()    /*ns: ![ ;,)}\n] .;*/
        {

           return And(()=>    Not(()=> OneOf(" ;,)}\n") ) && Any() );
		}
        public bool na()    /*na: s (',' / ')');*/
        {

           return And(()=>    s() && (    Char(',') || Char(')')) );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedCharset optimizedCharset1;
        internal static OptimizedCharset optimizedCharset2;
        
        internal static OptimizedLiterals optimizedLiterals0;
        
        static nLess()
        {
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('A','Z'),
                   new OptimizedCharset.Range('0','9'),
                   };
               char[] oneOfChars = new char[]    {'-','_','%','$','/'
                                                  ,'.','&','=',':',';'
                                                  ,'#','+','?'};
               optimizedCharset0= new OptimizedCharset(ranges,oneOfChars);
            }
            
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
               optimizedCharset1= new OptimizedCharset(ranges,oneOfChars);
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
               optimizedCharset2= new OptimizedCharset(ranges,oneOfChars);
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