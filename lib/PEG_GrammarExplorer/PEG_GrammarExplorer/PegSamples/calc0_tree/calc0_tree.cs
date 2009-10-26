/* created on 21.09.2008 16:11:37 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace calc0_tree
{
      
      enum Ecalc0_tree{Calc= 1, Assign= 2, Sum= 3, Prod= 4, Value= 5, Call= 6, Number= 7, 
                        ident= 8, S= 9};
      class calc0_tree : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public calc0_tree()
            : base()
        {
            
        }
        public calc0_tree(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Ecalc0_tree ruleEnum = (Ecalc0_tree)id;
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
        public bool Calc()    /*[1]^^Calc:  ((^'print' / Assign / Sum) 
            ([\r\n]/!./FATAL<"end of line expected">)
            [ \r\n\t\v]* )+ 
            (!./FATAL<"not recognized">);*/
        {

           return TreeNT((int)Ecalc0_tree.Calc,()=>
                And(()=>  
                     PlusRepeat(()=>    
                      And(()=>      
                               (        
                                       TreeChars(()=> Char('p','r','i','n','t') )
                                    || Assign()
                                    || Sum())
                            && (        
                                       OneOf("\r\n")
                                    || Not(()=> Any() )
                                    || Fatal("end of line expected"))
                            && OptRepeat(()=> OneOf(" \r\n\t\v") ) ) )
                  && (    Not(()=> Any() ) || Fatal("not recognized")) ) );
		}
        public bool Assign()    /*[2]^Assign:S ident S '=' S Sum;*/
        {

           return TreeAST((int)Ecalc0_tree.Assign,()=>
                And(()=>  
                     S()
                  && ident()
                  && S()
                  && Char('=')
                  && S()
                  && Sum() ) );
		}
        public bool Sum()    /*[3]^Sum:   Prod  (^[+-] S @Prod)*;*/
        {

           return TreeAST((int)Ecalc0_tree.Sum,()=>
                And(()=>  
                     Prod()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> OneOf("+-") )
                            && S()
                            && (    Prod() || Fatal("<<Prod>> expected")) ) ) ) );
		}
        public bool Prod()    /*[4]^Prod:  Value (^[* /] S @Value)*;*/
        {

           return TreeAST((int)Ecalc0_tree.Prod,()=>
                And(()=>  
                     Value()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> OneOf("*/") )
                            && S()
                            && (    Value() || Fatal("<<Value>> expected")) ) ) ) );
		}
        public bool Value()    /*[5] Value: (Number/'('S Sum @')'S/Call/ident) S;*/
        {

           return And(()=>  
                     (    
                         Number()
                      || And(()=>      
                               Char('(')
                            && S()
                            && Sum()
                            && (    Char(')') || Fatal("<<')'>> expected"))
                            && S() )
                      || Call()
                      || ident())
                  && S() );
		}
        public bool Call()    /*[6]^Call:   ident S '(' @Sum @')' S;*/
        {

           return TreeAST((int)Ecalc0_tree.Call,()=>
                And(()=>  
                     ident()
                  && S()
                  && Char('(')
                  && (    Sum() || Fatal("<<Sum>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool Number()    /*[7]^Number:[0-9]+ ('.' [0-9]+)?([eE][+-][0-9]+)?;*/
        {

           return TreeAST((int)Ecalc0_tree.Number,()=>
                And(()=>  
                     PlusRepeat(()=> In('0','9') )
                  && Option(()=>    
                      And(()=>      
                               Char('.')
                            && PlusRepeat(()=> In('0','9') ) ) )
                  && Option(()=>    
                      And(()=>      
                               OneOf("eE")
                            && OneOf("+-")
                            && PlusRepeat(()=> In('0','9') ) ) ) ) );
		}
        public bool ident()    /*[8]^ident: [A-Za-z_][A-Za-z_0-9]*;*/
        {

           return TreeAST((int)Ecalc0_tree.ident,()=>
                And(()=>  
                     (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) ) ) );
		}
        public bool S()    /*[9] S:	    [ \t\v]*;*/
        {

           return OptRepeat(()=> OneOf(" \t\v") );
		}
		#endregion Grammar Rules
   }
}