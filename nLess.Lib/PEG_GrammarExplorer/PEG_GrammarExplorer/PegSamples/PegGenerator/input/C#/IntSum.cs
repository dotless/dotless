/* created on 25.09.2008 15:13:26 from peg generator V1.0 using 'C_KernighanRitchie2_peg.txt' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace IntSum
{
      
      enum EIntSum{Sum= 1, S= 2};
      class IntSum : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public IntSum()
            : base()
        {
            
        }
        public IntSum(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   EIntSum ruleEnum = (EIntSum)id;
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
        public bool Sum()    /*Sum:    S [0-9]+  ([+-] S [0-9]+)* S 	;*/
        {

           return And(()=>  
                     S()
                  && PlusRepeat(()=> In('0','9') )
                  && OptRepeat(()=>    
                      And(()=>      
                               OneOf("+-")
                            && S()
                            && PlusRepeat(()=> In('0','9') ) ) )
                  && S() );
		}
        public bool S()    /*S:	 [ \n\r\t\v]*			;*/
        {

           return OptRepeat(()=> OneOf(" \n\r\t\v") );
		}
		#endregion Grammar Rules
   }
}