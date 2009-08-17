/* created on 17/08/2009 10:07:44 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace nLess
{
      
      enum EnLess{Parse= 1, comment= 2, declaration= 3, ident= 4, variable= 5, 
                   expressions= 6, expression= 7, @operator= 8, entity= 9, fonts= 10, 
                   font= 11, literal= 12, keyword= 13, @string= 14, dimension= 15, 
                   number= 16, unit= 17, color= 18, rgb= 19, hex= 20, WS= 21, ws= 22, 
                   s= 23, S= 24, ns= 25};
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
        public bool Parse()    /*^^Parse:  (comment/declaration)*;*/
        {

           return TreeNT((int)EnLess.Parse,()=>
                OptRepeat(()=>     comment() || declaration() ) );
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
        public bool @operator()    /*^^operator: S [-+* /] S / [-+* /] ;


//******************************************** Entity*/
        {

           return TreeNT((int)EnLess.@operator,()=>
                  
                     And(()=>    S() && OneOf("-+*/") && S() )
                  || OneOf("-+*/") );
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
        public bool @string()    /*^^string: ['] (!['] . )* ['];*/
        {

           return TreeNT((int)EnLess.@string,()=>
                And(()=>  
                     OneOf("'")
                  && OptRepeat(()=>    
                      And(()=>    Not(()=> OneOf("'") ) && Any() ) )
                  && OneOf("'") ) );
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
               string[] literals=
               { "px","em","pc","%","ex","s","pt","cm",
                  "mm" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            
        }
        #endregion Optimization Data 
           }
}