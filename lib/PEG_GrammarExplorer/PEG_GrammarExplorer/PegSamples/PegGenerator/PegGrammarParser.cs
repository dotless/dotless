using System;
using Peg.Base;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace Peg.Samples
{
	enum EPegGrammar{peg_module= 1, peg_head= 2, peg_tail= 3, checked_eos= 4, peg_specification= 5, 
                        enumeration_definitions= 6, attribute= 7, attribute_value= 8, attribute_key= 9, checked_peg_rules= 10,
                        peg_rules = 11, peg_rule = 12, checked_colon = 13, lhs_of_rule = 14, using_sem_block = 97, rule_name_and_params = 15, peg_params = 98, 
                        rhs_of_rule= 16, choice= 17, term= 18, semantic_block= 19, sem_block_name= 96, 
                        block_content= 95, checked_atom= 21, check_non_postfix= 22, atom= 23, atom_prefix= 24, atom_postfix= 25, 
                        into= 26, tree_or_ast= 27, terminal= 28, any_char= 29, character_set= 30, 
                        char_set_char= 31, char_set_range= 32, suffixed_literal= 33, literal= 34, quoted_content= 35,
                        case_insensitve = 36, bit_access = 37, bit_range = 38, char_spec = 39, non_terminal = 40, peg_args = 99,
                        rule_name = 41, rule_ref = 42, rule_param = 100, into_variable = 43, ident = 44, elaborated_rule_id = 45, 
                        rule_id= 46, message= 47, throw_args= 48, fatal_args= 49, multiline_double_quote_literal= 50, 
                        enumerator= 51, double_quote_literal= 52, dbl_quoted_content= 53, checked_in_quote_char= 54, in_quote_char= 55, 
                        code_point= 56, hex_number= 57, binary_number= 58, decimal_number= 59, hexadecimal_digits= 60, 
                        binary_digits= 61, decimal_digits= 62, escape_sequence= 63, escape_char= 64, escape_int= 65, 
                        checked_escape_sequence= 66, peek_symbol= 67, not_symbol= 68, option_symbol= 69, repetition_symbol= 70, 
                        star= 71, plus= 72, repetition_range= 73, checked_range_spec= 74, range_lower_limit= 75, 
                        range_upper_limit= 76, optional_upper_limit= 77, mandatory_symbol= 78, set_negation= 79, ast_symbol= 80, 
                        tree_symbol= 81, lower_limit= 82, upper_limit= 83, limit_spec= 84, numeric_limit= 85, 
                        integer= 86, enumeration_definition= 87, S= 88, S_n= 89, spaces= 90, 
                        comment= 91, end_of_line_char= 92, enumeration_terminator= 93, printable_char= 94,
                        create_spec= 105, create_method= 106,
                        fatal= 100,warning=101,
                        toplevel_semantic_blocks=102,

                        named_semantic_block = 211, anonymous_semantic_block = 212,
                        semantic_block_content = 213, quoted = 214, sem_comment = 221, 
                        W = 223, B = 224, sem_ident = 228, into_declaration = 229, into_typedecl = 230,
                        into_type = 231, variable_declarators = 232, variable_declarator = 233,
                        variable_initializer = 234, variable = 235, outer_ident = 236, sem_func_declaration = 237,
                        creator_func_declaration = 238, sem_func_header = 239, creator_func_header = 240,
                        creator_formal_pars = 241, creator_params = 242, func_declaration = 243,
                        func_header = 244, return_type = 245, member_name = 246, method_body = 248,
                        braced_content = 249, paren_content = 250, single_line_comment = 251,
                        directive = 252, multi_line_comment = 253, 
                        creator_name = 255, formal_pars = 256, sem_param = 257, param_ident = 258,
                        parenthized = 259, method_modifier = 260, constructor_decl = 261,
                        constructor_header = 262, constructor_initializer = 263, designator = 264,
                        desig_ident = 265, sem_mem_access = 266, index_access = 267, index_content = 268,
                        invocation = 269, field_modifier = 270, field_declaration = 271,
                        destructor_decl = 272, code_declaration = 273, type_ref=274,
                        rank_specifiers = 275, rank_specifier = 276, dim_separators=277,
                        operator_decl = 278, prop_event_decl = 279, indexer_decl= 280,
                        prop_ind_event_block = 281, accessor_declaration = 282, operator_declarator= 283,@operator= 284
                        
            };
	class PegGrammarParser : PegCharParser
    {
        #region abstract PegNode subclass for tree modifying customers
        abstract public class PGParserNode : PegNode
        {
            internal PGParserNode(PegNode parent, int id)
                : base(parent, id)
            {
            }
            internal abstract string TreeNodeToString(string src);
        }
        #endregion abstract PegNode subclass for tree modifying customers
        #region Constructors
        public PegGrammarParser(string src, TextWriter FerrOut)
			: base(src,FerrOut)
			{
			}
         public PegGrammarParser()
            : base()
        {
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                    EPegGrammar ruleEnum = (EPegGrammar)id;
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
        public override bool Fatal(string sMsg)
        {
            TreeCharsWithId((int)ESpecialNodes.eFatal, ()=>true);
            return base.Fatal(sMsg);
        }
        public override string TreeNodeToString(PegNode node)
        {
            string label = base.TreeNodeToString(node);
            if (node is PGParserNode) label += ": " + ((PGParserNode)node).TreeNodeToString(src_);
            return label;
        }
        #endregion Overrides
        #region PegNode derived classes and associated Creator functions
        public class MultiLineDblQuoteNode : PegNode
        {
            internal MultiLineDblQuoteNode(PegNode parent, int id)
                : base(parent, id)
            {
            }
            public override string GetAsString(string s)
            {
                return quoted_;
            }
            public override PegNode Clone()
            {
                MultiLineDblQuoteNode clone= new MultiLineDblQuoteNode(parent_,id_);
                clone.quoted_ = quoted_;
                CloneSubTrees(clone);
                clone.match_ = match_;
                return clone;
            }
            public string quoted_;

        }
        public class IntValue : PegNode
        {
            public override string GetAsString(string s)
            {
                return value_.ToString();
            }
            internal IntValue(PegNode parent, int id, PegBegEnd match, int val)
                :base(parent,id,match)
            {
                value_= val;
            }
            int value_;
        }
        protected PegNode QuoteNodeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                return new MultiLineDblQuoteNode(parentOrCreated, id);
            else if (phase == ECreatorPhase.eCreationComplete)
            {
                Debug.Assert(parentOrCreated.id_ == (int)EPegGrammar.multiline_double_quote_literal);
                StringBuilder s = new StringBuilder();
                for (PegNode dblQuotedContent = parentOrCreated.child_; dblQuotedContent != null; dblQuotedContent = dblQuotedContent.next_)
                {
                    Debug.Assert(dblQuotedContent.id_ == (int)EPegGrammar.dbl_quoted_content);
                    s.Append(dblQuotedContent.GetAsString(src_));
                }
                MultiLineDblQuoteNode n = (MultiLineDblQuoteNode)parentOrCreated;
                n.quoted_ = s.ToString();
                return parentOrCreated;
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
        }
        public class TRange : PegNode
        {
            public TRange(PegNode parent, int id)
                : base(parent, id)
            {
            }
            public override string GetAsString(string src)
            {
                return "{" + lower.ToString() + ", " + upper.ToString() + "}";
            }
            public override PegNode Clone()
            {
                TRange r = new TRange(parent_, id_);
                r.lower = lower;
                r.upper = upper;
                r.lowerInto= lowerInto;
                r.upperInto= upperInto;
                if (lowerIntoVar != null)
                {
                    r.lowerIntoVar= lowerIntoVar.Clone();
                }
                if (upperIntoVar != null)
                {
                    r.upperIntoVar = upperIntoVar.Clone();
                }
                r.CloneSubTrees(r);
                return r;
            }
            internal int lower, upper;
            internal string lowerInto, upperInto;
            internal PegNode lowerIntoVar, upperIntoVar;
        }
        void SetLimit(PegNode limitNode, ref int limit, ref string limitInto)
        {
            switch (limitNode.id_)
            {
                case (int)EPegGrammar.numeric_limit:
                    limit = Int32.Parse(limitNode.GetAsString(src_));
                    return;
                case (int)EPegGrammar.into_variable:
                    limitInto = limitNode.GetAsString(src_);
                    return;
                default: Debug.Assert(false);
                    return;
            }
        }
        protected PegNode MakeUpperNumericLimit(PegNode parent, Int32 upperLimit)
        {
            PegNode upperLimitNode = new PegNode(parent, (int)EPegGrammar.upper_limit,parent.match_);
            PegNode numeric_limit = new IntValue(upperLimitNode, (int)EPegGrammar.numeric_limit, parent.match_, upperLimit);
            upperLimitNode.child_ = numeric_limit;
            return upperLimitNode;
        }
        protected PegNode TRepetitionCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                return new TRange(parentOrCreated, id);
            else if (phase == ECreatorPhase.eCreationComplete)
            {

                TRange n = parentOrCreated as TRange;
                if (n.child_ != null)
                {
                    switch (n.child_.id_)
                    {
                        case (int)EPegGrammar.option_symbol:
                            n.lower = 0;
                            n.upper = 1;
                            return n;
                        case (int)EPegGrammar.star:
                            n.lower = 0;
                            n.upper = Int32.MaxValue;
                            return n;
                        case (int)EPegGrammar.plus:
                            n.lower = 1;
                            n.upper = Int32.MaxValue;
                            return n;
                        case (int)EPegGrammar.lower_limit:
                            if (n.child_.child_ != null)
                            {
                                PegNode lowerLimit = n.child_.child_;
                                if (n.child_.next_ == null)
                                {
                                    n.child_.next_ = n.child_.Clone();
                                    n.child_.next_.id_ = (int)EPegGrammar.upper_limit;
                                }
                                else
                                {
                                    Debug.Assert(n.child_.next_.id_ == (int)EPegGrammar.optional_upper_limit);
                                    if (n.child_.next_.child_ == null) /*no upper limit has been specified*/
                                    {
                                        n.child_.next_ = MakeUpperNumericLimit(n.child_,Int32.MaxValue);
                                    }
                                    else
                                    {   //remove node optional_upper_limit'
                                        Debug.Assert(n.child_.next_.child_.id_ == (int)EPegGrammar.upper_limit);
                                        n.child_.next_ = n.child_.next_.child_;
                                    }
                                }
                                PegNode upperLimit = n.child_.next_.child_;
                                SetLimit(lowerLimit, ref n.lower, ref n.lowerInto);
                                SetLimit(upperLimit, ref n.upper, ref n.upperInto); 
                                return n;

                            }
                            Debug.Assert(false);
                            return null;
                        default: Debug.Assert(false);
                            return null;
                    }
                }
                return parentOrCreated;
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
        }
        protected PegNode TBitRangeCreator(ECreatorPhase phase, PegNode parentOrCreated, int id)
        {
            if (phase == ECreatorPhase.eCreate || phase == ECreatorPhase.eCreateAndComplete)
                return new TRange(parentOrCreated, id);
            else if (phase == ECreatorPhase.eCreationComplete)
            {
                TRange n = parentOrCreated as TRange;
                if (n.child_ != null)
                {
                    switch (n.child_.id_)
                    {
                        case (int)EPegGrammar.lower_limit:
                            if (n.child_.child_ != null)
                            {
                                PegNode lowerLimit = n.child_.child_;
                                PegNode upperLimit = n.child_.next_ != null ? n.child_.next_.child_ : null;
                                SetLimit(lowerLimit, ref n.lower, ref n.lowerInto);
                                if (upperLimit != null) SetLimit(upperLimit, ref n.upper, ref n.upperInto);
                                else
                                {
                                    n.upper = n.lower;
                                    n.upperInto = n.lowerInto;
                                }
                                return n;

                            }
                            Debug.Assert(false);
                            return null;
                        default: Debug.Assert(false);
                            return null;
                    }
                }
                return parentOrCreated;
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
        }
        #endregion
        #region Grammar Rules
		public bool peg_module()	/*[1] ^^peg_module:       S peg_head peg_specification peg_tail checked_eos;*/

		{
			return TreeNT((int)EPegGrammar.peg_module,()=>
				And(()=>
					S()
				&&	peg_head()
				&&	peg_specification()
                && (peg_tail() || Fatal("<< <<Grammar >> expected"))
				&&	checked_eos()
				));
		}
		public bool peg_head()	/*[2] ^^peg_head:         @'<<Grammar' B S (attribute S)*  @'>>' S		;*/

		{
			return TreeNT((int)EPegGrammar.peg_head,()=>
				And(()=>
                    (IChar("<<GRAMMAR") || Fatal("<< <<Grammar >> expected"))
				&&	B()
                &&  S()
				&&	OptRepeat(()=>And(()=>attribute()&&	S()))
				&&	(Char('>','>') ||	Fatal("<<'>>'>> expected"))
				&&	S()
				));
		}
		public bool peg_tail()	/*[3]   peg_tail:         @'<</Grammar' S @'>>' S					;*/
		{
			return
				And(()=>
                    IChar("<</GRAMMAR")
				&&	S()
				&&	(Char('>','>') ||	Fatal("<< '>>' >> expected"))
				&&	S()
				);
		}
		public bool checked_eos()	/*[4]   checked_eos:      !. / FATAL<"end of PEG specification expected">		;*/
		{
			return
					Not(()=>Any())
				||	Fatal("end of PEG specification expected");
		}
        public bool peg_specification()	/*[5] ^^peg_specification: rule_assoc_info checked_peg_rules enumeration_definitions;*/
		{
			return TreeNT((int)EPegGrammar.peg_specification,()=>
				And(()=>
                    toplevel_semantic_blocks()
				&&	checked_peg_rules()
				&&	enumeration_definitions()
				));
		}
        public bool toplevel_semantic_blocks() /*[102]^^rule_assoc_info: semantic_block*;*/
        {
            return TreeNT((int)EPegGrammar.toplevel_semantic_blocks,()=>OptRepeat(()=>semantic_block()));
        }
		public bool enumeration_definitions()	/*[6] ^^enumeration_definitions: enumeration_definition*					;*/

		{
			return TreeNT((int)EPegGrammar.enumeration_definitions,()=>
				                        OptRepeat(()=>enumeration_definition()));
		}
		public bool attribute()	/*[7] ^^attribute:        attribute_key S @'=' S attribute_value S;*/

		{
			return TreeNT((int)EPegGrammar.attribute,()=>
				And(()=>
					attribute_key()
				&&	S()
				&&	(Char('=') || Fatal("<<'='>> expected"))
				&&	S()
				&&	attribute_value()
				&&	S()
				));
		}
		public bool attribute_value()	/*[8] ^^attribute_value:  literal / double_quote_literal				;*/

		{
			return TreeNT((int)EPegGrammar.attribute_value,()=>
					literal()
				||	double_quote_literal());
		}
		public bool attribute_key()	/*[9] ^^attribute_key:    ident							;*/
		{
			return TreeNT((int)EPegGrammar.attribute_key,()=> ident());
		}
		public bool checked_peg_rules()	/*[10]  checked_peg_rules:peg_rules / FATAL<"at least one PEG rule expected">	;*/
		{
			return  peg_rules() ||	Fatal("at least one PEG rule expected");
		}
		public bool peg_rules()	/*[11]^^peg_rules:        S peg_rule+						;*/
		{
			return TreeNT((int)EPegGrammar.peg_rules,()=>
				            And(()=>S() &&	PlusRepeat(()=>peg_rule())));
		}
		public bool peg_rule()	/*[12]^^peg_rule:         lhs_of_rule  checked_colon rhs_of_rule? @';' S 		;*/
		{
			return TreeNT((int)EPegGrammar.peg_rule,()=>
				And(()=>
					lhs_of_rule()
				&&	checked_colon()
				&&	Option(()=>rhs_of_rule())
				&&	(Char(';') || Fatal("<<';'>> expected"))
				&&	S()
				));
		}
		public bool checked_colon()	/*[13]  checked_colon:    ':' S / !'=' FATAL<"one of << :  = >> expected">	;*/
		{
			return
					And(()=>Char(':')&&	S())
				||	And(()=>Not(()=>Char('=')) && Fatal("one of << :  = >> expected")
				);
		}
        public bool lhs_of_rule()	/*[14]^^lhs_of_rule:      elaborated_rule_id?  tree_or_ast? create_spec? 
                        rule_name_and_params (semantic_block/using_sem_block)?  ;*/
		{
			return TreeNT((int)EPegGrammar.lhs_of_rule,()=>
				And(()=>
				   Option(()=>elaborated_rule_id())
				&& Option(()=>tree_or_ast())
                && Option(() => create_spec())
                && rule_name_and_params()
				&&	Option(()=>semantic_block() ||	using_sem_block())
				));
		}
		public bool using_sem_block()	/*[97]  using_sem_block:  'using' S sem_block_name S;*/
		{
			return
				And(()=>
					Char('u','s','i','n','g')
				&&	S()
				&&	sem_block_name()
				&&	S()
				);
		}
        public bool rule_name_and_params()	/*[15]  rule_name_and_params: rule_name S peg_params? 				;*/
        {
            return
                And(() => rule_name() && S() && Option(() => peg_params()));
        }
        public bool peg_params()	/*[98]^^peg_params:	'<' S rule_param ( S ',' S rule_param)* S '>' S		;*/
        {
            return TreeNT((int)EPegGrammar.peg_params, () =>
                And(() =>
                   Char('<')
                && S()
                && rule_param()
                && OptRepeat(() => And(() => S() && Char(',') && S() && rule_param()))
                && S()
                && Char('>')
                && S()
                ));
        }
		public bool rhs_of_rule()	/*[16]^^rhs_of_rule:      choice ( '/' S @choice)*				;*/
		{
			return TreeNT((int)EPegGrammar.rhs_of_rule,()=>
				And(()=>
					choice()
				&&	OptRepeat(()=>
                            And(()=>
					        Char('/')
				        &&	S()
				        &&	(choice() || Fatal("<<choice>> expected"))
				        ))
				));
		}
		public bool choice()	/*[17]^^choice: 	        term+ 							;*/
		{
			return TreeNT((int)EPegGrammar.choice,()=> PlusRepeat(()=>term()));
		}
		public bool term()	/*[18]^^term:	        atom_prefix checked_atom check_non_postfix	/ 
				checked_atom  atom_postfix? 			;*/

		{
			return TreeNT((int)EPegGrammar.term,()=>
					And(()=> atom_prefix() &&	checked_atom() &&	check_non_postfix())
				||	And(()=> checked_atom()&&	Option(()=>atom_postfix())));
		}
		
        bool GenericQuoted(Matcher quoteBeg, Matcher inner, Matcher quoteEnd,Matcher errHandler)
        {
            return And(() =>
                       Peek(quoteBeg)
                   && (And(() => quoteBeg() && inner() && quoteEnd()) || errHandler())
                   );
            
        }
		public bool block_content()	/*[95]  block_content:    (literal / double_quote_literal / '{' block_content @'}' / 
			                                   comment / !'}' . )*  ;*/
		{
			return
				OptRepeat(()=>
					literal()
				||	double_quote_literal()
			    ||	GenericQuoted(()=>Char('{'),()=>block_content(),()=>Char('}'),()=>Fatal("{ not followed by '}'"))
				||	comment()
				||	(And(()=>Not(()=>Char('}')) &&	Any())
					));
		}
		public bool checked_atom()	/*[21]  checked_atom:     atom / 
			![;/)] FATAL<"one of << PEG element, ;, /  ) >> expected">;*/
		{
			return
					atom()
				||	And(()=>Not(()=>OneOf(';',',','/',')','>')) &&	Fatal("one of << PEG element, ;, /  ) >> expected"));
		}
		public bool check_non_postfix()	/*[22]  check_non_postfix:!atom_postfix / FATAL<"use ( ) around previous element ">;*/
		{
			return
					Not(()=>atom_postfix())
				||	Fatal("use ( ) around previous element ");
		}
		public bool atom()	/*[23]^^atom:	        terminal /  message / non_terminal /'(' S rhs_of_rule @')' S;*/
		{
			return TreeNT((int)EPegGrammar.atom,()=>
					    terminal()
				    ||	message()
				    ||	non_terminal()
				    ||	(And(()=>
					        Char('(')
				        &&	S()
				        &&	rhs_of_rule()
				        &&	(Char(')') ||	Fatal("<<')'>> expected"))
				        &&	S()
				        )));
		}
		public bool atom_prefix()	/*[24]^^atom_prefix:      (tree_or_ast / peek_symbol / not_symbol / mandatory_symbol) S ;*/
		{
			return TreeNT((int)EPegGrammar.atom_prefix,()=>
				And(()=>
                    ( tree_or_ast() ||	peek_symbol() ||	not_symbol() ||	mandatory_symbol() )
				&&	S()
				));
		}
		public bool atom_postfix()	/*[25]^^atom_postfix:     (option_symbol/repetition_symbol/repetition_range/into)S;*/
		{
			return TreeNT((int)EPegGrammar.atom_postfix,()=>
				And(()=>
                    ( option_symbol() || repetition_symbol() ||	repetition_range() || into())
				&&	S()
				));
		}
		public bool into()	/*[26]  into:             ':' S into_variable S					;*/
		{
			return
				And(()=>
					Char(':')
				&&	S()
				&&	into_variable()
				&&	S()
				);
		}
		public bool tree_or_ast()	/*[27]  tree_or_ast:      (tree_symbol  / ast_symbol) S				;*/
		{
			return
				And(()=>
                    ( tree_symbol() || ast_symbol() )
				&&	S()
				);
		}
		public bool terminal()	/*[28]  terminal:	        (suffixed_literal / code_point / any_char 
					/ character_set / bit_access) S 	;*/
		{
			return
				And(()=>
                    (   suffixed_literal()
				    ||	code_point() &&  ( B() || Fatal("codepoint must not immediately by followed by letter "))
				    ||	any_char()
				    ||	character_set()
				    ||	bit_access()
					)
				&&	S()
				);
		}
		public bool any_char()	/*[29]^^any_char:	        '.'							;*/
		{
			return TreeNT((int)EPegGrammar.any_char,()=> Char('.'));
		}
		public bool character_set()	/*[30]^^character_set:    '[' set_negation?
			((char_set_range/char_set_char)+ /
			            FATAL<"at least one character expected">)
		  @']'								;*/

		{
			return TreeNT((int)EPegGrammar.character_set,()=>
				And(()=>
					Char('[')
				&&	Option(()=>set_negation())
				&&	(   PlusRepeat(()=> char_set_range() ||	char_set_char())
				    ||	Fatal("at least one character expected")
					)
				&&	(
					Char(']')
				||	Fatal("<<']'>> expected")
					)
				));
		}
		public bool char_set_char()	/*[31]^^char_set_char:    !']'   (escape_sequence/code_point/printable_char)  	;*/
		{
			return TreeNT((int)EPegGrammar.char_set_char,()=>
				And(()=>
					Not(()=>Char(']'))
				&&	(escape_sequence()  ||	code_point() ||	printable_char() )
				));
		}
		public bool char_set_range()	/*[32]^^char_set_range:	char_set_char '-' char_set_char				;*/
		{
			return TreeNT((int)EPegGrammar.char_set_range,()=>
				And(()=>
					char_set_char()
				&&	Char('-')
				&&	char_set_char()
				));
		}
		public bool suffixed_literal()	/*[33]^^suffixed_literal:	literal S case_insensitve?				;*/
		{
			return TreeNT((int)EPegGrammar.suffixed_literal,()=>
				And(()=>
					literal()
				&&	S()
				&&	Option(()=>case_insensitve())
				));
		}
		public bool literal()	/*[34]  literal:	        ['] quoted_content @[']					;*/
		{
			return
				And(()=>
					Char('\'')
				&&	quoted_content()
				&&	( Char('\'') ||	Fatal("<<[']>> expected"))
				);
		}
		public bool quoted_content()	/*[35]^^quoted_content:	(!'\'' checked_in_quote_char)*				;*/
		{
			return TreeNT((int)EPegGrammar.quoted_content,()=>
				OptRepeat(()=>And(()=>Not(()=>Char('\'')) &&	checked_in_quote_char()))
                );
		}
		public bool case_insensitve()	/*[36]^^case_insensitve:	[\\][iI] S 						;*/
		{
			return TreeNT((int)EPegGrammar.case_insensitve,()=>
				And(()=>
					Char('\\')
				&&	OneOf('i','I')
				&&	S()
				));
		}
		public bool bit_access()	/*[37]^^bit_access: 	'BITS' S 
			@'<' S bit_range @',' S @char_spec S into? @'>'S;*/
		{
            bool ok= TreeNT((int)EPegGrammar.bit_access,()=>
				And(()=>
					Char('B','I','T','S')
				&&	S()
				&&	(Char('<') ||	Fatal("<<'<'>> expected"))
				&&	S()
				&&	bit_range()
				&&	(Char(',') || Fatal("<<','>> expected"))
				&&	S()
				&&	(char_spec()  ||	Fatal("<<code point or character set>> expected"))
                &&  Option(()=>And( ()=> Char(',') && (into()||Fatal("into variable expected")))) 
				&&	S()
				&&	( Char('>') ||	Fatal("<<'>'>> expected") )
				&&	S()
				));
            return ok;
		}
		public bool bit_range()	/*[38]^^bit_range:	lower_limit S ('-' S upper_limit)? 		;*/
		{
			return TreeNT(TBitRangeCreator,(int)EPegGrammar.bit_range,()=>
				And(()=>
					lower_limit()
				&&	S()
                && Option(() => And(() => Char('-') && S() && (upper_limit()||Fatal("<< upper limit >> expected"))))
				));
		}
		public bool char_spec()	    /*[39] char_spec:		character_set / code_point				;*/
		{
            return any_char() || character_set() || code_point();
		}
		public bool non_terminal()	/*[40] non_terminal:	rule_ref   S  						;*/
		{
            return And(() => rule_ref() && S() && Option(() => peg_args()) );
		}
        public bool peg_args()	/*[99]^^peg_args:		'<' S rhs_of_rule (',' S rhs_of_rule)* '>' S		;*/
        {
            return TreeNT((int)EPegGrammar.peg_args, () =>
                And(() =>
                   Char('<')
                && S()
                && rhs_of_rule()
                && OptRepeat(() => And(() => Char(',') && S() && rhs_of_rule() ))
                && Char('>')
                && S()
                ));
        }
		public bool rule_name()	    /*[41]^^rule_name:        ident 							;*/
		{
			return TreeNT((int)EPegGrammar.rule_name,()=> ident());
		}
		public bool rule_ref()	    /*[42]^^rule_ref:		ident 							;*/
		{
			return TreeNT((int)EPegGrammar.rule_ref,()=> ident());
		}
        public bool rule_param()	/*[100]^^rule_param:	ident							;*/
        {
            return TreeNT((int)EPegGrammar.rule_param, () => ident());
        }
		public bool into_variable()	/*[43]^^into_variable:	ident							;*/
		{
			return TreeNT((int)EPegGrammar.into_variable,()=> ident());
		}
		public bool ident()	        /*[44] ident:		[A-Za-z][A-Za-z_0-9]*					;*/
		{
			return
				And(()=>
					In('A','Z', 'a','z')
				&&	OptRepeat(()=>In('A','Z', 'a','z', '0','9')||Char('_'))
				);
		}
		public bool elaborated_rule_id()	/*[45] elaborated_rule_id:(![A-Za-z0-9/_\0] .)* rule_id (![A-Za-z0-9/_\0] .)*	;*/
		{
			return
				And(()=>
					OptRepeat(()=>
                        And(()=>
				            Not(()=>In('A','Z', 'a','z', '0','9')||OneOf('/','_','^'))
			            &&	Any()
			            ))
				&&	rule_id()
				&&	OptRepeat(()=>
                        And(()=>
					        Not(()=>In('A','Z', 'a','z', '0','9')||OneOf('/','_','^'))
				        &&	Any()))
				);
		}
		public bool rule_id()	/*[46] ^^rule_id:		integer							;*/
		{
			return TreeNT((int)EPegGrammar.rule_id,()=> integer());
		}
        public bool fatal()
        {
            return TreeNT((int)EPegGrammar.fatal, ()=>Char('F', 'A', 'T', 'A', 'L'));
        }
        public bool warning()
        {
            return TreeNT((int)EPegGrammar.warning, () => Char('W', 'A', 'R', 'N', 'I', 'N', 'G'));
        }
		public bool message()	/*[47]message:		'FATAL'  S @'<' S fatal_args  S @'>' S	/
			'WARNING'S @'<' S fatal_args  S @'>' S			;*/
		{
			return TreeNT((int)EPegGrammar.message,()=>
					And(()=>
					    (fatal()||warning())
				    &&	S()
				    &&	( Char('<') ||	Fatal("<<'<'>> expected"))
				    &&	S()
				    &&	fatal_args()
				    &&	S()
				    &&	( Char('>') ||	Fatal("<<'>'>> expected") )
				    &&	S()
				    )
				  );
		}
		public bool throw_args()	/*[48] throw_args:	rule_name / FATAL<"rule name expected">   		;*/
		{
			return  rule_name() ||	Fatal("rule name expected");
		}
		public bool fatal_args()	/*[49] fatal_args:	multiline_double_quote_literal / enumerator /
			FATAL<"one of << \"<string>\" or enumeration >> expected">;*/
		{
			return
					multiline_double_quote_literal()
				||	enumerator()
				||	Fatal("one of << \"<string>\" or enumeration >> expected");
        }
        public bool multiline_double_quote_literal()	/*[50]^^multiline_double_quote_literal:
			double_quote_literal  (S double_quote_literal)*		;*/
		{
			return TreeNT(QuoteNodeCreator,(int)EPegGrammar.multiline_double_quote_literal,()=>
				And(()=>
					double_quote_literal()
				&&	OptRepeat(()=>And(()=> S() &&	double_quote_literal() ))
				));
		}
		public bool enumerator()	/*[51]^^enumerator:	ident							;*/
		{
			return TreeNT((int)EPegGrammar.enumerator,()=> ident());
		}
		public bool double_quote_literal()	/*[52] double_quote_literal:
                        '"'  dbl_quoted_content  @'"'				;*/
		{
			return
				And(()=>
					Char('"')
				&&	dbl_quoted_content()
				&&	( Char('"') ||	Fatal("<<'\"'>> expected") )
				);
		}
		public bool dbl_quoted_content()	/*[53] ^^dbl_quoted_content:	(!'"' checked_in_quote_char)*				;*/
		{
			return TreeNT((int)EPegGrammar.dbl_quoted_content,()=>
				OptRepeat(()=>
                    And(()=>
					    Not(()=>Char('"'))
				    &&	checked_in_quote_char()
				)));
		}
		public bool checked_in_quote_char()	/*[54] checked_in_quote_char:	
                        in_quote_char / 
			![\t\r\n\0] FATAL<"printable character or escape expected">;*/
		{
			return
					in_quote_char()
				||	And(()=>
					    Not(()=>OneOf('\t','\r','\n','\0'))
				    &&	Fatal("printable character or escape expected")
				);
		}
		public bool in_quote_char()	/*[55]  in_quote_char:	escape_sequence / printable_char			;*/
		{
			return  escape_sequence() ||	printable_char();
		}
		public bool code_point()	/*[56]^^code_point:	'#'([xX] hex_number / [bB] binary_number / decimal_number);*/
		{
			return TreeNT((int)EPegGrammar.code_point,()=>
				And(()=>
					Char('#')
				&&	(   And(()=>OneOf('x','X') &&	hex_number() )
				    ||	And(()=>OneOf('b','B') &&	binary_number())
				    ||	decimal_number()
					)
				));
		}
		public bool hex_number()	/*[57]  hex_number:	hexadecimal_digits / '(' hexadecimal_digits @')' /
					FATAL<"hexadecimal digit expected">	;*/
		{
			return
					hexadecimal_digits()
				||	And(()=>
					    Char('(')
				    &&	hexadecimal_digits()
				    &&	(
					    Char(')')
				    ||	Fatal("<<')'>> expected")
					    )
				    )
				||	Fatal("hexadecimal digit expected");
		}
		public bool binary_number()	/*[58]  binary_number:	binary_digits / '(' binary_digits @')' /
			FATAL<"binary digit expected">				;*/
		{
			return
				
					binary_digits()
				||	And(()=>
					    Char('(')
				    &&	binary_digits()
				    &&	(
					    Char(')')
				    ||	Fatal("<<')'>> expected")
					    )
				    )
				||	Fatal("binary digit expected");
		}
		public bool decimal_number()	/*[59] decimal_number:	decimal_digits / '(' decimal_digits @')' 		;*/
		{
			return
					decimal_digits()
				||	And(()=>
					    Char('(')
				    &&	decimal_digits()
				    &&	( Char(')') ||	Fatal("<<')'>> expected") )
				);
		}
		public bool hexadecimal_digits()	/*[60]^^hexadecimal_digits: [0-9a-fA-F]+	;*/
		{
			return TreeNT((int)EPegGrammar.hexadecimal_digits,()=>
				PlusRepeat(()=>In('0','9', 'a','f', 'A','F')));
		}
		public bool binary_digits()	/*[61]^^binary_digits:	[01]+						;*/
		{
			return TreeNT((int)EPegGrammar.binary_digits,()=>
				PlusRepeat(()=>OneOf('0','1')));
		}
		public bool decimal_digits()	/*[62]^^decimal_digits:	[0-9]+					;*/
		{
			return TreeNT((int)EPegGrammar.decimal_digits,()=>
				PlusRepeat(()=>In('0','9')));
		}
		public bool escape_sequence()	/*[63]  escape_sequence:	'\\' (escape_char / escape_int) 			;*/
		{
			return
				And(()=>
					Char('\\')
				&&	( escape_char() ||	escape_int() )
				);
		}
		public bool escape_char()	/*[64]^^escape_char:	[nrvt\]\\'"#]				;*/
		{
			return TreeNT((int)EPegGrammar.escape_char,()=>OneOf("nrvt]\\'\"#"));
		}
		public bool escape_int()	/*[65]^^escape_int:	[0-9]+ 							;*/
		{
			return TreeNT((int)EPegGrammar.escape_int,()=>
				PlusRepeat(()=>In('0','7')));
		}
		public bool checked_escape_sequence()	/*[66] checked_escape_sequence: 
                        escape_sequence / FATAL<"illegal escape sequence">	;*/
		{
			return  escape_sequence() ||	Fatal("illegal escape sequence");
		}
		public bool peek_symbol()	/*[67]^^peek_symbol:	'&'							;*/
		{
			return TreeNT((int)EPegGrammar.peek_symbol,()=> Char('&'));
		}
		public bool not_symbol()	/*[68]^^not_symbol:	'!'							;*/
		{
			return TreeNT((int)EPegGrammar.not_symbol,()=> Char('!'));
		}
		public bool option_symbol()	/*[69]^^option_symbol:	'?'							;*/
		{
            return TreeNT(TRepetitionCreator, (int)EPegGrammar.repetition_range,
                        () => optional_symbol());
		}
        public bool optional_symbol()
        {
            return TreeNT((int)EPegGrammar.option_symbol, () => Char('?'));
        }
		public bool repetition_symbol()	/*[70]  repetition_symbol:star / plus						;*/
		{
			return  TreeNT(TRepetitionCreator,(int)EPegGrammar.repetition_range,()=>star() ||	plus());
		}
		public bool star()	/*[71]^^star:		'*'							;*/
		{
			return TreeNT((int)EPegGrammar.star,()=>Char('*'));
		}
		public bool plus()	/*[72]^^plus:		'+'							;*/
		{
			return TreeNT((int)EPegGrammar.plus,()=> Char('+'));
		}
		public bool repetition_range()	/*[73]^^repetition_range:	'{' S checked_range_spec S @'}'				;*/
		{
			return TreeNT(TRepetitionCreator,(int)EPegGrammar.repetition_range,()=>
				And(()=>
					Char('{')
				&&	S()
				&&	checked_range_spec()
				&&	S()
				&&	( Char('}') ||	Fatal("<<'}'>> expected") )
				));
		}
		public bool checked_range_spec()	/*[74] checked_range_spec:range_lower_limit / range_upper_limit /
			                FATAL<"illegal character range">	;*/
		{
			return
				
					range_lower_limit()
				||	range_upper_limit()
				||	Fatal("illegal character range");
		}
		public bool range_lower_limit()	/*[75]  range_lower_limit:lower_limit S (',' S optional_upper_limit)? 		;*/
		{
			return
				And(()=>
					lower_limit()
				&&	S()
				&&	Option(()=>And(()=> Char(',') &&	S() &&	optional_upper_limit()))
				);
		}
		public bool range_upper_limit()	/*[76]  range_upper_limit:',' S upper_limit		    ;*/

		{
			return And(()=> Char(',') &&	S() &&	upper_limit() );
		}
		public bool optional_upper_limit()	/*[77]  optional_upper_limit:	upper_limit?    ;*/
		{
			return TreeNT((int)EPegGrammar.optional_upper_limit,()=>Option(()=>upper_limit()));
		}
		public bool mandatory_symbol()	/*[78] ^mandatory_symbol: '@'						;*/

		{
			return TreeAST((int)EPegGrammar.mandatory_symbol,()=> Char('@'));
		}
		public bool set_negation()	/*[79] ^set_negation:	'^'							    ;*/

		{
			return TreeAST((int)EPegGrammar.set_negation,()=> Char('^'));
		}
		public bool ast_symbol()	/*[80] ^ast_symbol:	'^'							        ;*/
		{
			return TreeAST((int)EPegGrammar.ast_symbol,()=> Char('^'));
		}
		public bool tree_symbol()	/*[81] ^tree_symbol:	'^^'							;*/
		{
			return TreeAST((int)EPegGrammar.tree_symbol,()=> Char('^','^'));
		}
		public bool lower_limit()	/*[82]^^lower_limit:	limit_spec						;*/
		{
			return TreeNT((int)EPegGrammar.lower_limit,()=> limit_spec());
		}
		public bool upper_limit()	/*[83]^^upper_limit:	limit_spec						;*/
		{
			return TreeNT((int)EPegGrammar.upper_limit,()=> limit_spec());
		}
		public bool limit_spec()	/*[84]  limit_spec:	numeric_limit / into				;*/
		{
			return  numeric_limit() ||	into();
		}
		public bool numeric_limit()	/*[85]^^numeric_limit:	integer							;*/
		{
			return TreeNT((int)EPegGrammar.numeric_limit,()=> integer());
		}
		public bool integer()	/*[86] integer:		[0-9]+							        ;*/
		{
			return PlusRepeat(()=>In('0','9'));
		}
		public bool enumeration_definition()	
            /*[87]^^enumeration_definition: enumerator S '='  S 
			                                ( 	multiline_double_quote_literal  / 
				                                FATAL<"\"<string>\" expected">
			                                )	
			                                S enumeration_terminator S				    ;*/
		{
			return TreeNT((int)EPegGrammar.enumeration_definition,()=>
				And(()=>
					enumerator()
				&&	S()
				&&	Char('=')
				&&	S()
				&&	(multiline_double_quote_literal() ||	Fatal("\"<string>\" expected") )
				&&	enumeration_terminator()
				&&	S()
				));
		}
		public bool S()	/*[88] S:			[ \t\r\n\v]* (comment S)? 				;*/
		{
			return
				And(()=>
					OptRepeat(()=>OneOf(' ','\t','\r','\n','\v'))
				&&	Option(()=>And(()=> comment() &&	S() ))
				);
		}
		public bool S_n()	/*[89] S_n:			[ \t\r\v]*    comment? 				;*/
		{
			return
				And(()=>
					OptRepeat(()=>OneOf(' ','\t','\r','\v'))
				&&	Option(()=>comment())
				);
		}
		public bool spaces()	/*[90] spaces:			[ \t\r\n\v]+ (comment S)? 	;*/
		{
			return
				And(()=>PlusRepeat(()=> OneOf(' ','\t','\r','\n','\v'))
				&&	Option(()=>And(()=> comment() && S() ))
				);
		}
		public bool comment()	/*[91] comment:		'//' (![\n] .)* &end_of_line_char;*/
		{
			return
				And(()=>
					Char('/','/')
				&&	OptRepeat(()=>And(()=>Not(()=>Char('\n')) &&	Any() ))
				&&	Peek(()=>end_of_line_char())
				);
		}
		public bool end_of_line_char()	/*[92] end_of_line_char:	'\n' / !.		;*/
		{
			return  Char('\n') ||	Not(()=>Any());
		}
		public bool enumeration_terminator()	/*[93] enumeration_terminator: 
                        S ';' / S_n '\n' / S peg_tail / FATAL<"one of << line break ; >> expected">;*/
		{
			return
					And(()=> S() &&	Char(';') )
				||	And(()=> S_n() &&	Char('\n') )
                ||  Peek(()=>peg_tail())
				||	Fatal("one of << line break ; >> expected");
		}
		public bool printable_char()	/*[94]^^printable_char:	[#x20-#x7E]			;*/
		{
			return TreeNT((int)EPegGrammar.printable_char,()=>
				In('\x20','\x7E'));
		}
        public bool create_spec()    /*[95]^^create_spec:      'CREATE' S '<' S create_method  S '>' S                 ;*/
        {

            return TreeNT((int)EPegGrammar.create_spec, () =>
                 And(() =>
                      Char('C', 'R', 'E', 'A', 'T', 'E')
                   && S()
                   && Char('<')
                   && S()
                   && create_method()
                   && S()
                   && Char('>')
                   && S()));
        }
        public bool create_method()    /*[96]^^create_method:    ident 							;*/
        {

            return TreeNT((int)EPegGrammar.create_method, () =>
                 ident());
        }

        #region semantic block Grammar Rules
        public bool semantic_block()    /*[210]semantic_block    :   named_semantic_block 
                        /   anonymous_semantic_block;*/
        {

            return named_semantic_block() || anonymous_semantic_block();
        }
        public bool named_semantic_block()    /*[211]^^named_semantic_block:
                        sem_block_name W anonymous_semantic_block ;*/
        {

            return TreeNT((int)EPegGrammar.named_semantic_block, () =>
                 And(() =>
                      sem_block_name()
                   && W()
                   && anonymous_semantic_block()));
        }
        public bool anonymous_semantic_block()    /*[212]^^anonymous_semantic_block:
			&'{' ( 
				'{'  semantic_block_content '}' W 
			     /  FATAL<"{ not followed by }">
			     )					    ;*/
        {

            return TreeNT((int)EPegGrammar.anonymous_semantic_block, () =>
                 And(() =>
                      Peek(() => Char('{'))
                   && (
                          And(() =>
                                Char('{')
                             && semantic_block_content()
                             && Char('}')
                             && W())
                       || Fatal("{ not followed by }"))));
        }
        public bool semantic_block_content()    /*[213]^^semantic_block_content:   
		       (   W
				(   into_declaration
				/   field_declaration
                                /   constructor_decl
				/   destructor_decl
				/   creator_func_declaration
				/   sem_func_declaration
				/   func_declaration
				/   code_declaration
				/   outer_ident
				/   quoted 
				/   '{'  @braced_content @'}' 
				/   !'}' .
				) 
			)* W					;*/
        {

            return TreeNT((int)EPegGrammar.semantic_block_content, () =>
                 And(() =>
                      OptRepeat(() =>
                       And(() =>
                                W()
                             && (
                                        into_declaration()
                                     || field_declaration()
                                     || constructor_decl()
                                     || destructor_decl()
                                     || creator_func_declaration()
                                     || sem_func_declaration()
                                     || func_declaration()
                                     || code_declaration()
                                     || outer_ident()
                                     || quoted()
                                     || And(() =>
                                                  Char('{')
                                               && (
                                                              braced_content()
                                                           || Fatal("<<braced_content>> expected"))
                                               && (Char('}') || Fatal("<<'}'>> expected")))
                                     || And(() => Not(() => Char('}')) && Any()))))
                   && W()));
        }
        public bool quoted()    /*[214]^^quoted:	        	['] ('\\' . / !['].)* @[']
			/	["] ('\\' . / !'"'.)* @["]	;*/
        {

            return TreeNT((int)EPegGrammar.quoted, () =>

                      And(() =>
                          OneOf("'")
                       && OptRepeat(() =>

                                        And(() => Char('\\') && Any())
                                     || And(() => Not(() => OneOf("'")) && Any()))
                       && (OneOf("'") || Fatal("<<[']>> expected")))
                   || And(() =>
                          OneOf("\"")
                       && OptRepeat(() =>

                                        And(() => Char('\\') && Any())
                                     || And(() => Not(() => Char('"')) && Any()))
                       && (OneOf("\"") || Fatal("<<[\"]>> expected"))));
        }
        public bool sem_comment()    /*[221] sem_comment:        single_line_comment 
			/ multi_line_comment
			/ directive				;*/
        {

            return
                      single_line_comment()
                   || multi_line_comment()
                   || directive();
        }
        public bool W()    /*[223] W:			([ \t\r\n\v] / sem_comment)* 	;*/
        {

            return OptRepeat(() => OneOf(" \t\r\n\v") || sem_comment());
        }
        public bool B()    /*[224] B:			![A-Za-z_0-9]			;*/
        {

            return Not(() => (In('A', 'Z', 'a', 'z', '0', '9') || OneOf("_")));
        }
        public bool sem_ident()    /*[228] sem_ident:	'@'? [A-Za-z_][A-Za-z_0-9]*		;*/
        {

            return And(() =>
                      Option(() => Char('@'))
                   && (In('A', 'Z', 'a', 'z') || OneOf("_"))
                   && OptRepeat(() =>
                       (In('A', 'Z', 'a', 'z', '0', '9') || OneOf("_"))));
        }
        public bool into_declaration()    /*[229]^^into_declaration: field_modifier* into_typedecl   
				variable_declarators	';' W 	;*/
        {

            return TreeNT((int)EPegGrammar.into_declaration, () =>
                 And(() =>
                      OptRepeat(() => field_modifier())
                   && into_typedecl()
                   && variable_declarators()
                   && Char(';')
                   && W()));
        }
        public bool into_typedecl()    /*[230]  into_typedecl:	into_type B W				;*/
        {

            return And(() => into_type() && B() && W());
        }
        public bool into_type()    /*[231]^^into_type:       'int' / 'byte' / 'string' 
		      / 'PegBegEnd' / 'double'			;*/
        {

            return TreeNT((int)EPegGrammar.into_type, () =>

                      Char('i', 'n', 't')
                   || Char('b', 'y', 't', 'e')
                   || Char('s', 't', 'r', 'i', 'n', 'g')
                   || Char("PegBegEnd")
                   || Char('d', 'o', 'u', 'b', 'l', 'e'));
        }
        public bool variable_declarators()    /*[232]^^variable_declarators:
			variable_declarator 
			(',' W   variable_declarator)*		;*/
        {

            return TreeNT((int)EPegGrammar.variable_declarators, () =>
                 And(() =>
                      variable_declarator()
                   && OptRepeat(() =>
                       And(() =>
                                Char(',')
                             && W()
                             && variable_declarator()))));
        }
        public bool variable_declarator()    /*[233]variable_declarator: 		
			variable W ('=' W   variable_initializer)? ;*/
        {

            return And(() =>
                      variable()
                   && W()
                   && Option(() =>
                       And(() =>
                                Char('=')
                             && W()
                             && variable_initializer())));
        }
        public bool variable_initializer()    /*[234]variable_initializer:
			( W
				( quoted 
				/ '{' W  braced_content  '}' 
				/ ![;,] .
				) 
			)*	W				;*/
        {

            return And(() =>
                      OptRepeat(() =>
                       And(() =>
                                W()
                             && (
                                        quoted()
                                     || And(() =>
                                                  Char('{')
                                               && W()
                                               && braced_content()
                                               && Char('}'))
                                     || And(() => Not(() => OneOf(";,")) && Any()))))
                   && W());
        }
        public bool variable()    /*[235]^^variable: 	sem_ident				;*/
        {

            return TreeNT((int)EPegGrammar.variable, () => sem_ident());
        }
        public bool outer_ident()    /*[236]^^outer_ident:	sem_ident				;*/
        {

            return TreeNT((int)EPegGrammar.outer_ident, () =>
                 sem_ident());
        }
        public bool sem_func_declaration()    /*[237]^^sem_func_declaration:
			sem_func_header   method_body		;*/
        {

            return TreeNT((int)EPegGrammar.sem_func_declaration, () =>
                 And(() => sem_func_header() && method_body()));
        }
        public bool creator_func_declaration()    /*[238]^^creator_func_declaration:
			creator_func_header method_body		;*/
        {

            return TreeNT((int)EPegGrammar.creator_func_declaration, () =>
                 And(() => creator_func_header() && method_body()));
        }
        public bool sem_func_header()    /*[239]^^sem_func_header:	method_modifier* 
                        'bool' B W    member_name W  formal_pars;*/
        {

            return TreeNT((int)EPegGrammar.sem_func_header, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && Char('b', 'o', 'o', 'l')
                   && B()
                   && W()
                   && member_name()
                   && W()
                   && formal_pars()));
        }
        public bool creator_func_header()    /*[240]^^creator_func_header: 
                        method_modifier*
                        'PegNode' B W member_name W 
                        creator_formal_pars                     ;*/
        {

            return TreeNT((int)EPegGrammar.creator_func_header, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && Char('P', 'e', 'g', 'N', 'o', 'd', 'e')
                   && B()
                   && W()
                   && member_name()
                   && W()
                   && creator_formal_pars()));
        }
        public bool creator_formal_pars()    /*[241]^^creator_formal_pars:'(' W creator_params ')' W            ;*/
        {

            return TreeNT((int)EPegGrammar.creator_formal_pars, () =>
                 And(() =>
                      Char('(')
                   && W()
                   && creator_params()
                   && Char(')')
                   && W()));
        }
        public bool creator_params()    /*[242]^^creator_params:   'ECreatorPhase' B W sem_ident W ',' W 
                        'PegNode' B W sem_ident W ',' W  
                        'int' B W sem_ident                         ;*/
        {

            return TreeNT((int)EPegGrammar.creator_params, () =>
                 And(() =>
                      Char("ECreatorPhase")
                   && B()
                   && W()
                   && sem_ident()
                   && W()
                   && Char(',')
                   && W()
                   && Char('P', 'e', 'g', 'N', 'o', 'd', 'e')
                   && B()
                   && W()
                   && sem_ident()
                   && W()
                   && Char(',')
                   && W()
                   && Char('i', 'n', 't')
                   && B()
                   && W()
                   && sem_ident()));
        }
        public bool func_declaration()    /*[243]^^func_declaration: func_header method_body                 ;*/
        {

            return TreeNT((int)EPegGrammar.func_declaration, () =>
                 And(() => func_header() && method_body()));
        }
        public bool func_header()    /*[244]^^func_header:	method_modifier* return_type   member_name W  formal_pars;*/
        {

            return TreeNT((int)EPegGrammar.func_header, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && return_type()
                   && member_name()
                   && W()
                   && formal_pars()));
        }
        public bool return_type()    /*[245]^^return_type:	sem_ident B W 				;*/
        {

            return TreeNT((int)EPegGrammar.return_type, () =>
                 And(() => sem_ident() && B() && W()));
        }
        public bool member_name()    /*[246]^^member_name:	sem_ident 					;*/
        {

            return TreeNT((int)EPegGrammar.member_name, () =>
                 sem_ident());
        }
        public bool method_body()    /*[248]^^method_body:	'{' W  braced_content?   '}' W 	;*/
        {

            return TreeNT((int)EPegGrammar.method_body, () =>
                 And(() =>
                      Char('{')
                   && W()
                   && Option(() => braced_content())
                   && Char('}')
                   && W()));
        }
        public bool braced_content()    /*[249]^^braced_content:	( W 
				(  quoted 
				/ '{' @braced_content  @'}'
				/ designator
			        /  !'}' .
			        ) 
			)* W					;*/
        {

            return TreeNT((int)EPegGrammar.braced_content, () =>
                 And(() =>
                      OptRepeat(() =>
                       And(() =>
                                W()
                             && (
                                        quoted()
                                     || And(() =>
                                                  Char('{')
                                               && (
                                                              braced_content()
                                                           || Fatal("<<braced_content>> expected"))
                                               && (Char('}') || Fatal("<<'}'>> expected")))
                                     || designator()
                                     || And(() => Not(() => Char('}')) && Any()))))
                   && W()));
        }
        public bool paren_content()    /*[250]^^paren_content:    ( W 
				(  quoted 
				/ '(' @paren_content @')'
				/ designator
			        /  !')' .
			        ) 
			)* W					;*/
        {

            return TreeNT((int)EPegGrammar.paren_content, () =>
                 And(() =>
                      OptRepeat(() =>
                       And(() =>
                                W()
                             && (
                                        quoted()
                                     || And(() =>
                                                  Char('(')
                                               && (
                                                              paren_content()
                                                           || Fatal("<<paren_content>> expected"))
                                               && (Char(')') || Fatal("<<')'>> expected")))
                                     || designator()
                                     || And(() => Not(() => Char(')')) && Any()))))
                   && W()));
        }
        public bool single_line_comment()    /*[251]single_line_comment:'//' (![\n] .)* &end_of_line_char      ;*/
        {

            return And(() =>
                      Char('/', '/')
                   && OptRepeat(() =>
                       And(() => Not(() => OneOf("\n")) && Any()))
                   && Peek(() => end_of_line_char()));
        }
        public bool directive()    /*[252]directive:		'#' (![\n] .)* &end_of_line_char        ;*/
        {

            return And(() =>
                      Char('#')
                   && OptRepeat(() =>
                       And(() => Not(() => OneOf("\n")) && Any()))
                   && Peek(() => end_of_line_char()));
        }
        public bool multi_line_comment()    /*[253]multi_line_comment: '/*' (!'* /' .)* '* /'                   ;*/
        {

            return And(() =>
                      Char('/', '*')
                   && OptRepeat(() =>
                       And(() => Not(() => Char('*', '/')) && Any()))
                   && Char('*', '/'));
        }
        public bool sem_block_name()    /*[254]^^sem_block_name:  sem_ident                              ;*/
        {

            return TreeNT((int)EPegGrammar.sem_block_name, () =>
                 sem_ident());
        }
        public bool creator_name()    /*[255]^^creator_name:    sem_ident                              ;*/
        {

            return TreeNT((int)EPegGrammar.creator_name, () =>
                 sem_ident());
        }
        public bool formal_pars()    /*[256]^^formal_pars:	'(' W (sem_param<')'> (',' W sem_param<')'>)*)? @')' W   ;*/
        {

            return TreeNT((int)EPegGrammar.formal_pars, () =>
                 And(() =>
                      Char('(')
                   && W()
                   && Option(() =>
                       And(() =>
                                sem_param(() => Char(')'))
                             && OptRepeat(() =>
                                     And(() =>
                                                  Char(',')
                                               && W()
                                               && sem_param(() => Char(')'))))))
                   && (Char(')') || Fatal("<<')'>> expected"))
                   && W()));
        }
        public bool sem_param(Matcher Terminator)    /*[257]^^sem_param<Terminator>:   (param_ident / parenthized / (!Terminator [^,({}a-zA-Z_])+)+;*/
        {

            return TreeNT((int)EPegGrammar.sem_param, () =>
                 PlusRepeat(() =>

                          param_ident()
                       || parenthized()
                       || PlusRepeat(() =>
                             And(() =>
                                        Not(() => Terminator())
                                     && (NotIn("azAZ") || NotOneOf(",({}_"))))));
        }
        public bool param_ident()    /*[258]^^param_ident:     sem_ident;*/
        {

            return TreeNT((int)EPegGrammar.param_ident, () =>
                 sem_ident());
        }
        public bool parenthized()    /*[259]^^parenthized:        '{' W  braced_content?   '}' W 
                        /  '(' paren_content @')' W ;*/
        {

            return TreeNT((int)EPegGrammar.parenthized, () =>

                      And(() =>
                          Char('{')
                       && W()
                       && Option(() => braced_content())
                       && Char('}')
                       && W())
                   || And(() =>
                          Char('(')
                       && paren_content()
                       && (Char(')') || Fatal("<<')'>> expected"))
                       && W()));
        }
        public bool method_modifier()    /*[260]^^method_modifier:	('new' / 'public' / 'protected' / 'internal' / 
			 'private' / 'static' / 'virtual' / 
		         'sealed' / 'override' / 'abstract' / 'extern' / 'unsafe' ) B W;*/
        {

            return TreeNT((int)EPegGrammar.method_modifier, () =>
                 And(() =>
                      OneOfLiterals(optimizedLiterals0)
                   && B()
                   && W()));
        }
        public bool constructor_decl()    /*[261]^^constructor_decl: constructor_header method_body;*/
        {

            return TreeNT((int)EPegGrammar.constructor_decl, () =>
                 And(() => constructor_header() && method_body()));
        }
        public bool constructor_header()    /*[262]^^constructor_header: method_modifier* member_name  W formal_pars   constructor_initializer?;*/
        {

            return TreeNT((int)EPegGrammar.constructor_header, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && member_name()
                   && W()
                   && formal_pars()
                   && Option(() => constructor_initializer())));
        }
        public bool constructor_initializer()    /*[263]^^constructor_initializer:':' W   (^'base'/^'this') W   formal_pars;*/
        {

            return TreeNT((int)EPegGrammar.constructor_initializer, () =>
                 And(() =>
                      Char(':')
                   && W()
                   && (
                          TreeChars(() => Char('b', 'a', 's', 'e'))
                       || TreeChars(() => Char('t', 'h', 'i', 's')))
                   && W()
                   && formal_pars()));
        }
        public bool designator()    /*[264]^^designator:      desig_ident W (sem_mem_access/index_access/invocation)*;*/
        {

            return TreeNT((int)EPegGrammar.designator, () =>
                 And(() =>
                      desig_ident()
                   && W()
                   && OptRepeat(() =>
                           sem_mem_access() || index_access() || invocation())));
        }
        public bool desig_ident()    /*[265]^^desig_ident:     sem_ident;*/
        {

            return TreeNT((int)EPegGrammar.desig_ident, () =>
                 sem_ident());
        }
        public bool sem_mem_access()    /*[266]^^sem_mem_access:  '.' W desig_ident W;*/
        {

            return TreeNT((int)EPegGrammar.sem_mem_access, () =>
                 And(() => Char('.') && W() && desig_ident() && W()));
        }
        public bool index_access()    /*[267]^^index_access:    '[' W  @index_content    W ']'  W;*/
        {

            return TreeNT((int)EPegGrammar.index_access, () =>
                 And(() =>
                      Char('[')
                   && W()
                   && (
                          index_content()
                       || Fatal("<<index_content>> expected"))
                   && W()
                   && Char(']')
                   && W()));
        }
        public bool index_content()    /*[268]^^index_content:   ( W (quoted/'['@index_content@']'/ designator / !']'.) )* W;*/
        {

            return TreeNT((int)EPegGrammar.index_content, () =>
                 And(() =>
                      OptRepeat(() =>
                       And(() =>
                                W()
                             && (
                                        quoted()
                                     || And(() =>
                                                  Char('[')
                                               && (
                                                              index_content()
                                                           || Fatal("<<index_content>> expected"))
                                               && (Char(']') || Fatal("<<']'>> expected")))
                                     || designator()
                                     || And(() => Not(() => Char(']')) && Any()))))
                   && W()));
        }
        public bool invocation()    /*[269]^^invocation:      '(' W  paren_content  @')' W;*/
        {

            return TreeNT((int)EPegGrammar.invocation, () =>
                 And(() =>
                      Char('(')
                   && W()
                   && paren_content()
                   && (Char(')') || Fatal("<<')'>> expected"))
                   && W()));
        }
        public bool field_modifier()    /*[270]^^field_modifier:  ('new' / 'public' / 'protected' / 'internal' / 
                         'private' / 'static' / 'readonly' / 'volatile' / 'unsafe') B W;*/
        {

            return TreeNT((int)EPegGrammar.field_modifier, () =>
                 And(() =>
                      OneOfLiterals(optimizedLiterals1)
                   && B()
                   && W()));
        }
        public bool field_declaration()    /*[271]^^field_declaration:field_modifier* type_ref   variable_declarators ';' W 	;*/
        {

            return TreeNT((int)EPegGrammar.field_declaration, () =>
                 And(() =>
                      OptRepeat(() => field_modifier())
                   && type_ref()
                   && variable_declarators()
                   && Char(';')
                   && W()));
        }
        public bool destructor_decl()    /*[272]^^destructor_decl: method_modifier* '~' W member_name W  '(' W  ')' W  ;*/
        {

            return TreeNT((int)EPegGrammar.destructor_decl, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && Char('~')
                   && W()
                   && member_name()
                   && W()
                   && Char('(')
                   && W()
                   && Char(')')
                   && W())
                   && method_body());
        }
        public bool code_declaration()    /*[273]^^code_declaration: method_modifier* (operator_decl/indexer_decl/prop_event_decl);*/
        {

            return TreeNT((int)EPegGrammar.code_declaration, () =>
                 And(() =>
                      OptRepeat(() => method_modifier())
                   && (
                          operator_decl()
                       || indexer_decl()
                       || prop_event_decl())));
        }
        public bool type_ref()    /*[274]^^type_ref:        sem_ident W rank_specifiers?;*/
        {

            return TreeNT((int)EPegGrammar.type_ref, () =>
                 And(() =>
                      sem_ident()
                   && W()
                   && Option(() => rank_specifiers())));
        }
        public bool rank_specifiers()    /*[275]rank_specifiers: 	rank_specifier+;*/
        {

            return PlusRepeat(() => rank_specifier());
        }
        public bool rank_specifier()    /*[276]rank_specifier: 	'[' W   dim_separators?   ']' W;*/
        {

            return And(() =>
                      Char('[')
                   && W()
                   && Option(() => dim_separators())
                   && Char(']')
                   && W());
        }
        public bool dim_separators()    /*[277]dim_separators: 	(',' W)+;*/
        {

            return PlusRepeat(() => And(() => Char(',') && W()));
        }
        public bool operator_decl()    /*[278]operator_decl:     operator_declarator method_body;*/
        {

            return And(() => operator_declarator() && method_body());
        }
        public bool prop_event_decl()    /*[279]prop_event_decl: ('event' B W)?  type_ref  member_name W  prop_ind_event_block;*/
        {

            return And(() =>
                      Option(() =>
                       And(() => Char('e', 'v', 'e', 'n', 't') && B() && W()))
                   && type_ref()
                   && member_name()
                   && W()
                   && prop_ind_event_block());
        }
        public bool indexer_decl()    /*[280]indexer_decl:	(type_ref   '.' W)? 'this' W    '[' W  formal_pars   @']' W prop_ind_event_block;*/
        {

            return And(() =>
                      Option(() =>
                       And(() => type_ref() && Char('.') && W()))
                   && Char('t', 'h', 'i', 's')
                   && W()
                   && Char('[')
                   && W()
                   && formal_pars()
                   && (Char(']') || Fatal("<<']'>> expected"))
                   && W()
                   && prop_ind_event_block());
        }
        public bool prop_ind_event_block()    /*[281]prop_ind_event_block:
                        '{' W  accessor_declaration+   @'}' W;*/
        {

            return And(() =>
                      Char('{')
                   && W()
                   && PlusRepeat(() => accessor_declaration())
                   && (Char('}') || Fatal("<<'}'>> expected"))
                   && W());
        }
        public bool accessor_declaration()    /*[282]accessor_declaration: method_modifier* ('get'/'set'/'add'/'remove') W  method_body;*/
        {

            return And(() =>
                      OptRepeat(() => method_modifier())
                   && (
                          Char('g', 'e', 't')
                       || Char('s', 'e', 't')
                       || Char('a', 'd', 'd')
                       || Char('r', 'e', 'm', 'o', 'v', 'e'))
                   && W()
                   && method_body());
        }
        public bool operator_declarator()    /*[283]operator_declarator: type_ref (('implicit' / 'explicit' ) B W)?   
			'operator' W   operator W   '(' W   type_ref   sem_ident (',' W type_ref sem_ident)? W   ')' W;*/
        {

            return And(() =>
                      type_ref()
                   && Option(() =>
                       And(() =>
                                (Char("implicit") || Char("explicit"))
                             && B()
                             && W()))
                   && Char("operator")
                   && W()
                   && @operator()
                   && W()
                   && Char('(')
                   && W()
                   && type_ref()
                   && sem_ident()
                   && Option(() =>
                       And(() =>
                                Char(',')
                             && W()
                             && type_ref()
                             && sem_ident()))
                   && W()
                   && Char(')')
                   && W());
        }
        public bool @operator()    /*[284]operator:           '++' /  '--' / '+' /   '-' /    '!'  /   '~' /  'true' /   'false' /
                         '+' / '-' / '*' / '/' / '%' / '&' / '|' / '^' / '<<' / '>>' / '==' / '!=' / '>=' / '<=' / '>' / '<' /
			 type_ref;*/
        {

            return
                      Char('+', '+')
                   || Char('-', '-')
                   || Char('+')
                   || Char('-')
                   || Char('!')
                   || Char('~')
                   || Char('t', 'r', 'u', 'e')
                   || Char('f', 'a', 'l', 's', 'e')
                   || Char('+')
                   || Char('-')
                   || Char('*')
                   || Char('/')
                   || Char('%')
                   || Char('&')
                   || Char('|')
                   || Char('^')
                   || Char('<', '<')
                   || Char('>', '>')
                   || Char('=', '=')
                   || Char('!', '=')
                   || Char('>', '=')
                   || Char('<', '=')
                   || Char('>')
                   || Char('<')
                   || type_ref();
        }
        #endregion semantic block Grammar Rules
        #endregion Grammar Rules
        #region Optimization Data
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedLiterals optimizedLiterals0;
        internal static OptimizedLiterals optimizedLiterals1;
        static PegGrammarParser()
        {
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('a','z'),
                   new OptimizedCharset.Range('A','Z'),
                   };
               char[] oneOfChars = new char[]    {',',')','(','{','}'
                                                  ,'_'};
               optimizedCharset0= new OptimizedCharset(ranges,oneOfChars, true);
            }
             
            {
               string[] literals=
               { "new","public","protected","internal","private","static","virtual","sealed",
                  "override","abstract","extern","unsafe" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","static","readonly","volatile",
                  "unsafe" };
               optimizedLiterals1= new OptimizedLiterals(literals);
            }


        }
        #endregion  Optimization Data
    }
}
