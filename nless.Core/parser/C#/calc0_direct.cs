/* created on 16/08/2009 10:47:17 from peg generator V1.0 using '' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace calc0_direct
{
      
      enum Ecalc0_direct{Expr= 1, Sum= 2, Product= 3, Value= 4, Number= 5, S= 6};
      class calc0_direct : PegCharParser 
      {
        class Top{ // semantic top level block using C# as host language
          internal double result;
          internal bool print_(){Console.WriteLine("{0}",result);return true;}
        }
        Top top;
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public calc0_direct()
            : base()
        {
            top= new Top();

        }
        public calc0_direct(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            top= new Top();

        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Ecalc0_direct ruleEnum = (Ecalc0_direct)id;
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
        public bool Expr()    /*Expr:    S Sum   (!. print_ / FATAL<"following code not recognized">);*/
        {

           return And(()=>  
                     S()
                  && Sum()
                  && (    
                         And(()=>    Not(()=> Any() ) && top.print_() )
                      || Fatal("following code not recognized")) );
		}
   class _Sum{  //semantic rule related block using C# as host language 
      double v;
      internal bool save_()  {v=  parent_.top.result;parent_.top.result=0; return true;}
      internal bool add_()   {v+= parent_.top.result;parent_.top.result=0;return true;}
      internal bool sub_()   {v-= parent_.top.result;parent_.top.result=0;return true;} 
      internal bool store_() {parent_.top.result= v; return true;}
      internal _Sum(calc0_direct grammarClass){ parent_ = grammarClass; }
      calc0_direct parent_;
      }	
        public bool Sum()    /*Sum
{  //semantic rule related block using C# as host language 
   double v;
   bool save_()  {v=  result;result=0; return true;}
   bool add_()   {v+= result;result=0;return true;}
   bool sub_()   {v-= result;result=0;return true;} 
   bool store_() {result= v; return true;}
}	:     
	Product save_
                ('+' S Product add_
		/'-' S Product sub_)* store_		;*/
        {

             var _sem= new _Sum(this);

           return And(()=>  
                     Product()
                  && _sem.save_()
                  && OptRepeat(()=>    
                            
                               And(()=>        
                                       Char('+')
                                    && S()
                                    && Product()
                                    && _sem.add_() )
                            || And(()=>        
                                       Char('-')
                                    && S()
                                    && Product()
                                    && _sem.sub_() ) )
                  && _sem.store_() );
		}
   class _Product{ //semantic rule related block using C# as host language 
      double v;
      internal bool save_()  {v=  parent_.top.result;parent_.top.result=0; return true;}
      internal bool mul_()   {v*= parent_.top.result;parent_.top.result=0; return true;}
      internal bool div_()   {v/= parent_.top.result;parent_.top.result=0;return true;} 
      internal bool store_() {parent_.top.result= v;return true;}	
      internal _Product(calc0_direct grammarClass){ parent_ = grammarClass; }
      calc0_direct parent_;
      }	
        public bool Product()    /*Product
{ //semantic rule related block using C# as host language 
   double v;
   bool save_()  {v=  result;result=0; return true;}
   bool mul_()   {v*= result;result=0; return true;}
   bool div_()   {v/= result;result=0;return true;} 
   bool store_() {result= v;return true;}	
}	: 
	Value  save_
	        ('*' S Value mul_
		/'/' S Value div_)* store_		;*/
        {

             var _sem= new _Product(this);

           return And(()=>  
                     Value()
                  && _sem.save_()
                  && OptRepeat(()=>    
                            
                               And(()=>        
                                       Char('*')
                                    && S()
                                    && Value()
                                    && _sem.mul_() )
                            || And(()=>        
                                       Char('/')
                                    && S()
                                    && Value()
                                    && _sem.div_() ) )
                  && _sem.store_() );
		}
        public bool Value()    /*Value:   Number S / '(' S Sum ')' S	;*/
        {

           return   
                     And(()=>    Number() && S() )
                  || And(()=>    
                         Char('(')
                      && S()
                      && Sum()
                      && Char(')')
                      && S() );
		}
   class _Number{ //semantic rule related block using C# as host language 
      internal string sNumber;
      internal bool store_(){double.TryParse(sNumber,out parent_.top.result);return true;}
      internal _Number(calc0_direct grammarClass){ parent_ = grammarClass; }
      calc0_direct parent_;
      }
   	
        public bool Number()    /*Number
{ //semantic rule related block using C# as host language 
   string sNumber;
   bool store_(){double.TryParse(sNumber,out result);return true;}
}
	:  ([0-9]+ ('.' [0-9]+)?):sNumber store_	;*/
        {

             var _sem= new _Number(this);

           return And(()=>  
                     Into(()=>    
                      And(()=>      
                               PlusRepeat(()=> In('0','9') )
                            && Option(()=>        
                                    And(()=>          
                                                 Char('.')
                                              && PlusRepeat(()=> In('0','9') ) ) ) ),out _sem.sNumber)
                  && _sem.store_() );
		}
        public bool S()    /*S:	 [ \n\r\t\v]*					;*/
        {

           return OptRepeat(()=> OneOf(" \n\r\t\v") );
		}
		#endregion Grammar Rules
   }
}