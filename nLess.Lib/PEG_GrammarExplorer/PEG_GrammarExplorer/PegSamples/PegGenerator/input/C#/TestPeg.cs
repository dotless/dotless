/* created on 30.09.2008 09:24:59 from peg generator V1.0 using 'TestPeg.txt' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace TestPeg
{
      
      enum ETestPeg{Test1= 1, Test2= 2, Test3= 3, Test4= 4, Test5= 5, name_list= 10, 
                     list= 11, binary= 12, name= 13, S= 14};
      class TestPeg : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public TestPeg()
            : base()
        {
            
        }
        public TestPeg(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   ETestPeg ruleEnum = (ETestPeg)id;
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
        public bool Test1()    /*[1] Test1: [["':({}}];*/
        {

           return OneOf(optimizedCharset0);
		}
        public bool Test2()    /*[2] Test2: '\'[":({}]}';*/
        {

           return Char("\'[\":({}]}");
		}
        public bool Test3()    /*[3] Test3: '\'["}';*/
        {

           return Char('\'','[','"','}');
		}
        public bool Test4()    /*[4] Test4: (((Test1)))Test2;*/
        {

           return And(()=>    Test1() && Test2() );
		}
        public bool Test5()    /*[5] Test5: #x25#b11#30;*/
        {

           return And(()=>  
                     Char('\u0025')
                  && Char('\u0003')
                  && Char('\u001e') );
		}
        public bool name_list()    /*[10]name_list: list<name,','>;*/
        {

           return list(()=> name() ,()=>Char(',') );
		}
        public bool list(Matcher operand, Matcher separator)    /*[11]list<operand,separator>:	
			binary<operand,separator>;*/
        {

           return binary(()=> operand() ,()=>separator() );
		}
        public bool binary(Matcher operand, Matcher @operator)    /*[12]binary<operand,operator>:
			operand S (operator S @operand S)* ;*/
        {

           return And(()=>  
                     operand()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>      
                               @operator()
                            && S()
                            && (    operand() || Fatal("<<operand>> expected"))
                            && S() ) ) );
		}
        public bool name()    /*[13]name: [A-Za-z_][A-Za-z_0-9]*;*/
        {

           return And(()=>  
                     (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) ) );
		}
        public bool S()    /*[14]S:    [ \r\t\v\n]*;*/
        {

           return OptRepeat(()=> OneOf(" \r\t\v\n") );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        
        static TestPeg()
        {
            {
               char[] oneOfChars = new char[]    {'[','"','\'',':','('
                                                  ,'{','}','}'};
               optimizedCharset0= new OptimizedCharset(null,oneOfChars);
            }
            
            
            
        }
        #endregion Optimization Data 
           }
}