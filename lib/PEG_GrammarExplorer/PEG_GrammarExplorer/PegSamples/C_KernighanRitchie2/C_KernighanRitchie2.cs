/* created on 21.09.2008 15:47:26 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace C_KernighanRitchie2
{
      
      enum EC_KernighanRitchie2{translation_unit= 1, external_declaration= 2, function_definition= 3, 
                                 declaration= 4, declaration_specifiers= 5, storage_class_specifier= 6, 
                                 type_specifier= 7, type_qualifier= 8, struct_or_union_specifier= 9, 
                                 member_block= 900, struct_or_union= 10, init_declarator= 11, 
                                 struct_declaration= 12, specifier_qualifier_list= 13, struct_declarator= 14, 
                                 enum_specifier= 15, enum_block= 150, enumerator= 16, declarator= 17, 
                                 direct_declarator= 18, pointer= 19, parameter_type_list= 20, 
                                 parameter_type_list_impl= 200, parameter_declaration= 21, initializer= 22, 
                                 type_name= 23, abstract_declarator= 24, direct_abstract_declarator= 25, 
                                 typedef_name= 26, statement= 27, labeled_statement= 28, expression_statement= 29, 
                                 compound_statement= 30, selection_statement= 31, jump_statement= 32, 
                                 expression= 33, assignment_expression= 34, assignment_operator= 35, 
                                 conditional_expression= 36, logical_or_expression= 37, logical_and_expression= 38, 
                                 inclusive_or_expression= 39, exclusive_or_expression= 40, and_expression= 41, 
                                 equality_expression= 42, relational_expression= 43, shift_expression= 44, 
                                 additive_expression= 45, multiplicative_expression= 46, cast_expression= 47, 
                                 parenthized_type_name= 470, unary_expression= 48, unary_operator= 49, 
                                 postfix_expression= 50, call= 500, primary_expression= 51, constant= 52, 
                                 identifier= 53, keyword= 54, iteration_statement= 55, for_init_expression= 550, 
                                 for_condition= 551, for_incr= 552, S= 56, c_comment= 57, constant_expression= 58, 
                                 @string= 59, integer_constant= 60, character_constant= 61, floating_constant= 62, 
                                 enumeration_constant= 63, exponent= 64, escape_sequence= 65, 
                                 chars= 66, @char= 67, list= 70, binary= 80, key_detail= 100, 
                                 c_preprocessing_directive= 160, B= 161, decimal_int= 162, fraction= 163, 
                                 l= 164, u= 165, f= 166, hexadecimal_constant= 167};
      class C_KernighanRitchie2 : PegCharParser 
      {
        class _Top{
            #region data members
            int  struct_level;
            int  par_list_level;
            bool is_defined_type_spec;
            bool is_in_typedef_definition;

            internal string defined_typedef_name;
            internal string declarator_ident;
            System.Collections.Generic.List<System.Collections.Generic.HashSet<string>> typedefNames; 
            #endregion data members
            #region semantic functions
            internal bool typedef_name_lookup_()	
            {
                 if( is_defined_type_spec) return false; 
                 else return isTypedefName(defined_typedef_name);
            }
            internal bool check_typedef_declarator_()
            {
               if( is_in_typedef_definition && par_list_level==0 && struct_level==0 ) insertTypedefName( declarator_ident );
               return true;
            }
            internal bool enter_scope_()  { pushScope(); return true; }
            internal bool leave_scope_() { popScope();  return true; }
            internal bool enter_params_(){++par_list_level;return true;}
            internal bool leave_params_(){--par_list_level;return true;}
            internal bool enter_struct_(){++struct_level;return true;}
            internal bool leave_struct_(){--struct_level;return true;}
            internal bool set_in_typedef_(){is_in_typedef_definition=true;return true;}
            internal bool set_not_in_typedef_(){is_in_typedef_definition=false;return true;}
            internal bool set_type_specifier_defined_() {is_defined_type_spec= true; return true;}
            internal bool init_declaration_specifiers_(){is_defined_type_spec=false;return true;}
            internal bool init_()
            {
                is_defined_type_spec=false;
                is_in_typedef_definition= false;
                par_list_level=0;
                struct_level= 0;
                typedefNames= null;
                pushScope();
                return true;
            }
            #endregion  semantic functions
            void insertTypedefName(string ident)
            {
                   typedefNames[typedefNames.Count-1].Add(ident);
            }
            bool isTypedefName(string defined_typedef_name)
            {
                 for(int i= typedefNames.Count;i>0;--i)
                 {
                     if( typedefNames[i-1].Contains(defined_typedef_name) ) return true;
                 }
                 return false;
            }
            void pushScope()
            {
                if( typedefNames==null ) typedefNames= new System.Collections.Generic.List<System.Collections.Generic.HashSet<string>>();
                typedefNames.Add(new System.Collections.Generic.HashSet<string>());
            }
            void popScope()
            {
                if (typedefNames != null) typedefNames.RemoveAt(typedefNames.Count - 1);
            }
}

//declarations
_Top _top;

         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.ascii;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public C_KernighanRitchie2()
            : base()
        {
            _top= new _Top();

        }
        public C_KernighanRitchie2(string src,TextWriter FerrOut)
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
                   EC_KernighanRitchie2 ruleEnum = (EC_KernighanRitchie2)id;
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
        public bool translation_unit()    /*[1]^^translation_unit:	init_ S @(external_declaration+) (!./FATAL<"illegal code before end of C file">);*/
        {

           return TreeNT((int)EC_KernighanRitchie2.translation_unit,()=>
                And(()=>  
                     _top.init_()
                  && S()
                  && (    
                         PlusRepeat(()=> external_declaration() )
                      || Fatal("<<(external_declaration+)>> expected"))
                  && (    
                         Not(()=> Any() )
                      || Fatal("illegal code before end of C file")) ) );
		}
        public bool external_declaration()    /*[2]^external_declaration: function_definition / declaration ;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.external_declaration,()=>
                    function_definition() || declaration() );
		}
        public bool function_definition()    /*[3]^^function_definition:declaration_specifiers? declarator 
			declaration* compound_statement;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.function_definition,()=>
                And(()=>  
                     Option(()=> declaration_specifiers() )
                  && declarator()
                  && OptRepeat(()=> declaration() )
                  && compound_statement() ) );
		}
        public bool declaration()    /*[4]^^declaration:	set_not_in_typedef_ declaration_specifiers list<init_declarator,','>? @';' S;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.declaration,()=>
                And(()=>  
                     _top.set_not_in_typedef_()
                  && declaration_specifiers()
                  && Option(()=>    
                      list(()=> init_declarator() ,()=>Char(',') ) )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool declaration_specifiers()    /*[5]^^declaration_specifiers: 			
                        init_declaration_specifiers_
			((storage_class_specifier / 
			type_specifier set_type_specifier_defined_ /
			type_qualifier )S)+;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.declaration_specifiers,()=>
                And(()=>  
                     _top.init_declaration_specifiers_()
                  && PlusRepeat(()=>    
                      And(()=>      
                               (        
                                       storage_class_specifier()
                                    || And(()=>          
                                                 type_specifier()
                                              && _top.set_type_specifier_defined_() )
                                    || type_qualifier())
                            && S() ) ) ) );
		}
        public bool storage_class_specifier()    /*[6]^^storage_class_specifier:
			('auto'/'register'/'static'/'extern') B / 'typedef' B set_in_typedef_;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.storage_class_specifier,()=>
                  
                     And(()=>    
                         (      
                               Char('a','u','t','o')
                            || Char("register")
                            || Char('s','t','a','t','i','c')
                            || Char('e','x','t','e','r','n'))
                      && B() )
                  || And(()=>    
                         Char('t','y','p','e','d','e','f')
                      && B()
                      && _top.set_in_typedef_() ) );
		}
        public bool type_specifier()    /*[7]^^type_specifier:	^'void' B / ^'char' B / ^'short' B / ^'int' B / ^'long' B / 
			^'float' B / ^'double' B / ^'signed' B / ^'unsigned' B /
			struct_or_union_specifier / enum_specifier / typedef_name ;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.type_specifier,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('v','o','i','d') )
                      && B() )
                  || And(()=>    
                         TreeChars(()=> Char('c','h','a','r') )
                      && B() )
                  || And(()=>    
                         TreeChars(()=> Char('s','h','o','r','t') )
                      && B() )
                  || And(()=>    TreeChars(()=> Char('i','n','t') ) && B() )
                  || And(()=>    
                         TreeChars(()=> Char('l','o','n','g') )
                      && B() )
                  || And(()=>    
                         TreeChars(()=> Char('f','l','o','a','t') )
                      && B() )
                  || And(()=>    
                         TreeChars(()=> Char('d','o','u','b','l','e') )
                      && B() )
                  || And(()=>    
                         TreeChars(()=> Char('s','i','g','n','e','d') )
                      && B() )
                  || And(()=>    TreeChars(()=> Char("unsigned") ) && B() )
                  || struct_or_union_specifier()
                  || enum_specifier()
                  || typedef_name() );
		}
        public bool type_qualifier()    /*[8]^^type_qualifier:	('const'/'volatile') B;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.type_qualifier,()=>
                And(()=>  
                     (    Char('c','o','n','s','t') || Char("volatile"))
                  && B() ) );
		}
        public bool struct_or_union_specifier()    /*[9]^^struct_or_union_specifier:
			struct_or_union B S (identifier S member_block? / member_block);*/
        {

           return TreeNT((int)EC_KernighanRitchie2.struct_or_union_specifier,()=>
                And(()=>  
                     struct_or_union()
                  && B()
                  && S()
                  && (    
                         And(()=>      
                               identifier()
                            && S()
                            && Option(()=> member_block() ) )
                      || member_block()) ) );
		}
        public bool member_block()    /*[900]member_block:      '{' enter_struct_  S @(struct_declaration+)  leave_struct_ @'}' S;*/
        {

           return And(()=>  
                     Char('{')
                  && _top.enter_struct_()
                  && S()
                  && (    
                         PlusRepeat(()=> struct_declaration() )
                      || Fatal("<<(struct_declaration+)>> expected"))
                  && _top.leave_struct_()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() );
		}
        public bool struct_or_union()    /*[10]^^struct_or_union:	'struct' / 'union';*/
        {

           return TreeNT((int)EC_KernighanRitchie2.struct_or_union,()=>
                  
                     Char('s','t','r','u','c','t')
                  || Char('u','n','i','o','n') );
		}
        public bool init_declarator()    /*[11]^init_declarator:	declarator ('=' S @initializer)?;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.init_declarator,()=>
                And(()=>  
                     declarator()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && (        
                                       initializer()
                                    || Fatal("<<initializer>> expected")) ) ) ) );
		}
        public bool struct_declaration()    /*[12]^^struct_declaration:
                        specifier_qualifier_list list<struct_declarator,','> @';' S;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.struct_declaration,()=>
                And(()=>  
                     specifier_qualifier_list()
                  && list(()=> struct_declarator() ,()=>Char(',') )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool specifier_qualifier_list()    /*[13]^^specifier_qualifier_list:
			(type_specifier S / type_qualifier S)+;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.specifier_qualifier_list,()=>
                PlusRepeat(()=>  
                      
                         And(()=>    type_specifier() && S() )
                      || And(()=>    type_qualifier() && S() ) ) );
		}
        public bool struct_declarator()    /*[14]^^struct_declarator:	declarator (':' S @constant_expression )? /
			':' S @constant_expression;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.struct_declarator,()=>
                  
                     And(()=>    
                         declarator()
                      && Option(()=>      
                            And(()=>        
                                       Char(':')
                                    && S()
                                    && (          
                                                 constant_expression()
                                              || Fatal("<<constant_expression>> expected")) ) ) )
                  || And(()=>    
                         Char(':')
                      && S()
                      && (      
                               constant_expression()
                            || Fatal("<<constant_expression>> expected")) ) );
		}
        public bool enum_specifier()    /*[15]^^enum_specifier:	'enum' B S  (identifier S enum_block? / enum_block);*/
        {

           return TreeNT((int)EC_KernighanRitchie2.enum_specifier,()=>
                And(()=>  
                     Char('e','n','u','m')
                  && B()
                  && S()
                  && (    
                         And(()=>      
                               identifier()
                            && S()
                            && Option(()=> enum_block() ) )
                      || enum_block()) ) );
		}
        public bool enum_block()    /*[150]enum_block:        '{' S enumerator (',' S enumerator)* ','? S @'}' S;*/
        {

           return And(()=>  
                     Char('{')
                  && S()
                  && enumerator()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && enumerator() ) )
                  && Option(()=> Char(',') )
                  && S()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() );
		}
        public bool enumerator()    /*[16]^^enumerator:	identifier S ('=' S @constant_expression)?;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.enumerator,()=>
                And(()=>  
                     identifier()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && (        
                                       constant_expression()
                                    || Fatal("<<constant_expression>> expected")) ) ) ) );
		}
        public bool declarator()    /*[17]^^declarator:	pointer? direct_declarator;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.declarator,()=>
                And(()=>  
                     Option(()=> pointer() )
                  && direct_declarator() ) );
		}
        public bool direct_declarator()    /*[18]^^direct_declarator:(identifier:declarator_ident check_typedef_declarator_ S / '(' S @declarator @')' S)
			(^'[' S constant_expression? @']' S/
			 '(' S parameter_type_list @')' S/
			 '(' S list<identifier,','>? @')' S
			)*;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.direct_declarator,()=>
                And(()=>  
                     (    
                         And(()=>      
                               Into(()=> identifier(),out _top.declarator_ident)
                            && _top.check_typedef_declarator_()
                            && S() )
                      || And(()=>      
                               Char('(')
                            && S()
                            && (    declarator() || Fatal("<<declarator>> expected"))
                            && (    Char(')') || Fatal("<<')'>> expected"))
                            && S() ))
                  && OptRepeat(()=>    
                            
                               And(()=>        
                                       TreeChars(()=> Char('[') )
                                    && S()
                                    && Option(()=> constant_expression() )
                                    && (    Char(']') || Fatal("<<']'>> expected"))
                                    && S() )
                            || And(()=>        
                                       Char('(')
                                    && S()
                                    && parameter_type_list()
                                    && (    Char(')') || Fatal("<<')'>> expected"))
                                    && S() )
                            || And(()=>        
                                       Char('(')
                                    && S()
                                    && Option(()=>          
                                              list(()=> identifier() ,()=>Char(',') ) )
                                    && (    Char(')') || Fatal("<<')'>> expected"))
                                    && S() ) ) ) );
		}
        public bool pointer()    /*[19]^^pointer:		('*' S (type_qualifier S)* )+;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.pointer,()=>
                PlusRepeat(()=>  
                  And(()=>    
                         Char('*')
                      && S()
                      && OptRepeat(()=> And(()=>    type_qualifier() && S() ) ) ) ) );
		}
        public bool parameter_type_list()    /*[20]^^parameter_type_list: enter_params_ (parameter_type_list_impl / leave_params_ !.) leave_params_;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.parameter_type_list,()=>
                And(()=>  
                     _top.enter_params_()
                  && (    
                         parameter_type_list_impl()
                      || And(()=>    _top.leave_params_() && Not(()=> Any() ) ))
                  && _top.leave_params_() ) );
		}
        public bool parameter_type_list_impl()    /*[200]parameter_type_list_impl:
			parameter_declaration S (',' S parameter_declaration S)* (',' S @'...' S)? ;*/
        {

           return And(()=>  
                     parameter_declaration()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && parameter_declaration()
                            && S() ) )
                  && Option(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    Char('.','.','.') || Fatal("<<'...'>> expected"))
                            && S() ) ) );
		}
        public bool parameter_declaration()    /*[21]^^parameter_declaration:	
			declaration_specifiers 
			(declarator/abstract_declarator?);*/
        {

           return TreeNT((int)EC_KernighanRitchie2.parameter_declaration,()=>
                And(()=>  
                     declaration_specifiers()
                  && (    declarator() || Option(()=> abstract_declarator() )) ) );
		}
        public bool initializer()    /*[22]^^initializer:	assignment_expression / 
			'{' S initializer (',' S initializer)* ','? S @'}' S;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.initializer,()=>
                  
                     assignment_expression()
                  || And(()=>    
                         Char('{')
                      && S()
                      && initializer()
                      && OptRepeat(()=>      
                            And(()=>    Char(',') && S() && initializer() ) )
                      && Option(()=> Char(',') )
                      && S()
                      && (    Char('}') || Fatal("<<'}'>> expected"))
                      && S() ) );
		}
        public bool type_name()    /*[23]^^type_name:	specifier_qualifier_list abstract_declarator?;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.type_name,()=>
                And(()=>  
                     specifier_qualifier_list()
                  && Option(()=> abstract_declarator() ) ) );
		}
        public bool abstract_declarator()    /*[24]^^abstract_declarator:key_detail<pointer,direct_abstract_declarator>;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.abstract_declarator,()=>
                key_detail(()=>  
                  pointer()  
                  ,()=>direct_abstract_declarator() ) );
		}
        public bool direct_abstract_declarator()    /*[25]^^direct_abstract_declarator:
			key_detail<'(' S abstract_declarator @')',
				   (^'['S constant_expression? @']' S / '('S parameter_type_list? @')' S)+
				  >;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.direct_abstract_declarator,()=>
                key_detail(()=>  
                  And(()=>    
                         Char('(')
                      && S()
                      && abstract_declarator()
                      && (    Char(')') || Fatal("<<')'>> expected")) )  
                  ,()=>PlusRepeat(()=>    
                            
                               And(()=>        
                                       TreeChars(()=> Char('[') )
                                    && S()
                                    && Option(()=> constant_expression() )
                                    && (    Char(']') || Fatal("<<']'>> expected"))
                                    && S() )
                            || And(()=>        
                                       Char('(')
                                    && S()
                                    && Option(()=> parameter_type_list() )
                                    && (    Char(')') || Fatal("<<')'>> expected"))
                                    && S() ) ) ) );
		}
        public bool typedef_name()    /*[26]^^typedef_name:	identifier:defined_typedef_name   typedef_name_lookup_ ;

//statements*/
        {

           return TreeNT((int)EC_KernighanRitchie2.typedef_name,()=>
                And(()=>  
                     Into(()=> identifier(),out _top.defined_typedef_name)
                  && _top.typedef_name_lookup_() ) );
		}
        public bool statement()    /*[27]^statement:	labeled_statement /
			compound_statement /
			selection_statement /
			iteration_statement /
			jump_statement /
			expression_statement;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.statement,()=>
                  
                     labeled_statement()
                  || compound_statement()
                  || selection_statement()
                  || iteration_statement()
                  || jump_statement()
                  || expression_statement() );
		}
        public bool labeled_statement()    /*[28]^^labeled_statement:identifier S ':' S statement /
			^'case' B S @constant_expression ':' S @statement/
			^'default'  S @':' S @statement ;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.labeled_statement,()=>
                  
                     And(()=>    
                         identifier()
                      && S()
                      && Char(':')
                      && S()
                      && statement() )
                  || And(()=>    
                         TreeChars(()=> Char('c','a','s','e') )
                      && B()
                      && S()
                      && (      
                               constant_expression()
                            || Fatal("<<constant_expression>> expected"))
                      && Char(':')
                      && S()
                      && (    statement() || Fatal("<<statement>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('d','e','f','a','u','l','t') )
                      && S()
                      && (    Char(':') || Fatal("<<':'>> expected"))
                      && S()
                      && (    statement() || Fatal("<<statement>> expected")) ) );
		}
        public bool expression_statement()    /*[29]^^expression_statement: expression @';' S /  ';' S ;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.expression_statement,()=>
                  
                     And(()=>    
                         expression()
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S() )
                  || And(()=>    Char(';') && S() ) );
		}
        public bool compound_statement()    /*[30]^^compound_statement:'{' enter_scope_ S declaration* statement*  leave_scope_ @'}' S;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.compound_statement,()=>
                And(()=>  
                     Char('{')
                  && _top.enter_scope_()
                  && S()
                  && OptRepeat(()=> declaration() )
                  && OptRepeat(()=> statement() )
                  && _top.leave_scope_()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool selection_statement()    /*[31]^^selection_statement:^'if' S '(' S expression ')' S statement 
			(^'else' B S @statement)? /
			^'switch' B S @'(' S @expression @')' S statement;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.selection_statement,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('i','f') )
                      && S()
                      && Char('(')
                      && S()
                      && expression()
                      && Char(')')
                      && S()
                      && statement()
                      && Option(()=>      
                            And(()=>        
                                       TreeChars(()=> Char('e','l','s','e') )
                                    && B()
                                    && S()
                                    && (    statement() || Fatal("<<statement>> expected")) ) ) )
                  || And(()=>    
                         TreeChars(()=> Char('s','w','i','t','c','h') )
                      && B()
                      && S()
                      && (    Char('(') || Fatal("<<'('>> expected"))
                      && S()
                      && (    expression() || Fatal("<<expression>> expected"))
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S()
                      && statement() ) );
		}
        public bool jump_statement()    /*[32]^^jump_statement:	^'goto' B S @identifier S ';' S /
			^'continue'  S @';' S /
			^'break' S @';' S /
			^'return' B S expression? @';' S;

								
//expressions*/
        {

           return TreeNT((int)EC_KernighanRitchie2.jump_statement,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('g','o','t','o') )
                      && B()
                      && S()
                      && (    identifier() || Fatal("<<identifier>> expected"))
                      && S()
                      && Char(';')
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char("continue") )
                      && S()
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('b','r','e','a','k') )
                      && S()
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('r','e','t','u','r','n') )
                      && B()
                      && S()
                      && Option(()=> expression() )
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S() ) );
		}
        public bool expression()    /*[33]^expression: 	list<assignment_expression,','>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.expression,()=>
                list(()=> assignment_expression() ,()=>Char(',') ) );
		}
        public bool assignment_expression()    /*[34]^assignment_expression: 
			!parenthized_type_name 
                         unary_expression assignment_operator S @assignment_expression /
			conditional_expression;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.assignment_expression,()=>
                  
                     And(()=>    
                         Not(()=> parenthized_type_name() )
                      && unary_expression()
                      && assignment_operator()
                      && S()
                      && (      
                               assignment_expression()
                            || Fatal("<<assignment_expression>> expected")) )
                  || conditional_expression() );
		}
        public bool assignment_operator()    /*[35]^assignment_operator:'='!'=' / ('*=' / '/=' / '%=' / '+=' / '-=' / '<<=' /
			'>>=' / '&=' / '^=' / '|=');*/
        {

           return TreeAST((int)EC_KernighanRitchie2.assignment_operator,()=>
                  
                     And(()=>    Char('=') && Not(()=> Char('=') ) )
                  || OneOfLiterals(optimizedLiterals0) );
		}
        public bool conditional_expression()    /*[36]^conditional_expression: 
			logical_or_expression 
			(^'?' S @expression @(^':') S @conditional_expression)?;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.conditional_expression,()=>
                And(()=>  
                     logical_or_expression()
                  && Option(()=>    
                      And(()=>      
                               TreeChars(()=> Char('?') )
                            && S()
                            && (    expression() || Fatal("<<expression>> expected"))
                            && (        
                                       TreeChars(()=> Char(':') )
                                    || Fatal("<<(^':')>> expected"))
                            && S()
                            && (        
                                       conditional_expression()
                                    || Fatal("<<conditional_expression>> expected")) ) ) ) );
		}
        public bool logical_or_expression()    /*[37]^logical_or_expression: 	
			binary<logical_and_expression,^'||'>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.logical_or_expression,()=>
                binary(()=>  
                  logical_and_expression()  
                  ,()=>TreeChars(()=> Char('|','|') ) ) );
		}
        public bool logical_and_expression()    /*[38]^logical_and_expression:	
			binary<inclusive_or_expression,^'&&'>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.logical_and_expression,()=>
                binary(()=>  
                  inclusive_or_expression()  
                  ,()=>TreeChars(()=> Char('&','&') ) ) );
		}
        public bool inclusive_or_expression()    /*[39]^inclusive_or_expression: 
			binary<exclusive_or_expression,^'|'>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.inclusive_or_expression,()=>
                binary(()=>  
                  exclusive_or_expression()  
                  ,()=>TreeChars(()=> Char('|') ) ) );
		}
        public bool exclusive_or_expression()    /*[40]^exclusive_or_expression:
			binary<and_expression,^'^'>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.exclusive_or_expression,()=>
                binary(()=>  
                  and_expression()  
                  ,()=>TreeChars(()=> Char('^') ) ) );
		}
        public bool and_expression()    /*[41]^and_expression: 	binary<equality_expression,^'&'>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.and_expression,()=>
                binary(()=>  
                  equality_expression()  
                  ,()=>TreeChars(()=> Char('&') ) ) );
		}
        public bool equality_expression()    /*[42]^equality_expression:binary<relational_expression,^('=='/'!=')>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.equality_expression,()=>
                binary(()=>  
                  relational_expression()  
                  ,()=>TreeChars(()=>     Char('=','=') || Char('!','=') ) ) );
		}
        public bool relational_expression()    /*[43]^relational_expression:
			binary<shift_expression,^('<='/'>='/'<'/'>')>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.relational_expression,()=>
                binary(()=>  
                  shift_expression()  
                  ,()=>TreeChars(()=>    
                            
                               Char('<','=')
                            || Char('>','=')
                            || Char('<')
                            || Char('>') ) ) );
		}
        public bool shift_expression()    /*[44]^shift_expression:	binary<additive_expression,^('<<'/'>>')>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.shift_expression,()=>
                binary(()=>  
                  additive_expression()  
                  ,()=>TreeChars(()=>     Char('<','<') || Char('>','>') ) ) );
		}
        public bool additive_expression()    /*[45]^additive_expression:binary<multiplicative_expression,^('+'/'-')>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.additive_expression,()=>
                binary(()=>  
                  multiplicative_expression()  
                  ,()=>TreeChars(()=>     Char('+') || Char('-') ) ) );
		}
        public bool multiplicative_expression()    /*[46]^multiplicative_expression:
			binary<cast_expression,^('*'/'/'/'%')>;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.multiplicative_expression,()=>
                binary(()=>  
                  cast_expression()  
                  ,()=>TreeChars(()=>     Char('*') || Char('/') || Char('%') ) ) );
		}
        public bool cast_expression()    /*[47]^cast_expression:	parenthized_type_name  S @cast_expression / unary_expression ;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.cast_expression,()=>
                  
                     And(()=>    
                         parenthized_type_name()
                      && S()
                      && (      
                               cast_expression()
                            || Fatal("<<cast_expression>> expected")) )
                  || unary_expression() );
		}
        public bool parenthized_type_name()    /*[470]parenthized_type_name:
                        '(' S type_name @')';*/
        {

           return And(()=>  
                     Char('(')
                  && S()
                  && type_name()
                  && (    Char(')') || Fatal("<<')'>> expected")) );
		}
        public bool unary_expression()    /*[48]^unary_expression:	postfix_expression 	/
			^'++' S @unary_expression /
			^'--' S @unary_expression /
			unary_operator S @cast_expression /
			^'sizeof' S '(' S type_name S @')' S / 
			^'sizeof' B S @unary_expression;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.unary_expression,()=>
                  
                     postfix_expression()
                  || And(()=>    
                         TreeChars(()=> Char('+','+') )
                      && S()
                      && (      
                               unary_expression()
                            || Fatal("<<unary_expression>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('-','-') )
                      && S()
                      && (      
                               unary_expression()
                            || Fatal("<<unary_expression>> expected")) )
                  || And(()=>    
                         unary_operator()
                      && S()
                      && (      
                               cast_expression()
                            || Fatal("<<cast_expression>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('s','i','z','e','o','f') )
                      && S()
                      && Char('(')
                      && S()
                      && type_name()
                      && S()
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('s','i','z','e','o','f') )
                      && B()
                      && S()
                      && (      
                               unary_expression()
                            || Fatal("<<unary_expression>> expected")) ) );
		}
        public bool unary_operator()    /*[49]^unary_operator:	'&'/'*'/'+'/'-'/'~'/'!';*/
        {

           return TreeAST((int)EC_KernighanRitchie2.unary_operator,()=>
                  
                     Char('&')
                  || Char('*')
                  || Char('+')
                  || Char('-')
                  || Char('~')
                  || Char('!') );
		}
        public bool postfix_expression()    /*[50]^postfix_expression:primary_expression 
		(
			(^'[' S expression @']'  /
			call  /
			^'.' S @identifier /
			^'->' S @identifier /
			^'++'  /
			^'--' 
			) S
		)*;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.postfix_expression,()=>
                And(()=>  
                     primary_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               (        
                                       And(()=>          
                                                 TreeChars(()=> Char('[') )
                                              && S()
                                              && expression()
                                              && (    Char(']') || Fatal("<<']'>> expected")) )
                                    || call()
                                    || And(()=>          
                                                 TreeChars(()=> Char('.') )
                                              && S()
                                              && (            
                                                             identifier()
                                                          || Fatal("<<identifier>> expected")) )
                                    || And(()=>          
                                                 TreeChars(()=> Char('-','>') )
                                              && S()
                                              && (            
                                                             identifier()
                                                          || Fatal("<<identifier>> expected")) )
                                    || TreeChars(()=> Char('+','+') )
                                    || TreeChars(()=> Char('-','-') ))
                            && S() ) ) ) );
		}
        public bool call()    /*[500]^call:             '(' S list<assignment_expression,','> @')' ;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.call,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && list(()=> assignment_expression() ,()=>Char(',') )
                  && (    Char(')') || Fatal("<<')'>> expected")) ) );
		}
        public bool primary_expression()    /*[51]^primary_expression: (identifier / 
			 constant / 
			 string / 
			 '(' S expression @')') S;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.primary_expression,()=>
                And(()=>  
                     (    
                         identifier()
                      || constant()
                      || @string()
                      || And(()=>      
                               Char('(')
                            && S()
                            && expression()
                            && (    Char(')') || Fatal("<<')'>> expected")) ))
                  && S() ) );
		}
        public bool constant()    /*[52]constant:		integer_constant /
			character_constant /
			floating_constant /
			enumeration_constant;*/
        {

           return   
                     integer_constant()
                  || character_constant()
                  || floating_constant()
                  || enumeration_constant();
		}
        public bool identifier()    /*[53]^identifier:	!(keyword B) [A-Za-z_][A-Za-z_0-9]*;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.identifier,()=>
                And(()=>  
                     Not(()=> And(()=>    keyword() && B() ) )
                  && (In('A','Z', 'a','z')||OneOf("_"))
                  && OptRepeat(()=>    
                      (In('A','Z', 'a','z', '0','9')||OneOf("_")) ) ) );
		}
        public bool keyword()    /*[54]keyword:		'auto' / 'register' /'static' /'extern' /'typedef' /
			'void' / 'char' / 'short' / 'int' / 'long' / 
			'float' / 'double' / 'signed' / 'unsigned'/
			'const'/'volatile'/ 'struct' / 'union'/
			'enum'/ 'case'/'default'/
			'if'/'else'/ 'switch'/'goto'/
			'continue'/'break'/'return'/
			'sizeof'/ 'while' / 'do' / 'for';*/
        {

           return OneOfLiterals(optimizedLiterals1);
		}
        public bool iteration_statement()    /*[55]^iteration_statement:^'while' S @'(' S expression S @')' S @statement/
			^'do' B S @statement @'while' S @'(' S expression @')' S @';' S/
			^'for' S @'(' S for_init_expression? @';' S for_condition? @';' S for_incr? @')' S statement;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.iteration_statement,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('w','h','i','l','e') )
                      && S()
                      && (    Char('(') || Fatal("<<'('>> expected"))
                      && S()
                      && expression()
                      && S()
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S()
                      && (    statement() || Fatal("<<statement>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('d','o') )
                      && B()
                      && S()
                      && (    statement() || Fatal("<<statement>> expected"))
                      && (      
                               Char('w','h','i','l','e')
                            || Fatal("<<'while'>> expected"))
                      && S()
                      && (    Char('(') || Fatal("<<'('>> expected"))
                      && S()
                      && expression()
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S()
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('f','o','r') )
                      && S()
                      && (    Char('(') || Fatal("<<'('>> expected"))
                      && S()
                      && Option(()=> for_init_expression() )
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S()
                      && Option(()=> for_condition() )
                      && (    Char(';') || Fatal("<<';'>> expected"))
                      && S()
                      && Option(()=> for_incr() )
                      && (    Char(')') || Fatal("<<')'>> expected"))
                      && S()
                      && statement() ) );
		}
        public bool for_init_expression()    /*[550]^^for_init_expression:expression;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.for_init_expression,()=>
                expression() );
		}
        public bool for_condition()    /*[551]^^for_condition:    expression;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.for_condition,()=>
                expression() );
		}
        public bool for_incr()    /*[552]^^for_incr:         expression;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.for_incr,()=>
                expression() );
		}
        public bool S()    /*[56]S:			(c_comment / c_preprocessing_directive / [ \r\n\v\t])*;*/
        {

           return OptRepeat(()=>  
                      
                         c_comment()
                      || c_preprocessing_directive()
                      || OneOf(" \r\n\v\t") );
		}
        public bool c_comment()    /*[57]c_comment:		'/*'  ( (!'* /' .)*  '* /' / FATAL<"comment not closed before end of file"> );*/
        {

           return And(()=>  
                     Char('/','*')
                  && (    
                         And(()=>      
                               OptRepeat(()=>        
                                    And(()=>    Not(()=> Char('*','/') ) && Any() ) )
                            && Char('*','/') )
                      || Fatal("comment not closed before end of file")) );
		}
        public bool constant_expression()    /*[58]^constant_expression:conditional_expression;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.constant_expression,()=>
                conditional_expression() );
		}
        public bool @string()    /*[59]^string:		l?["](escape_sequence/chars)*["];*/
        {

           return TreeAST((int)EC_KernighanRitchie2.@string,()=>
                And(()=>  
                     Option(()=> l() )
                  && OneOf("\"")
                  && OptRepeat(()=>     escape_sequence() || chars() )
                  && OneOf("\"") ) );
		}
        public bool integer_constant()    /*[60]^integer_constant: 	(hexadecimal_constant / decimal_int)(l u/l u/ u l / u / l)? B ;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.integer_constant,()=>
                And(()=>  
                     (    hexadecimal_constant() || decimal_int())
                  && Option(()=>    
                            
                               And(()=>    l() && u() )
                            || And(()=>    l() && u() )
                            || And(()=>    u() && l() )
                            || u()
                            || l() )
                  && B() ) );
		}
        public bool character_constant()    /*[61]^character_constant:  l?['] (escape_sequence/char) ['];*/
        {

           return TreeAST((int)EC_KernighanRitchie2.character_constant,()=>
                And(()=>  
                     Option(()=> l() )
                  && OneOf("'")
                  && (    escape_sequence() || @char())
                  && OneOf("'") ) );
		}
        public bool floating_constant()    /*[62]^floating_constant:	(decimal_int exponent / decimal_int '.' fraction?)(l f/l f/ f l / f / l);*/
        {

           return TreeAST((int)EC_KernighanRitchie2.floating_constant,()=>
                And(()=>  
                     (    
                         And(()=>    decimal_int() && exponent() )
                      || And(()=>      
                               decimal_int()
                            && Char('.')
                            && Option(()=> fraction() ) ))
                  && (    
                         And(()=>    l() && f() )
                      || And(()=>    l() && f() )
                      || And(()=>    f() && l() )
                      || f()
                      || l()) ) );
		}
        public bool enumeration_constant()    /*[63]^enumeration_constant:identifier;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.enumeration_constant,()=>
                identifier() );
		}
        public bool exponent()    /*[64]^exponent:		[eE][+-]?[0-9]+;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.exponent,()=>
                And(()=>  
                     OneOf("eE")
                  && Option(()=> OneOf("+-") )
                  && PlusRepeat(()=> In('0','9') ) ) );
		}
        public bool escape_sequence()    /*[65]^escape_sequence:	'\\' ([0-7]{1,3} / ['"?\\abfnrtv] / 'x' [0-9a-fA-F]+);*/
        {

           return TreeAST((int)EC_KernighanRitchie2.escape_sequence,()=>
                And(()=>  
                     Char('\\')
                  && (    
                         ForRepeat(1,3,()=> In('0','7') )
                      || OneOf(optimizedCharset0)
                      || And(()=>      
                               Char('x')
                            && PlusRepeat(()=> In('0','9', 'a','f', 'A','F') ) )) ) );
		}
        public bool chars()    /*[66]^chars:		[#x20#x21#x23-#x7F]+;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.chars,()=>
                PlusRepeat(()=>  
                  (In('\u0023','\u007f')||OneOf("\u0020\u0021")) ) );
		}
        public bool @char()    /*[67]^char:		[#x20-#x26#x28-#x7F];
//generic rules*/
        {

           return TreeAST((int)EC_KernighanRitchie2.@char,()=>
                In('\u0020','\u0026', '\u0028','\u007f') );
		}
        public bool list(Matcher operand, Matcher separator)    /*[70]list<operand,separator>:	
			binary<operand,separator>;*/
        {

           return binary(()=> operand() ,()=>separator() );
		}
        public bool binary(Matcher operand, Matcher @operator)    /*[80]binary<operand,operator>:
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
        public bool key_detail(Matcher key, Matcher detail)    /*[100]key_detail<key,detail>:
			key S detail? / detail;*/
        {

           return   
                     And(()=>    key() && S() && Option(()=> detail() ) )
                  || detail();
		}
        public bool c_preprocessing_directive()    /*[160]c_preprocessing_directive: '#' ('\\' '\r'?'\n'  /  !'\n' . )*;*/
        {

           return And(()=>  
                     Char('#')
                  && OptRepeat(()=>    
                            
                               And(()=>        
                                       Char('\\')
                                    && Option(()=> Char('\r') )
                                    && Char('\n') )
                            || And(()=>    Not(()=> Char('\n') ) && Any() ) ) );
		}
        public bool B()    /*[161]B: 	       ![A-Za-z_0-9];*/
        {

           return Not(()=> (In('A','Z', 'a','z', '0','9')||OneOf("_")) );
		}
        public bool decimal_int()    /*[162]^decimal_int:	[0-9]+;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.decimal_int,()=>
                PlusRepeat(()=> In('0','9') ) );
		}
        public bool fraction()    /*[163]^^fraction:	decimal_int exponent? / exponent;*/
        {

           return TreeNT((int)EC_KernighanRitchie2.fraction,()=>
                  
                     And(()=>    decimal_int() && Option(()=> exponent() ) )
                  || exponent() );
		}
        public bool l()    /*[164]^l:		[lL];*/
        {

           return TreeAST((int)EC_KernighanRitchie2.l,()=>
                OneOf("lL") );
		}
        public bool u()    /*[165]^u:		[uU];*/
        {

           return TreeAST((int)EC_KernighanRitchie2.u,()=>
                OneOf("uU") );
		}
        public bool f()    /*[166]^f:		[fF];*/
        {

           return TreeAST((int)EC_KernighanRitchie2.f,()=>
                OneOf("fF") );
		}
        public bool hexadecimal_constant()    /*[167]^hexadecimal_constant:('0x'/'0X')[0-9a-fA-F]+;*/
        {

           return TreeAST((int)EC_KernighanRitchie2.hexadecimal_constant,()=>
                And(()=>  
                     (    Char('0','x') || Char('0','X'))
                  && PlusRepeat(()=> In('0','9', 'a','f', 'A','F') ) ) );
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        
        internal static OptimizedLiterals optimizedLiterals0;
        internal static OptimizedLiterals optimizedLiterals1;
        
        static C_KernighanRitchie2()
        {
            {
               char[] oneOfChars = new char[]    {'\'','"','?','\\','a'
                                                  ,'b','f','n','r','t'
                                                  ,'v'};
               optimizedCharset0= new OptimizedCharset(null,oneOfChars);
            }
            
            
            {
               string[] literals=
               { "*=","/=","%=","+=","-=","<<=",">>=","&=",
                  "^=","|=" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "auto","register","static","extern","typedef","void","char","short",
                  "int","long","float","double","signed","unsigned","const","volatile",
                  "struct","union","enum","case","default","if","else","switch",
                  "goto","continue","break","return","sizeof","while","do","for" };
               optimizedLiterals1= new OptimizedLiterals(literals);
            }

            
        }
        #endregion Optimization Data 
           }
}