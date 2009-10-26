/* created on 07.10.2008 01:42:39 from peg generator V1.0 using 'python_2_5_2_peg.txt' as input*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace python_2_5_2_i
{
      
      enum Epython_2_5_2_i{interactive_input= 1, file_input= 2, eval_input= 3, input_input= 4, 
                            atom= 5, enclosure= 6, literal= 7, parenth_form= 8, parenth_form_content= 9, 
                            list_display= 10, list_display_content= 11, list_comprehension= 12, 
                            list_for= 13, old_expression_list= 14, list_iter= 15, list_if= 16, 
                            generator_expression= 17, generator_expression_content= 18, genexpr_for= 19, 
                            genexpr_iter= 20, genexpr_if= 21, dict_display= 22, dict_display_content= 23, 
                            key_datum_list= 24, key_datum= 25, string_conversion= 26, yield_atom= 27, 
                            yield_atom_content= 28, yield_expression= 29, primary= 30, attributeref= 31, 
                            slicing= 32, slicing_content= 33, slice_list= 34, slice_item= 35, 
                            slice= 36, stride= 37, lower_bound= 38, upper_bound= 39, ellipsis= 40, 
                            call_args= 41, call_args_content= 42, argument_list= 43, seq_arg= 44, 
                            dict_arg= 45, positional_arguments= 46, keyword_arguments= 47, 
                            keyword_item= 48, power= 49, u_expr= 50, m_expr= 51, a_expr= 52, 
                            shift_expr= 53, and_expr= 54, xor_expr= 55, or_expr= 56, comparison= 57, 
                            comp_operator= 58, is_operator= 59, in_operator= 60, expression= 61, 
                            old_expression= 62, conditional_expression= 63, or_test= 64, 
                            and_test= 65, not_test= 66, not_symbol= 67, lambda_form= 68, 
                            old_lambda_form= 69, expression_list= 70, simple_stmt= 71, expression_stmt= 72, 
                            assert_stmt= 73, assignment_stmt= 74, target_list= 75, target= 76, 
                            target_contents= 77, assignable_primary= 78, targetlist_end= 79, 
                            augmented_assignment_stmt= 80, augop= 81, pass_stmt= 82, del_stmt= 83, 
                            print_stmt= 84, return_stmt= 85, yield_stmt= 86, raise_stmt= 87, 
                            break_stmt= 88, continue_stmt= 89, import_stmt= 90, import_name= 91, 
                            import_from= 92, import_as= 93, imports_in_parens= 94, imports_in_parens_content= 95, 
                            imports_list= 96, rest_import_list= 97, module= 98, relative_module= 99, 
                            name= 100, global_stmt= 101, exec_stmt= 102, compound_stmt= 103, 
                            suite= 104, statement= 105, stmt_list= 106, if_stmt= 107, while_stmt= 108, 
                            for_stmt= 109, try_stmt= 110, finally_only_handler= 111, except_handler= 112, 
                            with_stmt= 113, funcdef= 114, parameter_list_in= 115, decorators= 116, 
                            decorator= 117, decorater_in= 118, dotted_name= 119, parameter_list= 120, 
                            seq_dict_pars= 121, defparameter= 122, sublist= 123, parameter= 124, 
                            parameter_in= 125, funcname= 126, classdef= 127, inheritance= 128, 
                            classname= 129, S= 130, S2= 131, identifier= 132, INDENT= 133, 
                            KEEPINDENT= 134, DEDENT= 135, NEWLINE= 136, NL= 137, BLANKLINE= 138, 
                            NAME= 139, IDENT= 140, KEYWORD= 141, COMMENT= 142, ENDMARKER= 143, 
                            NUMBER= 144, STRING= 145, integer= 146, floatnumber= 147, pointfloat= 148, 
                            exponent= 149, imagnumber= 150, stringprefix= 151, raw_indicating_stringprefix= 152, 
                            unicodeonly_stringprefix= 153, shortstring= 154, shortstringitem1_content= 155, 
                            shortstringitem2_content= 156, longstring= 166, longstring1_content= 157, 
                            longstring2_content= 158, shortstringitem= 159, longstringitem= 160, 
                            check_unrecognized_escape= 161, escapeseq= 162, unicode_4digits= 163, 
                            unicode_8digits= 164, hex_2digits= 165, set_explicit_line_joining_= 167};
      class python_2_5_2_i : PegCharParser 
      {
        class _Top{
              internal bool init_() 
              { in_raw_mode= false;
                indentStack.Clear(); 
                indentStack.Push(0); 
                indentLength_=-1; 
                return true; 
              }
              #region raw string handling
              bool in_raw_mode;
              internal bool set_raw_(){in_raw_mode=true;return true;}
              internal bool unset_raw_(){in_raw_mode=false;return true;}
              internal bool in_raw_(){return in_raw_mode;}
              #endregion raw string handling
              #region implicit line joining
              bool in_impl_line_join_;
              internal bool in_implicit_line_joining_(){return in_impl_line_join_;}
              internal bool set_implicit_line_joining_(bool bOn,out bool prev)
              {prev=in_impl_line_join_;in_impl_line_join_=bOn;return true;}
              internal bool set_implicit_line_joining_(bool bOn)
              {in_impl_line_join_=bOn;return true;}
              #endregion implicit line joining
              internal string indent_;
              int indentLength_;//accounts for tabs
              int indentLengthToRestore_;
              System.Collections.Generic.Stack<int> indentStack= new System.Collections.Generic.Stack<int>();
              int NormalizedIndentLength()
              {
                 int indentLength = 0;
                 for (int i = 0; i < indent_.Length; ++i)
                 {
                    if (indent_[i] == '\t') indentLength += 8 - i % 8;
                    else                    indentLength++;
                 }
                 return indentLength;
              }
              internal bool continue_after_dedent_()
              {
                 indentLengthToRestore_= indentLength_;
                 if( indentLength_== -1 ) return false;
                 if (indentLength_ == indentStack.Peek()){
                    indentLength_=-1;
                    return true;
                 }
                 return false;
              }
              internal bool RESTOREINDENT_(){indentLength_= indentLengthToRestore_;return false;}
              internal bool keep_handler_()
              {
                  if( indentLength_!=-1 ) return false;//indentation belongs to other block
                  indentLength_= NormalizedIndentLength();
                  bool kept= indentStack.Peek()== indentLength_;
                  if( kept ) indentLength_=-1;
                  return kept;
              }
              internal bool indent_handler_()
              {
                  if( indentLength_!=-1 ) return false;//indentation belongs to other block
                  int indentLength = NormalizedIndentLength();
                  if (indentLength <= indentStack.Peek()) return false;
                  else{
                        indentStack.Push(indentLength);
                        indentLength_=-1;
                        return true;
                  }
              }
              internal bool dedent_handler_()
              {
                 System.Diagnostics.Debug.Assert(indentStack.Count>1);
                 if (indentLength_ >= indentStack.Peek() || !indentStack.Contains(indentLength_)) return false;
                 else{
                    indentStack.Pop();
                    if( indentStack.Count==1 ) indentLength_=-1;
                    return true;
                 }
              }
        }
        _Top _top;
        class Line_join_sem_ : IDisposable{
              bool prev_;
              internal Line_join_sem_(python_2_5_2_i parent){parent_= parent; parent_._top.set_implicit_line_joining_(true,out prev_);}
              
              python_2_5_2_i parent_;
              public void Dispose(){parent_._top.set_implicit_line_joining_(prev_);}
        }
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public python_2_5_2_i()
            : base()
        {
            _top= new _Top();


        }
        public python_2_5_2_i(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            _top= new _Top();


        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   Epython_2_5_2_i ruleEnum = (Epython_2_5_2_i)id;
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
        public bool interactive_input()    /*[1] ^^interactive_input: (compound_stmt/stmt_list?)
                        ( NEWLINE / FATAL<"syntax error: line break expected">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.interactive_input,()=>
                And(()=>  
                     (    compound_stmt() || Option(()=> stmt_list() ))
                  && (    
                         NEWLINE()
                      || Fatal("syntax error: line break expected")) ) );
		}
        public bool file_input()    /*[2] ^^file_input:       init_ BLANKLINE* (NEWLINE / statement)* ENDMARKER
                        (!./FATAL<"wrong indent or non-python text here">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.file_input,()=>
                And(()=>  
                     _top.init_()
                  && OptRepeat(()=> BLANKLINE() )
                  && OptRepeat(()=>     NEWLINE() || statement() )
                  && ENDMARKER()
                  && (    
                         Not(()=> Any() )
                      || Fatal("wrong indent or non-python text here")) ) );
		}
        public bool eval_input()    /*[3] ^^eval_input:       init_ expression_list NEWLINE*  ENDMARKER;*/
        {

           return TreeNT((int)Epython_2_5_2_i.eval_input,()=>
                And(()=>  
                     _top.init_()
                  && expression_list()
                  && OptRepeat(()=> NEWLINE() )
                  && ENDMARKER() ) );
		}
        public bool input_input()    /*[4] ^^input_input:      init_ expression_list NEWLINE  ENDMARKER;*/
        {

           return TreeNT((int)Epython_2_5_2_i.input_input,()=>
                And(()=>  
                     _top.init_()
                  && expression_list()
                  && NEWLINE()
                  && ENDMARKER() ) );
		}
        public bool atom()    /*[5] ^^atom:              identifier S / literal / enclosure;*/
        {

           return TreeNT((int)Epython_2_5_2_i.atom,()=>
                  
                     And(()=>    identifier() && S() )
                  || literal()
                  || enclosure() );
		}
        public bool enclosure()    /*[6]   enclosure:         generator_expression / dict_display
                       / string_conversion / yield_atom
                       / parenth_form / list_display;*/
        {

           return   
                     generator_expression()
                  || dict_display()
                  || string_conversion()
                  || yield_atom()
                  || parenth_form()
                  || list_display();
		}
        public bool literal()    /*[7]   literal :          (STRING S)+ / NUMBER S ;*/
        {

           return   
                     PlusRepeat(()=> And(()=>    STRING() && S() ) )
                  || And(()=>    NUMBER() && S() );
		}
        public bool parenth_form()    /*[8]   parenth_form:      '(' parenth_form_content @')' S;*/
        {

           return And(()=>  
                     Char('(')
                  && parenth_form_content()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() );
		}
        public bool parenth_form_content()    /*[9]   parenth_form_content 
using Line_join_sem_:    S expression_list?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && Option(()=> expression_list() ) );            
      }

		}
        public bool list_display()    /*[10]^^list_display:      '[' list_display_content @']' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.list_display,()=>
                And(()=>  
                     Char('[')
                  && list_display_content()
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S() ) );
		}
        public bool list_display_content()    /*[11]  list_display_content
using Line_join_sem_:   S (list_comprehension / expression_list )?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>  
                     S()
                  && Option(()=>    
                          list_comprehension() || expression_list() ) );            
      }

		}
        public bool list_comprehension()    /*[12]^^list_comprehension:expression list_for;*/
        {

           return TreeNT((int)Epython_2_5_2_i.list_comprehension,()=>
                And(()=>    expression() && list_for() ) );
		}
        public bool list_for()    /*[13]^^list_for:          'for' S2 target_list @'in' S2 old_expression_list list_iter?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.list_for,()=>
                And(()=>  
                     Char('f','o','r')
                  && S2()
                  && target_list()
                  && (    Char('i','n') || Fatal("<<'in'>> expected"))
                  && S2()
                  && old_expression_list()
                  && Option(()=> list_iter() ) ) );
		}
        public bool old_expression_list()    /*[14]^^old_expression_list:
                         old_expression ((',' S old_expression)+ (','S)?)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.old_expression_list,()=>
                And(()=>  
                     old_expression()
                  && Option(()=>    
                      And(()=>      
                               PlusRepeat(()=>        
                                    And(()=>    Char(',') && S() && old_expression() ) )
                            && Option(()=> And(()=>    Char(',') && S() ) ) ) ) ) );
		}
        public bool list_iter()    /*[15]^^list_iter:         list_for / list_if;*/
        {

           return TreeNT((int)Epython_2_5_2_i.list_iter,()=>
                    list_for() || list_if() );
		}
        public bool list_if()    /*[16]^^list_if:           'if' S2 old_expression list_iter?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.list_if,()=>
                And(()=>  
                     Char('i','f')
                  && S2()
                  && old_expression()
                  && Option(()=> list_iter() ) ) );
		}
        public bool generator_expression()    /*[17]^^generator_expression: 
                         '(' generator_expression_content @')' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.generator_expression,()=>
                And(()=>  
                     Char('(')
                  && generator_expression_content()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool generator_expression_content()    /*[18]generator_expression_content
using Line_join_sem_:    S expression genexpr_for;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && expression() && genexpr_for() );            
      }

		}
        public bool genexpr_for()    /*[19]^^genexpr_for:       'for' S2 target_list @'in' S2 or_test genexpr_iter?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.genexpr_for,()=>
                And(()=>  
                     Char('f','o','r')
                  && S2()
                  && target_list()
                  && (    Char('i','n') || Fatal("<<'in'>> expected"))
                  && S2()
                  && or_test()
                  && Option(()=> genexpr_iter() ) ) );
		}
        public bool genexpr_iter()    /*[20]^^genexpr_iter:      genexpr_for / genexpr_if;*/
        {

           return TreeNT((int)Epython_2_5_2_i.genexpr_iter,()=>
                    genexpr_for() || genexpr_if() );
		}
        public bool genexpr_if()    /*[21]^^genexpr_if:        'if' S2 old_expression genexpr_iter?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.genexpr_if,()=>
                And(()=>  
                     Char('i','f')
                  && S2()
                  && old_expression()
                  && Option(()=> genexpr_iter() ) ) );
		}
        public bool dict_display()    /*[22]^^dict_display:      '{' dict_display_content @'}' S ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.dict_display,()=>
                And(()=>  
                     Char('{')
                  && dict_display_content()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool dict_display_content()    /*[23] dict_display_content
using Line_join_sem_:    S key_datum_list?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && Option(()=> key_datum_list() ) );            
      }

		}
        public bool key_datum_list()    /*[24]^^key_datum_list:    key_datum (',' S key_datum)* (',' S)? ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.key_datum_list,()=>
                And(()=>  
                     key_datum()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && key_datum() ) )
                  && Option(()=> And(()=>    Char(',') && S() ) ) ) );
		}
        public bool key_datum()    /*[25]^^key_datum:         expression @':' S expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.key_datum,()=>
                And(()=>  
                     expression()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && expression() ) );
		}
        public bool string_conversion()    /*[26]^^string_conversion: '`' S expression_list @'`' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.string_conversion,()=>
                And(()=>  
                     Char('`')
                  && S()
                  && expression_list()
                  && (    Char('`') || Fatal("<<'`'>> expected"))
                  && S() ) );
		}
        public bool yield_atom()    /*[27]^^yield_atom:        '(' yield_atom_content @')' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.yield_atom,()=>
                And(()=>  
                     Char('(')
                  && yield_atom_content()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool yield_atom_content()    /*[28]yield_atom_content
using Line_join_sem_:    S yield_expression;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && yield_expression() );            
      }

		}
        public bool yield_expression()    /*[29]^^yield_expression:  'yield' S2 expression_list?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.yield_expression,()=>
                And(()=>  
                     Char('y','i','e','l','d')
                  && S2()
                  && Option(()=> expression_list() ) ) );
		}
        public bool primary()    /*[30] ^primary:           atom (attributeref /  slicing  / call_args)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.primary,()=>
                And(()=>  
                     atom()
                  && OptRepeat(()=>    
                          attributeref() || slicing() || call_args() ) ) );
		}
        public bool attributeref()    /*[31]^^attributeref:      '.' S @identifier S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.attributeref,()=>
                And(()=>  
                     Char('.')
                  && S()
                  && (    identifier() || Fatal("<<identifier>> expected"))
                  && S() ) );
		}
        public bool slicing()    /*[32]^^slicing:           '[' slicing_content @']' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.slicing,()=>
                And(()=>  
                     Char('[')
                  && slicing_content()
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S() ) );
		}
        public bool slicing_content()    /*[33]slicing_content
using Line_join_sem_:    S slice_list;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && slice_list() );            
      }

		}
        public bool slice_list()    /*[34]^^slice_list:        slice_item (',' S slice_item)* (','S)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.slice_list,()=>
                And(()=>  
                     slice_item()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && slice_item() ) )
                  && Option(()=> And(()=>    Char(',') && S() ) ) ) );
		}
        public bool slice_item()    /*[35]^^slice_item:        ellipsis / slice / expression ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.slice_item,()=>
                    ellipsis() || slice() || expression() );
		}
        public bool slice()    /*[36]^^slice:             lower_bound? ':' S upper_bound? stride?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.slice,()=>
                And(()=>  
                     Option(()=> lower_bound() )
                  && Char(':')
                  && S()
                  && Option(()=> upper_bound() )
                  && Option(()=> stride() ) ) );
		}
        public bool stride()    /*[37]^^stride:            ':' S expression?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.stride,()=>
                And(()=>  
                     Char(':')
                  && S()
                  && Option(()=> expression() ) ) );
		}
        public bool lower_bound()    /*[38]^^lower_bound:       expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.lower_bound,()=>
                expression() );
		}
        public bool upper_bound()    /*[39]^^upper_bound:       expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.upper_bound,()=>
                expression() );
		}
        public bool ellipsis()    /*[40]^^ellipsis:          '.' S '.' S '.' S; // or '...' ?*/
        {

           return TreeNT((int)Epython_2_5_2_i.ellipsis,()=>
                And(()=>  
                     Char('.')
                  && S()
                  && Char('.')
                  && S()
                  && Char('.')
                  && S() ) );
		}
        public bool call_args()    /*[41]^^call_args:         '(' call_args_content @')' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.call_args,()=>
                And(()=>  
                     Char('(')
                  && call_args_content()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool call_args_content()    /*[42] call_args_content
using Line_join_sem_      :S (expression genexpr_for / argument_list (',' S)? )?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>  
                     S()
                  && Option(()=>    
                            
                               And(()=>    expression() && genexpr_for() )
                            || And(()=>        
                                       argument_list()
                                    && Option(()=> And(()=>    Char(',') && S() ) ) ) ) );            
      }

		}
        public bool argument_list()    /*[43]^^argument_list:     keyword_arguments seq_arg? dict_arg?
                       / positional_arguments ((',' S) keyword_arguments)? seq_arg? dict_arg?
                       / seq_arg dict_arg?
                       / dict_arg;*/
        {

           return TreeNT((int)Epython_2_5_2_i.argument_list,()=>
                  
                     And(()=>    
                         keyword_arguments()
                      && Option(()=> seq_arg() )
                      && Option(()=> dict_arg() ) )
                  || And(()=>    
                         positional_arguments()
                      && Option(()=>      
                            And(()=>        
                                       And(()=>    Char(',') && S() )
                                    && keyword_arguments() ) )
                      && Option(()=> seq_arg() )
                      && Option(()=> dict_arg() ) )
                  || And(()=>    seq_arg() && Option(()=> dict_arg() ) )
                  || dict_arg() );
		}
        public bool seq_arg()    /*[44]^^seq_arg:           ',' S '*' S expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.seq_arg,()=>
                And(()=>  
                     Char(',')
                  && S()
                  && Char('*')
                  && S()
                  && expression() ) );
		}
        public bool dict_arg()    /*[45]^^dict_arg:          ',' S '**' S expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.dict_arg,()=>
                And(()=>  
                     Char(',')
                  && S()
                  && Char('*','*')
                  && S()
                  && expression() ) );
		}
        public bool positional_arguments()    /*[46]^^positional_arguments: 
                         expression (',' S !keyword_item expression)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.positional_arguments,()=>
                And(()=>  
                     expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && Not(()=> keyword_item() )
                            && expression() ) ) ) );
		}
        public bool keyword_arguments()    /*[47]^^keyword_arguments: keyword_item (',' S keyword_item)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.keyword_arguments,()=>
                And(()=>  
                     keyword_item()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && keyword_item() ) ) ) );
		}
        public bool keyword_item()    /*[48]^^keyword_item:      identifier S '=' S expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.keyword_item,()=>
                And(()=>  
                     identifier()
                  && S()
                  && Char('=')
                  && S()
                  && expression() ) );
		}
        public bool power()    /*[49] ^power:             primary ('**' S @u_expr)?;*/
        {

           return TreeAST((int)Epython_2_5_2_i.power,()=>
                And(()=>  
                     primary()
                  && Option(()=>    
                      And(()=>      
                               Char('*','*')
                            && S()
                            && (    u_expr() || Fatal("<<u_expr>> expected")) ) ) ) );
		}
        public bool u_expr()    /*[50] ^u_expr:            power / ^'-' S @u_expr / ^'+'  S @u_expr / ^'~' S @u_expr;*/
        {

           return TreeAST((int)Epython_2_5_2_i.u_expr,()=>
                  
                     power()
                  || And(()=>    
                         TreeChars(()=> Char('-') )
                      && S()
                      && (    u_expr() || Fatal("<<u_expr>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('+') )
                      && S()
                      && (    u_expr() || Fatal("<<u_expr>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('~') )
                      && S()
                      && (    u_expr() || Fatal("<<u_expr>> expected")) ) );
		}
        public bool m_expr()    /*[51] ^m_expr:            u_expr (^('*'/'//'/'/'/'%') S u_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.m_expr,()=>
                And(()=>  
                     u_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=>        
                                              
                                                 Char('*')
                                              || Char('/','/')
                                              || Char('/')
                                              || Char('%') )
                            && S()
                            && u_expr() ) ) ) );
		}
        public bool a_expr()    /*[52] ^a_expr:            m_expr ( ^[+-] S  @m_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.a_expr,()=>
                And(()=>  
                     m_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> OneOf("+-") )
                            && S()
                            && (    m_expr() || Fatal("<<m_expr>> expected")) ) ) ) );
		}
        public bool shift_expr()    /*[53] ^shift_expr:        a_expr (^('<<' / '>>' ) S @a_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.shift_expr,()=>
                And(()=>  
                     a_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=>     Char('<','<') || Char('>','>') )
                            && S()
                            && (    a_expr() || Fatal("<<a_expr>> expected")) ) ) ) );
		}
        public bool and_expr()    /*[54] ^and_expr:          shift_expr ('&' S @shift_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.and_expr,()=>
                And(()=>  
                     shift_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('&')
                            && S()
                            && (    shift_expr() || Fatal("<<shift_expr>> expected")) ) ) ) );
		}
        public bool xor_expr()    /*[55] ^xor_expr:          and_expr ( '^' S @and_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.xor_expr,()=>
                And(()=>  
                     and_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('^')
                            && S()
                            && (    and_expr() || Fatal("<<and_expr>> expected")) ) ) ) );
		}
        public bool or_expr()    /*[56] ^or_expr:           xor_expr ( '|' S @xor_expr)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.or_expr,()=>
                And(()=>  
                     xor_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('|')
                            && S()
                            && (    xor_expr() || Fatal("<<xor_expr>> expected")) ) ) ) );
		}
        public bool comparison()    /*[57] ^comparison:        or_expr ( comp_operator S @or_expr )*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.comparison,()=>
                And(()=>  
                     or_expr()
                  && OptRepeat(()=>    
                      And(()=>      
                               comp_operator()
                            && S()
                            && (    or_expr() || Fatal("<<or_expr>> expected")) ) ) ) );
		}
        public bool comp_operator()    /*[58]^^comp_operator:     '>=' / '<=' / '<>' / '<' / '>' / '==' / '!=' / 
                         is_operator / in_operator;*/
        {

           return TreeNT((int)Epython_2_5_2_i.comp_operator,()=>
                  
                     Char('>','=')
                  || Char('<','=')
                  || Char('<','>')
                  || Char('<')
                  || Char('>')
                  || Char('=','=')
                  || Char('!','=')
                  || is_operator()
                  || in_operator() );
		}
        public bool is_operator()    /*[59]^^is_operator:	 'is' S2 (not_symbol S2)? ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.is_operator,()=>
                And(()=>  
                     Char('i','s')
                  && S2()
                  && Option(()=> And(()=>    not_symbol() && S2() ) ) ) );
		}
        public bool in_operator()    /*[60]^^in_operator:	 (not_symbol S2)? 'in';*/
        {

           return TreeNT((int)Epython_2_5_2_i.in_operator,()=>
                And(()=>  
                     Option(()=> And(()=>    not_symbol() && S2() ) )
                  && Char('i','n') ) );
		}
        public bool expression()    /*[61]  expression:        conditional_expression / lambda_form;*/
        {

           return     conditional_expression() || lambda_form();
		}
        public bool old_expression()    /*[62]^^old_expression:    or_test / old_lambda_form;*/
        {

           return TreeNT((int)Epython_2_5_2_i.old_expression,()=>
                    or_test() || old_lambda_form() );
		}
        public bool conditional_expression()    /*[63]^^conditional_expression: 
                         or_test ('if' S2 @or_test @(^'else') S2 expression)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.conditional_expression,()=>
                And(()=>  
                     or_test()
                  && Option(()=>    
                      And(()=>      
                               Char('i','f')
                            && S2()
                            && (    or_test() || Fatal("<<or_test>> expected"))
                            && (        
                                       TreeChars(()=> Char('e','l','s','e') )
                                    || Fatal("<<(^'else')>> expected"))
                            && S2()
                            && expression() ) ) ) );
		}
        public bool or_test()    /*[64] ^or_test:           and_test ('or' S2 @and_test)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.or_test,()=>
                And(()=>  
                     and_test()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('o','r')
                            && S2()
                            && (    and_test() || Fatal("<<and_test>> expected")) ) ) ) );
		}
        public bool and_test()    /*[65] ^and_test:          not_test ( 'and' S2 @not_test)*;*/
        {

           return TreeAST((int)Epython_2_5_2_i.and_test,()=>
                And(()=>  
                     not_test()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('a','n','d')
                            && S2()
                            && (    not_test() || Fatal("<<not_test>> expected")) ) ) ) );
		}
        public bool not_test()    /*[66] ^not_test:          (not_symbol S2)* comparison;*/
        {

           return TreeAST((int)Epython_2_5_2_i.not_test,()=>
                And(()=>  
                     OptRepeat(()=> And(()=>    not_symbol() && S2() ) )
                  && comparison() ) );
		}
        public bool not_symbol()    /*[67]^^not_symbol:        'not';*/
        {

           return TreeNT((int)Epython_2_5_2_i.not_symbol,()=>
                Char('n','o','t') );
		}
        public bool lambda_form()    /*[68]^^lambda_form:       'lambda' S2 parameter_list? @':' S expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.lambda_form,()=>
                And(()=>  
                     Char('l','a','m','b','d','a')
                  && S2()
                  && Option(()=> parameter_list() )
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && expression() ) );
		}
        public bool old_lambda_form()    /*[69]^^old_lambda_form:   'lambda' S2 parameter_list? @':' S old_expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.old_lambda_form,()=>
                And(()=>  
                     Char('l','a','m','b','d','a')
                  && S2()
                  && Option(()=> parameter_list() )
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && old_expression() ) );
		}
        public bool expression_list()    /*[70]  expression_list:   expression ( ',' S expression )* (',' S)?;*/
        {

           return And(()=>  
                     expression()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && expression() ) )
                  && Option(()=> And(()=>    Char(',') && S() ) ) );
		}
        public bool simple_stmt()    /*[71]  simple_stmt:
                         assert_stmt
                       / pass_stmt
                       / del_stmt
                       / print_stmt
                       / return_stmt
                       / yield_stmt
                       / raise_stmt
                       / break_stmt
                       / continue_stmt
                       / import_stmt
                       / global_stmt
                       / exec_stmt
                       / augmented_assignment_stmt
                       / assignment_stmt
                       / expression_stmt;*/
        {

           return   
                     assert_stmt()
                  || pass_stmt()
                  || del_stmt()
                  || print_stmt()
                  || return_stmt()
                  || yield_stmt()
                  || raise_stmt()
                  || break_stmt()
                  || continue_stmt()
                  || import_stmt()
                  || global_stmt()
                  || exec_stmt()
                  || augmented_assignment_stmt()
                  || assignment_stmt()
                  || expression_stmt();
		}
        public bool expression_stmt()    /*[72]^^expression_stmt:   expression_list;*/
        {

           return TreeNT((int)Epython_2_5_2_i.expression_stmt,()=>
                expression_list() );
		}
        public bool assert_stmt()    /*[73]^^assert_stmt:       'assert' S2 @expression (',' S @expression)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.assert_stmt,()=>
                And(()=>  
                     Char('a','s','s','e','r','t')
                  && S2()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && Option(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    expression() || Fatal("<<expression>> expected")) ) ) ) );
		}
        public bool assignment_stmt()    /*[74]^^assignment_stmt:   (target_list '='!'=' S)+ @(expression_list / yield_expression);*/
        {

           return TreeNT((int)Epython_2_5_2_i.assignment_stmt,()=>
                And(()=>  
                     PlusRepeat(()=>    
                      And(()=>      
                               target_list()
                            && Char('=')
                            && Not(()=> Char('=') )
                            && S() ) )
                  && (    
                         (    expression_list() || yield_expression())
                      || Fatal("<<(expression_list  or  yield_expression)>> expected")) ) );
		}
        public bool target_list()    /*[75]^^target_list:       target (',' S target)* (',' S)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.target_list,()=>
                And(()=>  
                     target()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && target() ) )
                  && Option(()=> And(()=>    Char(',') && S() ) ) ) );
		}
        public bool target()    /*[76]^^target:            '(' target_contents ')' S
                       / '[' target_contents ']' S
                       / assignable_primary;*/
        {

           return TreeNT((int)Epython_2_5_2_i.target,()=>
                  
                     And(()=>    
                         Char('(')
                      && target_contents()
                      && Char(')')
                      && S() )
                  || And(()=>    
                         Char('[')
                      && target_contents()
                      && Char(']')
                      && S() )
                  || assignable_primary() );
		}
        public bool target_contents()    /*[77]target_contents
using Line_join_sem_:    S target_list;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && target_list() );            
      }

		}
        public bool assignable_primary()    /*[78]^^assignable_primary:
                         (identifier S/ enclosure !targetlist_end)    
                         (attributeref /  slicing  / call_args !targetlist_end )*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.assignable_primary,()=>
                And(()=>  
                     (    
                         And(()=>    identifier() && S() )
                      || And(()=>      
                               enclosure()
                            && Not(()=> targetlist_end() ) ))
                  && OptRepeat(()=>    
                            
                               attributeref()
                            || slicing()
                            || And(()=>        
                                       call_args()
                                    && Not(()=> targetlist_end() ) ) ) ) );
		}
        public bool targetlist_end()    /*[79]^^targetlist_end:    S ([,=] S / 'in' S2);*/
        {

           return TreeNT((int)Epython_2_5_2_i.targetlist_end,()=>
                And(()=>  
                     S()
                  && (    
                         And(()=>    OneOf(",=") && S() )
                      || And(()=>    Char('i','n') && S2() )) ) );
		}
        public bool augmented_assignment_stmt()    /*[80]^^augmented_assignment_stmt: 
                         target augop S @(expression_list / yield_expression);*/
        {

           return TreeNT((int)Epython_2_5_2_i.augmented_assignment_stmt,()=>
                And(()=>  
                     target()
                  && augop()
                  && S()
                  && (    
                         (    expression_list() || yield_expression())
                      || Fatal("<<(expression_list  or  yield_expression)>> expected")) ) );
		}
        public bool augop()    /*[81]^^augop: 	         '+=' / '-=' / '*=' / '/=' / '%=' / '**='
                       / '>>=' / '<<=' / '&=' / '^=' / '/=' / '//=';	// '//=' added by me*/
        {

           return TreeNT((int)Epython_2_5_2_i.augop,()=>
                OneOfLiterals(optimizedLiterals0) );
		}
        public bool pass_stmt()    /*[82]^^pass_stmt:         'pass' S2;*/
        {

           return TreeNT((int)Epython_2_5_2_i.pass_stmt,()=>
                And(()=>    Char('p','a','s','s') && S2() ) );
		}
        public bool del_stmt()    /*[83]^^del_stmt:          'del' S2 @target_list;*/
        {

           return TreeNT((int)Epython_2_5_2_i.del_stmt,()=>
                And(()=>  
                     Char('d','e','l')
                  && S2()
                  && (    target_list() || Fatal("<<target_list>> expected")) ) );
		}
        public bool print_stmt()    /*[84]^^print_stmt:        'print' S2 (  '>>' S expression (',' S expression)+ (',' S)?
                          / expression (',' S expression)* (',' S)?
                                   )?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.print_stmt,()=>
                And(()=>  
                     Char('p','r','i','n','t')
                  && S2()
                  && Option(()=>    
                            
                               And(()=>        
                                       Char('>','>')
                                    && S()
                                    && expression()
                                    && PlusRepeat(()=>          
                                              And(()=>    Char(',') && S() && expression() ) )
                                    && Option(()=> And(()=>    Char(',') && S() ) ) )
                            || And(()=>        
                                       expression()
                                    && OptRepeat(()=>          
                                              And(()=>    Char(',') && S() && expression() ) )
                                    && Option(()=> And(()=>    Char(',') && S() ) ) ) ) ) );
		}
        public bool return_stmt()    /*[85]^^return_stmt:       'return' S2 expression_list?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.return_stmt,()=>
                And(()=>  
                     Char('r','e','t','u','r','n')
                  && S2()
                  && Option(()=> expression_list() ) ) );
		}
        public bool yield_stmt()    /*[86]^^yield_stmt:        yield_expression;*/
        {

           return TreeNT((int)Epython_2_5_2_i.yield_stmt,()=>
                yield_expression() );
		}
        public bool raise_stmt()    /*[87]^^raise_stmt:        'raise' S2 (expression (',' S expression (',' S expression)?)?)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.raise_stmt,()=>
                And(()=>  
                     Char('r','a','i','s','e')
                  && S2()
                  && Option(()=>    
                      And(()=>      
                               expression()
                            && Option(()=>        
                                    And(()=>          
                                                 Char(',')
                                              && S()
                                              && expression()
                                              && Option(()=>            
                                                          And(()=>    Char(',') && S() && expression() ) ) ) ) ) ) ) );
		}
        public bool break_stmt()    /*[88]^^break_stmt:        'break' S2;*/
        {

           return TreeNT((int)Epython_2_5_2_i.break_stmt,()=>
                And(()=>    Char('b','r','e','a','k') && S2() ) );
		}
        public bool continue_stmt()    /*[89]^^continue_stmt:     'continue' S2;*/
        {

           return TreeNT((int)Epython_2_5_2_i.continue_stmt,()=>
                And(()=>    Char("continue") && S2() ) );
		}
        public bool import_stmt()    /*[90]^^import_stmt:       import_name / import_from;*/
        {

           return TreeNT((int)Epython_2_5_2_i.import_stmt,()=>
                    import_name() || import_from() );
		}
        public bool import_name()    /*[91]^^import_name:       'import' S2 module import_as? ( ',' S module import_as? )*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.import_name,()=>
                And(()=>  
                     Char('i','m','p','o','r','t')
                  && S2()
                  && module()
                  && Option(()=> import_as() )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && module()
                            && Option(()=> import_as() ) ) ) ) );
		}
        public bool import_from()    /*[92]^^import_from:       'from' S2 module @'import' S '*' S
                       / 'from' S2 relative_module @'import' S2  
                                   (imports_in_parens / imports_list);*/
        {

           return TreeNT((int)Epython_2_5_2_i.import_from,()=>
                  
                     And(()=>    
                         Char('f','r','o','m')
                      && S2()
                      && module()
                      && (      
                               Char('i','m','p','o','r','t')
                            || Fatal("<<'import'>> expected"))
                      && S()
                      && Char('*')
                      && S() )
                  || And(()=>    
                         Char('f','r','o','m')
                      && S2()
                      && relative_module()
                      && (      
                               Char('i','m','p','o','r','t')
                            || Fatal("<<'import'>> expected"))
                      && S2()
                      && (    imports_in_parens() || imports_list()) ) );
		}
        public bool import_as()    /*[93]^^import_as:         'as' S2 @name;*/
        {

           return TreeNT((int)Epython_2_5_2_i.import_as,()=>
                And(()=>  
                     Char('a','s')
                  && S2()
                  && (    name() || Fatal("<<name>> expected")) ) );
		}
        public bool imports_in_parens()    /*[94]^^imports_in_parens: '(' imports_in_parens_content @')' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.imports_in_parens,()=>
                And(()=>  
                     Char('(')
                  && imports_in_parens_content()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool imports_in_parens_content()    /*[95] imports_in_parens_content:
                         S2 identifier  S import_as? rest_import_list (',' S)?;*/
        {

           return And(()=>  
                     S2()
                  && identifier()
                  && S()
                  && Option(()=> import_as() )
                  && rest_import_list()
                  && Option(()=> And(()=>    Char(',') && S() ) ) );
		}
        public bool imports_list()    /*[96]^^imports_list:      identifier S import_as? rest_import_list;*/
        {

           return TreeNT((int)Epython_2_5_2_i.imports_list,()=>
                And(()=>  
                     identifier()
                  && S()
                  && Option(()=> import_as() )
                  && rest_import_list() ) );
		}
        public bool rest_import_list()    /*[97]^^rest_import_list:  (',' S identifier S import_as? )*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.rest_import_list,()=>
                OptRepeat(()=>  
                  And(()=>    
                         Char(',')
                      && S()
                      && identifier()
                      && S()
                      && Option(()=> import_as() ) ) ) );
		}
        public bool module()    /*[98]^^module:            (identifier S '.' S)* identifier S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.module,()=>
                And(()=>  
                     OptRepeat(()=>    
                      And(()=>    identifier() && S() && Char('.') && S() ) )
                  && identifier()
                  && S() ) );
		}
        public bool relative_module()    /*[99]^^relative_module:   ('.' S)* module / ('.' S)+;*/
        {

           return TreeNT((int)Epython_2_5_2_i.relative_module,()=>
                  
                     And(()=>    
                         OptRepeat(()=> And(()=>    Char('.') && S() ) )
                      && module() )
                  || PlusRepeat(()=> And(()=>    Char('.') && S() ) ) );
		}
        public bool name()    /*[100]^^name:             identifier S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.name,()=>
                And(()=>    identifier() && S() ) );
		}
        public bool global_stmt()    /*[101]^^global_stmt:      'global' S2 @identifier S (',' S @identifier S)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.global_stmt,()=>
                And(()=>  
                     Char('g','l','o','b','a','l')
                  && S2()
                  && (    identifier() || Fatal("<<identifier>> expected"))
                  && S()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    identifier() || Fatal("<<identifier>> expected"))
                            && S() ) ) ) );
		}
        public bool exec_stmt()    /*[102]^^exec_stmt:        'exec' S2 or_expr ('in' S2 expression (',' S expression)?)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.exec_stmt,()=>
                And(()=>  
                     Char('e','x','e','c')
                  && S2()
                  && or_expr()
                  && Option(()=>    
                      And(()=>      
                               Char('i','n')
                            && S2()
                            && expression()
                            && Option(()=>        
                                    And(()=>    Char(',') && S() && expression() ) ) ) ) ) );
		}
        public bool compound_stmt()    /*[103]^^compound_stmt:    if_stmt / while_stmt / for_stmt / try_stmt / with_stmt 
                       / funcdef / classdef;*/
        {

           return TreeNT((int)Epython_2_5_2_i.compound_stmt,()=>
                  
                     if_stmt()
                  || while_stmt()
                  || for_stmt()
                  || try_stmt()
                  || with_stmt()
                  || funcdef()
                  || classdef() );
		}
        public bool suite()    /*[104]^^suite:            stmt_list @(NEWLINE/ENDMARKER) / 
                         NEWLINE INDENT @statement (KEEPINDENT @statement)* DEDENT;*/
        {

           return TreeNT((int)Epython_2_5_2_i.suite,()=>
                  
                     And(()=>    
                         stmt_list()
                      && (      
                               (    NEWLINE() || ENDMARKER())
                            || Fatal("<<(NEWLINE or ENDMARKER)>> expected")) )
                  || And(()=>    
                         NEWLINE()
                      && INDENT()
                      && (    statement() || Fatal("<<statement>> expected"))
                      && OptRepeat(()=>      
                            And(()=>        
                                       KEEPINDENT()
                                    && (    statement() || Fatal("<<statement>> expected")) ) )
                      && DEDENT() ) );
		}
        public bool statement()    /*[105]  statement:        compound_stmt / stmt_list (NEWLINE/ENDMARKER);*/
        {

           return   
                     compound_stmt()
                  || And(()=>    
                         stmt_list()
                      && (    NEWLINE() || ENDMARKER()) );
		}
        public bool stmt_list()    /*[106]  stmt_list:        simple_stmt (';' S simple_stmt)* (';' S)? ;*/
        {

           return And(()=>  
                     simple_stmt()
                  && OptRepeat(()=>    
                      And(()=>    Char(';') && S() && simple_stmt() ) )
                  && Option(()=> And(()=>    Char(';') && S() ) ) );
		}
        public bool if_stmt()    /*[107]^^if_stmt:          'if' S2 @expression @':' S suite
                         (KEEPINDENT ^'elif'  S2 @expression @':' S suite / RESTOREINDENT_ )*
                         (KEEPINDENT ^'else' S @':' S suite / RESTOREINDENT_ )?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.if_stmt,()=>
                And(()=>  
                     Char('i','f')
                  && S2()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite()
                  && OptRepeat(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && TreeChars(()=> Char('e','l','i','f') )
                                    && S2()
                                    && (          
                                                 expression()
                                              || Fatal("<<expression>> expected"))
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() )
                  && Option(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && TreeChars(()=> Char('e','l','s','e') )
                                    && S()
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() ) ) );
		}
        public bool while_stmt()    /*[108]^^while_stmt:        'while' S2 @expression @':' S suite
                         (KEEPINDENT 'else' S @':' S suite / RESTOREINDENT_ )?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.while_stmt,()=>
                And(()=>  
                     Char('w','h','i','l','e')
                  && S2()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite()
                  && Option(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && Char('e','l','s','e')
                                    && S()
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() ) ) );
		}
        public bool for_stmt()    /*[109]^^for_stmt:          'for' S2 target_list @'in' S2 expression_list
                          @':' S suite (KEEPINDENT 'else' S @':' S suite / RESTOREINDENT_ )?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.for_stmt,()=>
                And(()=>  
                     Char('f','o','r')
                  && S2()
                  && target_list()
                  && (    Char('i','n') || Fatal("<<'in'>> expected"))
                  && S2()
                  && expression_list()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite()
                  && Option(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && Char('e','l','s','e')
                                    && S()
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() ) ) );
		}
        public bool try_stmt()    /*[110]^^try_stmt:          'try' S @':' S suite ( except_handler / finally_only_handler );*/
        {

           return TreeNT((int)Epython_2_5_2_i.try_stmt,()=>
                And(()=>  
                     Char('t','r','y')
                  && S()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite()
                  && (    except_handler() || finally_only_handler()) ) );
		}
        public bool finally_only_handler()    /*[111]^^finally_only_handler: 
                         KEEPINDENT 'finally' S @':' S suite / RESTOREINDENT_ ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.finally_only_handler,()=>
                  
                     And(()=>    
                         KEEPINDENT()
                      && Char('f','i','n','a','l','l','y')
                      && S()
                      && (    Char(':') || Fatal("<<':'>> expected"))
                      && S()
                      && suite() )
                  || _top.RESTOREINDENT_() );
		}
        public bool except_handler()    /*[112]^^except_handler:   (KEEPINDENT ^'except' S2 (expression (',' S target)?)? @':' S suite / RESTOREINDENT_ )+
                         (KEEPINDENT ^'else' S @':' S suite / RESTOREINDENT_ )? finally_only_handler?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.except_handler,()=>
                And(()=>  
                     PlusRepeat(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && TreeChars(()=> Char('e','x','c','e','p','t') )
                                    && S2()
                                    && Option(()=>          
                                              And(()=>            
                                                             expression()
                                                          && Option(()=>              
                                                                        And(()=>    Char(',') && S() && target() ) ) ) )
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() )
                  && Option(()=>    
                            
                               And(()=>        
                                       KEEPINDENT()
                                    && TreeChars(()=> Char('e','l','s','e') )
                                    && S()
                                    && (    Char(':') || Fatal("<<':'>> expected"))
                                    && S()
                                    && suite() )
                            || _top.RESTOREINDENT_() )
                  && Option(()=> finally_only_handler() ) ) );
		}
        public bool with_stmt()    /*[113]^^with_stmt:        'with' S2 expression ('as' S2 @target)? @':' S suite;*/
        {

           return TreeNT((int)Epython_2_5_2_i.with_stmt,()=>
                And(()=>  
                     Char('w','i','t','h')
                  && S2()
                  && expression()
                  && Option(()=>    
                      And(()=>      
                               Char('a','s')
                            && S2()
                            && (    target() || Fatal("<<target>> expected")) ) )
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite() ) );
		}
        public bool funcdef()    /*[114]^^funcdef:          decorators? 'def' S2 @funcname S '(' parameter_list_in @')' S @':' S suite;*/
        {

           return TreeNT((int)Epython_2_5_2_i.funcdef,()=>
                And(()=>  
                     Option(()=> decorators() )
                  && Char('d','e','f')
                  && S2()
                  && (    funcname() || Fatal("<<funcname>> expected"))
                  && S()
                  && Char('(')
                  && parameter_list_in()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && suite() ) );
		}
        public bool parameter_list_in()    /*[115] parameter_list_in
using Line_join_sem_:    S parameter_list?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>    S() && Option(()=> parameter_list() ) );            
      }

		}
        public bool decorators()    /*[116]^^decorators:       decorator+;*/
        {

           return TreeNT((int)Epython_2_5_2_i.decorators,()=>
                PlusRepeat(()=> decorator() ) );
		}
        public bool decorator()    /*[117]^^decorator:        '@' S dotted_name ('(' decorater_in @')' S)? NEWLINE 
                         (KEEPINDENT/FATAL<"decorator continuation incorrectly indented">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.decorator,()=>
                And(()=>  
                     Char('@')
                  && S()
                  && dotted_name()
                  && Option(()=>    
                      And(()=>      
                               Char('(')
                            && decorater_in()
                            && (    Char(')') || Fatal("<<')'>> expected"))
                            && S() ) )
                  && NEWLINE()
                  && (    
                         KEEPINDENT()
                      || Fatal("decorator continuation incorrectly indented")) ) );
		}
        public bool decorater_in()    /*[118] decorater_in
using Line_join_sem_:    S (argument_list (','S)?)?;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return And(()=>  
                     S()
                  && Option(()=>    
                      And(()=>      
                               argument_list()
                            && Option(()=> And(()=>    Char(',') && S() ) ) ) ) );            
      }

		}
        public bool dotted_name()    /*[119]^^dotted_name:      identifier S('.' S identifier S)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.dotted_name,()=>
                And(()=>  
                     identifier()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>    Char('.') && S() && identifier() && S() ) ) ) );
		}
        public bool parameter_list()    /*[120]^^parameter_list:   defparameter  (',' S defparameter )* (',' S seq_dict_pars?)? / seq_dict_pars;*/
        {

           return TreeNT((int)Epython_2_5_2_i.parameter_list,()=>
                  
                     And(()=>    
                         defparameter()
                      && OptRepeat(()=>      
                            And(()=>    Char(',') && S() && defparameter() ) )
                      && Option(()=>      
                            And(()=>        
                                       Char(',')
                                    && S()
                                    && Option(()=> seq_dict_pars() ) ) ) )
                  || seq_dict_pars() );
		}
        public bool seq_dict_pars()    /*[121]^^seq_dict_pars:    ('*' S identifier S (',' S '**' S identifier S)? / '**' S identifier S);*/
        {

           return TreeNT((int)Epython_2_5_2_i.seq_dict_pars,()=>
                  
                     And(()=>    
                         Char('*')
                      && S()
                      && identifier()
                      && S()
                      && Option(()=>      
                            And(()=>        
                                       Char(',')
                                    && S()
                                    && Char('*','*')
                                    && S()
                                    && identifier()
                                    && S() ) ) )
                  || And(()=>    
                         Char('*','*')
                      && S()
                      && identifier()
                      && S() ) );
		}
        public bool defparameter()    /*[122]^^defparameter:     parameter ('=' S @expression)?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.defparameter,()=>
                And(()=>  
                     parameter()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && (    expression() || Fatal("<<expression>> expected")) ) ) ) );
		}
        public bool sublist()    /*[123]^^sublist:          parameter (',' S parameter)* ','?;*/
        {

           return TreeNT((int)Epython_2_5_2_i.sublist,()=>
                And(()=>  
                     parameter()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && parameter() ) )
                  && Option(()=> Char(',') ) ) );
		}
        public bool parameter()    /*[124]^^parameter:        identifier S / '('  parameter_in  @')' S;*/
        {

           return TreeNT((int)Epython_2_5_2_i.parameter,()=>
                  
                     And(()=>    identifier() && S() )
                  || And(()=>    
                         Char('(')
                      && parameter_in()
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S() ) );
		}
        public bool parameter_in()    /*[125] parameter_in:      S @sublist;*/
        {

           return And(()=>  
                     S()
                  && (    sublist() || Fatal("<<sublist>> expected")) );
		}
        public bool funcname()    /*[126]^^funcname:         identifier;*/
        {

           return TreeNT((int)Epython_2_5_2_i.funcname,()=>
                identifier() );
		}
        public bool classdef()    /*[127]^^classdef:         'class' S classname S inheritance? ':' S suite;*/
        {

           return TreeNT((int)Epython_2_5_2_i.classdef,()=>
                And(()=>  
                     Char('c','l','a','s','s')
                  && S()
                  && classname()
                  && S()
                  && Option(()=> inheritance() )
                  && Char(':')
                  && S()
                  && suite() ) );
		}
        public bool inheritance()    /*[128]^^inheritance
using Line_join_sem_:    '('  S expression_list?  @')' S;*/
        {

             using(var _sem= new Line_join_sem_(this)){
           return TreeNT((int)Epython_2_5_2_i.inheritance,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && Option(()=> expression_list() )
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );            
      }

		}
        public bool classname()    /*[129]^^classname:        identifier;

//token definitions added for PEG following the docs at python.org*/
        {

           return TreeNT((int)Epython_2_5_2_i.classname,()=>
                identifier() );
		}
        public bool S()    /*[130]  S:                ( [ \t\v\f]+ /
                           COMMENT     / 
                           '\\' NL  / 
                          NL in_implicit_line_joining_
                         )* ;*/
        {

           return OptRepeat(()=>  
                      
                         PlusRepeat(()=> OneOf(" \t\v\f") )
                      || COMMENT()
                      || And(()=>    Char('\\') && NL() )
                      || And(()=>    NL() && _top.in_implicit_line_joining_() ) );
		}
        public bool S2()    /*[131]S2:                 ![A-Za-z_0-9] S;*/
        {

           return And(()=>  
                     Not(()=> (In('A','Z', 'a','z', '0','9')||OneOf("_")) )
                  && S() );
		}
        public bool identifier()    /*[132]^^identifier:       !(KEYWORD S2) [A-Za-z_][A-Za-z0-9_]*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.identifier,()=>
                And(()=>  
                     Not(()=> And(()=>    KEYWORD() && S2() ) )
                  && (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) ) ) );
		}
        public bool INDENT()    /*[133]INDENT:             S:indent_ (indent_handler_/FATAL<"indentation error">);*/
        {

           return And(()=>  
                     Into(()=> S(),out _top.indent_)
                  && (    _top.indent_handler_() || Fatal("indentation error")) );
		}
        public bool KEEPINDENT()    /*[134]KEEPINDENT:         continue_after_dedent_ / S:indent_ keep_handler_;*/
        {

           return   
                     _top.continue_after_dedent_()
                  || And(()=>    
                         Into(()=> S(),out _top.indent_)
                      && _top.keep_handler_() );
		}
        public bool DEDENT()    /*[135]DEDENT:             S dedent_handler_/FATAL<"indentation error">;*/
        {

           return   
                     And(()=>    S() && _top.dedent_handler_() )
                  || Fatal("indentation error");
		}
        public bool NEWLINE()    /*[136]NEWLINE:            NL BLANKLINE*;*/
        {

           return And(()=>    NL() && OptRepeat(()=> BLANKLINE() ) );
		}
        public bool NL()    /*[137]NL:                 '\r'?'\n' / '\n' / '\r' ;*/
        {

           return   
                     And(()=>    Option(()=> Char('\r') ) && Char('\n') )
                  || Char('\n')
                  || Char('\r');
		}
        public bool BLANKLINE()    /*[138]BLANKLINE:          [ \t\v\f]* COMMENT? NL/ [ \t\v\f]+ COMMENT? !. / COMMENT !. ;*/
        {

           return   
                     And(()=>    
                         OptRepeat(()=> OneOf(" \t\v\f") )
                      && Option(()=> COMMENT() )
                      && NL() )
                  || And(()=>    
                         PlusRepeat(()=> OneOf(" \t\v\f") )
                      && Option(()=> COMMENT() )
                      && Not(()=> Any() ) )
                  || And(()=>    COMMENT() && Not(()=> Any() ) );
		}
        public bool NAME()    /*[139]NAME:               !(KEYWORD S2) [A-Za-z_][A-Za-z0-9_]* S;*/
        {

           return And(()=>  
                     Not(()=> And(()=>    KEYWORD() && S2() ) )
                  && (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) )
                  && S() );
		}
        public bool IDENT()    /*[140]IDENT:              [A-Za-z_][A-Za-z0-9_]*;*/
        {

           return And(()=>  
                     (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) ) );
		}
        public bool KEYWORD()    /*[141]KEYWORD: 
                         ('and' /      'del' /       'from' /      'not' /       'while' /    
                          'as' /        'elif' /      'global' /    'or' /        'with' /     
                          'assert' /    'else' /      'if' /        'pass' /      'yield' /    
                          'break' /     'except' /    'import' /    'print' /  
                          'class' /     'exec' /      'in' /        'raise' /  
                          'continue' /  'finally' /   'is' /        'return' / 
                          'def' /       'for' /       'lambda' /    'try') ;*/
        {

           return OneOfLiterals(optimizedLiterals1);
		}
        public bool COMMENT()    /*[142]COMMENT:            '#' (!NL.)*;*/
        {

           return And(()=>  
                     Char('#')
                  && OptRepeat(()=> And(()=>    Not(()=> NL() ) && Any() ) ) );
		}
        public bool ENDMARKER()    /*[143]ENDMARKER:          !./WARNING<"file end expected">;*/
        {

           return     Not(()=> Any() ) || Warning("file end expected");
		}
        public bool NUMBER()    /*[144]NUMBER:           imagnumber / floatnumber / integer ^([lL]?) ;*/
        {

           return   
                     imagnumber()
                  || floatnumber()
                  || And(()=>    
                         integer()
                      && TreeChars(()=> Option(()=> OneOf("lL") ) ) );
		}
        public bool STRING()    /*[145]STRING:           stringprefix? (longstring/shortstring) unset_raw_;*/
        {

           return And(()=>  
                     Option(()=> stringprefix() )
                  && (    longstring() || shortstring())
                  && _top.unset_raw_() );
		}
        public bool integer()    /*[146]^^integer:          [1-9][0-9]* / '0'[xX] [0-9a-fA-F]+ / '0'[0-7]*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.integer,()=>
                  
                     And(()=>    
                         In('1','9')
                      && OptRepeat(()=> In('0','9') ) )
                  || And(()=>    
                         Char('0')
                      && OneOf("xX")
                      && PlusRepeat(()=> In('0','9', 'a','f', 'A','F') ) )
                  || And(()=>    Char('0') && OptRepeat(()=> In('0','7') ) ) );
		}
        public bool floatnumber()    /*[147]^^floatnumber:      pointfloat exponent? / [0-9]+ exponent;*/
        {

           return TreeNT((int)Epython_2_5_2_i.floatnumber,()=>
                  
                     And(()=>    pointfloat() && Option(()=> exponent() ) )
                  || And(()=>    
                         PlusRepeat(()=> In('0','9') )
                      && exponent() ) );
		}
        public bool pointfloat()    /*[148]pointfloat:         [0-9]* '.' [0-9]+ / [0-9]+ '.';*/
        {

           return   
                     And(()=>    
                         OptRepeat(()=> In('0','9') )
                      && Char('.')
                      && PlusRepeat(()=> In('0','9') ) )
                  || And(()=>    PlusRepeat(()=> In('0','9') ) && Char('.') );
		}
        public bool exponent()    /*[149]exponent:           [eE][+-]?[0-9]+;*/
        {

           return And(()=>  
                     OneOf("eE")
                  && Option(()=> OneOf("+-") )
                  && PlusRepeat(()=> In('0','9') ) );
		}
        public bool imagnumber()    /*[150]^^imagnumber:       (floatnumber / [0-9]+) [jJ];*/
        {

           return TreeNT((int)Epython_2_5_2_i.imagnumber,()=>
                And(()=>  
                     (    floatnumber() || PlusRepeat(()=> In('0','9') ))
                  && OneOf("jJ") ) );
		}
        public bool stringprefix()    /*[151]  stringprefix:     raw_indicating_stringprefix / unicodeonly_stringprefix;*/
        {

           return   
                     raw_indicating_stringprefix()
                  || unicodeonly_stringprefix();
		}
        public bool raw_indicating_stringprefix()    /*[152]^^raw_indicating_stringprefix: 
                         ('r' /   'ur' /  'R' /   'UR' /  'Ur' /  'uR') set_raw_;*/
        {

           return TreeNT((int)Epython_2_5_2_i.raw_indicating_stringprefix,()=>
                And(()=>  
                     (    
                         Char('r')
                      || Char('u','r')
                      || Char('R')
                      || Char('U','R')
                      || Char('U','r')
                      || Char('u','R'))
                  && _top.set_raw_() ) );
		}
        public bool unicodeonly_stringprefix()    /*[153]^^unicodeonly_stringprefix:   
                         'u' /  'U' ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.unicodeonly_stringprefix,()=>
                    Char('u') || Char('U') );
		}
        public bool shortstring()    /*[154]shortstring:        '"' shortstringitem1_content 
                         ('"' / FATAL<"string not terminated before input end">) / 
                         ['] shortstringitem2_content 
                         ([']/ FATAL<"string not terminated before input end">);*/
        {

           return   
                     And(()=>    
                         Char('"')
                      && shortstringitem1_content()
                      && (      
                               Char('"')
                            || Fatal("string not terminated before input end")) )
                  || And(()=>    
                         OneOf("'")
                      && shortstringitem2_content()
                      && (      
                               OneOf("'")
                            || Fatal("string not terminated before input end")) );
		}
        public bool shortstringitem1_content()    /*[155]^^shortstringitem1_content: 
                         (!'"' shortstringitem)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.shortstringitem1_content,()=>
                OptRepeat(()=>  
                  And(()=>    Not(()=> Char('"') ) && shortstringitem() ) ) );
		}
        public bool shortstringitem2_content()    /*[156]^^shortstringitem2_content: 
                         (!['] shortstringitem)*;*/
        {

           return TreeNT((int)Epython_2_5_2_i.shortstringitem2_content,()=>
                OptRepeat(()=>  
                  And(()=>    Not(()=> OneOf("'") ) && shortstringitem() ) ) );
		}
        public bool longstring()    /*longstring:              '"""' longstring1_content 
                         ('"""' / FATAL<"triple quoted string not terminated">) 
                       / ['][']['] longstring2_content 
                         (['][']['] / FATAL<"triple quoted string not terminated">);*/
        {

           return   
                     And(()=>    
                         Char('"','"','"')
                      && longstring1_content()
                      && (      
                               Char('"','"','"')
                            || Fatal("triple quoted string not terminated")) )
                  || And(()=>    
                         OneOf("'")
                      && OneOf("'")
                      && OneOf("'")
                      && longstring2_content()
                      && (      
                               And(()=>    OneOf("'") && OneOf("'") && OneOf("'") )
                            || Fatal("triple quoted string not terminated")) );
		}
        public bool longstring1_content()    /*[157]^^longstring1_content: (!'"""' longstringitem)* ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.longstring1_content,()=>
                OptRepeat(()=>  
                  And(()=>    
                         Not(()=> Char('"','"','"') )
                      && longstringitem() ) ) );
		}
        public bool longstring2_content()    /*[158]^^longstring2_content: (!(['][']['])longstringitem)* ;*/
        {

           return TreeNT((int)Epython_2_5_2_i.longstring2_content,()=>
                OptRepeat(()=>  
                  And(()=>    
                         Not(()=>      
                            And(()=>    OneOf("'") && OneOf("'") && OneOf("'") ) )
                      && longstringitem() ) ) );
		}
        public bool shortstringitem()    /*[159]shortstringitem:    escapeseq 
                       / '\\' NL
                       / '\\' check_unrecognized_escape  . 
                       / [^\n] 
                       / FATAL<"new line in string not allowed">;*/
        {

           return   
                     escapeseq()
                  || And(()=>    Char('\\') && NL() )
                  || And(()=>    
                         Char('\\')
                      && check_unrecognized_escape()
                      && Any() )
                  || NotOneOf("\n")
                  || Fatal("new line in string not allowed");
		}
        public bool longstringitem()    /*[160]longstringitem:     escapeseq / '\\' check_unrecognized_escape . /  .  ;*/
        {

           return   
                     escapeseq()
                  || And(()=>    
                         Char('\\')
                      && check_unrecognized_escape()
                      && Any() )
                  || Any();
		}
        public bool check_unrecognized_escape()    /*[161]check_unrecognized_escape: 
                         !in_raw_ WARNING<"unrecognized escape"> / &.;*/
        {

           return   
                     And(()=>    
                         Not(()=> _top.in_raw_() )
                      && Warning("unrecognized escape") )
                  || Peek(()=> Any() );
		}
        public bool escapeseq()    /*[162]escapeseq:          in_raw_ '\\' ( unicode_4digits  / unicode_8digits ) 
                       / !in_raw_ '\\' 
                         ( [\\'"abfnrtv]
                         / 'N{' NAME '}' 
                         / unicode_4digits 
                         / unicode_8digits 
                         / [0-8]{1,3}
                         / hex_2digits );*/
        {

           return   
                     And(()=>    
                         _top.in_raw_()
                      && Char('\\')
                      && (    unicode_4digits() || unicode_8digits()) )
                  || And(()=>    
                         Not(()=> _top.in_raw_() )
                      && Char('\\')
                      && (      
                               OneOf(optimizedCharset0)
                            || And(()=>    Char('N','{') && NAME() && Char('}') )
                            || unicode_4digits()
                            || unicode_8digits()
                            || ForRepeat(1,3,()=> In('0','8') )
                            || hex_2digits()) );
		}
        public bool unicode_4digits()    /*[163]^^unicode_4digits:  'u'([0-9A-Fa-f]{4}/WARNING<"u must be followed by 4 hex digits">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.unicode_4digits,()=>
                And(()=>  
                     Char('u')
                  && (    
                         ForRepeat(4,4,()=> In('0','9', 'A','F', 'a','f') )
                      || Warning("u must be followed by 4 hex digits")) ) );
		}
        public bool unicode_8digits()    /*[164]^^unicode_8digits:  'U'([0-9A-Fa-f]{8}/WARNING<"U must be followed by 8 hex digits">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.unicode_8digits,()=>
                And(()=>  
                     Char('U')
                  && (    
                         ForRepeat(8,8,()=> In('0','9', 'A','F', 'a','f') )
                      || Warning("U must be followed by 8 hex digits")) ) );
		}
        public bool hex_2digits()    /*[165]^^hex_2digits:      'x'([0-9A-Fa-f]{2} /WARNING<"x must be followed by 2 hex digits">);*/
        {

           return TreeNT((int)Epython_2_5_2_i.hex_2digits,()=>
                And(()=>  
                     Char('x')
                  && (    
                         ForRepeat(2,2,()=> In('0','9', 'A','F', 'a','f') )
                      || Warning("x must be followed by 2 hex digits")) ) );
		}
        public bool set_explicit_line_joining_()    /*set_explicit_line_joining_:;*/
        {

           return true;
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        internal static OptimizedLiterals optimizedLiterals0;
        internal static OptimizedLiterals optimizedLiterals1;
        
        static python_2_5_2_i()
        {
            {
               char[] oneOfChars = new char[]    {'\\','\'','"','a','b'
                                                  ,'f','n','r','t','v'
                                                  };
               optimizedCharset0= new OptimizedCharset(null,oneOfChars);
            }
            
            
            {
               string[] literals=
               { "+=","-=","*=","/=","%=","**=",">>=","<<=",
                  "&=","^=","/=","//=" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "and","del","from","not","while","as","elif","global",
                  "or","with","assert","else","if","pass","yield","break",
                  "except","import","print","class","exec","in","raise","continue",
                  "finally","is","return","def","for","lambda","try" };
               optimizedLiterals1= new OptimizedLiterals(literals);
            }

            
        }
        #endregion Optimization Data 
           }
}