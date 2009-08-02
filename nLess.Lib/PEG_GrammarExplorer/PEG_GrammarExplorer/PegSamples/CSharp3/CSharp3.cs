/* created on 21.09.2008 16:13:48 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace CSharp3
{
      
      enum ECSharp3{new_line= 1, comment= 2, single_line_comment= 3, input_characters= 4, 
                     input_character= 5, new_line_character= 6, delimited_comment= 7, 
                     whitespace= 8, unicode_escape_sequence= 9, identifier= 10, available_identifier= 11, 
                     identifier_or_keyword= 12, identifier_start_character= 13, identifier_part_characters= 14, 
                     identifier_part_character= 15, letter_character= 16, combining_character= 17, 
                     decimal_digit_character= 18, connecting_character= 19, formatting_character= 20, 
                     keyword= 21, literal= 22, boolean_literal= 23, integer_literal= 24, 
                     decimal_integer_literal= 25, decimal_digits= 26, decimal_digit= 27, 
                     integer_type_suffix= 28, hexadecimal_integer_literal= 29, hex_digits= 30, 
                     hex_digit= 31, real_literal= 32, fraction= 33, exponent_part= 34, 
                     sign= 35, real_type_suffix= 36, character_literal= 37, character= 38, 
                     single_character= 39, simple_escape_sequence= 40, hexadecimal_escape_sequence= 41, 
                     string_literal= 42, regular_string_literal= 43, regular_string_literal_characters= 44, 
                     regular_string_literal_character= 45, single_regular_string_literal_character= 46, 
                     verbatim_string_literal= 47, verbatim_string_literal_characters= 48, 
                     verbatim_string_literal_character= 49, single_verbatim_string_literal_character= 50, 
                     quote_escape_sequence= 51, null_literal= 52, operator_or_punctuator= 53, 
                     right_shift= 54, right_shift_assignment= 55, pp_directive= 56, 
                     conditional_symbol= 57, symbolName= 58, pp_expression= 59, pp_or_expression= 60, 
                     pp_and_expression= 61, pp_equality_expression= 62, pp_unary_expression= 63, 
                     pp_primary_expression= 64, pp_declaration= 65, pp_new_line= 66, 
                     pp_conditional= 67, pp_if_section= 68, pp_elif_section= 69, pp_else_section= 70, 
                     pp_endif= 71, pp_diagnostic= 72, pp_message= 73, pp_region= 74, 
                     pp_start_region= 75, pp_end_region= 76, pp_line= 77, line_indicator= 78, 
                     file_name= 79, file_name_characters= 80, file_name_character= 81, 
                     pp_pragma= 82, pragma_body= 83, pragma_warning_body= 84, warning_action= 85, 
                     warning_list= 86, namespace_name= 87, type_name= 88, namespace_or_type_name= 89, 
                     name= 90, type= 91, pointer_type= 92, pointers= 93, void_pointer= 94, 
                     value_type= 95, struct_type= 96, simple_type= 97, numeric_type= 98, 
                     integral_type= 99, floating_point_type= 100, is_nullable= 101, 
                     non_nullable_type= 102, enum_type= 103, non_array_reference_type= 104, 
                     class_type= 105, interface_type= 106, array_type= 107, non_array_non_nullable_type= 108, 
                     non_array_type= 109, rank_specifiers= 110, rank_specifier= 111, 
                     dim_separators= 112, delegate_type= 113, type_argument_list= 114, 
                     type_arguments= 115, type_argument= 116, type_parameter= 117, 
                     variable_reference= 118, argument_list= 119, argument= 120, postfix_expression= 121, 
                     postfix_operation= 122, invocation= 123, member_access= 124, 
                     pointer_member_access= 125, element_access= 126, index= 127, 
                     post_incr= 128, post_decr= 129, primary_expression= 130, sizeof_expression= 131, 
                     simple_name= 132, parenthesized_expression= 133, special_member_access= 134, 
                     predefined_type= 135, expression_list= 136, this_access= 137, 
                     base_access= 138, post_increment_expression= 139, post_decrement_expression= 140, 
                     object_creation_expression= 141, object_or_collection_initializer= 142, 
                     object_initializer= 143, member_initializer_list= 144, member_initializer= 145, 
                     initializer_value= 146, collection_initializer= 147, element_initializer_list= 148, 
                     element_initializer= 149, initial_value_list= 150, array_creation_expression= 151, 
                     array_size= 152, delegate_creation_expression= 153, anonymous_object_creation_expression= 154, 
                     anonymous_object_initializer= 155, member_declarator_list= 156, 
                     member_declarator= 157, full_member_access= 158, typeof_expression= 159, 
                     unbound_type_name= 160, generic_dimension_specifier= 161, commas= 162, 
                     checked_expression= 163, unchecked_expression= 164, default_value_expression= 165, 
                     unary_expression= 166, creation_expression= 167, pre_increment_expression= 168, 
                     pre_decrement_expression= 169, cast_expression= 170, multiplicative_expression= 171, 
                     additive_expression= 172, shift_expression= 173, relational_expression= 174, 
                     equality_expression= 175, and_expression= 176, exclusive_or_expression= 177, 
                     inclusive_or_expression= 178, conditional_and_expression= 179, 
                     conditional_or_expression= 180, null_coalescing_expression= 181, 
                     conditional_expression= 182, if_else_expression= 183, lambda_expression= 184, 
                     anonymous_method_expression= 185, anonymous_function_signature= 186, 
                     explicit_anonymous_function_signature= 187, explicit_anonymous_function_parameter_list= 188, 
                     explicit_anonymous_function_parameter= 189, anonymous_function_parameter_modifier= 190, 
                     implicit_anonymous_function_signature= 191, implicit_anonymous_function_parameter_list= 192, 
                     implicit_anonymous_function_parameter= 193, anonymous_function_body= 194, 
                     query_expression= 195, from_clause= 196, query_body= 197, query_body_clauses= 198, 
                     query_body_clause= 199, let_clause= 200, where_clause= 201, join_into_clause= 202, 
                     orderby_clause= 203, orderings= 204, ordering= 205, ordering_direction= 206, 
                     select_or_group_clause= 207, select_clause= 208, group_clause= 209, 
                     query_continuation= 210, assignment= 211, assignment_operator= 212, 
                     expression= 213, non_assignment_expression= 214, constant_expression= 215, 
                     boolean_expression= 216, statement= 217, embedded_statement= 218, 
                     block= 219, statement_list= 220, empty_statement= 221, labeled_statement= 222, 
                     label= 223, declaration_statement= 224, local_variable_declaration= 225, 
                     local_variable_type= 226, local_variable_declarators= 227, local_variable_declarator= 228, 
                     variable_name= 229, local_variable_initializer= 230, stackalloc_initializer= 231, 
                     local_constant_declaration= 232, constant_declarators= 233, constant_declarator= 234, 
                     constant_name= 235, expression_statement= 236, unsafe_statement= 237, 
                     fixed_statement= 238, fixed_pointer_declarators= 239, fixed_pointer_declarator= 240, 
                     fixed_pointer_initializer= 241, statement_expression= 242, call_or_post_incr_decr= 243, 
                     selection_statement= 244, if_statement= 245, switch_statement= 246, 
                     switch_block= 247, switch_sections= 248, switch_section= 249, 
                     switch_labels= 250, switch_label= 251, iteration_statement= 252, 
                     while_statement= 253, do_statement= 254, for_statement= 255, 
                     for_initializer= 256, for_condition= 257, for_iterator= 258, 
                     statement_expression_list= 259, foreach_statement= 260, jump_statement= 261, 
                     break_statement= 262, continue_statement= 263, goto_statement= 264, 
                     return_statement= 265, throw_statement= 266, try_statement= 267, 
                     catch_clauses= 268, specific_catch_clauses= 269, specific_catch_clause= 270, 
                     general_catch_clause= 271, finally_clause= 272, checked_statement= 273, 
                     unchecked_statement= 274, lock_statement= 275, using_statement= 276, 
                     resource_acquisition= 277, yield_statement= 278, compilation_unit= 279, 
                     namespace_declaration= 280, qualified_identifier= 281, namespace_body= 282, 
                     extern_alias_directives= 283, extern_alias_directive= 284, alias_name= 285, 
                     using_directives= 286, using_directive= 287, using_alias_directive= 288, 
                     using_alias_name= 289, using_namespace_directive= 290, namespace_member_declarations= 291, 
                     namespace_member_declaration= 292, type_declaration= 293, qualified_alias_member= 294, 
                     class_declaration= 295, class_name= 296, class_modifiers= 297, 
                     class_modifier= 298, type_parameter_list= 299, type_parameters= 300, 
                     class_base= 301, interface_type_list= 302, type_parameter_constraints_clauses= 303, 
                     type_parameter_constraints_clause= 304, type_parameter_constraints= 305, 
                     primary_constraint= 306, secondary_constraints= 307, constructor_constraint= 308, 
                     class_body= 309, class_member_declarations= 310, class_member_declaration= 311, 
                     constant_declaration= 312, constant_modifiers= 313, constant_modifier= 314, 
                     field_declaration= 315, field_modifiers= 316, field_modifier= 317, 
                     variable_declarators= 318, variable_declarator= 319, variable_initializer= 320, 
                     method_declaration= 321, method_header= 322, method_modifiers= 323, 
                     method_modifier= 324, return_type= 325, interface_name_before_member= 326, 
                     method_body= 327, missing_body= 328, formal_parameter_list= 329, 
                     fixed_parameters= 330, fixed_parameter= 331, parameter_modifier= 332, 
                     parameter_array= 333, property_declaration= 334, property_modifiers= 335, 
                     property_modifier= 336, member_name= 337, accessor_declarations= 338, 
                     get_accessor_declaration= 339, set_accessor_declaration= 340, 
                     accessor_modifier= 341, accessor_body= 342, event_declaration= 343, 
                     event_modifiers= 344, event_modifier= 345, event_accessor_declarations= 346, 
                     add_accessor_declaration= 347, remove_accessor_declaration= 348, 
                     indexer_declaration= 349, indexer_modifiers= 350, indexer_modifier= 351, 
                     indexer_declarator= 352, operator_declaration= 353, operator_modifiers= 354, 
                     operator_modifier= 355, operator_declarator= 356, unary_operator_declarator= 357, 
                     overloadable_unary_operator= 358, binary_operator_declarator= 359, 
                     overloadable_binary_operator= 360, conversion_operator_declarator= 361, 
                     operator_body= 362, constructor_declaration= 363, constructor_modifiers= 364, 
                     constructor_modifier= 365, constructor_declarator= 366, constructor_initializer= 367, 
                     constructor_body= 368, static_constructor_declaration= 369, static_constructor_modifiers= 370, 
                     static_constructor_body= 371, destructor_declaration= 372, destructor_body= 373, 
                     struct_declaration= 374, struct_name= 375, struct_modifiers= 376, 
                     struct_modifier= 377, struct_interfaces= 378, struct_body= 379, 
                     struct_member_declarations= 380, struct_member_declaration= 381, 
                     fixed_size_buffer_declaration= 382, fixed_size_buffer_modifiers= 383, 
                     fixed_size_buffer_modifier= 384, buffer_element_type= 385, fixed_size_buffer_declarators= 386, 
                     fixed_size_buffer_declarator= 387, array_initializer= 388, variable_initializer_list= 389, 
                     interface_declaration= 390, interface_name= 391, interface_modifiers= 392, 
                     interface_modifier= 393, interface_base= 394, interface_body= 395, 
                     interface_member_declarations= 396, interface_member_declaration= 397, 
                     interface_method_declaration= 398, interface_property_declaration= 399, 
                     interface_accessors= 400, interface_event_declaration= 401, interface_indexer_declaration= 402, 
                     enum_declaration= 403, enum_name= 404, enum_base= 405, enum_body= 406, 
                     enum_modifiers= 407, enum_modifier= 408, enum_member_declarations= 409, 
                     enum_member_declaration= 410, enumerator_name= 411, delegate_declaration= 412, 
                     delegate_name= 413, delegate_modifiers= 414, delegate_modifier= 415, 
                     global_attributes= 416, global_attribute_sections= 417, global_attribute_section= 418, 
                     global_attribute_target_specifier= 419, global_attribute_target= 420, 
                     attributes= 421, attribute_sections= 422, attribute_section= 423, 
                     attribute_target_specifier= 424, attribute_target= 425, attribute_list= 426, 
                     attribute= 427, attribute_name= 428, attribute_arguments= 429, 
                     positional_argument_list= 430, positional_argument= 431, named_argument_list= 432, 
                     named_argument= 433, parameter_name= 434, attribute_argument_expression= 435, 
                     B= 436, S= 437, Zs= 438, Lu= 439, Ll= 440, Lt= 441, Lm= 442, 
                     Lo= 443, Nl= 444, Mn= 445, Mc= 446, Nd= 447, Pc= 448, Cf= 449};
      class CSharp3 : PegCharParser 
      {
        class Top
{
	#region data members
        internal string unicode_;
	#endregion data members
        bool GetUnicodeCategory(out System.Globalization.UnicodeCategory cat)
        {
            uint val;
            cat= System.Globalization.UnicodeCategory.Control;
            System.Diagnostics.Debug.Assert(unicode_.Length>=5 );
            val = UInt32.Parse(unicode_.Substring(2), System.Globalization.NumberStyles.HexNumber);
            char uniCode= (char)val;
            cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(uniCode);
            return true;
        }
	internal bool UnicodeEscapeIsLetter_()
    {
        System.Globalization.UnicodeCategory cat;
        if (GetUnicodeCategory(out cat))
        {
            return cat == System.Globalization.UnicodeCategory.UppercaseLetter
                || cat == System.Globalization.UnicodeCategory.LowercaseLetter
                || cat == System.Globalization.UnicodeCategory.TitlecaseLetter
                || cat == System.Globalization.UnicodeCategory.ModifierLetter
                || cat == System.Globalization.UnicodeCategory.OtherLetter
                || cat == System.Globalization.UnicodeCategory.LetterNumber;
        }
        return false;
    }
	internal bool UnicodeEscapeIsNd_()
    {
        System.Globalization.UnicodeCategory cat;
        return GetUnicodeCategory(out cat) && cat == System.Globalization.UnicodeCategory.DecimalDigitNumber;
    }
    internal bool UnicodeEscapeIsMnOrMc_()
    {
        System.Globalization.UnicodeCategory cat;
        return GetUnicodeCategory(out cat) 
            && (   cat == System.Globalization.UnicodeCategory.NonSpacingMark
                || cat == System.Globalization.UnicodeCategory.SpacingCombiningMark);
    }
	internal bool UnicodeEscapeIsPc_()
    {
        System.Globalization.UnicodeCategory cat;
        return GetUnicodeCategory(out cat) 
            && cat == System.Globalization.UnicodeCategory.ConnectorPunctuation;
    }
	internal bool UnicodeEscapeIsCf_()
    {
        System.Globalization.UnicodeCategory cat;
        return GetUnicodeCategory(out cat)
            && cat == System.Globalization.UnicodeCategory.Format;
    }
}

//A.1.1 Line terminators
//----------------------
Top top;

         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.utf8;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public CSharp3()
            : base()
        {
            top= new Top();

        }
        public CSharp3(string src,TextWriter FerrOut)
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
                   ECSharp3 ruleEnum = (ECSharp3)id;
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
        public bool new_line()    /*new_line: '\r\n' /  [\r\n#x0085#x0085#x2028#x2029];
//	Carriage return character (U+000D) /
//	Line feed character (U+000A) /
//	Carriage return character (U+000D) followed by line feed character (U+000A) /
//	Next line character (U+0085) /
//	Line separator character (U+2028) /
//	Paragraph separator character (U+2029) /
//A.1.2 Comments
//----------------------*/
        {

           return   
                     Char('\r','\n')
                  || OneOf("\r\n\u0085\u0085\u2028\u2029");
		}
        public bool comment()    /*comment: 		single_line_comment / delimited_comment;*/
        {

           return     single_line_comment() || delimited_comment();
		}
        public bool single_line_comment()    /*single_line_comment: 	'//'   input_characters?;*/
        {

           return And(()=>  
                     Char('/','/')
                  && Option(()=> input_characters() ) );
		}
        public bool input_characters()    /*input_characters: 	input_character+;*/
        {

           return PlusRepeat(()=> input_character() );
		}
        public bool input_character()    /*input_character:  	[^#x000D#x000A#x0085#x2028#x2029]; //Any Unicode character except a new_line_character*/
        {

           return NotOneOf("\u000d\u000a\u0085\u2028\u2029");
		}
        public bool new_line_character()    /*new_line_character: 	[#x000D#x000A#x0085#x2028#x2029];
//Carriage return character (U+000D)
//Line feed character (U+000A)
//Next line character (U+0085)
//Line separator character (U+2028)
//Paragraph separator character (U+2029)*/
        {

           return OneOf("\u000d\u000a\u0085\u2028\u2029");
		}
        public bool delimited_comment()    /*delimited_comment:	'/*'  (!'* /' . )*   '* /';
//A.1.3 White space
//----------------------*/
        {

           return And(()=>  
                     Char('/','*')
                  && OptRepeat(()=>    
                      And(()=>    Not(()=> Char('*','/') ) && Any() ) )
                  && Char('*','/') );
		}
        public bool whitespace()    /*whitespace:  		Zs/#x0009/#x000B/#x000C;   
//Horizontal tab character (U+0009)
//Vertical tab character (U+000B)
//Form feed character (U+000C)

//A.1.5 Unicode character escape sequences
//----------------------*/
        {

           return   
                     Zs()
                  || Char('\u0009')
                  || Char('\u000b')
                  || Char('\u000c');
		}
        public bool unicode_escape_sequence()    /*unicode_escape_sequence:'\u'   hex_digit{4} / '\U'   hex_digit{8};
//A.1.6 Identifiers
//----------------------*/
        {

           return   
                     And(()=>    
                         Char('\\','u')
                      && ForRepeat(4,4,()=> hex_digit() ) )
                  || And(()=>    
                         Char('\\','U')
                      && ForRepeat(8,8,()=> hex_digit() ) );
		}
        public bool identifier()    /*identifier: 			 '@'   identifier_or_keyword / available_identifier;*/
        {

           return   
                     And(()=>    Char('@') && identifier_or_keyword() )
                  || available_identifier();
		}
        public bool available_identifier()    /*available_identifier:  		!(keyword B) identifier_or_keyword; //An identifier_or_keyword that is not a keyword*/
        {

           return And(()=>  
                     Not(()=> And(()=>    keyword() && B() ) )
                  && identifier_or_keyword() );
		}
        public bool identifier_or_keyword()    /*identifier_or_keyword: 		identifier_start_character   identifier_part_characters?;*/
        {

           return And(()=>  
                     identifier_start_character()
                  && Option(()=> identifier_part_characters() ) );
		}
        public bool identifier_start_character()    /*identifier_start_character: 	letter_character / '_' ;*/
        {

           return     letter_character() || Char('_');
		}
        public bool identifier_part_characters()    /*identifier_part_characters: 	identifier_part_character+;*/
        {

           return PlusRepeat(()=> identifier_part_character() );
		}
        public bool identifier_part_character()    /*identifier_part_character: 	letter_character / decimal_digit_character / 
				connecting_character /combining_character / formatting_character;*/
        {

           return   
                     letter_character()
                  || decimal_digit_character()
                  || connecting_character()
                  || combining_character()
                  || formatting_character();
		}
        public bool letter_character()    /*letter_character: 		Lu / Ll / Lt / Lm / Lo / Nl /
 				unicode_escape_sequence:unicode_  UnicodeEscapeIsLetter_;
//A Unicode character of classes Lu, Ll, Lt, Lm, Lo, or Nl or unicode escape representing one of Lu,...*/
        {

           return   
                     Lu()
                  || Ll()
                  || Lt()
                  || Lm()
                  || Lo()
                  || Nl()
                  || And(()=>    
                         Into(()=> unicode_escape_sequence(),out top.unicode_)
                      && top.UnicodeEscapeIsLetter_() );
		}
        public bool combining_character()    /*combining_character:   		Mn / Mc / unicode_escape_sequence:unicode_ UnicodeEscapeIsMnOrMc_; 
//A Unicode character of classes Mn or Mc /A unicode_escape_sequence representing a character of classes Mn or Mc*/
        {

           return   
                     Mn()
                  || Mc()
                  || And(()=>    
                         Into(()=> unicode_escape_sequence(),out top.unicode_)
                      && top.UnicodeEscapeIsMnOrMc_() );
		}
        public bool decimal_digit_character()    /*decimal_digit_character: 	Nd /  unicode_escape_sequence:unicode_ UnicodeEscapeIsNd_; 
//A unicode-escape-sequence representing a character of the class Nd*/
        {

           return   
                     Nd()
                  || And(()=>    
                         Into(()=> unicode_escape_sequence(),out top.unicode_)
                      && top.UnicodeEscapeIsNd_() );
		}
        public bool connecting_character()    /*connecting_character:   	Pc / unicode_escape_sequence:unicode_ UnicodeEscapeIsPc_;
//A Unicode character of the class Pc
//A unicode-escape-sequence representing a character of the class Pc*/
        {

           return   
                     Pc()
                  || And(()=>    
                         Into(()=> unicode_escape_sequence(),out top.unicode_)
                      && top.UnicodeEscapeIsPc_() );
		}
        public bool formatting_character()    /*formatting_character:  		Cf / unicode_escape_sequence:unicode_ UnicodeEscapeIsCf_;
//A Unicode character of the class Cf
//A unicode-escape-sequence representing a character of the class Cf
//A.1.7 Keywords
//----------------------*/
        {

           return   
                     Cf()
                  || And(()=>    
                         Into(()=> unicode_escape_sequence(),out top.unicode_)
                      && top.UnicodeEscapeIsCf_() );
		}
        public bool keyword()    /*keyword:  
'abstract'	/'as'		/'base'		/'bool'		/'break'	/
'byte'		/'case'		/'catch'	/'char'		/'checked'	/
'class'		/'const'	/'continue'	/'decimal'	/'default'	/
'delegate'	/'do'		/'double'	/'else'		/'enum'		/
'event'		/'explicit'	/'extern'	/'false'	/'finally'	/
'fixed'		/'float'	/'for'		/'foreach'	/'goto'		/
'if'		/'implicit'	/'in'		/'int'		/'interface'	/
'internal'	/'is'		/'lock'		/'long'		/'namespace'	/
'new'		/'null'		/'object'	/'operator'	/'out'		/
'override'	/'params'	/'private'	/'protected'	/'public'	/
'readonly'	/'ref'		/'return'	/'sbyte'	/'sealed'	/
'short'		/'sizeof'	/'stackalloc'	/'static'	/'string'	/
'struct'	/'switch'	/'this'		/'throw'	/'true'		/
'try'		/'typeof'	/'uint'		/'ulong'	/'unchecked'	/
'unsafe'	/'ushort'	/'using'	/'virtual'	/'void'		/
'volatile'	/'while';
//A.1.8 Literals
//----------------------*/
        {

           return OneOfLiterals(optimizedLiterals0);
		}
        public bool literal()    /*literal: 		(boolean_literal /real_literal / integer_literal / character_literal / string_literal / null_literal) S;*/
        {

           return And(()=>  
                     (    
                         boolean_literal()
                      || real_literal()
                      || integer_literal()
                      || character_literal()
                      || string_literal()
                      || null_literal())
                  && S() );
		}
        public bool boolean_literal()    /*^boolean_literal:  	'true' B / 'false' B;*/
        {

           return TreeAST((int)ECSharp3.boolean_literal,()=>
                  
                     And(()=>    Char('t','r','u','e') && B() )
                  || And(()=>    Char('f','a','l','s','e') && B() ) );
		}
        public bool integer_literal()    /*integer_literal: 	 hexadecimal_integer_literal / decimal_integer_literal ;*/
        {

           return   
                     hexadecimal_integer_literal()
                  || decimal_integer_literal();
		}
        public bool decimal_integer_literal()    /*^decimal_integer_literal:decimal_digits   integer_type_suffix?;*/
        {

           return TreeAST((int)ECSharp3.decimal_integer_literal,()=>
                And(()=>  
                     decimal_digits()
                  && Option(()=> integer_type_suffix() ) ) );
		}
        public bool decimal_digits()    /*decimal_digits: 	decimal_digit+;*/
        {

           return PlusRepeat(()=> decimal_digit() );
		}
        public bool decimal_digit()    /*decimal_digit: 		[0-9];*/
        {

           return In('0','9');
		}
        public bool integer_type_suffix()    /*integer_type_suffix:  	[uU][lL] / [lL][uU];*/
        {

           return   
                     And(()=>    OneOf("uU") && OneOf("lL") )
                  || And(()=>    OneOf("lL") && OneOf("uU") );
		}
        public bool hexadecimal_integer_literal()    /*^hexadecimal_integer_literal: 
			'0'[xX] hex_digits   integer_type_suffix?;*/
        {

           return TreeAST((int)ECSharp3.hexadecimal_integer_literal,()=>
                And(()=>  
                     Char('0')
                  && OneOf("xX")
                  && hex_digits()
                  && Option(()=> integer_type_suffix() ) ) );
		}
        public bool hex_digits()    /*hex_digits: 		hex_digit+;*/
        {

           return PlusRepeat(()=> hex_digit() );
		}
        public bool hex_digit()    /*hex_digit: 		[0-9A-Fa-f];*/
        {

           return In('0','9', 'A','F', 'a','f');
		}
        public bool real_literal()    /*^real_literal: 		decimal_digits  ( exponent_part   real_type_suffix? / real_type_suffix / fraction) / fraction;*/
        {

           return TreeAST((int)ECSharp3.real_literal,()=>
                  
                     And(()=>    
                         decimal_digits()
                      && (      
                               And(()=>        
                                       exponent_part()
                                    && Option(()=> real_type_suffix() ) )
                            || real_type_suffix()
                            || fraction()) )
                  || fraction() );
		}
        public bool fraction()    /*fraction:		'.'   decimal_digits   exponent_part?   real_type_suffix?;*/
        {

           return And(()=>  
                     Char('.')
                  && decimal_digits()
                  && Option(()=> exponent_part() )
                  && Option(()=> real_type_suffix() ) );
		}
        public bool exponent_part()    /*exponent_part: 		[eE]   sign?   decimal_digits;*/
        {

           return And(()=>  
                     OneOf("eE")
                  && Option(()=> sign() )
                  && decimal_digits() );
		}
        public bool sign()    /*sign:  			[+-];*/
        {

           return OneOf("+-");
		}
        public bool real_type_suffix()    /*real_type_suffix: 	[FfDdMm];*/
        {

           return OneOf("FfDdMm");
		}
        public bool character_literal()    /*^character_literal: 	['] character  ['];*/
        {

           return TreeAST((int)ECSharp3.character_literal,()=>
                And(()=>    OneOf("'") && character() && OneOf("'") ) );
		}
        public bool character()    /*character: 		single_character / simple_escape_sequence / hexadecimal_escape_sequence / unicode_escape_sequence;*/
        {

           return   
                     single_character()
                  || simple_escape_sequence()
                  || hexadecimal_escape_sequence()
                  || unicode_escape_sequence();
		}
        public bool single_character()    /*single_character:  	[^'\\#x000D#x000A#x0085#x2028#x2029]; // Any character except ' (U+0027), \ (U+005C), and new-line-character*/
        {

           return NotOneOf("'\\\u000d\u000a\u0085\u2028\u2029");
		}
        public bool simple_escape_sequence()    /*simple_escape_sequence: '\\' ['"\\0abfnrtv];*/
        {

           return And(()=>    Char('\\') && OneOf(optimizedCharset0) );
		}
        public bool hexadecimal_escape_sequence()    /*hexadecimal_escape_sequence: '\\x'   hex_digit   hex_digit?   hex_digit?   hex_digit?;*/
        {

           return And(()=>  
                     Char('\\','x')
                  && hex_digit()
                  && Option(()=> hex_digit() )
                  && Option(()=> hex_digit() )
                  && Option(()=> hex_digit() ) );
		}
        public bool string_literal()    /*^string_literal: regular_string_literal / verbatim_string_literal;*/
        {

           return TreeAST((int)ECSharp3.string_literal,()=>
                    regular_string_literal() || verbatim_string_literal() );
		}
        public bool regular_string_literal()    /*regular_string_literal: '"'   regular_string_literal_characters?   '"';*/
        {

           return And(()=>  
                     Char('"')
                  && Option(()=> regular_string_literal_characters() )
                  && Char('"') );
		}
        public bool regular_string_literal_characters()    /*regular_string_literal_characters: regular_string_literal_character+;*/
        {

           return PlusRepeat(()=> regular_string_literal_character() );
		}
        public bool regular_string_literal_character()    /*regular_string_literal_character: 	
					single_regular_string_literal_character
				/	simple_escape_sequence
				/	hexadecimal_escape_sequence
				/	unicode_escape_sequence;*/
        {

           return   
                     single_regular_string_literal_character()
                  || simple_escape_sequence()
                  || hexadecimal_escape_sequence()
                  || unicode_escape_sequence();
		}
        public bool single_regular_string_literal_character()    /*single_regular_string_literal_character: [^"\\#x000D#x000A#x0085#x2028#x2029]; //Any character except " (U+0022), " (U+005C), and new-line-character*/
        {

           return NotOneOf("\"\\\u000d\u000a\u0085\u2028\u2029");
		}
        public bool verbatim_string_literal()    /*verbatim_string_literal: '@"'   verbatim_string_literal_characters?  '"';*/
        {

           return And(()=>  
                     Char('@','"')
                  && Option(()=> verbatim_string_literal_characters() )
                  && Char('"') );
		}
        public bool verbatim_string_literal_characters()    /*verbatim_string_literal_characters: verbatim_string_literal_character+;*/
        {

           return PlusRepeat(()=> verbatim_string_literal_character() );
		}
        public bool verbatim_string_literal_character()    /*verbatim_string_literal_character: single_verbatim_string_literal_character/quote_escape_sequence;*/
        {

           return   
                     single_verbatim_string_literal_character()
                  || quote_escape_sequence();
		}
        public bool single_verbatim_string_literal_character()    /*single_verbatim_string_literal_character: [^"];		//any character except "*/
        {

           return NotOneOf("\"");
		}
        public bool quote_escape_sequence()    /*quote_escape_sequence: '""';*/
        {

           return Char('"','"');
		}
        public bool null_literal()    /*null_literal:'null' B;
//A.1.9 Operators and punctuators
//----------------------*/
        {

           return And(()=>    Char('n','u','l','l') && B() );
		}
        public bool operator_or_punctuator()    /*operator_or_punctuator: 
'{' /	'}' /	'[' /	']' /	'(' /	')' /	'.' /	',' /	':' /	';'  /
'+' /	'-' /	'*' /	' /' /	'%' /	'&' /	'|' /	'^' /	'!' /	'~'  /
'=' /	'<' /	'>' /	'?' /	'??' /	'::' /	'++' /	'--' /	'&&' /	'||' /
'->' /	'==' /	'!=' /	'<=' /	'>=' /	'+=' /	'-=' /	'*=' /	' /=' /	'%=' /
'&=' /	'|=' /	'^=' /	'<<' / '>>' / '<<=' / '>>=' / '=>';*/
        {

           return OneOfLiterals(optimizedLiterals1);
		}
        public bool right_shift()    /*right_shift: '>|>';*/
        {

           return Char('>','|','>');
		}
        public bool right_shift_assignment()    /*right_shift_assignment: '>|>=';
//A.1.10 Pre
//-----------------------processing directives*/
        {

           return Char('>','|','>','=');
		}
        public bool pp_directive()    /*pp_directive: pp_declaration / pp_conditional / pp_line / pp_diagnostic / pp_region  / pp_pragma;*/
        {

           return   
                     pp_declaration()
                  || pp_conditional()
                  || pp_line()
                  || pp_diagnostic()
                  || pp_region()
                  || pp_pragma();
		}
        public bool conditional_symbol()    /*conditional_symbol: !('true' /'false') keyword B / symbolName ; //Any identifier_or_keyword except true or false*/
        {

           return   
                     And(()=>    
                         Not(()=>      
                                    
                                       Char('t','r','u','e')
                                    || Char('f','a','l','s','e') )
                      && keyword()
                      && B() )
                  || symbolName();
		}
        public bool symbolName()    /*^^symbolName:    identifier;*/
        {

           return TreeNT((int)ECSharp3.symbolName,()=> identifier() );
		}
        public bool pp_expression()    /*pp_expression: whitespace*   pp_or_expression   whitespace*;*/
        {

           return And(()=>  
                     OptRepeat(()=> whitespace() )
                  && pp_or_expression()
                  && OptRepeat(()=> whitespace() ) );
		}
        public bool pp_or_expression()    /*pp_or_expression: pp_and_expression  (whitespace*  '||'   whitespace*   pp_and_expression)*;*/
        {

           return And(()=>  
                     pp_and_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               OptRepeat(()=> whitespace() )
                            && Char('|','|')
                            && OptRepeat(()=> whitespace() )
                            && pp_and_expression() ) ) );
		}
        public bool pp_and_expression()    /*pp_and_expression: pp_equality_expression whitespace*   ('&&'   whitespace*   pp_equality_expression)*;*/
        {

           return And(()=>  
                     pp_equality_expression()
                  && OptRepeat(()=> whitespace() )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('&','&')
                            && OptRepeat(()=> whitespace() )
                            && pp_equality_expression() ) ) );
		}
        public bool pp_equality_expression()    /*pp_equality_expression: pp_unary_expression (whitespace*   ('==' /'!=')   whitespace*   pp_unary_expression)*;*/
        {

           return And(()=>  
                     pp_unary_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               OptRepeat(()=> whitespace() )
                            && (    Char('=','=') || Char('!','='))
                            && OptRepeat(()=> whitespace() )
                            && pp_unary_expression() ) ) );
		}
        public bool pp_unary_expression()    /*pp_unary_expression: ('!'   whitespace*)?  pp_primary_expression;*/
        {

           return And(()=>  
                     Option(()=>    
                      And(()=>      
                               Char('!')
                            && OptRepeat(()=> whitespace() ) ) )
                  && pp_primary_expression() );
		}
        public bool pp_primary_expression()    /*pp_primary_expression:
'true' / 'false' / conditional_symbol / '('   whitespace*   pp_expression   whitespace*   ')';*/
        {

           return   
                     Char('t','r','u','e')
                  || Char('f','a','l','s','e')
                  || conditional_symbol()
                  || And(()=>    
                         Char('(')
                      && OptRepeat(()=> whitespace() )
                      && pp_expression()
                      && OptRepeat(()=> whitespace() )
                      && Char(')') );
		}
        public bool pp_declaration()    /*^^pp_declaration: whitespace*  '#'   whitespace*   ('define'/'undef')   whitespace+   conditional_symbol   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3.pp_declaration,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && (    
                         Char('d','e','f','i','n','e')
                      || Char('u','n','d','e','f'))
                  && PlusRepeat(()=> whitespace() )
                  && conditional_symbol()
                  && pp_new_line() ) );
		}
        public bool pp_new_line()    /*pp_new_line: whitespace*   single_line_comment?   new_line;*/
        {

           return And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Option(()=> single_line_comment() )
                  && new_line() );
		}
        public bool pp_conditional()    /*pp_conditional: 	pp_if_section  / pp_elif_section /    pp_else_section /  pp_endif;*/
        {

           return   
                     pp_if_section()
                  || pp_elif_section()
                  || pp_else_section()
                  || pp_endif();
		}
        public bool pp_if_section()    /*^^pp_if_section: 		whitespace*   '#'   whitespace*   'if'   whitespace+   pp_expression   pp_new_line   ;*/
        {

           return TreeNT((int)ECSharp3.pp_if_section,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('i','f')
                  && PlusRepeat(()=> whitespace() )
                  && pp_expression()
                  && pp_new_line() ) );
		}
        public bool pp_elif_section()    /*^^pp_elif_section: 	whitespace*   '#'   whitespace*   'elif'   whitespace+   pp_expression   pp_new_line ;*/
        {

           return TreeNT((int)ECSharp3.pp_elif_section,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('e','l','i','f')
                  && PlusRepeat(()=> whitespace() )
                  && pp_expression()
                  && pp_new_line() ) );
		}
        public bool pp_else_section()    /*^^pp_else_section: 	whitespace*   '#'   whitespace*   'else'  pp_new_line;*/
        {

           return TreeNT((int)ECSharp3.pp_else_section,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('e','l','s','e')
                  && pp_new_line() ) );
		}
        public bool pp_endif()    /*^^pp_endif: 		whitespace*   '#'   whitespace*   'endif'   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3.pp_endif,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('e','n','d','i','f')
                  && pp_new_line() ) );
		}
        public bool pp_diagnostic()    /*^^pp_diagnostic: whitespace*   '#'   whitespace*   ('error'/'warning')   pp_message;*/
        {

           return TreeNT((int)ECSharp3.pp_diagnostic,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && (    
                         Char('e','r','r','o','r')
                      || Char('w','a','r','n','i','n','g'))
                  && pp_message() ) );
		}
        public bool pp_message()    /*pp_message: new_line / whitespace*   input_characters?   new_line;*/
        {

           return   
                     new_line()
                  || And(()=>    
                         OptRepeat(()=> whitespace() )
                      && Option(()=> input_characters() )
                      && new_line() );
		}
        public bool pp_region()    /*pp_region: pp_start_region  /  pp_end_region;*/
        {

           return     pp_start_region() || pp_end_region();
		}
        public bool pp_start_region()    /*^^pp_start_region: whitespace*   '#'   whitespace*   'region'   pp_message;*/
        {

           return TreeNT((int)ECSharp3.pp_start_region,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('r','e','g','i','o','n')
                  && pp_message() ) );
		}
        public bool pp_end_region()    /*^^pp_end_region: whitespace*   '#'   whitespace*   'endregion'   pp_message;*/
        {

           return TreeNT((int)ECSharp3.pp_end_region,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char("endregion")
                  && pp_message() ) );
		}
        public bool pp_line()    /*^^pp_line: whitespace*   '#'   whitespace*   'line'   whitespace+   line_indicator   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3.pp_line,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('l','i','n','e')
                  && PlusRepeat(()=> whitespace() )
                  && line_indicator()
                  && pp_new_line() ) );
		}
        public bool line_indicator()    /*line_indicator: decimal_digits   (whitespace+   file_name )? / 'default' / 'hidden';*/
        {

           return   
                     And(()=>    
                         decimal_digits()
                      && Option(()=>      
                            And(()=>        
                                       PlusRepeat(()=> whitespace() )
                                    && file_name() ) ) )
                  || Char('d','e','f','a','u','l','t')
                  || Char('h','i','d','d','e','n');
		}
        public bool file_name()    /*file_name: '"'   file_name_characters   '"';*/
        {

           return And(()=>  
                     Char('"')
                  && file_name_characters()
                  && Char('"') );
		}
        public bool file_name_characters()    /*file_name_characters: 	file_name_character+;*/
        {

           return PlusRepeat(()=> file_name_character() );
		}
        public bool file_name_character()    /*file_name_character: 	!'"' input_character ;//Any input_character except "*/
        {

           return And(()=>    Not(()=> Char('"') ) && input_character() );
		}
        public bool pp_pragma()    /*^^pp_pragma: 		whitespace*   '#'   whitespace*   'pragma'   whitespace+   pragma_body   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3.pp_pragma,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('p','r','a','g','m','a')
                  && PlusRepeat(()=> whitespace() )
                  && pragma_body()
                  && pp_new_line() ) );
		}
        public bool pragma_body()    /*pragma_body: 		pragma_warning_body;*/
        {

           return pragma_warning_body();
		}
        public bool pragma_warning_body()    /*pragma_warning_body: 	'warning'   whitespace+   warning_action  (whitespace+   warning_list)?;*/
        {

           return And(()=>  
                     Char('w','a','r','n','i','n','g')
                  && PlusRepeat(()=> whitespace() )
                  && warning_action()
                  && Option(()=>    
                      And(()=>      
                               PlusRepeat(()=> whitespace() )
                            && warning_list() ) ) );
		}
        public bool warning_action()    /*^^warning_action: 	'disable' / 'restore';*/
        {

           return TreeNT((int)ECSharp3.warning_action,()=>
                  
                     Char('d','i','s','a','b','l','e')
                  || Char('r','e','s','t','o','r','e') );
		}
        public bool warning_list()    /*^^warning_list: 		decimal_digits  (whitespace*   ','   whitespace+   @decimal_digits)*;
//A.2 Syntactic grammar
//---------------------
//A.2.1 Basic concepts
//----------------------*/
        {

           return TreeNT((int)ECSharp3.warning_list,()=>
                And(()=>  
                     decimal_digits()
                  && OptRepeat(()=>    
                      And(()=>      
                               OptRepeat(()=> whitespace() )
                            && Char(',')
                            && PlusRepeat(()=> whitespace() )
                            && (        
                                       decimal_digits()
                                    || Fatal("<<decimal_digits>> expected")) ) ) ) );
		}
        public bool namespace_name()    /*^namespace_name: 		namespace_or_type_name;*/
        {

           return TreeAST((int)ECSharp3.namespace_name,()=>
                namespace_or_type_name() );
		}
        public bool type_name()    /*^^type_name: 			namespace_or_type_name;*/
        {

           return TreeNT((int)ECSharp3.type_name,()=>
                namespace_or_type_name() );
		}
        public bool namespace_or_type_name()    /*^namespace_or_type_name: 	( qualified_alias_member / name S type_argument_list? )  
			 	('.' S   @name S  type_argument_list?)*;*/
        {

           return TreeAST((int)ECSharp3.namespace_or_type_name,()=>
                And(()=>  
                     (    
                         qualified_alias_member()
                      || And(()=>      
                               name()
                            && S()
                            && Option(()=> type_argument_list() ) ))
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('.')
                            && S()
                            && (    name() || Fatal("<<name>> expected"))
                            && S()
                            && Option(()=> type_argument_list() ) ) ) ) );
		}
        public bool name()    /*^^name:				identifier;
//A.2.2 Types
//----------------------*/
        {

           return TreeNT((int)ECSharp3.name,()=> identifier() );
		}
        public bool type()    /*^^type: 			void_pointer / non_array_type rank_specifiers? pointers?;*/
        {

           return TreeNT((int)ECSharp3.type,()=>
                  
                     void_pointer()
                  || And(()=>    
                         non_array_type()
                      && Option(()=> rank_specifiers() )
                      && Option(()=> pointers() ) ) );
		}
        public bool pointer_type()    /*^^pointer_type:			void_pointer / non_array_type rank_specifiers? pointers;*/
        {

           return TreeNT((int)ECSharp3.pointer_type,()=>
                  
                     void_pointer()
                  || And(()=>    
                         non_array_type()
                      && Option(()=> rank_specifiers() )
                      && pointers() ) );
		}
        public bool pointers()    /*^^pointers:			'*' S ('*' S)*;*/
        {

           return TreeNT((int)ECSharp3.pointers,()=>
                And(()=>  
                     Char('*')
                  && S()
                  && OptRepeat(()=> And(()=>    Char('*') && S() ) ) ) );
		}
        public bool void_pointer()    /*^^void_pointer:			'void' S '*' S;*/
        {

           return TreeNT((int)ECSharp3.void_pointer,()=>
                And(()=>  
                     Char('v','o','i','d')
                  && S()
                  && Char('*')
                  && S() ) );
		}
        public bool value_type()    /*value_type: 			struct_type / enum_type;*/
        {

           return     struct_type() || enum_type();
		}
        public bool struct_type()    /*struct_type: 			type_name / simple_type ;*/
        {

           return     type_name() || simple_type();
		}
        public bool simple_type()    /*simple_type: 			numeric_type / ^'bool' B S;*/
        {

           return   
                     numeric_type()
                  || And(()=>    
                         TreeChars(()=> Char('b','o','o','l') )
                      && B()
                      && S() );
		}
        public bool numeric_type()    /*numeric_type: 			integral_type / floating_point_type / ^'decimal' B S;*/
        {

           return   
                     integral_type()
                  || floating_point_type()
                  || And(()=>    
                         TreeChars(()=> Char('d','e','c','i','m','a','l') )
                      && B()
                      && S() );
		}
        public bool integral_type()    /*integral_type:			^('sbyte' / 'byte' / 'short' / 'ushort' / 'int' / 'uint' / 'long' / 'ulong' / 'char') B S;*/
        {

           return And(()=>  
                     TreeChars(()=> OneOfLiterals(optimizedLiterals2) )
                  && B()
                  && S() );
		}
        public bool floating_point_type()    /*floating_point_type:		^('float'/'double') B S;*/
        {

           return And(()=>  
                     TreeChars(()=>    
                            
                               Char('f','l','o','a','t')
                            || Char('d','o','u','b','l','e') )
                  && B()
                  && S() );
		}
        public bool is_nullable()    /*^is_nullable: 			'?' S;*/
        {

           return TreeAST((int)ECSharp3.is_nullable,()=>
                And(()=>    Char('?') && S() ) );
		}
        public bool non_nullable_type()    /*non_nullable_type: 		void_pointer / non_array_non_nullable_type rank_specifiers? pointers?;*/
        {

           return   
                     void_pointer()
                  || And(()=>    
                         non_array_non_nullable_type()
                      && Option(()=> rank_specifiers() )
                      && Option(()=> pointers() ) );
		}
        public bool enum_type()    /*enum_type: 			type_name;*/
        {

           return type_name();
		}
        public bool non_array_reference_type()    /*non_array_reference_type: 	^'object' B S / ^'string' B S / type_name;*/
        {

           return   
                     And(()=>    
                         TreeChars(()=> Char('o','b','j','e','c','t') )
                      && B()
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('s','t','r','i','n','g') )
                      && B()
                      && S() )
                  || type_name();
		}
        public bool class_type()    /*class_type:			non_array_reference_type;*/
        {

           return non_array_reference_type();
		}
        public bool interface_type()    /*^interface_type: 		type_name;*/
        {

           return TreeAST((int)ECSharp3.interface_type,()=>
                type_name() );
		}
        public bool array_type()    /*^array_type: 			non_array_type   rank_specifiers;*/
        {

           return TreeAST((int)ECSharp3.array_type,()=>
                And(()=>    non_array_type() && rank_specifiers() ) );
		}
        public bool non_array_non_nullable_type()    /*non_array_non_nullable_type:	( value_type / non_array_reference_type / type_parameter );*/
        {

           return   
                     value_type()
                  || non_array_reference_type()
                  || type_parameter();
		}
        public bool non_array_type()    /*non_array_type: 		non_array_non_nullable_type  is_nullable? ;*/
        {

           return And(()=>  
                     non_array_non_nullable_type()
                  && Option(()=> is_nullable() ) );
		}
        public bool rank_specifiers()    /*rank_specifiers: 		rank_specifier+;*/
        {

           return PlusRepeat(()=> rank_specifier() );
		}
        public bool rank_specifier()    /*^^rank_specifier: 		'[' S   dim_separators?   ']' S;*/
        {

           return TreeNT((int)ECSharp3.rank_specifier,()=>
                And(()=>  
                     Char('[')
                  && S()
                  && Option(()=> dim_separators() )
                  && Char(']')
                  && S() ) );
		}
        public bool dim_separators()    /*dim_separators: 		(',' S)+;*/
        {

           return PlusRepeat(()=> And(()=>    Char(',') && S() ) );
		}
        public bool delegate_type()    /*^^delegate_type: 		type_name;*/
        {

           return TreeNT((int)ECSharp3.delegate_type,()=>
                type_name() );
		}
        public bool type_argument_list()    /*^^type_argument_list:		'<'  S type_arguments   '>' S;*/
        {

           return TreeNT((int)ECSharp3.type_argument_list,()=>
                And(()=>  
                     Char('<')
                  && S()
                  && type_arguments()
                  && Char('>')
                  && S() ) );
		}
        public bool type_arguments()    /*type_arguments: 		type_argument (',' S  type_argument)*;*/
        {

           return And(()=>  
                     type_argument()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && type_argument() ) ) );
		}
        public bool type_argument()    /*^^type_argument: 		type;*/
        {

           return TreeNT((int)ECSharp3.type_argument,()=> type() );
		}
        public bool type_parameter()    /*^^type_parameter: 		identifier S;
//A.2.3 Variables
//----------------------*/
        {

           return TreeNT((int)ECSharp3.type_parameter,()=>
                And(()=>    identifier() && S() ) );
		}
        public bool variable_reference()    /*^variable_reference: 		expression;
//A.2.4 Expressions
//----------------------*/
        {

           return TreeAST((int)ECSharp3.variable_reference,()=>
                expression() );
		}
        public bool argument_list()    /*^^argument_list: 		argument (',' S   argument)*;*/
        {

           return TreeNT((int)ECSharp3.argument_list,()=>
                And(()=>  
                     argument()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && argument() ) ) ) );
		}
        public bool argument()    /*^^argument: 			expression / ^'ref' B S  @variable_reference / ^'out' B S  @variable_reference;*/
        {

           return TreeNT((int)ECSharp3.argument,()=>
                  
                     expression()
                  || And(()=>    
                         TreeChars(()=> Char('r','e','f') )
                      && B()
                      && S()
                      && (      
                               variable_reference()
                            || Fatal("<<variable_reference>> expected")) )
                  || And(()=>    
                         TreeChars(()=> Char('o','u','t') )
                      && B()
                      && S()
                      && (      
                               variable_reference()
                            || Fatal("<<variable_reference>> expected")) ) );
		}
        public bool postfix_expression()    /*postfix_expression: 		primary_expression postfix_operation*;*/
        {

           return And(()=>  
                     primary_expression()
                  && OptRepeat(()=> postfix_operation() ) );
		}
        public bool postfix_operation()    /*postfix_operation:              (invocation/member_access/pointer_member_access/element_access/post_incr/post_decr);*/
        {

           return   
                     invocation()
                  || member_access()
                  || pointer_member_access()
                  || element_access()
                  || post_incr()
                  || post_decr();
		}
        public bool invocation()    /*^^invocation:			'(' S  argument_list?  @')' S;*/
        {

           return TreeNT((int)ECSharp3.invocation,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && Option(()=> argument_list() )
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool member_access()    /*^^member_access:		'.' S  @name S  type_argument_list?;*/
        {

           return TreeNT((int)ECSharp3.member_access,()=>
                And(()=>  
                     Char('.')
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && Option(()=> type_argument_list() ) ) );
		}
        public bool pointer_member_access()    /*^^pointer_member_access:	'->' S @name S;*/
        {

           return TreeNT((int)ECSharp3.pointer_member_access,()=>
                And(()=>  
                     Char('-','>')
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S() ) );
		}
        public bool element_access()    /*^^element_access:		'[' S  @index    S ']' S;*/
        {

           return TreeNT((int)ECSharp3.element_access,()=>
                And(()=>  
                     Char('[')
                  && S()
                  && (    index() || Fatal("<<index>> expected"))
                  && S()
                  && Char(']')
                  && S() ) );
		}
        public bool index()    /*^^index:			expression_list;*/
        {

           return TreeNT((int)ECSharp3.index,()=> expression_list() );
		}
        public bool post_incr()    /*^post_incr:                     '++' S;*/
        {

           return TreeAST((int)ECSharp3.post_incr,()=>
                And(()=>    Char('+','+') && S() ) );
		}
        public bool post_decr()    /*^post_decr:                     '--' S;*/
        {

           return TreeAST((int)ECSharp3.post_decr,()=>
                And(()=>    Char('-','-') && S() ) );
		}
        public bool primary_expression()    /*primary_expression:		literal / simple_name / parenthesized_expression /this_access / base_access / creation_expression /
				typeof_expression /  checked_expression / unchecked_expression / default_value_expression /
				special_member_access / sizeof_expression;*/
        {

           return   
                     literal()
                  || simple_name()
                  || parenthesized_expression()
                  || this_access()
                  || base_access()
                  || creation_expression()
                  || typeof_expression()
                  || checked_expression()
                  || unchecked_expression()
                  || default_value_expression()
                  || special_member_access()
                  || sizeof_expression();
		}
        public bool sizeof_expression()    /*^^sizeof_expression:		'sizeof' S  '(' S   type   ')' S;*/
        {

           return TreeNT((int)ECSharp3.sizeof_expression,()=>
                And(()=>  
                     Char('s','i','z','e','o','f')
                  && S()
                  && Char('(')
                  && S()
                  && type()
                  && Char(')')
                  && S() ) );
		}
        public bool simple_name()    /*^simple_name: 			name B S   type_argument_list?;*/
        {

           return TreeAST((int)ECSharp3.simple_name,()=>
                And(()=>  
                     name()
                  && B()
                  && S()
                  && Option(()=> type_argument_list() ) ) );
		}
        public bool parenthesized_expression()    /*^parenthesized_expression: 	'(' S   expression   ')' S;*/
        {

           return TreeAST((int)ECSharp3.parenthesized_expression,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && expression()
                  && Char(')')
                  && S() ) );
		}
        public bool special_member_access()    /*special_member_access: 		predefined_type S  '.' S  @name S  type_argument_list?
			/	qualified_alias_member   '.' S  @name S;*/
        {

           return   
                     And(()=>    
                         predefined_type()
                      && S()
                      && Char('.')
                      && S()
                      && (    name() || Fatal("<<name>> expected"))
                      && S()
                      && Option(()=> type_argument_list() ) )
                  || And(()=>    
                         qualified_alias_member()
                      && Char('.')
                      && S()
                      && (    name() || Fatal("<<name>> expected"))
                      && S() );
		}
        public bool predefined_type()    /*^predefined_type: 		(
				'bool'	  /  'byte'	  /  'char'	  /  'decimal'  /  'double'  /  'float'	  /  'int'    /  'long' /
				'object'  /  'sbyte'	  /  'short'	  /  'string'  /  'uint'     /  'ulong'	  /  'ushort' 
				) B ;*/
        {

           return TreeAST((int)ECSharp3.predefined_type,()=>
                And(()=>    OneOfLiterals(optimizedLiterals3) && B() ) );
		}
        public bool expression_list()    /*expression_list: 		expression (',' S   @expression)*;*/
        {

           return And(()=>  
                     expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    expression() || Fatal("<<expression>> expected")) ) ) );
		}
        public bool this_access()    /*^this_access: 			'this' B S;*/
        {

           return TreeAST((int)ECSharp3.this_access,()=>
                And(()=>    Char('t','h','i','s') && B() && S() ) );
		}
        public bool base_access()    /*^base_access: 			'base'  S ( '.'  S @name S / '[' S  @index   @']' S );*/
        {

           return TreeAST((int)ECSharp3.base_access,()=>
                And(()=>  
                     Char('b','a','s','e')
                  && S()
                  && (    
                         And(()=>      
                               Char('.')
                            && S()
                            && (    name() || Fatal("<<name>> expected"))
                            && S() )
                      || And(()=>      
                               Char('[')
                            && S()
                            && (    index() || Fatal("<<index>> expected"))
                            && (    Char(']') || Fatal("<<']'>> expected"))
                            && S() )) ) );
		}
        public bool post_increment_expression()    /*^^post_increment_expression: 	postfix_expression   '++' S;*/
        {

           return TreeNT((int)ECSharp3.post_increment_expression,()=>
                And(()=>    postfix_expression() && Char('+','+') && S() ) );
		}
        public bool post_decrement_expression()    /*^^post_decrement_expression: 	postfix_expression   '--' S;*/
        {

           return TreeNT((int)ECSharp3.post_decrement_expression,()=>
                And(()=>    postfix_expression() && Char('-','-') && S() ) );
		}
        public bool object_creation_expression()    /*^^object_creation_expression: 	'new'  S type  
				( '(' S   argument_list?   ')'  S   object_or_collection_initializer? / object_or_collection_initializer );*/
        {

           return TreeNT((int)ECSharp3.object_creation_expression,()=>
                And(()=>  
                     Char('n','e','w')
                  && S()
                  && type()
                  && (    
                         And(()=>      
                               Char('(')
                            && S()
                            && Option(()=> argument_list() )
                            && Char(')')
                            && S()
                            && Option(()=> object_or_collection_initializer() ) )
                      || object_or_collection_initializer()) ) );
		}
        public bool object_or_collection_initializer()    /*object_or_collection_initializer: 
				object_initializer / collection_initializer;*/
        {

           return     object_initializer() || collection_initializer();
		}
        public bool object_initializer()    /*^^object_initializer:		'{'  S (member_initializer_list ','? S)?   '}' S;*/
        {

           return TreeNT((int)ECSharp3.object_initializer,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=>    
                      And(()=>      
                               member_initializer_list()
                            && Option(()=> Char(',') )
                            && S() ) )
                  && Char('}')
                  && S() ) );
		}
        public bool member_initializer_list()    /*member_initializer_list: 	member_initializer ( ',' S  @member_initializer )*;*/
        {

           return And(()=>  
                     member_initializer()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (        
                                       member_initializer()
                                    || Fatal("<<member_initializer>> expected")) ) ) );
		}
        public bool member_initializer()    /*^member_initializer: 		member_name  S '=' S   @initializer_value;*/
        {

           return TreeAST((int)ECSharp3.member_initializer,()=>
                And(()=>  
                     member_name()
                  && S()
                  && Char('=')
                  && S()
                  && (    
                         initializer_value()
                      || Fatal("<<initializer_value>> expected")) ) );
		}
        public bool initializer_value()    /*^^initializer_value: 		expression / object_or_collection_initializer;*/
        {

           return TreeNT((int)ECSharp3.initializer_value,()=>
                    expression() || object_or_collection_initializer() );
		}
        public bool collection_initializer()    /*^^collection_initializer: 	'{' S  element_initializer_list  ','?  S @'}' S;*/
        {

           return TreeNT((int)ECSharp3.collection_initializer,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && element_initializer_list()
                  && Option(()=> Char(',') )
                  && S()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool element_initializer_list()    /*^^element_initializer_list: 	element_initializer  (',' S   element_initializer)*;*/
        {

           return TreeNT((int)ECSharp3.element_initializer_list,()=>
                And(()=>  
                     element_initializer()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && element_initializer() ) ) ) );
		}
        public bool element_initializer()    /*^^element_initializer: 		'{' S  initial_value_list    @'}' S / non_assignment_expression ;*/
        {

           return TreeNT((int)ECSharp3.element_initializer,()=>
                  
                     And(()=>    
                         Char('{')
                      && S()
                      && initial_value_list()
                      && (    Char('}') || Fatal("<<'}'>> expected"))
                      && S() )
                  || non_assignment_expression() );
		}
        public bool initial_value_list()    /*^^initial_value_list:	        expression_list;*/
        {

           return TreeNT((int)ECSharp3.initial_value_list,()=>
                expression_list() );
		}
        public bool array_creation_expression()    /*^^array_creation_expression: 	'new' B S
				(	non_array_type   '[' S  array_size   @']' S   rank_specifiers?   array_initializer?
				/	array_type   array_initializer 
				/    	rank_specifier   array_initializer
				);*/
        {

           return TreeNT((int)ECSharp3.array_creation_expression,()=>
                And(()=>  
                     Char('n','e','w')
                  && B()
                  && S()
                  && (    
                         And(()=>      
                               non_array_type()
                            && Char('[')
                            && S()
                            && array_size()
                            && (    Char(']') || Fatal("<<']'>> expected"))
                            && S()
                            && Option(()=> rank_specifiers() )
                            && Option(()=> array_initializer() ) )
                      || And(()=>    array_type() && array_initializer() )
                      || And(()=>    rank_specifier() && array_initializer() )) ) );
		}
        public bool array_size()    /*^^array_size:			expression_list;*/
        {

           return TreeNT((int)ECSharp3.array_size,()=>
                expression_list() );
		}
        public bool delegate_creation_expression()    /*^^delegate_creation_expression:	'new' S  delegate_type   '(' S  expression  @')' S;*/
        {

           return TreeNT((int)ECSharp3.delegate_creation_expression,()=>
                And(()=>  
                     Char('n','e','w')
                  && S()
                  && delegate_type()
                  && Char('(')
                  && S()
                  && expression()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool anonymous_object_creation_expression()    /*^^anonymous_object_creation_expression: 
				'new' S   anonymous_object_initializer;*/
        {

           return TreeNT((int)ECSharp3.anonymous_object_creation_expression,()=>
                And(()=>  
                     Char('n','e','w')
                  && S()
                  && anonymous_object_initializer() ) );
		}
        public bool anonymous_object_initializer()    /*^^anonymous_object_initializer:  '{' S   (member_declarator_list ','? S)?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.anonymous_object_initializer,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=>    
                      And(()=>      
                               member_declarator_list()
                            && Option(()=> Char(',') )
                            && S() ) )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool member_declarator_list()    /*^^member_declarator_list: 	member_declarator  (',' S  @member_declarator)*;*/
        {

           return TreeNT((int)ECSharp3.member_declarator_list,()=>
                And(()=>  
                     member_declarator()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (        
                                       member_declarator()
                                    || Fatal("<<member_declarator>> expected")) ) ) ) );
		}
        public bool member_declarator()    /*^^member_declarator: 		member_name  S  '=' S   @expression /  full_member_access /simple_name  ;*/
        {

           return TreeNT((int)ECSharp3.member_declarator,()=>
                  
                     And(()=>    
                         member_name()
                      && S()
                      && Char('=')
                      && S()
                      && (    expression() || Fatal("<<expression>> expected")) )
                  || full_member_access()
                  || simple_name() );
		}
        public bool full_member_access()    /*^^full_member_access:		primary_expression (!(member_access [,)}]) postfix_operation)* member_access;*/
        {

           return TreeNT((int)ECSharp3.full_member_access,()=>
                And(()=>  
                     primary_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Not(()=>        
                                    And(()=>    member_access() && OneOf(",)}") ) )
                            && postfix_operation() ) )
                  && member_access() ) );
		}
        public bool typeof_expression()    /*^^typeof_expression: 		'typeof' B S   '(' S   (type !generic_dimension_specifier/unbound_type_name/'void' B S)   @')' S;*/
        {

           return TreeNT((int)ECSharp3.typeof_expression,()=>
                And(()=>  
                     Char('t','y','p','e','o','f')
                  && B()
                  && S()
                  && Char('(')
                  && S()
                  && (    
                         And(()=>      
                               type()
                            && Not(()=> generic_dimension_specifier() ) )
                      || unbound_type_name()
                      || And(()=>    Char('v','o','i','d') && B() && S() ))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool unbound_type_name()    /*^^unbound_type_name: 		(name S ('::' S @name S)?  generic_dimension_specifier?)
			 	(  '.' S   @name S  generic_dimension_specifier?)*;*/
        {

           return TreeNT((int)ECSharp3.unbound_type_name,()=>
                And(()=>  
                     And(()=>    
                         name()
                      && S()
                      && Option(()=>      
                            And(()=>        
                                       Char(':',':')
                                    && S()
                                    && (    name() || Fatal("<<name>> expected"))
                                    && S() ) )
                      && Option(()=> generic_dimension_specifier() ) )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('.')
                            && S()
                            && (    name() || Fatal("<<name>> expected"))
                            && S()
                            && Option(()=> generic_dimension_specifier() ) ) ) ) );
		}
        public bool generic_dimension_specifier()    /*^^generic_dimension_specifier: 	'<' S   commas?   @'>' S;*/
        {

           return TreeNT((int)ECSharp3.generic_dimension_specifier,()=>
                And(()=>  
                     Char('<')
                  && S()
                  && Option(()=> commas() )
                  && (    Char('>') || Fatal("<<'>'>> expected"))
                  && S() ) );
		}
        public bool commas()    /*^commas: 			(',' S)+;*/
        {

           return TreeAST((int)ECSharp3.commas,()=>
                PlusRepeat(()=> And(()=>    Char(',') && S() ) ) );
		}
        public bool checked_expression()    /*^^checked_expression: 		'checked' S    @'(' S  @expression   @')' S;*/
        {

           return TreeNT((int)ECSharp3.checked_expression,()=>
                And(()=>  
                     Char('c','h','e','c','k','e','d')
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool unchecked_expression()    /*^^unchecked_expression: 	'unchecked' S  @'(' S   @expression   @')' S;*/
        {

           return TreeNT((int)ECSharp3.unchecked_expression,()=>
                And(()=>  
                     Char("unchecked")
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool default_value_expression()    /*^^default_value_expression: 	'default' S    '(' S  @type   @')' S;*/
        {

           return TreeNT((int)ECSharp3.default_value_expression,()=>
                And(()=>  
                     Char('d','e','f','a','u','l','t')
                  && S()
                  && Char('(')
                  && S()
                  && (    type() || Fatal("<<type>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool unary_expression()    /*^unary_expression: 		cast_expression /
				postfix_expression / 
				pre_increment_expression / 
				pre_decrement_expression / 
				^'+' S   unary_expression / 
				^'-' S   unary_expression / 
				^'!' S   unary_expression / 
				^'~' S   unary_expression / 
				^'*' S   unary_expression / 
				^'&' S   unary_expression / 
                                anonymous_method_expression;*/
        {

           return TreeAST((int)ECSharp3.unary_expression,()=>
                  
                     cast_expression()
                  || postfix_expression()
                  || pre_increment_expression()
                  || pre_decrement_expression()
                  || And(()=>    
                         TreeChars(()=> Char('+') )
                      && S()
                      && unary_expression() )
                  || And(()=>    
                         TreeChars(()=> Char('-') )
                      && S()
                      && unary_expression() )
                  || And(()=>    
                         TreeChars(()=> Char('!') )
                      && S()
                      && unary_expression() )
                  || And(()=>    
                         TreeChars(()=> Char('~') )
                      && S()
                      && unary_expression() )
                  || And(()=>    
                         TreeChars(()=> Char('*') )
                      && S()
                      && unary_expression() )
                  || And(()=>    
                         TreeChars(()=> Char('&') )
                      && S()
                      && unary_expression() )
                  || anonymous_method_expression() );
		}
        public bool creation_expression()    /*^creation_expression:           array_creation_expression / 
				object_creation_expression / 
				delegate_creation_expression / 
				anonymous_object_creation_expression;*/
        {

           return TreeAST((int)ECSharp3.creation_expression,()=>
                  
                     array_creation_expression()
                  || object_creation_expression()
                  || delegate_creation_expression()
                  || anonymous_object_creation_expression() );
		}
        public bool pre_increment_expression()    /*^^pre_increment_expression: 	'++' S   unary_expression;*/
        {

           return TreeNT((int)ECSharp3.pre_increment_expression,()=>
                And(()=>    Char('+','+') && S() && unary_expression() ) );
		}
        public bool pre_decrement_expression()    /*^^pre_decrement_expression: 	'--' S   unary_expression;*/
        {

           return TreeNT((int)ECSharp3.pre_decrement_expression,()=>
                And(()=>    Char('-','-') && S() && unary_expression() ) );
		}
        public bool cast_expression()    /*^cast_expression: 	        ('(' S   type   ')' S &([~!(]/identifier/literal/!('as' B/'is' B) keyword B)
				/ !parenthesized_expression   '(' S type ')' )
				S   unary_expression;*/
        {

           return TreeAST((int)ECSharp3.cast_expression,()=>
                And(()=>  
                     (    
                         And(()=>      
                               Char('(')
                            && S()
                            && type()
                            && Char(')')
                            && S()
                            && Peek(()=>        
                                              
                                                 OneOf("~!(")
                                              || identifier()
                                              || literal()
                                              || And(()=>            
                                                             Not(()=>              
                                                                                        
                                                                                           And(()=>    Char('a','s') && B() )
                                                                                        || And(()=>    Char('i','s') && B() ) )
                                                          && keyword()
                                                          && B() ) ) )
                      || And(()=>      
                               Not(()=> parenthesized_expression() )
                            && Char('(')
                            && S()
                            && type()
                            && Char(')') ))
                  && S()
                  && unary_expression() ) );
		}
        public bool multiplicative_expression()    /*^multiplicative_expression: 	unary_expression ( ^[* /%] S  unary_expression )*;*/
        {

           return TreeAST((int)ECSharp3.multiplicative_expression,()=>
                And(()=>  
                     unary_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> OneOf("*/%") )
                            && S()
                            && unary_expression() ) ) ) );
		}
        public bool additive_expression()    /*^additive_expression: 		multiplicative_expression ( ^[+-] S  multiplicative_expression )*;*/
        {

           return TreeAST((int)ECSharp3.additive_expression,()=>
                And(()=>  
                     multiplicative_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> OneOf("+-") )
                            && S()
                            && multiplicative_expression() ) ) ) );
		}
        public bool shift_expression()    /*^shift_expression: 		additive_expression ( ^('<<' / '>>') S  additive_expression )*;*/
        {

           return TreeAST((int)ECSharp3.shift_expression,()=>
                And(()=>  
                     additive_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=>     Char('<','<') || Char('>','>') )
                            && S()
                            && additive_expression() ) ) ) );
		}
        public bool relational_expression()    /*^relational_expression: 	shift_expression (^('<='/'>='/'<'/'>') S shift_expression)* 
                                (('is' B/'as' B) S non_nullable_type)? ;*/
        {

           return TreeAST((int)ECSharp3.relational_expression,()=>
                And(()=>  
                     shift_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=>        
                                              
                                                 Char('<','=')
                                              || Char('>','=')
                                              || Char('<')
                                              || Char('>') )
                            && S()
                            && shift_expression() ) )
                  && Option(()=>    
                      And(()=>      
                               (        
                                       And(()=>    Char('i','s') && B() )
                                    || And(()=>    Char('a','s') && B() ))
                            && S()
                            && non_nullable_type() ) ) ) );
		}
        public bool equality_expression()    /*^equality_expression: 		relational_expression (^('=='/'!=') S relational_expression)*;*/
        {

           return TreeAST((int)ECSharp3.equality_expression,()=>
                And(()=>  
                     relational_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=>     Char('=','=') || Char('!','=') )
                            && S()
                            && relational_expression() ) ) ) );
		}
        public bool and_expression()    /*^and_expression: 		equality_expression ('&' S equality_expression)*;*/
        {

           return TreeAST((int)ECSharp3.and_expression,()=>
                And(()=>  
                     equality_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('&')
                            && S()
                            && equality_expression() ) ) ) );
		}
        public bool exclusive_or_expression()    /*^exclusive_or_expression: 	and_expression ('^' S and_expression)*;*/
        {

           return TreeAST((int)ECSharp3.exclusive_or_expression,()=>
                And(()=>  
                     and_expression()
                  && OptRepeat(()=>    
                      And(()=>    Char('^') && S() && and_expression() ) ) ) );
		}
        public bool inclusive_or_expression()    /*^inclusive_or_expression: 	exclusive_or_expression ('|' S exclusive_or_expression)*;*/
        {

           return TreeAST((int)ECSharp3.inclusive_or_expression,()=>
                And(()=>  
                     exclusive_or_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('|')
                            && S()
                            && exclusive_or_expression() ) ) ) );
		}
        public bool conditional_and_expression()    /*^conditional_and_expression: 	inclusive_or_expression ('&&' S inclusive_or_expression)*;*/
        {

           return TreeAST((int)ECSharp3.conditional_and_expression,()=>
                And(()=>  
                     inclusive_or_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('&','&')
                            && S()
                            && inclusive_or_expression() ) ) ) );
		}
        public bool conditional_or_expression()    /*^conditional_or_expression: 	conditional_and_expression ('||' S conditional_and_expression)*;*/
        {

           return TreeAST((int)ECSharp3.conditional_or_expression,()=>
                And(()=>  
                     conditional_and_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char('|','|')
                            && S()
                            && conditional_and_expression() ) ) ) );
		}
        public bool null_coalescing_expression()    /*^null_coalescing_expression: 	conditional_or_expression ('??' S null_coalescing_expression)?;*/
        {

           return TreeAST((int)ECSharp3.null_coalescing_expression,()=>
                And(()=>  
                     conditional_or_expression()
                  && Option(()=>    
                      And(()=>      
                               Char('?','?')
                            && S()
                            && null_coalescing_expression() ) ) ) );
		}
        public bool conditional_expression()    /*^conditional_expression: 	null_coalescing_expression if_else_expression?;*/
        {

           return TreeAST((int)ECSharp3.conditional_expression,()=>
                And(()=>  
                     null_coalescing_expression()
                  && Option(()=> if_else_expression() ) ) );
		}
        public bool if_else_expression()    /*^if_else_expression:            ('?' S   expression   ':' S   expression);*/
        {

           return TreeAST((int)ECSharp3.if_else_expression,()=>
                And(()=>  
                     Char('?')
                  && S()
                  && expression()
                  && Char(':')
                  && S()
                  && expression() ) );
		}
        public bool lambda_expression()    /*^lambda_expression: 		anonymous_function_signature   '=>' S   anonymous_function_body;*/
        {

           return TreeAST((int)ECSharp3.lambda_expression,()=>
                And(()=>  
                     anonymous_function_signature()
                  && Char('=','>')
                  && S()
                  && anonymous_function_body() ) );
		}
        public bool anonymous_method_expression()    /*^anonymous_method_expression: 	'delegate' S   explicit_anonymous_function_signature?   block;*/
        {

           return TreeAST((int)ECSharp3.anonymous_method_expression,()=>
                And(()=>  
                     Char("delegate")
                  && S()
                  && Option(()=> explicit_anonymous_function_signature() )
                  && block() ) );
		}
        public bool anonymous_function_signature()    /*^anonymous_function_signature: 	explicit_anonymous_function_signature  / implicit_anonymous_function_signature;*/
        {

           return TreeAST((int)ECSharp3.anonymous_function_signature,()=>
                  
                     explicit_anonymous_function_signature()
                  || implicit_anonymous_function_signature() );
		}
        public bool explicit_anonymous_function_signature()    /*^explicit_anonymous_function_signature:
				'(' S  explicit_anonymous_function_parameter_list?   ')' S;*/
        {

           return TreeAST((int)ECSharp3.explicit_anonymous_function_signature,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && Option(()=>    
                      explicit_anonymous_function_parameter_list() )
                  && Char(')')
                  && S() ) );
		}
        public bool explicit_anonymous_function_parameter_list()    /*^explicit_anonymous_function_parameter_list:
                                explicit_anonymous_function_parameter (',' S explicit_anonymous_function_parameter)*;*/
        {

           return TreeAST((int)ECSharp3.explicit_anonymous_function_parameter_list,()=>
                And(()=>  
                     explicit_anonymous_function_parameter()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && explicit_anonymous_function_parameter() ) ) ) );
		}
        public bool explicit_anonymous_function_parameter()    /*^explicit_anonymous_function_parameter: anonymous_function_parameter_modifier?   type   parameter_name S;*/
        {

           return TreeAST((int)ECSharp3.explicit_anonymous_function_parameter,()=>
                And(()=>  
                     Option(()=> anonymous_function_parameter_modifier() )
                  && type()
                  && parameter_name()
                  && S() ) );
		}
        public bool anonymous_function_parameter_modifier()    /*^anonymous_function_parameter_modifier: ^'ref' B S / ^'out' B S;*/
        {

           return TreeAST((int)ECSharp3.anonymous_function_parameter_modifier,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('r','e','f') )
                      && B()
                      && S() )
                  || And(()=>    
                         TreeChars(()=> Char('o','u','t') )
                      && B()
                      && S() ) );
		}
        public bool implicit_anonymous_function_signature()    /*^implicit_anonymous_function_signature:
				'(' S  implicit_anonymous_function_parameter_list?   ')' S
			/	implicit_anonymous_function_parameter;*/
        {

           return TreeAST((int)ECSharp3.implicit_anonymous_function_signature,()=>
                  
                     And(()=>    
                         Char('(')
                      && S()
                      && Option(()=>      
                            implicit_anonymous_function_parameter_list() )
                      && Char(')')
                      && S() )
                  || implicit_anonymous_function_parameter() );
		}
        public bool implicit_anonymous_function_parameter_list()    /*^^implicit_anonymous_function_parameter_list:
                                implicit_anonymous_function_parameter (',' S implicit_anonymous_function_parameter)*;*/
        {

           return TreeNT((int)ECSharp3.implicit_anonymous_function_parameter_list,()=>
                And(()=>  
                     implicit_anonymous_function_parameter()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && implicit_anonymous_function_parameter() ) ) ) );
		}
        public bool implicit_anonymous_function_parameter()    /*implicit_anonymous_function_parameter: 
                                parameter_name S;*/
        {

           return And(()=>    parameter_name() && S() );
		}
        public bool anonymous_function_body()    /*^anonymous_function_body: 	expression / block;*/
        {

           return TreeAST((int)ECSharp3.anonymous_function_body,()=>
                    expression() || block() );
		}
        public bool query_expression()    /*^query_expression: 		from_clause   query_body;*/
        {

           return TreeAST((int)ECSharp3.query_expression,()=>
                And(()=>    from_clause() && query_body() ) );
		}
        public bool from_clause()    /*^from_clause: 			'from' B S   (!(name S 'in' B) type)?   name S ![;=,]  'in' B S  @expression;*/
        {

           return TreeAST((int)ECSharp3.from_clause,()=>
                And(()=>  
                     Char('f','r','o','m')
                  && B()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Not(()=>        
                                    And(()=>          
                                                 name()
                                              && S()
                                              && Char('i','n')
                                              && B() ) )
                            && type() ) )
                  && name()
                  && S()
                  && Not(()=> OneOf(";=,") )
                  && Char('i','n')
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected")) ) );
		}
        public bool query_body()    /*^query_body: 			query_body_clauses?   select_or_group_clause   query_continuation?;*/
        {

           return TreeAST((int)ECSharp3.query_body,()=>
                And(()=>  
                     Option(()=> query_body_clauses() )
                  && select_or_group_clause()
                  && Option(()=> query_continuation() ) ) );
		}
        public bool query_body_clauses()    /*^query_body_clauses: 		query_body_clause+;*/
        {

           return TreeAST((int)ECSharp3.query_body_clauses,()=>
                PlusRepeat(()=> query_body_clause() ) );
		}
        public bool query_body_clause()    /*^query_body_clause: 		from_clause / let_clause / where_clause / join_into_clause  /  orderby_clause;*/
        {

           return TreeAST((int)ECSharp3.query_body_clause,()=>
                  
                     from_clause()
                  || let_clause()
                  || where_clause()
                  || join_into_clause()
                  || orderby_clause() );
		}
        public bool let_clause()    /*^let_clause: 			'let' B S   @name S  '=' S   @expression;*/
        {

           return TreeAST((int)ECSharp3.let_clause,()=>
                And(()=>  
                     Char('l','e','t')
                  && B()
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && Char('=')
                  && S()
                  && (    expression() || Fatal("<<expression>> expected")) ) );
		}
        public bool where_clause()    /*^where_clause: 			'where' B S   @boolean_expression;*/
        {

           return TreeAST((int)ECSharp3.where_clause,()=>
                And(()=>  
                     Char('w','h','e','r','e')
                  && B()
                  && S()
                  && (    
                         boolean_expression()
                      || Fatal("<<boolean_expression>> expected")) ) );
		}
        public bool join_into_clause()    /*^join_into_clause: 			'join' B S   (!(name S 'in' B) @type)?   @name S  ^'in' B S   @expression  
				 ^'on' B S   @expression   ^'equals' B S   @expression  (^'into' B S   @name S)?;*/
        {

           return TreeAST((int)ECSharp3.join_into_clause,()=>
                And(()=>  
                     Char('j','o','i','n')
                  && B()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Not(()=>        
                                    And(()=>          
                                                 name()
                                              && S()
                                              && Char('i','n')
                                              && B() ) )
                            && (    type() || Fatal("<<type>> expected")) ) )
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && TreeChars(()=> Char('i','n') )
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && TreeChars(()=> Char('o','n') )
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && TreeChars(()=> Char('e','q','u','a','l','s') )
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && Option(()=>    
                      And(()=>      
                               TreeChars(()=> Char('i','n','t','o') )
                            && B()
                            && S()
                            && (    name() || Fatal("<<name>> expected"))
                            && S() ) ) ) );
		}
        public bool orderby_clause()    /*^^orderby_clause: 		'orderby' B S   @orderings;*/
        {

           return TreeNT((int)ECSharp3.orderby_clause,()=>
                And(()=>  
                     Char('o','r','d','e','r','b','y')
                  && B()
                  && S()
                  && (    orderings() || Fatal("<<orderings>> expected")) ) );
		}
        public bool orderings()    /*orderings: 			ordering  (',' S   @ordering)*;*/
        {

           return And(()=>  
                     ordering()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    ordering() || Fatal("<<ordering>> expected")) ) ) );
		}
        public bool ordering()    /*^^ordering: 			expression    ordering_direction?;*/
        {

           return TreeNT((int)ECSharp3.ordering,()=>
                And(()=>  
                     expression()
                  && Option(()=> ordering_direction() ) ) );
		}
        public bool ordering_direction()    /*^ordering_direction: 		('ascending' / 'descending' ) B S;*/
        {

           return TreeAST((int)ECSharp3.ordering_direction,()=>
                And(()=>  
                     (    Char("ascending") || Char("descending"))
                  && B()
                  && S() ) );
		}
        public bool select_or_group_clause()    /*select_or_group_clause: 	select_clause / group_clause;*/
        {

           return     select_clause() || group_clause();
		}
        public bool select_clause()    /*^^select_clause: 		'select' B S   @expression;*/
        {

           return TreeNT((int)ECSharp3.select_clause,()=>
                And(()=>  
                     Char('s','e','l','e','c','t')
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected")) ) );
		}
        public bool group_clause()    /*^^group_clause:  		'group' B S   @expression   'by' B S   @expression;*/
        {

           return TreeNT((int)ECSharp3.group_clause,()=>
                And(()=>  
                     Char('g','r','o','u','p')
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && Char('b','y')
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected")) ) );
		}
        public bool query_continuation()    /*^query_continuation: 		'into' B S   @name S  @query_body;*/
        {

           return TreeAST((int)ECSharp3.query_continuation,()=>
                And(()=>  
                     Char('i','n','t','o')
                  && B()
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && (    query_body() || Fatal("<<query_body>> expected")) ) );
		}
        public bool assignment()    /*^assignment: 			unary_expression   assignment_operator S   expression;*/
        {

           return TreeAST((int)ECSharp3.assignment,()=>
                And(()=>  
                     unary_expression()
                  && assignment_operator()
                  && S()
                  && expression() ) );
		}
        public bool assignment_operator()    /*^assignment_operator: 		'=' !'>' / '+=' / '-=' / '*=' / '/=' / '%=' / '&=' / '|=' / '^=' / '<<=' / '>>=';*/
        {

           return TreeAST((int)ECSharp3.assignment_operator,()=>
                  
                     And(()=>    Char('=') && Not(()=> Char('>') ) )
                  || Char('+','=')
                  || Char('-','=')
                  || Char('*','=')
                  || Char('/','=')
                  || Char('%','=')
                  || Char('&','=')
                  || Char('|','=')
                  || Char('^','=')
                  || Char('<','<','=')
                  || Char('>','>','=') );
		}
        public bool expression()    /*^expression: 			![,)};] (assignment / non_assignment_expression) ;*/
        {

           return TreeAST((int)ECSharp3.expression,()=>
                And(()=>  
                     Not(()=> OneOf(",)};") )
                  && (    assignment() || non_assignment_expression()) ) );
		}
        public bool non_assignment_expression()    /*^non_assignment_expression: 	query_expression / lambda_expression / conditional_expression;*/
        {

           return TreeAST((int)ECSharp3.non_assignment_expression,()=>
                  
                     query_expression()
                  || lambda_expression()
                  || conditional_expression() );
		}
        public bool constant_expression()    /*^^constant_expression: 		expression;*/
        {

           return TreeNT((int)ECSharp3.constant_expression,()=>
                expression() );
		}
        public bool boolean_expression()    /*^^boolean_expression: 		expression;
//A.2.5 Statements
//----------------------*/
        {

           return TreeNT((int)ECSharp3.boolean_expression,()=>
                expression() );
		}
        public bool statement()    /*statement: 			labeled_statement / declaration_statement / embedded_statement;*/
        {

           return   
                     labeled_statement()
                  || declaration_statement()
                  || embedded_statement();
		}
        public bool embedded_statement()    /*embedded_statement: 		checked_statement / unchecked_statement / using_statement  / expression_statement / 
				block / empty_statement / selection_statement / iteration_statement / 
				jump_statement / try_statement  / lock_statement / 
				 yield_statement / unsafe_statement /fixed_statement ;*/
        {

           return   
                     checked_statement()
                  || unchecked_statement()
                  || using_statement()
                  || expression_statement()
                  || block()
                  || empty_statement()
                  || selection_statement()
                  || iteration_statement()
                  || jump_statement()
                  || try_statement()
                  || lock_statement()
                  || yield_statement()
                  || unsafe_statement()
                  || fixed_statement();
		}
        public bool block()    /*^^block: 			'{' S  statement_list?   @'}' S ;*/
        {

           return TreeNT((int)ECSharp3.block,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> statement_list() )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool statement_list()    /*statement_list: 		statement+;*/
        {

           return PlusRepeat(()=> statement() );
		}
        public bool empty_statement()    /*^^empty_statement: 		';' S;*/
        {

           return TreeNT((int)ECSharp3.empty_statement,()=>
                And(()=>    Char(';') && S() ) );
		}
        public bool labeled_statement()    /*^^labeled_statement: 		label S  ':' !':' S @statement;*/
        {

           return TreeNT((int)ECSharp3.labeled_statement,()=>
                And(()=>  
                     label()
                  && S()
                  && Char(':')
                  && Not(()=> Char(':') )
                  && S()
                  && (    statement() || Fatal("<<statement>> expected")) ) );
		}
        public bool label()    /*^^label:			identifier;*/
        {

           return TreeNT((int)ECSharp3.label,()=> identifier() );
		}
        public bool declaration_statement()    /*^declaration_statement: 	local_variable_declaration   ';' S  /  local_constant_declaration   ';' S;*/
        {

           return TreeAST((int)ECSharp3.declaration_statement,()=>
                  
                     And(()=>    
                         local_variable_declaration()
                      && Char(';')
                      && S() )
                  || And(()=>    
                         local_constant_declaration()
                      && Char(';')
                      && S() ) );
		}
        public bool local_variable_declaration()    /*^^local_variable_declaration: 	local_variable_type   local_variable_declarators;*/
        {

           return TreeNT((int)ECSharp3.local_variable_declaration,()=>
                And(()=>  
                     local_variable_type()
                  && local_variable_declarators() ) );
		}
        public bool local_variable_type()    /*^local_variable_type: 		^'var' B S / type;*/
        {

           return TreeAST((int)ECSharp3.local_variable_type,()=>
                  
                     And(()=>    
                         TreeChars(()=> Char('v','a','r') )
                      && B()
                      && S() )
                  || type() );
		}
        public bool local_variable_declarators()    /*local_variable_declarators: 	local_variable_declarator  (',' S   @local_variable_declarator)*;*/
        {

           return And(()=>  
                     local_variable_declarator()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (        
                                       local_variable_declarator()
                                    || Fatal("<<local_variable_declarator>> expected")) ) ) );
		}
        public bool local_variable_declarator()    /*^^local_variable_declarator: 	variable_name S ('=' S local_variable_initializer)?;*/
        {

           return TreeNT((int)ECSharp3.local_variable_declarator,()=>
                And(()=>  
                     variable_name()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && local_variable_initializer() ) ) ) );
		}
        public bool variable_name()    /*^^variable_name:		identifier;*/
        {

           return TreeNT((int)ECSharp3.variable_name,()=>
                identifier() );
		}
        public bool local_variable_initializer()    /*^^local_variable_initializer: 	expression / array_initializer / stackalloc_initializer;*/
        {

           return TreeNT((int)ECSharp3.local_variable_initializer,()=>
                  
                     expression()
                  || array_initializer()
                  || stackalloc_initializer() );
		}
        public bool stackalloc_initializer()    /*^^stackalloc_initializer:	'stackalloc' B S   type   '[' S   expression   ']' S;*/
        {

           return TreeNT((int)ECSharp3.stackalloc_initializer,()=>
                And(()=>  
                     Char("stackalloc")
                  && B()
                  && S()
                  && type()
                  && Char('[')
                  && S()
                  && expression()
                  && Char(']')
                  && S() ) );
		}
        public bool local_constant_declaration()    /*^^local_constant_declaration: 	'const' B S   @type   constant_declarators;*/
        {

           return TreeNT((int)ECSharp3.local_constant_declaration,()=>
                And(()=>  
                     Char('c','o','n','s','t')
                  && B()
                  && S()
                  && (    type() || Fatal("<<type>> expected"))
                  && constant_declarators() ) );
		}
        public bool constant_declarators()    /*constant_declarators: 		constant_declarator (',' S @constant_declarator)*;*/
        {

           return And(()=>  
                     constant_declarator()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (        
                                       constant_declarator()
                                    || Fatal("<<constant_declarator>> expected")) ) ) );
		}
        public bool constant_declarator()    /*^^constant_declarator: 		constant_name S  '=' S   @constant_expression;*/
        {

           return TreeNT((int)ECSharp3.constant_declarator,()=>
                And(()=>  
                     constant_name()
                  && S()
                  && Char('=')
                  && S()
                  && (    
                         constant_expression()
                      || Fatal("<<constant_expression>> expected")) ) );
		}
        public bool constant_name()    /*^^constant_name:		identifier;*/
        {

           return TreeNT((int)ECSharp3.constant_name,()=>
                identifier() );
		}
        public bool expression_statement()    /*^expression_statement: 		statement_expression   ';' S;*/
        {

           return TreeAST((int)ECSharp3.expression_statement,()=>
                And(()=>    statement_expression() && Char(';') && S() ) );
		}
        public bool unsafe_statement()    /*unsafe_statement:		'unsafe' B S block;*/
        {

           return And(()=>  
                     Char('u','n','s','a','f','e')
                  && B()
                  && S()
                  && block() );
		}
        public bool fixed_statement()    /*fixed_statement:		'fixed' S  '(' S   pointer_type   fixed_pointer_declarators   ')' S   embedded_statement;*/
        {

           return And(()=>  
                     Char('f','i','x','e','d')
                  && S()
                  && Char('(')
                  && S()
                  && pointer_type()
                  && fixed_pointer_declarators()
                  && Char(')')
                  && S()
                  && embedded_statement() );
		}
        public bool fixed_pointer_declarators()    /*fixed_pointer_declarators:	fixed_pointer_declarator (',' S fixed_pointer_declarator)*;*/
        {

           return And(()=>  
                     fixed_pointer_declarator()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && fixed_pointer_declarator() ) ) );
		}
        public bool fixed_pointer_declarator()    /*fixed_pointer_declarator:	name S  '=' S   fixed_pointer_initializer;*/
        {

           return And(()=>  
                     name()
                  && S()
                  && Char('=')
                  && S()
                  && fixed_pointer_initializer() );
		}
        public bool fixed_pointer_initializer()    /*fixed_pointer_initializer:	(^'&' S)?   expression;*/
        {

           return And(()=>  
                     Option(()=>    
                      And(()=>    TreeChars(()=> Char('&') ) && S() ) )
                  && expression() );
		}
        public bool statement_expression()    /*statement_expression: 		pre_increment_expression   / 
                                pre_decrement_expression   /
                                assignment                 / 
                                call_or_post_incr_decr    /
				object_creation_expression;*/
        {

           return   
                     pre_increment_expression()
                  || pre_decrement_expression()
                  || assignment()
                  || call_or_post_incr_decr()
                  || object_creation_expression();
		}
        public bool call_or_post_incr_decr()    /*^^call_or_post_incr_decr:       primary_expression 
                                ( 
                                   (member_access/element_access) &postfix_operation / invocation / post_incr / post_decr 
                                 )+;*/
        {

           return TreeNT((int)ECSharp3.call_or_post_incr_decr,()=>
                And(()=>  
                     primary_expression()
                  && PlusRepeat(()=>    
                            
                               And(()=>        
                                       (    member_access() || element_access())
                                    && Peek(()=> postfix_operation() ) )
                            || invocation()
                            || post_incr()
                            || post_decr() ) ) );
		}
        public bool selection_statement()    /*selection_statement: 		if_statement / switch_statement;*/
        {

           return     if_statement() || switch_statement();
		}
        public bool if_statement()    /*^if_statement: 			'if'  B S   @'(' S   boolean_expression   ')' S   embedded_statement ( 'else' S   embedded_statement)?;*/
        {

           return TreeAST((int)ECSharp3.if_statement,()=>
                And(()=>  
                     Char('i','f')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && boolean_expression()
                  && Char(')')
                  && S()
                  && embedded_statement()
                  && Option(()=>    
                      And(()=>      
                               Char('e','l','s','e')
                            && S()
                            && embedded_statement() ) ) ) );
		}
        public bool switch_statement()    /*^switch_statement: 		'switch' B S   @'(' S   @expression   @')' S   switch_block;*/
        {

           return TreeAST((int)ECSharp3.switch_statement,()=>
                And(()=>  
                     Char('s','w','i','t','c','h')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && switch_block() ) );
		}
        public bool switch_block()    /*^^switch_block: 		'{' S   switch_sections?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.switch_block,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> switch_sections() )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool switch_sections()    /*switch_sections: 		switch_section+;*/
        {

           return PlusRepeat(()=> switch_section() );
		}
        public bool switch_section()    /*^switch_section: 		switch_labels   statement_list;*/
        {

           return TreeAST((int)ECSharp3.switch_section,()=>
                And(()=>    switch_labels() && statement_list() ) );
		}
        public bool switch_labels()    /*switch_labels: 			switch_label+;*/
        {

           return PlusRepeat(()=> switch_label() );
		}
        public bool switch_label()    /*^^switch_label: 		'case' B S   @constant_expression   @':' S / 'default' S   @':' S ;*/
        {

           return TreeNT((int)ECSharp3.switch_label,()=>
                  
                     And(()=>    
                         Char('c','a','s','e')
                      && B()
                      && S()
                      && (      
                               constant_expression()
                            || Fatal("<<constant_expression>> expected"))
                      && (    Char(':') || Fatal("<<':'>> expected"))
                      && S() )
                  || And(()=>    
                         Char('d','e','f','a','u','l','t')
                      && S()
                      && (    Char(':') || Fatal("<<':'>> expected"))
                      && S() ) );
		}
        public bool iteration_statement()    /*iteration_statement : 		while_statement / do_statement / for_statement / foreach_statement;*/
        {

           return   
                     while_statement()
                  || do_statement()
                  || for_statement()
                  || foreach_statement();
		}
        public bool while_statement()    /*^while_statement: 		'while' B S   @'(' S   boolean_expression   @')' S   @embedded_statement;*/
        {

           return TreeAST((int)ECSharp3.while_statement,()=>
                And(()=>  
                     Char('w','h','i','l','e')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && boolean_expression()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected")) ) );
		}
        public bool do_statement()    /*^do_statement: 			'do' B S   @embedded_statement   @'while' S   @'(' S  @boolean_expression   @')' S   ';' S ;*/
        {

           return TreeAST((int)ECSharp3.do_statement,()=>
                And(()=>  
                     Char('d','o')
                  && B()
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected"))
                  && (    
                         Char('w','h','i','l','e')
                      || Fatal("<<'while'>> expected"))
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && (    
                         boolean_expression()
                      || Fatal("<<boolean_expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && Char(';')
                  && S() ) );
		}
        public bool for_statement()    /*^^for_statement: 		'for'  B S   @'(' S  for_initializer?   @';' S   for_condition?   @';' S   for_iterator?   @')' S   @embedded_statement;*/
        {

           return TreeNT((int)ECSharp3.for_statement,()=>
                And(()=>  
                     Char('f','o','r')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && Option(()=> for_initializer() )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S()
                  && Option(()=> for_condition() )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S()
                  && Option(()=> for_iterator() )
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected")) ) );
		}
        public bool for_initializer()    /*^^for_initializer: 		local_variable_declaration / statement_expression_list;*/
        {

           return TreeNT((int)ECSharp3.for_initializer,()=>
                  
                     local_variable_declaration()
                  || statement_expression_list() );
		}
        public bool for_condition()    /*^^for_condition: 		boolean_expression;*/
        {

           return TreeNT((int)ECSharp3.for_condition,()=>
                boolean_expression() );
		}
        public bool for_iterator()    /*^^for_iterator: 		statement_expression_list;*/
        {

           return TreeNT((int)ECSharp3.for_iterator,()=>
                statement_expression_list() );
		}
        public bool statement_expression_list()    /*statement_expression_list: 	statement_expression (',' S   @statement_expression)*;*/
        {

           return And(()=>  
                     statement_expression()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (        
                                       statement_expression()
                                    || Fatal("<<statement_expression>> expected")) ) ) );
		}
        public bool foreach_statement()    /*^foreach_statement: 		'foreach' B S   @'(' S   local_variable_type   @variable_name B S  @'in' B S   @expression   @')' S   @embedded_statement;*/
        {

           return TreeAST((int)ECSharp3.foreach_statement,()=>
                And(()=>  
                     Char('f','o','r','e','a','c','h')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && local_variable_type()
                  && (    
                         variable_name()
                      || Fatal("<<variable_name>> expected"))
                  && B()
                  && S()
                  && (    Char('i','n') || Fatal("<<'in'>> expected"))
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected")) ) );
		}
        public bool jump_statement()    /*jump_statement: 		break_statement / continue_statement / goto_statement / return_statement / throw_statement;*/
        {

           return   
                     break_statement()
                  || continue_statement()
                  || goto_statement()
                  || return_statement()
                  || throw_statement();
		}
        public bool break_statement()    /*^^break_statement: 		'break' S   ';' S;*/
        {

           return TreeNT((int)ECSharp3.break_statement,()=>
                And(()=>  
                     Char('b','r','e','a','k')
                  && S()
                  && Char(';')
                  && S() ) );
		}
        public bool continue_statement()    /*^^continue_statement: 		'continue' S   ';' S;*/
        {

           return TreeNT((int)ECSharp3.continue_statement,()=>
                And(()=>    Char("continue") && S() && Char(';') && S() ) );
		}
        public bool goto_statement()    /*^^goto_statement: 		'goto'  B S   @(label S    / 'case' B S   @constant_expression    / 'default' S )  @';' S;*/
        {

           return TreeNT((int)ECSharp3.goto_statement,()=>
                And(()=>  
                     Char('g','o','t','o')
                  && B()
                  && S()
                  && (    
                         (      
                               And(()=>    label() && S() )
                            || And(()=>        
                                       Char('c','a','s','e')
                                    && B()
                                    && S()
                                    && (          
                                                 constant_expression()
                                              || Fatal("<<constant_expression>> expected")) )
                            || And(()=>        
                                       Char('d','e','f','a','u','l','t')
                                    && S() ))
                      || Fatal("<<(label S  or  'case' B S @constant_expression  or  'default' S )>> expected"))
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool return_statement()    /*^^return_statement: 		'return' B S   expression?   @';' S;*/
        {

           return TreeNT((int)ECSharp3.return_statement,()=>
                And(()=>  
                     Char('r','e','t','u','r','n')
                  && B()
                  && S()
                  && Option(()=> expression() )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool throw_statement()    /*^^throw_statement: 		'throw' B S   expression?   @';' S;*/
        {

           return TreeNT((int)ECSharp3.throw_statement,()=>
                And(()=>  
                     Char('t','h','r','o','w')
                  && B()
                  && S()
                  && Option(()=> expression() )
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool try_statement()    /*^^try_statement: 		'try' B S   @block   (catch_clauses finally_clause? / finally_clause );*/
        {

           return TreeNT((int)ECSharp3.try_statement,()=>
                And(()=>  
                     Char('t','r','y')
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected"))
                  && (    
                         And(()=>      
                               catch_clauses()
                            && Option(()=> finally_clause() ) )
                      || finally_clause()) ) );
		}
        public bool catch_clauses()    /*^^catch_clauses: 		specific_catch_clauses   general_catch_clause? / general_catch_clause;*/
        {

           return TreeNT((int)ECSharp3.catch_clauses,()=>
                  
                     And(()=>    
                         specific_catch_clauses()
                      && Option(()=> general_catch_clause() ) )
                  || general_catch_clause() );
		}
        public bool specific_catch_clauses()    /*specific_catch_clauses: 	specific_catch_clause+;*/
        {

           return PlusRepeat(()=> specific_catch_clause() );
		}
        public bool specific_catch_clause()    /*^^specific_catch_clause: 	'catch' B S   '('  S class_type   variable_name? S  @')' S   @block;*/
        {

           return TreeNT((int)ECSharp3.specific_catch_clause,()=>
                And(()=>  
                     Char('c','a','t','c','h')
                  && B()
                  && S()
                  && Char('(')
                  && S()
                  && class_type()
                  && Option(()=> variable_name() )
                  && S()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool general_catch_clause()    /*^^general_catch_clause: 	'catch' B S   block;*/
        {

           return TreeNT((int)ECSharp3.general_catch_clause,()=>
                And(()=>  
                     Char('c','a','t','c','h')
                  && B()
                  && S()
                  && block() ) );
		}
        public bool finally_clause()    /*^^finally_clause: 		'finally' B S   @block;*/
        {

           return TreeNT((int)ECSharp3.finally_clause,()=>
                And(()=>  
                     Char('f','i','n','a','l','l','y')
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool checked_statement()    /*^^checked_statement: 		'checked' B S   @block;*/
        {

           return TreeNT((int)ECSharp3.checked_statement,()=>
                And(()=>  
                     Char('c','h','e','c','k','e','d')
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool unchecked_statement()    /*^^unchecked_statement: 		'unchecked' B S   @block;*/
        {

           return TreeNT((int)ECSharp3.unchecked_statement,()=>
                And(()=>  
                     Char("unchecked")
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool lock_statement()    /*^^lock_statement: 		'lock' B S   @'(' S  @expression   @')' S   @embedded_statement;*/
        {

           return TreeNT((int)ECSharp3.lock_statement,()=>
                And(()=>  
                     Char('l','o','c','k')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && (    expression() || Fatal("<<expression>> expected"))
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected")) ) );
		}
        public bool using_statement()    /*^^using_statement: 		'using' B S  @'(' S   resource_acquisition   @')' S    @embedded_statement;*/
        {

           return TreeNT((int)ECSharp3.using_statement,()=>
                And(()=>  
                     Char('u','s','i','n','g')
                  && B()
                  && S()
                  && (    Char('(') || Fatal("<<'('>> expected"))
                  && S()
                  && resource_acquisition()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S()
                  && (    
                         embedded_statement()
                      || Fatal("<<embedded_statement>> expected")) ) );
		}
        public bool resource_acquisition()    /*^^resource_acquisition: 	local_variable_declaration / expression;*/
        {

           return TreeNT((int)ECSharp3.resource_acquisition,()=>
                    local_variable_declaration() || expression() );
		}
        public bool yield_statement()    /*^^yield_statement: 		'yield' B S   ('break' S /'return' B S   @expression)   @';' S ;
//A.2.6 Namespaces
//----------------------*/
        {

           return TreeNT((int)ECSharp3.yield_statement,()=>
                And(()=>  
                     Char('y','i','e','l','d')
                  && B()
                  && S()
                  && (    
                         And(()=>    Char('b','r','e','a','k') && S() )
                      || And(()=>      
                               Char('r','e','t','u','r','n')
                            && B()
                            && S()
                            && (    expression() || Fatal("<<expression>> expected")) ))
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool compilation_unit()    /*^^compilation_unit: 		S extern_alias_directives?   using_directives?  global_attributes? namespace_member_declarations?
				(!./FATAL<"following code not recognized as C# source">);*/
        {

           return TreeNT((int)ECSharp3.compilation_unit,()=>
                And(()=>  
                     S()
                  && Option(()=> extern_alias_directives() )
                  && Option(()=> using_directives() )
                  && Option(()=> global_attributes() )
                  && Option(()=> namespace_member_declarations() )
                  && (    
                         Not(()=> Any() )
                      || Fatal("following code not recognized as C# source")) ) );
		}
        public bool namespace_declaration()    /*^^namespace_declaration: 	'namespace' B S   @qualified_identifier S  @namespace_body   ;*/
        {

           return TreeNT((int)ECSharp3.namespace_declaration,()=>
                And(()=>  
                     Char("namespace")
                  && B()
                  && S()
                  && (    
                         qualified_identifier()
                      || Fatal("<<qualified_identifier>> expected"))
                  && S()
                  && (    
                         namespace_body()
                      || Fatal("<<namespace_body>> expected")) ) );
		}
        public bool qualified_identifier()    /*^^qualified_identifier: 	name S ('.' S name S)* ;*/
        {

           return TreeNT((int)ECSharp3.qualified_identifier,()=>
                And(()=>  
                     name()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>    Char('.') && S() && name() && S() ) ) ) );
		}
        public bool namespace_body()    /*^^namespace_body:		'{' S  extern_alias_directives?   using_directives?   namespace_member_declarations?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.namespace_body,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> extern_alias_directives() )
                  && Option(()=> using_directives() )
                  && Option(()=> namespace_member_declarations() )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool extern_alias_directives()    /*^^extern_alias_directives:	extern_alias_directive+;*/
        {

           return TreeNT((int)ECSharp3.extern_alias_directives,()=>
                PlusRepeat(()=> extern_alias_directive() ) );
		}
        public bool extern_alias_directive()    /*^^extern_alias_directive: 	'extern' B S   'alias' B S   alias_name S  ';' S;*/
        {

           return TreeNT((int)ECSharp3.extern_alias_directive,()=>
                And(()=>  
                     Char('e','x','t','e','r','n')
                  && B()
                  && S()
                  && Char('a','l','i','a','s')
                  && B()
                  && S()
                  && alias_name()
                  && S()
                  && Char(';')
                  && S() ) );
		}
        public bool alias_name()    /*^^alias_name:			identifier;*/
        {

           return TreeNT((int)ECSharp3.alias_name,()=> identifier() );
		}
        public bool using_directives()    /*^^using_directives: 		using_directive+;*/
        {

           return TreeNT((int)ECSharp3.using_directives,()=>
                PlusRepeat(()=> using_directive() ) );
		}
        public bool using_directive()    /*using_directive: 		using_alias_directive / using_namespace_directive;*/
        {

           return     using_alias_directive() || using_namespace_directive();
		}
        public bool using_alias_directive()    /*^^using_alias_directive: 	'using' B S   using_alias_name S  '=' S   @namespace_or_type_name   @';' S;*/
        {

           return TreeNT((int)ECSharp3.using_alias_directive,()=>
                And(()=>  
                     Char('u','s','i','n','g')
                  && B()
                  && S()
                  && using_alias_name()
                  && S()
                  && Char('=')
                  && S()
                  && (    
                         namespace_or_type_name()
                      || Fatal("<<namespace_or_type_name>> expected"))
                  && (    Char(';') || Fatal("<<';'>> expected"))
                  && S() ) );
		}
        public bool using_alias_name()    /*^^using_alias_name:		identifier;*/
        {

           return TreeNT((int)ECSharp3.using_alias_name,()=>
                identifier() );
		}
        public bool using_namespace_directive()    /*^^using_namespace_directive: 	'using' B S   namespace_name   ';' S;*/
        {

           return TreeNT((int)ECSharp3.using_namespace_directive,()=>
                And(()=>  
                     Char('u','s','i','n','g')
                  && B()
                  && S()
                  && namespace_name()
                  && Char(';')
                  && S() ) );
		}
        public bool namespace_member_declarations()    /*^^namespace_member_declarations:  namespace_member_declaration+;*/
        {

           return TreeNT((int)ECSharp3.namespace_member_declarations,()=>
                PlusRepeat(()=> namespace_member_declaration() ) );
		}
        public bool namespace_member_declaration()    /*namespace_member_declaration: 	namespace_declaration / type_declaration;*/
        {

           return     namespace_declaration() || type_declaration();
		}
        public bool type_declaration()    /*type_declaration: 		class_declaration / struct_declaration / interface_declaration / enum_declaration / delegate_declaration;*/
        {

           return   
                     class_declaration()
                  || struct_declaration()
                  || interface_declaration()
                  || enum_declaration()
                  || delegate_declaration();
		}
        public bool qualified_alias_member()    /*^qualified_alias_member: 	name S  '::' S   @name S  type_argument_list?;
//A.2.7 Classes
//----------------------*/
        {

           return TreeAST((int)ECSharp3.qualified_alias_member,()=>
                And(()=>  
                     name()
                  && S()
                  && Char(':',':')
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && Option(()=> type_argument_list() ) ) );
		}
        public bool class_declaration()    /*^^class_declaration: 		attributes?   class_modifiers?   ('partial' B S)?   'class' B S   class_name S  type_parameter_list?
				class_base?   type_parameter_constraints_clauses?   @class_body   (';' S)?;*/
        {

           return TreeNT((int)ECSharp3.class_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> class_modifiers() )
                  && Option(()=>    
                      And(()=>      
                               Char('p','a','r','t','i','a','l')
                            && B()
                            && S() ) )
                  && Char('c','l','a','s','s')
                  && B()
                  && S()
                  && class_name()
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Option(()=> class_base() )
                  && Option(()=> type_parameter_constraints_clauses() )
                  && (    class_body() || Fatal("<<class_body>> expected"))
                  && Option(()=> And(()=>    Char(';') && S() ) ) ) );
		}
        public bool class_name()    /*^^class_name:			identifier;*/
        {

           return TreeNT((int)ECSharp3.class_name,()=> identifier() );
		}
        public bool class_modifiers()    /*class_modifiers: 		class_modifier+;*/
        {

           return PlusRepeat(()=> class_modifier() );
		}
        public bool class_modifier()    /*^^class_modifier:		('new' / 'public' / 'protected' / 'internal' / 'private' / 'abstract' / 'sealed' / 'static' / 'unsafe') B S;*/
        {

           return TreeNT((int)ECSharp3.class_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals4)
                  && B()
                  && S() ) );
		}
        public bool type_parameter_list()    /*^^type_parameter_list: 		'<' S   type_parameters   '>' S;*/
        {

           return TreeNT((int)ECSharp3.type_parameter_list,()=>
                And(()=>  
                     Char('<')
                  && S()
                  && type_parameters()
                  && Char('>')
                  && S() ) );
		}
        public bool type_parameters()    /*^^type_parameters: 		(attributes?   type_parameter)(',' S   attributes?   type_parameter)*;
//type_parameter: 		identifier S;*/
        {

           return TreeNT((int)ECSharp3.type_parameters,()=>
                And(()=>  
                     And(()=>    
                         Option(()=> attributes() )
                      && type_parameter() )
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && Option(()=> attributes() )
                            && type_parameter() ) ) ) );
		}
        public bool class_base()    /*^^class_base: 			':' S  (( class_type  (',' S   interface_type_list)?) / interface_type_list);*/
        {

           return TreeNT((int)ECSharp3.class_base,()=>
                And(()=>  
                     Char(':')
                  && S()
                  && (    
                         And(()=>      
                               class_type()
                            && Option(()=>        
                                    And(()=>          
                                                 Char(',')
                                              && S()
                                              && interface_type_list() ) ) )
                      || interface_type_list()) ) );
		}
        public bool interface_type_list()    /*interface_type_list: 		interface_type (',' S interface_type)*;*/
        {

           return And(()=>  
                     interface_type()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && interface_type() ) ) );
		}
        public bool type_parameter_constraints_clauses()    /*type_parameter_constraints_clauses: 
                                type_parameter_constraints_clause+ ;*/
        {

           return PlusRepeat(()=> type_parameter_constraints_clause() );
		}
        public bool type_parameter_constraints_clause()    /*^^type_parameter_constraints_clause: 
                                'where' B S   type_parameter   @':' S   @type_parameter_constraints;*/
        {

           return TreeNT((int)ECSharp3.type_parameter_constraints_clause,()=>
                And(()=>  
                     Char('w','h','e','r','e')
                  && B()
                  && S()
                  && type_parameter()
                  && (    Char(':') || Fatal("<<':'>> expected"))
                  && S()
                  && (    
                         type_parameter_constraints()
                      || Fatal("<<type_parameter_constraints>> expected")) ) );
		}
        public bool type_parameter_constraints()    /*^^type_parameter_constraints: 	primary_constraint (',' S (secondary_constraints (',' S constructor_constraint)?/constructor_constraint))? 
			/	secondary_constraints 	(',' S constructor_constraint)? ;*/
        {

           return TreeNT((int)ECSharp3.type_parameter_constraints,()=>
                  
                     And(()=>    
                         primary_constraint()
                      && Option(()=>      
                            And(()=>        
                                       Char(',')
                                    && S()
                                    && (          
                                                 And(()=>            
                                                             secondary_constraints()
                                                          && Option(()=>              
                                                                        And(()=>                
                                                                                           Char(',')
                                                                                        && S()
                                                                                        && constructor_constraint() ) ) )
                                              || constructor_constraint()) ) ) )
                  || And(()=>    
                         secondary_constraints()
                      && Option(()=>      
                            And(()=>        
                                       Char(',')
                                    && S()
                                    && constructor_constraint() ) ) ) );
		}
        public bool primary_constraint()    /*^^primary_constraint: 		class_type / 'class' B S / 'struct' B S;*/
        {

           return TreeNT((int)ECSharp3.primary_constraint,()=>
                  
                     class_type()
                  || And(()=>    Char('c','l','a','s','s') && B() && S() )
                  || And(()=>    
                         Char('s','t','r','u','c','t')
                      && B()
                      && S() ) );
		}
        public bool secondary_constraints()    /*^^secondary_constraints: 	(interface_type / type_parameter) (',' S   (interface_type/type_parameter))*;*/
        {

           return TreeNT((int)ECSharp3.secondary_constraints,()=>
                And(()=>  
                     (    interface_type() || type_parameter())
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && (    interface_type() || type_parameter()) ) ) ) );
		}
        public bool constructor_constraint()    /*^^constructor_constraint: 	'new' S   '('  S @')' S;*/
        {

           return TreeNT((int)ECSharp3.constructor_constraint,()=>
                And(()=>  
                     Char('n','e','w')
                  && S()
                  && Char('(')
                  && S()
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool class_body()    /*^^class_body:			'{' S   class_member_declarations?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.class_body,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> class_member_declarations() )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool class_member_declarations()    /*class_member_declarations: 	class_member_declaration+;*/
        {

           return PlusRepeat(()=> class_member_declaration() );
		}
        public bool class_member_declaration()    /*class_member_declaration: 	constant_declaration / method_declaration / field_declaration  / property_declaration / 
				event_declaration / indexer_declaration / operator_declaration / constructor_declaration / 
				destructor_declaration / static_constructor_declaration / type_declaration;*/
        {

           return   
                     constant_declaration()
                  || method_declaration()
                  || field_declaration()
                  || property_declaration()
                  || event_declaration()
                  || indexer_declaration()
                  || operator_declaration()
                  || constructor_declaration()
                  || destructor_declaration()
                  || static_constructor_declaration()
                  || type_declaration();
		}
        public bool constant_declaration()    /*^^constant_declaration: 	        attributes?   constant_modifiers?   'const' B S   @type   constant_declarators   ';' S;*/
        {

           return TreeNT((int)ECSharp3.constant_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> constant_modifiers() )
                  && Char('c','o','n','s','t')
                  && B()
                  && S()
                  && (    type() || Fatal("<<type>> expected"))
                  && constant_declarators()
                  && Char(';')
                  && S() ) );
		}
        public bool constant_modifiers()    /*constant_modifiers: 		constant_modifier+;*/
        {

           return PlusRepeat(()=> constant_modifier() );
		}
        public bool constant_modifier()    /*^^constant_modifier: 		('new'  / 'public' / 'protected' / 'internal' / 'private') B S;
//constant_declarators: 		constant_declarator (',' S   constant_declarator)+;
//constant_declarator: 		identifier S  '=' S   constant_expression;*/
        {

           return TreeNT((int)ECSharp3.constant_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e'))
                  && B()
                  && S() ) );
		}
        public bool field_declaration()    /*^^field_declaration: 		attributes?   field_modifiers?   type   variable_declarators   ';' S;*/
        {

           return TreeNT((int)ECSharp3.field_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> field_modifiers() )
                  && type()
                  && variable_declarators()
                  && Char(';')
                  && S() ) );
		}
        public bool field_modifiers()    /*field_modifiers: 		field_modifier+;*/
        {

           return PlusRepeat(()=> field_modifier() );
		}
        public bool field_modifier()    /*^^field_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'static' / 'readonly' / 'volatile' / 'unsafe') B S;*/
        {

           return TreeNT((int)ECSharp3.field_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals5)
                  && B()
                  && S() ) );
		}
        public bool variable_declarators()    /*variable_declarators: 		variable_declarator (',' S   variable_declarator)*;*/
        {

           return And(()=>  
                     variable_declarator()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && variable_declarator() ) ) );
		}
        public bool variable_declarator()    /*^^variable_declarator: 		variable_name S ('=' S   variable_initializer)?;*/
        {

           return TreeNT((int)ECSharp3.variable_declarator,()=>
                And(()=>  
                     variable_name()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && variable_initializer() ) ) ) );
		}
        public bool variable_initializer()    /*^^variable_initializer: 		expression / array_initializer;*/
        {

           return TreeNT((int)ECSharp3.variable_initializer,()=>
                    expression() || array_initializer() );
		}
        public bool method_declaration()    /*^method_declaration: 		method_header   method_body;*/
        {

           return TreeAST((int)ECSharp3.method_declaration,()=>
                And(()=>    method_header() && method_body() ) );
		}
        public bool method_header()    /*^method_header: 			attributes?   method_modifiers?   (^'partial' B S)?   return_type   member_name S  type_parameter_list?
				'(' S   formal_parameter_list?   ')' S   type_parameter_constraints_clauses?;*/
        {

           return TreeAST((int)ECSharp3.method_header,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> method_modifiers() )
                  && Option(()=>    
                      And(()=>      
                               TreeChars(()=> Char('p','a','r','t','i','a','l') )
                            && B()
                            && S() ) )
                  && return_type()
                  && member_name()
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Char('(')
                  && S()
                  && Option(()=> formal_parameter_list() )
                  && Char(')')
                  && S()
                  && Option(()=> type_parameter_constraints_clauses() ) ) );
		}
        public bool method_modifiers()    /*method_modifiers: 		method_modifier+;*/
        {

           return PlusRepeat(()=> method_modifier() );
		}
        public bool method_modifier()    /*^method_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'static' / 'virtual' / 
				 'sealed' / 'override' / 'abstract' / 'extern' / 'unsafe' ) B S;*/
        {

           return TreeAST((int)ECSharp3.method_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals6)
                  && B()
                  && S() ) );
		}
        public bool return_type()    /*^^return_type: 			type / 'void' B S;*/
        {

           return TreeNT((int)ECSharp3.return_type,()=>
                  
                     type()
                  || And(()=>    Char('v','o','i','d') && B() && S() ) );
		}
        public bool interface_name_before_member()    /*^interface_name_before_member: (name S type_argument_list? / qualified_alias_member)  
			       ( ^'.' S   @name S !(type_parameter_list? [({]) type_argument_list?)*;*/
        {

           return TreeAST((int)ECSharp3.interface_name_before_member,()=>
                And(()=>  
                     (    
                         And(()=>      
                               name()
                            && S()
                            && Option(()=> type_argument_list() ) )
                      || qualified_alias_member())
                  && OptRepeat(()=>    
                      And(()=>      
                               TreeChars(()=> Char('.') )
                            && S()
                            && (    name() || Fatal("<<name>> expected"))
                            && S()
                            && Not(()=>        
                                    And(()=>          
                                                 Option(()=> type_parameter_list() )
                                              && OneOf("({") ) )
                            && Option(()=> type_argument_list() ) ) ) ) );
		}
        public bool method_body()    /*^^method_body: 			missing_body / block;*/
        {

           return TreeNT((int)ECSharp3.method_body,()=>
                    missing_body() || block() );
		}
        public bool missing_body()    /*^^missing_body:			';' S;*/
        {

           return TreeNT((int)ECSharp3.missing_body,()=>
                And(()=>    Char(';') && S() ) );
		}
        public bool formal_parameter_list()    /*formal_parameter_list: 		parameter_array / fixed_parameters (',' S parameter_array)?;*/
        {

           return   
                     parameter_array()
                  || And(()=>    
                         fixed_parameters()
                      && Option(()=>      
                            And(()=>    Char(',') && S() && parameter_array() ) ) );
		}
        public bool fixed_parameters()    /*fixed_parameters: 		fixed_parameter (',' S   fixed_parameter)*;*/
        {

           return And(()=>  
                     fixed_parameter()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && fixed_parameter() ) ) );
		}
        public bool fixed_parameter()    /*^fixed_parameter: 		attributes?   parameter_modifier?   type   parameter_name S;*/
        {

           return TreeAST((int)ECSharp3.fixed_parameter,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> parameter_modifier() )
                  && type()
                  && parameter_name()
                  && S() ) );
		}
        public bool parameter_modifier()    /*^parameter_modifier: 		^('ref' / 'out' / 'this' ) B S;*/
        {

           return TreeAST((int)ECSharp3.parameter_modifier,()=>
                And(()=>  
                     TreeChars(()=>    
                            
                               Char('r','e','f')
                            || Char('o','u','t')
                            || Char('t','h','i','s') )
                  && B()
                  && S() ) );
		}
        public bool parameter_array()    /*^parameter_array: 		attributes?   'params' B S   @array_type   @parameter_name S;*/
        {

           return TreeAST((int)ECSharp3.parameter_array,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Char('p','a','r','a','m','s')
                  && B()
                  && S()
                  && (    array_type() || Fatal("<<array_type>> expected"))
                  && (    
                         parameter_name()
                      || Fatal("<<parameter_name>> expected"))
                  && S() ) );
		}
        public bool property_declaration()    /*^property_declaration: 		attributes?   property_modifiers?   type   member_name S  '{' S  @accessor_declarations   @'}' S;*/
        {

           return TreeAST((int)ECSharp3.property_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> property_modifiers() )
                  && type()
                  && member_name()
                  && S()
                  && Char('{')
                  && S()
                  && (    
                         accessor_declarations()
                      || Fatal("<<accessor_declarations>> expected"))
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool property_modifiers()    /*property_modifiers: 		property_modifier+;*/
        {

           return PlusRepeat(()=> property_modifier() );
		}
        public bool property_modifier()    /*^property_modifier: 		('new' / 
                                 'public' / 
                                 'protected' / 
                                 'internal' / 
                                 'private' / 
                                 'static' / 
                                 'virtual' / 
                                 'sealed' / 
                                 'override' / 
                                 'abstract' / 
                                 'extern' /
				 'unsafe') S;*/
        {

           return TreeAST((int)ECSharp3.property_modifier,()=>
                And(()=>    OneOfLiterals(optimizedLiterals7) && S() ) );
		}
        public bool member_name()    /*^^member_name: 			interface_name_before_member '.' S  name S / name S ;*/
        {

           return TreeNT((int)ECSharp3.member_name,()=>
                  
                     And(()=>    
                         interface_name_before_member()
                      && Char('.')
                      && S()
                      && name()
                      && S() )
                  || And(()=>    name() && S() ) );
		}
        public bool accessor_declarations()    /*accessor_declarations: 		get_accessor_declaration   set_accessor_declaration? / set_accessor_declaration   get_accessor_declaration?;*/
        {

           return   
                     And(()=>    
                         get_accessor_declaration()
                      && Option(()=> set_accessor_declaration() ) )
                  || And(()=>    
                         set_accessor_declaration()
                      && Option(()=> get_accessor_declaration() ) );
		}
        public bool get_accessor_declaration()    /*^^get_accessor_declaration: 	attributes?   accessor_modifier?    'get' S   accessor_body;*/
        {

           return TreeNT((int)ECSharp3.get_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> accessor_modifier() )
                  && Char('g','e','t')
                  && S()
                  && accessor_body() ) );
		}
        public bool set_accessor_declaration()    /*^^set_accessor_declaration: 	attributes?   accessor_modifier?    'set' S   accessor_body;*/
        {

           return TreeNT((int)ECSharp3.set_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> accessor_modifier() )
                  && Char('s','e','t')
                  && S()
                  && accessor_body() ) );
		}
        public bool accessor_modifier()    /*^^accessor_modifier: 		('protected' / 'internal' / 'private' / 'protected' B S  'internal' / 'internal' B S   'protected' )B S ;*/
        {

           return TreeNT((int)ECSharp3.accessor_modifier,()=>
                And(()=>  
                     (    
                         Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || And(()=>      
                               Char("protected")
                            && B()
                            && S()
                            && Char("internal") )
                      || And(()=>      
                               Char("internal")
                            && B()
                            && S()
                            && Char("protected") ))
                  && B()
                  && S() ) );
		}
        public bool accessor_body()    /*^^accessor_body: 		block / ';' S;*/
        {

           return TreeNT((int)ECSharp3.accessor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool event_declaration()    /*^event_declaration: 		attributes?   event_modifiers?   'event' B S   type   
				(	variable_declarators  ';' S
				/ 	member_name  S '{' S   event_accessor_declarations   @'}' S
				);*/
        {

           return TreeAST((int)ECSharp3.event_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> event_modifiers() )
                  && Char('e','v','e','n','t')
                  && B()
                  && S()
                  && type()
                  && (    
                         And(()=>      
                               variable_declarators()
                            && Char(';')
                            && S() )
                      || And(()=>      
                               member_name()
                            && S()
                            && Char('{')
                            && S()
                            && event_accessor_declarations()
                            && (    Char('}') || Fatal("<<'}'>> expected"))
                            && S() )) ) );
		}
        public bool event_modifiers()    /*event_modifiers: 		event_modifier+;*/
        {

           return PlusRepeat(()=> event_modifier() );
		}
        public bool event_modifier()    /*^event_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'static' / 'virtual' / 
				'sealed' / 'override' / 'abstract' / 'extern' / 'unsafe')  B S;*/
        {

           return TreeAST((int)ECSharp3.event_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals8)
                  && B()
                  && S() ) );
		}
        public bool event_accessor_declarations()    /*^event_accessor_declarations: 	add_accessor_declaration   remove_accessor_declaration / remove_accessor_declaration   add_accessor_declaration;*/
        {

           return TreeAST((int)ECSharp3.event_accessor_declarations,()=>
                  
                     And(()=>    
                         add_accessor_declaration()
                      && remove_accessor_declaration() )
                  || And(()=>    
                         remove_accessor_declaration()
                      && add_accessor_declaration() ) );
		}
        public bool add_accessor_declaration()    /*^^add_accessor_declaration: 	attributes?   'add' S   block;*/
        {

           return TreeNT((int)ECSharp3.add_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Char('a','d','d')
                  && S()
                  && block() ) );
		}
        public bool remove_accessor_declaration()    /*^^remove_accessor_declaration: 	attributes?   'remove' S   block;*/
        {

           return TreeNT((int)ECSharp3.remove_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Char('r','e','m','o','v','e')
                  && S()
                  && block() ) );
		}
        public bool indexer_declaration()    /*^indexer_declaration: 		attributes?   indexer_modifiers?   indexer_declarator   '{' S   accessor_declarations   @'}' S ;*/
        {

           return TreeAST((int)ECSharp3.indexer_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> indexer_modifiers() )
                  && indexer_declarator()
                  && Char('{')
                  && S()
                  && accessor_declarations()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool indexer_modifiers()    /*indexer_modifiers: 		indexer_modifier+;*/
        {

           return PlusRepeat(()=> indexer_modifier() );
		}
        public bool indexer_modifier()    /*^indexer_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private'  / 'virtual' / 'sealed' / 
				'override' / 'abstract' / 'extern' / 'unsafe') B S;*/
        {

           return TreeAST((int)ECSharp3.indexer_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals9)
                  && B()
                  && S() ) );
		}
        public bool indexer_declarator()    /*^indexer_declarator: 		type  (interface_type   '.' S)? 'this' S    '[' S  formal_parameter_list   @']' S;*/
        {

           return TreeAST((int)ECSharp3.indexer_declarator,()=>
                And(()=>  
                     type()
                  && Option(()=>    
                      And(()=>    interface_type() && Char('.') && S() ) )
                  && Char('t','h','i','s')
                  && S()
                  && Char('[')
                  && S()
                  && formal_parameter_list()
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S() ) );
		}
        public bool operator_declaration()    /*^operator_declaration: 		attributes?   operator_modifiers   operator_declarator   operator_body;*/
        {

           return TreeAST((int)ECSharp3.operator_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && operator_modifiers()
                  && operator_declarator()
                  && operator_body() ) );
		}
        public bool operator_modifiers()    /*operator_modifiers: 		operator_modifier+;*/
        {

           return PlusRepeat(()=> operator_modifier() );
		}
        public bool operator_modifier()    /*^operator_modifier: 		('public' / 'static' / 'extern' / 'unsafe')S;*/
        {

           return TreeAST((int)ECSharp3.operator_modifier,()=>
                And(()=>  
                     (    
                         Char('p','u','b','l','i','c')
                      || Char('s','t','a','t','i','c')
                      || Char('e','x','t','e','r','n')
                      || Char('u','n','s','a','f','e'))
                  && S() ) );
		}
        public bool operator_declarator()    /*operator_declarator: 		unary_operator_declarator / binary_operator_declarator / conversion_operator_declarator;*/
        {

           return   
                     unary_operator_declarator()
                  || binary_operator_declarator()
                  || conversion_operator_declarator();
		}
        public bool unary_operator_declarator()    /*^unary_operator_declarator: 	type   'operator' S   overloadable_unary_operator   '(' S   type   parameter_name   ')' S;*/
        {

           return TreeAST((int)ECSharp3.unary_operator_declarator,()=>
                And(()=>  
                     type()
                  && Char("operator")
                  && S()
                  && overloadable_unary_operator()
                  && Char('(')
                  && S()
                  && type()
                  && parameter_name()
                  && Char(')')
                  && S() ) );
		}
        public bool overloadable_unary_operator()    /*^overloadable_unary_operator:  	( '++' /  '--' / '+' /   '-' /    '!'  /   '~' /  'true' /   'false') S;*/
        {

           return TreeAST((int)ECSharp3.overloadable_unary_operator,()=>
                And(()=>    OneOfLiterals(optimizedLiterals10) && S() ) );
		}
        public bool binary_operator_declarator()    /*^binary_operator_declarator: 	type   'operator' S   overloadable_binary_operator   '(' S  type   parameter_name   ',' S   type   parameter_name  S ')' S;*/
        {

           return TreeAST((int)ECSharp3.binary_operator_declarator,()=>
                And(()=>  
                     type()
                  && Char("operator")
                  && S()
                  && overloadable_binary_operator()
                  && Char('(')
                  && S()
                  && type()
                  && parameter_name()
                  && Char(',')
                  && S()
                  && type()
                  && parameter_name()
                  && S()
                  && Char(')')
                  && S() ) );
		}
        public bool overloadable_binary_operator()    /*^overloadable_binary_operator: 	('+' / '-' / '*' / '/' / '%' / '&' / '|' / '^' / '<<' / '>>' / '==' / '!=' / '>=' / '<=' / '>' / '<'  ) S;*/
        {

           return TreeAST((int)ECSharp3.overloadable_binary_operator,()=>
                And(()=>    OneOfLiterals(optimizedLiterals11) && S() ) );
		}
        public bool conversion_operator_declarator()    /*^conversion_operator_declarator: ('implicit' / 'explicit' ) B S   'operator' B S   type   '(' S   type   parameter_name S   ')' S ;*/
        {

           return TreeAST((int)ECSharp3.conversion_operator_declarator,()=>
                And(()=>  
                     (    Char("implicit") || Char("explicit"))
                  && B()
                  && S()
                  && Char("operator")
                  && B()
                  && S()
                  && type()
                  && Char('(')
                  && S()
                  && type()
                  && parameter_name()
                  && S()
                  && Char(')')
                  && S() ) );
		}
        public bool operator_body()    /*^^operator_body: 		block / ';' S;*/
        {

           return TreeNT((int)ECSharp3.operator_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool constructor_declaration()    /*^constructor_declaration: 	attributes?   constructor_modifiers?   constructor_declarator   constructor_body;*/
        {

           return TreeAST((int)ECSharp3.constructor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> constructor_modifiers() )
                  && constructor_declarator()
                  && constructor_body() ) );
		}
        public bool constructor_modifiers()    /*^constructor_modifiers: 	constructor_modifier+;*/
        {

           return TreeAST((int)ECSharp3.constructor_modifiers,()=>
                PlusRepeat(()=> constructor_modifier() ) );
		}
        public bool constructor_modifier()    /*^constructor_modifier: 		('public' / 'protected' / 'internal' / 'private' / 'extern' / 'unsafe') B S;*/
        {

           return TreeAST((int)ECSharp3.constructor_modifier,()=>
                And(()=>  
                     (    
                         Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || Char('e','x','t','e','r','n')
                      || Char('u','n','s','a','f','e'))
                  && B()
                  && S() ) );
		}
        public bool constructor_declarator()    /*^constructor_declarator: 	name  S '(' S  formal_parameter_list?   ')' S   constructor_initializer?;*/
        {

           return TreeAST((int)ECSharp3.constructor_declarator,()=>
                And(()=>  
                     name()
                  && S()
                  && Char('(')
                  && S()
                  && Option(()=> formal_parameter_list() )
                  && Char(')')
                  && S()
                  && Option(()=> constructor_initializer() ) ) );
		}
        public bool constructor_initializer()    /*^constructor_initializer:  	':' S   (^'base'/^'this') S   '(' S   argument_list?   ')' S ;*/
        {

           return TreeAST((int)ECSharp3.constructor_initializer,()=>
                And(()=>  
                     Char(':')
                  && S()
                  && (    
                         TreeChars(()=> Char('b','a','s','e') )
                      || TreeChars(()=> Char('t','h','i','s') ))
                  && S()
                  && Char('(')
                  && S()
                  && Option(()=> argument_list() )
                  && Char(')')
                  && S() ) );
		}
        public bool constructor_body()    /*^constructor_body: 		block / ';' S;*/
        {

           return TreeAST((int)ECSharp3.constructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool static_constructor_declaration()    /*^static_constructor_declaration: attributes?   static_constructor_modifiers  name S  '(' S  ')' S  static_constructor_body;*/
        {

           return TreeAST((int)ECSharp3.static_constructor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && static_constructor_modifiers()
                  && name()
                  && S()
                  && Char('(')
                  && S()
                  && Char(')')
                  && S()
                  && static_constructor_body() ) );
		}
        public bool static_constructor_modifiers()    /*^static_constructor_modifiers: 	('extern' B S)? ('unsafe' B S)? 'static' B S / 
				 'static' B S ('unsafe' B S)? ('extern' B S)? /
				'static' B S ('extern' B S) ('unsafe' B S)?/
				('extern' B S)? 'static' B S ('unsafe' B S)?/ 
				('unsafe' B S)? 'static' B S ('extern' B S)? / 
				('unsafe' B S)? ('extern' B S)? 'static' B S  ;*/
        {

           return TreeAST((int)ECSharp3.static_constructor_modifiers,()=>
                  
                     And(()=>    
                         Option(()=>      
                            And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S() ) )
                      && Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) )
                      && Char('s','t','a','t','i','c')
                      && B()
                      && S() )
                  || And(()=>    
                         Char('s','t','a','t','i','c')
                      && B()
                      && S()
                      && Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) )
                      && Option(()=>      
                            And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S() ) ) )
                  || And(()=>    
                         Char('s','t','a','t','i','c')
                      && B()
                      && S()
                      && And(()=>      
                               Char('e','x','t','e','r','n')
                            && B()
                            && S() )
                      && Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) ) )
                  || And(()=>    
                         Option(()=>      
                            And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S() ) )
                      && Char('s','t','a','t','i','c')
                      && B()
                      && S()
                      && Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) ) )
                  || And(()=>    
                         Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) )
                      && Char('s','t','a','t','i','c')
                      && B()
                      && S()
                      && Option(()=>      
                            And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S() ) ) )
                  || And(()=>    
                         Option(()=>      
                            And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S() ) )
                      && Option(()=>      
                            And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S() ) )
                      && Char('s','t','a','t','i','c')
                      && B()
                      && S() ) );
		}
        public bool static_constructor_body()    /*^static_constructor_body: 	block / ';' S;*/
        {

           return TreeAST((int)ECSharp3.static_constructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool destructor_declaration()    /*^destructor_declaration: 	attributes?   ('extern' B S 'unsafe' S / 'unsafe' B S 'extern' S)?   '~' S   name  S '(' S  ')' S    destructor_body;*/
        {

           return TreeAST((int)ECSharp3.destructor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=>    
                            
                               And(()=>        
                                       Char('e','x','t','e','r','n')
                                    && B()
                                    && S()
                                    && Char('u','n','s','a','f','e')
                                    && S() )
                            || And(()=>        
                                       Char('u','n','s','a','f','e')
                                    && B()
                                    && S()
                                    && Char('e','x','t','e','r','n')
                                    && S() ) )
                  && Char('~')
                  && S()
                  && name()
                  && S()
                  && Char('(')
                  && S()
                  && Char(')')
                  && S()
                  && destructor_body() ) );
		}
        public bool destructor_body()    /*^^destructor_body: 		block / ';' S;
//A.2.8 Structs
//----------------------*/
        {

           return TreeNT((int)ECSharp3.destructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool struct_declaration()    /*^struct_declaration: 		attributes?   struct_modifiers?   (^'partial' B S)?   'struct' B S   struct_name S   type_parameter_list?
					struct_interfaces?   type_parameter_constraints_clauses?   @struct_body   ';'?;*/
        {

           return TreeAST((int)ECSharp3.struct_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> struct_modifiers() )
                  && Option(()=>    
                      And(()=>      
                               TreeChars(()=> Char('p','a','r','t','i','a','l') )
                            && B()
                            && S() ) )
                  && Char('s','t','r','u','c','t')
                  && B()
                  && S()
                  && struct_name()
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Option(()=> struct_interfaces() )
                  && Option(()=> type_parameter_constraints_clauses() )
                  && (    struct_body() || Fatal("<<struct_body>> expected"))
                  && Option(()=> Char(';') ) ) );
		}
        public bool struct_name()    /*^^struct_name:			identifier;*/
        {

           return TreeNT((int)ECSharp3.struct_name,()=> identifier() );
		}
        public bool struct_modifiers()    /*struct_modifiers: 		struct_modifier+;*/
        {

           return PlusRepeat(()=> struct_modifier() );
		}
        public bool struct_modifier()    /*^struct_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'unsafe')B S;*/
        {

           return TreeAST((int)ECSharp3.struct_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || Char('u','n','s','a','f','e'))
                  && B()
                  && S() ) );
		}
        public bool struct_interfaces()    /*^^struct_interfaces: 		':' S   interface_type_list;*/
        {

           return TreeNT((int)ECSharp3.struct_interfaces,()=>
                And(()=>    Char(':') && S() && interface_type_list() ) );
		}
        public bool struct_body()    /*^^struct_body:			'{' S   struct_member_declarations?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.struct_body,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> struct_member_declarations() )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool struct_member_declarations()    /*struct_member_declarations: 	struct_member_declaration+;*/
        {

           return PlusRepeat(()=> struct_member_declaration() );
		}
        public bool struct_member_declaration()    /*^struct_member_declaration: 	constant_declaration / field_declaration / method_declaration / property_declaration / 		
				event_declaration / indexer_declaration / operator_declaration / 
				constructor_declaration / static_constructor_declaration / type_declaration /fixed_size_buffer_declaration;*/
        {

           return TreeAST((int)ECSharp3.struct_member_declaration,()=>
                  
                     constant_declaration()
                  || field_declaration()
                  || method_declaration()
                  || property_declaration()
                  || event_declaration()
                  || indexer_declaration()
                  || operator_declaration()
                  || constructor_declaration()
                  || static_constructor_declaration()
                  || type_declaration()
                  || fixed_size_buffer_declaration() );
		}
        public bool fixed_size_buffer_declaration()    /*^^fixed_size_buffer_declaration:  attributes?   fixed_size_buffer_modifiers?   'fixed'  B S buffer_element_type fixed_size_buffer_declarators   ;*/
        {

           return TreeNT((int)ECSharp3.fixed_size_buffer_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> fixed_size_buffer_modifiers() )
                  && Char('f','i','x','e','d')
                  && B()
                  && S()
                  && buffer_element_type()
                  && fixed_size_buffer_declarators() ) );
		}
        public bool fixed_size_buffer_modifiers()    /*fixed_size_buffer_modifiers:   fixed_size_buffer_modifier+;*/
        {

           return PlusRepeat(()=> fixed_size_buffer_modifier() );
		}
        public bool fixed_size_buffer_modifier()    /*^^fixed_size_buffer_modifier:    ('new' / 'public' / 'protected' / 'internal' / 'private' / 'unsafe') B S;*/
        {

           return TreeNT((int)ECSharp3.fixed_size_buffer_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || Char('u','n','s','a','f','e'))
                  && B()
                  && S() ) );
		}
        public bool buffer_element_type()    /*^^buffer_element_type:		type;*/
        {

           return TreeNT((int)ECSharp3.buffer_element_type,()=>
                type() );
		}
        public bool fixed_size_buffer_declarators()    /*fixed_size_buffer_declarators: fixed_size_buffer_declarator+;*/
        {

           return PlusRepeat(()=> fixed_size_buffer_declarator() );
		}
        public bool fixed_size_buffer_declarator()    /*fixed_size_buffer_declarator:  name S  '[' S   constant_expression   ']' S;
//A.2.9 Arrays
//----------------------
//array_type: 			non_array_type   rank_specifiers;
//non_array_type: 		type;
//rank_specifiers: 		rank_specifier+;
//rank_specifier:		'[' S   dim_separators?   ']' S;
//dim_separators: 		',' S  (',' S)*;*/
        {

           return And(()=>  
                     name()
                  && S()
                  && Char('[')
                  && S()
                  && constant_expression()
                  && Char(']')
                  && S() );
		}
        public bool array_initializer()    /*^^array_initializer:		'{' S  variable_initializer_list? (',' S)?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.array_initializer,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> variable_initializer_list() )
                  && Option(()=> And(()=>    Char(',') && S() ) )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool variable_initializer_list()    /*variable_initializer_list: 	variable_initializer (',' S variable_initializer)*;
//variable_initializer: 		expression / array_initializer;
//A.2.10 Interfaces
//----------------------*/
        {

           return And(()=>  
                     variable_initializer()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && variable_initializer() ) ) );
		}
        public bool interface_declaration()    /*^interface_declaration: 		attributes?   interface_modifiers?   ('partial' B S)?   'interface' B S   @interface_name S   type_parameter_list?
					interface_base?   type_parameter_constraints_clauses?   interface_body   ';'? S;*/
        {

           return TreeAST((int)ECSharp3.interface_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> interface_modifiers() )
                  && Option(()=>    
                      And(()=>      
                               Char('p','a','r','t','i','a','l')
                            && B()
                            && S() ) )
                  && Char("interface")
                  && B()
                  && S()
                  && (    
                         interface_name()
                      || Fatal("<<interface_name>> expected"))
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Option(()=> interface_base() )
                  && Option(()=> type_parameter_constraints_clauses() )
                  && interface_body()
                  && Option(()=> Char(';') )
                  && S() ) );
		}
        public bool interface_name()    /*interface_name:			identifier;*/
        {

           return identifier();
		}
        public bool interface_modifiers()    /*interface_modifiers: 		interface_modifier+;*/
        {

           return PlusRepeat(()=> interface_modifier() );
		}
        public bool interface_modifier()    /*^interface_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'unsafe') B S;*/
        {

           return TreeAST((int)ECSharp3.interface_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || Char('u','n','s','a','f','e'))
                  && B()
                  && S() ) );
		}
        public bool interface_base()    /*^^interface_base: 		':' S   interface_type_list;*/
        {

           return TreeNT((int)ECSharp3.interface_base,()=>
                And(()=>    Char(':') && S() && interface_type_list() ) );
		}
        public bool interface_body()    /*^^interface_body:		'{' S   interface_member_declarations?   '}' S;*/
        {

           return TreeNT((int)ECSharp3.interface_body,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=> interface_member_declarations() )
                  && Char('}')
                  && S() ) );
		}
        public bool interface_member_declarations()    /*interface_member_declarations: 	interface_member_declaration+;*/
        {

           return PlusRepeat(()=> interface_member_declaration() );
		}
        public bool interface_member_declaration()    /*^interface_member_declaration: 	interface_method_declaration / interface_property_declaration / interface_event_declaration / interface_indexer_declaration;*/
        {

           return TreeAST((int)ECSharp3.interface_member_declaration,()=>
                  
                     interface_method_declaration()
                  || interface_property_declaration()
                  || interface_event_declaration()
                  || interface_indexer_declaration() );
		}
        public bool interface_method_declaration()    /*^interface_method_declaration: 	attributes?   ('new' B S)?   return_type   name S  type_parameter_list?
				'('  S formal_parameter_list?   ')' S   type_parameter_constraints_clauses?  ';' S ;*/
        {

           return TreeAST((int)ECSharp3.interface_method_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=>    
                      And(()=>    Char('n','e','w') && B() && S() ) )
                  && return_type()
                  && name()
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Char('(')
                  && S()
                  && Option(()=> formal_parameter_list() )
                  && Char(')')
                  && S()
                  && Option(()=> type_parameter_constraints_clauses() )
                  && Char(';')
                  && S() ) );
		}
        public bool interface_property_declaration()    /*^interface_property_declaration: attributes?   ('new' B S)?   type   name  S '{' S  interface_accessors   '}' S;*/
        {

           return TreeAST((int)ECSharp3.interface_property_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=>    
                      And(()=>    Char('n','e','w') && B() && S() ) )
                  && type()
                  && name()
                  && S()
                  && Char('{')
                  && S()
                  && interface_accessors()
                  && Char('}')
                  && S() ) );
		}
        public bool interface_accessors()    /*^interface_accessors: 		attributes? 
				(  ^'get' S ';' S (attributes? ^'set' S ';' S)? 
				/  ^'set' S ';' S (attributes? ^'get' S ';' S)?
				);*/
        {

           return TreeAST((int)ECSharp3.interface_accessors,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && (    
                         And(()=>      
                               TreeChars(()=> Char('g','e','t') )
                            && S()
                            && Char(';')
                            && S()
                            && Option(()=>        
                                    And(()=>          
                                                 Option(()=> attributes() )
                                              && TreeChars(()=> Char('s','e','t') )
                                              && S()
                                              && Char(';')
                                              && S() ) ) )
                      || And(()=>      
                               TreeChars(()=> Char('s','e','t') )
                            && S()
                            && Char(';')
                            && S()
                            && Option(()=>        
                                    And(()=>          
                                                 Option(()=> attributes() )
                                              && TreeChars(()=> Char('g','e','t') )
                                              && S()
                                              && Char(';')
                                              && S() ) ) )) ) );
		}
        public bool interface_event_declaration()    /*^interface_event_declaration: 	attributes?   ('new' B S)?   'event' B S    type   name  S   ;*/
        {

           return TreeAST((int)ECSharp3.interface_event_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=>    
                      And(()=>    Char('n','e','w') && B() && S() ) )
                  && Char('e','v','e','n','t')
                  && B()
                  && S()
                  && type()
                  && name()
                  && S() ) );
		}
        public bool interface_indexer_declaration()    /*^interface_indexer_declaration:  attributes?   ('new' B S)?   type   'this' B S   '[' S   formal_parameter_list   @']' S   '{' S   interface_accessors   @'}' S;
//A.2.11 Enums
//----------------------*/
        {

           return TreeAST((int)ECSharp3.interface_indexer_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=>    
                      And(()=>    Char('n','e','w') && B() && S() ) )
                  && type()
                  && Char('t','h','i','s')
                  && B()
                  && S()
                  && Char('[')
                  && S()
                  && formal_parameter_list()
                  && (    Char(']') || Fatal("<<']'>> expected"))
                  && S()
                  && Char('{')
                  && S()
                  && interface_accessors()
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool enum_declaration()    /*^enum_declaration: 		attributes?   enum_modifiers?   'enum' B S   enum_name S   enum_base?   enum_body   (';' S)?;*/
        {

           return TreeAST((int)ECSharp3.enum_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> enum_modifiers() )
                  && Char('e','n','u','m')
                  && B()
                  && S()
                  && enum_name()
                  && S()
                  && Option(()=> enum_base() )
                  && enum_body()
                  && Option(()=> And(()=>    Char(';') && S() ) ) ) );
		}
        public bool enum_name()    /*^^enum_name:			identifier;*/
        {

           return TreeNT((int)ECSharp3.enum_name,()=> identifier() );
		}
        public bool enum_base()    /*^^enum_base: 			':' S integral_type;*/
        {

           return TreeNT((int)ECSharp3.enum_base,()=>
                And(()=>    Char(':') && S() && integral_type() ) );
		}
        public bool enum_body()    /*^^enum_body:			'{'  S (enum_member_declarations (',' S)?)?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3.enum_body,()=>
                And(()=>  
                     Char('{')
                  && S()
                  && Option(()=>    
                      And(()=>      
                               enum_member_declarations()
                            && Option(()=> And(()=>    Char(',') && S() ) ) ) )
                  && (    Char('}') || Fatal("<<'}'>> expected"))
                  && S() ) );
		}
        public bool enum_modifiers()    /*enum_modifiers: 		enum_modifier+;*/
        {

           return PlusRepeat(()=> enum_modifier() );
		}
        public bool enum_modifier()    /*^enum_modifier: 			('new' / 'public' / 'protected' / 'internal' / 'private') B S;*/
        {

           return TreeAST((int)ECSharp3.enum_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e'))
                  && B()
                  && S() ) );
		}
        public bool enum_member_declarations()    /*enum_member_declarations: 	enum_member_declaration (',' S   enum_member_declaration)*;*/
        {

           return And(()=>  
                     enum_member_declaration()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && enum_member_declaration() ) ) );
		}
        public bool enum_member_declaration()    /*^enum_member_declaration: 	attributes?   enumerator_name S ('=' S   @constant_expression )? S;*/
        {

           return TreeAST((int)ECSharp3.enum_member_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && enumerator_name()
                  && S()
                  && Option(()=>    
                      And(()=>      
                               Char('=')
                            && S()
                            && (        
                                       constant_expression()
                                    || Fatal("<<constant_expression>> expected")) ) )
                  && S() ) );
		}
        public bool enumerator_name()    /*^^enumerator_name:		identifier;
//A.2.12 Delegates
//----------------------*/
        {

           return TreeNT((int)ECSharp3.enumerator_name,()=>
                identifier() );
		}
        public bool delegate_declaration()    /*^delegate_declaration: 		attributes?   delegate_modifiers?   'delegate' B S   return_type   delegate_name  S type_parameter_list?   
				'(' S  formal_parameter_list?   ')' S   type_parameter_constraints_clauses?   ';' S;*/
        {

           return TreeAST((int)ECSharp3.delegate_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> delegate_modifiers() )
                  && Char("delegate")
                  && B()
                  && S()
                  && return_type()
                  && delegate_name()
                  && S()
                  && Option(()=> type_parameter_list() )
                  && Char('(')
                  && S()
                  && Option(()=> formal_parameter_list() )
                  && Char(')')
                  && S()
                  && Option(()=> type_parameter_constraints_clauses() )
                  && Char(';')
                  && S() ) );
		}
        public bool delegate_name()    /*^^delegate_name:		identifier;*/
        {

           return TreeNT((int)ECSharp3.delegate_name,()=>
                identifier() );
		}
        public bool delegate_modifiers()    /*delegate_modifiers: 		delegate_modifier+;*/
        {

           return PlusRepeat(()=> delegate_modifier() );
		}
        public bool delegate_modifier()    /*^delegate_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'unsafe') B S;
//A.2.13 Attributes
//----------------------*/
        {

           return TreeAST((int)ECSharp3.delegate_modifier,()=>
                And(()=>  
                     (    
                         Char('n','e','w')
                      || Char('p','u','b','l','i','c')
                      || Char("protected")
                      || Char("internal")
                      || Char('p','r','i','v','a','t','e')
                      || Char('u','n','s','a','f','e'))
                  && B()
                  && S() ) );
		}
        public bool global_attributes()    /*^^global_attributes: 		global_attribute_sections;*/
        {

           return TreeNT((int)ECSharp3.global_attributes,()=>
                global_attribute_sections() );
		}
        public bool global_attribute_sections()    /*global_attribute_sections: 	global_attribute_section+;*/
        {

           return PlusRepeat(()=> global_attribute_section() );
		}
        public bool global_attribute_section()    /*^global_attribute_section:	'[' S   global_attribute_target_specifier   attribute_list  (',' S)?  ']' S;*/
        {

           return TreeAST((int)ECSharp3.global_attribute_section,()=>
                And(()=>  
                     Char('[')
                  && S()
                  && global_attribute_target_specifier()
                  && attribute_list()
                  && Option(()=> And(()=>    Char(',') && S() ) )
                  && Char(']')
                  && S() ) );
		}
        public bool global_attribute_target_specifier()    /*^^global_attribute_target_specifier: global_attribute_target   ':' S;*/
        {

           return TreeNT((int)ECSharp3.global_attribute_target_specifier,()=>
                And(()=>  
                     global_attribute_target()
                  && Char(':')
                  && S() ) );
		}
        public bool global_attribute_target()    /*^^global_attribute_target: 	('assembly' / 'module') B S;*/
        {

           return TreeNT((int)ECSharp3.global_attribute_target,()=>
                And(()=>  
                     (    Char("assembly") || Char('m','o','d','u','l','e'))
                  && B()
                  && S() ) );
		}
        public bool attributes()    /*^^attributes: 			attribute_sections;*/
        {

           return TreeNT((int)ECSharp3.attributes,()=>
                attribute_sections() );
		}
        public bool attribute_sections()    /*attribute_sections: 		attribute_section+;*/
        {

           return PlusRepeat(()=> attribute_section() );
		}
        public bool attribute_section()    /*^^attribute_section:		'[' S   attribute_target_specifier?   attribute_list (',' S)?  ']' S;*/
        {

           return TreeNT((int)ECSharp3.attribute_section,()=>
                And(()=>  
                     Char('[')
                  && S()
                  && Option(()=> attribute_target_specifier() )
                  && attribute_list()
                  && Option(()=> And(()=>    Char(',') && S() ) )
                  && Char(']')
                  && S() ) );
		}
        public bool attribute_target_specifier()    /*^^attribute_target_specifier: 	attribute_target   ':' S;*/
        {

           return TreeNT((int)ECSharp3.attribute_target_specifier,()=>
                And(()=>    attribute_target() && Char(':') && S() ) );
		}
        public bool attribute_target()    /*^attribute_target: 		('field' / 'event' / 'method' / 'param' / 'property' / 'return' / 'type') S;*/
        {

           return TreeAST((int)ECSharp3.attribute_target,()=>
                And(()=>  
                     (    
                         Char('f','i','e','l','d')
                      || Char('e','v','e','n','t')
                      || Char('m','e','t','h','o','d')
                      || Char('p','a','r','a','m')
                      || Char("property")
                      || Char('r','e','t','u','r','n')
                      || Char('t','y','p','e'))
                  && S() ) );
		}
        public bool attribute_list()    /*attribute_list: 		attribute (',' S   attribute)*;*/
        {

           return And(()=>  
                     attribute()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && attribute() ) ) );
		}
        public bool attribute()    /*^^attribute: 			attribute_name   attribute_arguments?;*/
        {

           return TreeNT((int)ECSharp3.attribute,()=>
                And(()=>  
                     attribute_name()
                  && Option(()=> attribute_arguments() ) ) );
		}
        public bool attribute_name()    /*^^attribute_name:  		type_name;*/
        {

           return TreeNT((int)ECSharp3.attribute_name,()=>
                type_name() );
		}
        public bool attribute_arguments()    /*^^attribute_arguments:		'('  S
				(	named_argument_list 
				/	(positional_argument_list   (',' S   named_argument_list)?)?   
				)
				')' S;*/
        {

           return TreeNT((int)ECSharp3.attribute_arguments,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && (    
                         named_argument_list()
                      || Option(()=>      
                            And(()=>        
                                       positional_argument_list()
                                    && Option(()=>          
                                              And(()=>            
                                                             Char(',')
                                                          && S()
                                                          && named_argument_list() ) ) ) ))
                  && Char(')')
                  && S() ) );
		}
        public bool positional_argument_list()    /*positional_argument_list: 	positional_argument  (',' S   positional_argument)*;*/
        {

           return And(()=>  
                     positional_argument()
                  && OptRepeat(()=>    
                      And(()=>      
                               Char(',')
                            && S()
                            && positional_argument() ) ) );
		}
        public bool positional_argument()    /*^^positional_argument: 		attribute_argument_expression;*/
        {

           return TreeNT((int)ECSharp3.positional_argument,()=>
                attribute_argument_expression() );
		}
        public bool named_argument_list()    /*named_argument_list: 		named_argument (',' S   named_argument)*;*/
        {

           return And(()=>  
                     named_argument()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && named_argument() ) ) );
		}
        public bool named_argument()    /*^^named_argument: 		parameter_name S  '=' S   attribute_argument_expression;*/
        {

           return TreeNT((int)ECSharp3.named_argument,()=>
                And(()=>  
                     parameter_name()
                  && S()
                  && Char('=')
                  && S()
                  && attribute_argument_expression() ) );
		}
        public bool parameter_name()    /*^^parameter_name:               identifier;*/
        {

           return TreeNT((int)ECSharp3.parameter_name,()=>
                identifier() );
		}
        public bool attribute_argument_expression()    /*attribute_argument_expression: 	expression;

//identifier end boundary B and white space  S*/
        {

           return expression();
		}
        public bool B()    /*B: ![a-zA-Z_0-9];*/
        {

           return Not(()=> (In('a','z', 'A','Z', '0','9')||OneOf("_")) );
		}
        public bool S()    /*S: (comment/whitespace/new_line/pp_directive )*;


//Unicode categories*/
        {

           return OptRepeat(()=>  
                      
                         comment()
                      || whitespace()
                      || new_line()
                      || pp_directive() );
		}
        public bool Zs()    /*Zs: [#x0020#x00a0#x1680#x180e#x2000-#x200a#x202f#x205f#x3000];*/
        {

           return OneOf(optimizedCharset1);
		}
        public bool Lu()    /*Lu: [#x0041-#x005a#x00c0-#x00d6#x00d8-#x00de#x0100#x0102#x0104#x0106#x0108#x010a#x010c#x010e#x0110#x0112#x0114#x0116#x0118#x011a#x011c]
/   [#x011e#x0120#x0122#x0124#x0126#x0128#x012a#x012c#x012e#x0130#x0132#x0134#x0136#x0139#x013b#x013d#x013f#x0141#x0143#x0145#x0147]
/   [#x014a#x014c#x014e#x0150#x0152#x0154#x0156#x0158#x015a#x015c#x015e#x0160#x0162#x0164#x0166#x0168#x016a#x016c#x016e#x0170#x0172]
/   [#x0174#x0176#x0178-#x0179#x017b#x017d#x0181-#x0182#x0184#x0186-#x0187#x0189-#x018b#x018e-#x0191#x0193-#x0194#x0196-#x0198#x019c-#x019d]
/   [#x019f-#x01a0#x01a2#x01a4#x01a6-#x01a7#x01a9#x01ac#x01ae-#x01af#x01b1-#x01b3#x01b5#x01b7-#x01b8#x01bc#x01c4#x01c7#x01ca#x01cd#x01cf]
/   [#x01d1#x01d3#x01d5#x01d7#x01d9#x01db#x01de#x01e0#x01e2#x01e4#x01e6#x01e8#x01ea#x01ec#x01ee#x01f1#x01f4#x01f6-#x01f8#x01fa#x01fc#x01fe]
/   [#x0200#x0202#x0204#x0206#x0208#x020a#x020c#x020e#x0210#x0212#x0214#x0216#x0218#x021a#x021c#x021e#x0220#x0222#x0224#x0226#x0228#x022a]
/   [#x022c#x022e#x0230#x0232#x0386#x0388-#x038a#x038c#x038e-#x038f#x0391-#x03a1#x03a3-#x03ab#x03d2-#x03d4#x03d8#x03da#x03dc#x03de#x03e0]
/   [#x03e2#x03e4#x03e6#x03e8#x03ea#x03ec#x03ee#x03f4#x03f7#x03f9-#x03fa#x0400-#x042f#x0460#x0462#x0464#x0466#x0468#x046a#x046c#x046e#x0470]
/   [#x0472#x0474#x0476#x0478#x047a#x047c#x047e#x0480#x048a#x048c#x048e#x0490#x0492#x0494#x0496#x0498#x049a#x049c#x049e#x04a0#x04a2#x04a4]
/   [#x04a6#x04a8#x04aa#x04ac#x04ae#x04b0#x04b2#x04b4#x04b6#x04b8#x04ba#x04bc#x04be#x04c0-#x04c1#x04c3#x04c5#x04c7#x04c9#x04cb#x04cd#x04d0]
/   [#x04d2#x04d4#x04d6#x04d8#x04da#x04dc#x04de#x04e0#x04e2#x04e4#x04e6#x04e8#x04ea#x04ec#x04ee#x04f0#x04f2#x04f4#x04f8#x0500#x0502#x0504]
/   [#x0506#x0508#x050a#x050c#x050e#x0531-#x0556#x10a0-#x10c5#x1e00#x1e02#x1e04#x1e06#x1e08#x1e0a#x1e0c#x1e0e#x1e10#x1e12#x1e14#x1e16#x1e18]
/   [#x1e1a#x1e1c#x1e1e#x1e20#x1e22#x1e24#x1e26#x1e28#x1e2a#x1e2c#x1e2e#x1e30#x1e32#x1e34#x1e36#x1e38#x1e3a#x1e3c#x1e3e#x1e40#x1e42#x1e44]
/   [#x1e46#x1e48#x1e4a#x1e4c#x1e4e#x1e50#x1e52#x1e54#x1e56#x1e58#x1e5a#x1e5c#x1e5e#x1e60#x1e62#x1e64#x1e66#x1e68#x1e6a#x1e6c#x1e6e#x1e70]
/   [#x1e72#x1e74#x1e76#x1e78#x1e7a#x1e7c#x1e7e#x1e80#x1e82#x1e84#x1e86#x1e88#x1e8a#x1e8c#x1e8e#x1e90#x1e92#x1e94#x1ea0#x1ea2#x1ea4#x1ea6]
/   [#x1ea8#x1eaa#x1eac#x1eae#x1eb0#x1eb2#x1eb4#x1eb6#x1eb8#x1eba#x1ebc#x1ebe#x1ec0#x1ec2#x1ec4#x1ec6#x1ec8#x1eca#x1ecc#x1ece#x1ed0#x1ed2]
/   [#x1ed4#x1ed6#x1ed8#x1eda#x1edc#x1ede#x1ee0#x1ee2#x1ee4#x1ee6#x1ee8#x1eea#x1eec#x1eee#x1ef0#x1ef2#x1ef4#x1ef6#x1ef8#x1f08-#x1f0f#x1f18-#x1f1d]
/   [#x1f28-#x1f2f#x1f38-#x1f3f#x1f48-#x1f4d#x1f59#x1f5b#x1f5d#x1f5f#x1f68-#x1f6f#x1fb8-#x1fbb#x1fc8-#x1fcb#x1fd8-#x1fdb#x1fe8-#x1fec#x1ff8-#x1ffb]
/   [#x2102#x2107#x210b-#x210d#x2110-#x2112#x2115#x2119-#x211d#x2124#x2126#x2128#x212a-#x212d#x2130-#x2131#x2133#x213e-#x213f#x2145#xff21-#xff3a];


// Unicode Category Ll, Letter, Lowercase*/
        {

           return OneOf(optimizedCharset2);
		}
        public bool Ll()    /*Ll: [#x0061-#x007a#x00aa#x00b5#x00ba#x00df-#x00f6#x00f8-#x00ff#x0101#x0103#x0105#x0107#x0109#x010b#x010d#x010f#x0111#x0113]
/    [#x0115#x0117#x0119#x011b#x011d#x011f#x0121#x0123#x0125#x0127#x0129#x012b#x012d#x012f#x0131#x0133#x0135#x0137-#x0138#x013a]
/    [#x013c#x013e#x0140#x0142#x0144#x0146#x0148-#x0149#x014b#x014d#x014f#x0151#x0153#x0155#x0157#x0159#x015b#x015d#x015f#x0161]
/    [#x0163#x0165#x0167#x0169#x016b#x016d#x016f#x0171#x0173#x0175#x0177#x017a#x017c#x017e-#x0180#x0183#x0185#x0188#x018c-#x018d]
/    [#x0192#x0195#x0199-#x019b#x019e#x01a1#x01a3#x01a5#x01a8#x01aa-#x01ab#x01ad#x01b0#x01b4#x01b6#x01b9-#x01ba#x01bd-#x01bf#x01c6#x01c9]
/    [#x01cc#x01ce#x01d0#x01d2#x01d4#x01d6#x01d8#x01da#x01dc-#x01dd#x01df#x01e1#x01e3#x01e5#x01e7#x01e9#x01eb#x01ed#x01ef-#x01f0#x01f3#x01f5]
/    [#x01f9#x01fb#x01fd#x01ff#x0201#x0203#x0205#x0207#x0209#x020b#x020d#x020f#x0211#x0213#x0215#x0217#x0219#x021b#x021d#x021f#x0221#x0223]
/    [#x0225#x0227#x0229#x022b#x022d#x022f#x0231#x0233-#x0236#x0250-#x02af#x0390#x03ac-#x03ce#x03d0-#x03d1#x03d5-#x03d7#x03d9#x03db#x03dd]
/   [#x03df#x03e1#x03e3#x03e5#x03e7#x03e9#x03eb#x03ed#x03ef-#x03f3#x03f5#x03f8#x03fb#x0430-#x045f#x0461#x0463#x0465#x0467#x0469]
/   [#x046b#x046d#x046f#x0471#x0473#x0475#x0477#x0479#x047b#x047d#x047f#x0481#x048b#x048d#x048f#x0491#x0493#x0495#x0497#x0499]
/   [#x049b#x049d#x049f#x04a1#x04a3#x04a5#x04a7#x04a9#x04ab#x04ad#x04af#x04b1#x04b3#x04b5#x04b7#x04b9#x04bb#x04bd#x04bf#x04c2]
/   [#x04c4#x04c6#x04c8#x04ca#x04cc#x04ce#x04d1#x04d3#x04d5#x04d7#x04d9#x04db#x04dd#x04df#x04e1#x04e3#x04e5#x04e7#x04e9#x04eb]
/   [#x04ed#x04ef#x04f1#x04f3#x04f5#x04f9#x0501#x0503#x0505#x0507#x0509#x050b#x050d#x050f#x0561-#x0587#x1d00-#x1d2b#x1d62-#x1d6b]
/   [#x1e01#x1e03#x1e05#x1e07#x1e09#x1e0b#x1e0d#x1e0f#x1e11#x1e13#x1e15#x1e17#x1e19#x1e1b#x1e1d#x1e1f#x1e21#x1e23#x1e25#x1e27]
/   [#x1e29#x1e2b#x1e2d#x1e2f#x1e31#x1e33#x1e35#x1e37#x1e39#x1e3b#x1e3d#x1e3f#x1e41#x1e43#x1e45#x1e47#x1e49#x1e4b#x1e4d#x1e4f]
/   [#x1e51#x1e53#x1e55#x1e57#x1e59#x1e5b#x1e5d#x1e5f#x1e61#x1e63#x1e65#x1e67#x1e69#x1e6b#x1e6d#x1e6f#x1e71#x1e73#x1e75#x1e77]
/   [#x1e79#x1e7b#x1e7d#x1e7f#x1e81#x1e83#x1e85#x1e87#x1e89#x1e8b#x1e8d#x1e8f#x1e91#x1e93#x1e95-#x1e9b#x1ea1#x1ea3#x1ea5#x1ea7]
/   [#x1ea9#x1eab#x1ead#x1eaf#x1eb1#x1eb3#x1eb5#x1eb7#x1eb9#x1ebb#x1ebd#x1ebf#x1ec1#x1ec3#x1ec5#x1ec7#x1ec9#x1ecb#x1ecd#x1ecf]
/   [#x1ed1#x1ed3#x1ed5#x1ed7#x1ed9#x1edb#x1edd#x1edf#x1ee1#x1ee3#x1ee5#x1ee7#x1ee9#x1eeb#x1eed#x1eef#x1ef1#x1ef3#x1ef5#x1ef7]
/   [#x1ef9#x1f00-#x1f07#x1f10-#x1f15#x1f20-#x1f27#x1f30-#x1f37#x1f40-#x1f45#x1f50-#x1f57#x1f60-#x1f67#x1f70-#x1f7d#x1f80-#x1f87#x1f90-#x1f97]
/   [#x1fa0-#x1fa7#x1fb0-#x1fb4#x1fb6-#x1fb7#x1fbe#x1fc2-#x1fc4#x1fc6-#x1fc7#x1fd0-#x1fd3#x1fd6-#x1fd7#x1fe0-#x1fe7#x1ff2-#x1ff4]
/   [#x1ff6-#x1ff7#x2071#x207f#x210a#x210e-#x210f#x2113#x212f#x2134#x2139#x213d#x2146-#x2149#xfb00-#xfb06#xfb13-#xfb17#xff41-#xff5a];

// Unicode Category Lt: Letter, Titlecase*/
        {

           return OneOf(optimizedCharset3);
		}
        public bool Lt()    /*Lt: [#x01c5#x01c8#x01cb#x01f2#x1f88-#x1f8f#x1f98-#x1f9f#x1fa8-#x1faf#x1fbc#x1fcc#x1ffc];

// Unicode Category Lm: Letter, Modifier*/
        {

           return OneOf(optimizedCharset4);
		}
        public bool Lm()    /*Lm: 	[#x02b0-#x02c1#x02c6-#x02d1#x02e0-#x02e4#x02ee#x037a#x0559#x0640#x06e5-#x06e6]
/	[#x0e46#x0ec6#x17d7#x1843#x1d2c-#x1d61#x3005#x3031-#x3035#x303b#x309d-#x309e#x30fc-#x30fe#xff70#xff9e-#xff9f];
// Unicode Category Lo: Letter, Other*/
        {

           return OneOf(optimizedCharset5);
		}
        public bool Lo()    /*Lo: [#x01bb#x01c0-#x01c3#x05d0-#x05ea#x05f0-#x05f2#x0621-#x063a#x0641-#x064a#x066e-#x066f#x0671-#x06d3#x06d5#x06ee-#x06ef#x06fa-#x06fc]
/   [#x06ff#x0710#x0712-#x072f#x074d-#x074f#x0780-#x07a5#x07b1#x0904-#x0939#x093d#x0950#x0958-#x0961#x0985-#x098c#x098f-#x0990#x0993-#x09a8]
/   [#x09aa-#x09b0#x09b2#x09b6-#x09b9#x09bd#x09dc-#x09dd#x09df-#x09e1#x09f0-#x09f1#x0a05-#x0a0a#x0a0f-#x0a10#x0a13-#x0a28#x0a2a-#x0a30]
/   [#x0a32-#x0a33#x0a35-#x0a36#x0a38-#x0a39#x0a59-#x0a5c#x0a5e#x0a72-#x0a74#x0a85-#x0a8d#x0a8f-#x0a91#x0a93-#x0aa8#x0aaa-#x0ab0]
/   [#x0ab2-#x0ab3#x0ab5-#x0ab9#x0abd#x0ad0#x0ae0-#x0ae1#x0b05-#x0b0c#x0b0f-#x0b10#x0b13-#x0b28#x0b2a-#x0b30#x0b32-#x0b33#x0b35-#x0b39]
/   [#x0b3d#x0b5c-#x0b5d#x0b5f-#x0b61#x0b71#x0b83#x0b85-#x0b8a#x0b8e-#x0b90#x0b92-#x0b95#x0b99-#x0b9a#x0b9c#x0b9e-#x0b9f#x0ba3-#x0ba4]
/   [#x0ba8-#x0baa#x0bae-#x0bb5#x0bb7-#x0bb9#x0c05-#x0c0c#x0c0e-#x0c10#x0c12-#x0c28#x0c2a-#x0c33#x0c35-#x0c39#x0c60-#x0c61#x0c85-#x0c8c]
/   [#x0c8e-#x0c90#x0c92-#x0ca8#x0caa-#x0cb3#x0cb5-#x0cb9#x0cbd#x0cde#x0ce0-#x0ce1#x0d05-#x0d0c#x0d0e-#x0d10#x0d12-#x0d28#x0d2a-#x0d39]
/   [#x0d60-#x0d61#x0d85-#x0d96#x0d9a-#x0db1#x0db3-#x0dbb#x0dbd#x0dc0-#x0dc6#x0e01-#x0e30#x0e32-#x0e33#x0e40-#x0e45#x0e81-#x0e82#x0e84]
/   [#x0e87-#x0e88#x0e8a#x0e8d#x0e94-#x0e97#x0e99-#x0e9f#x0ea1-#x0ea3#x0ea5#x0ea7#x0eaa-#x0eab#x0ead-#x0eb0#x0eb2-#x0eb3#x0ebd#x0ec0-#x0ec4]
/   [#x0edc-#x0edd#x0f00#x0f40-#x0f47#x0f49-#x0f6a#x0f88-#x0f8b#x1000-#x1021#x1023-#x1027#x1029-#x102a#x1050-#x1055#x10d0-#x10f8]
/   [#x1100-#x1159#x115f-#x11a2#x11a8-#x11f9#x1200-#x1206#x1208-#x1246#x1248#x124a-#x124d#x1250-#x1256#x1258#x125a-#x125d#x1260-#x1286]
/   [#x1288#x128a-#x128d#x1290-#x12ae#x12b0#x12b2-#x12b5#x12b8-#x12be#x12c0#x12c2-#x12c5#x12c8-#x12ce#x12d0-#x12d6#x12d8-#x12ee#x12f0-#x130e]
/   [#x1310#x1312-#x1315#x1318-#x131e#x1320-#x1346#x1348-#x135a#x13a0-#x13f4#x1401-#x166c#x166f-#x1676#x1681-#x169a#x16a0-#x16ea]
/   [#x1700-#x170c#x170e-#x1711#x1720-#x1731#x1740-#x1751#x1760-#x176c#x176e-#x1770#x1780-#x17b3#x17dc#x1820-#x1842#x1844-#x1877#x1880-#x18a8]
/   [#x1900-#x191c#x1950-#x196d#x2135-#x2138#x3006#x303c#x3041-#x3096#x309f#x30a1-#x30fa#x30ff#x3105-#x312c#x3131-#x318e#x31a0-#x31b7]
/   [#x31f0-#x31ff#x3400#x4db5#x4e00#x9fa5#xa000-#xa48c#xac00#xd7a3#xfb1d#xfb1f-#xfb28#xfb2a-#xfb36#xfb38-#xfb3c#xfb3e#xfb40-#xfb41]
/   [#xfb43-#xfb44#xfb46-#xfbb1#xfbd3-#xfd3d#xfd50-#xfd8f#xfd92-#xfdc7#xfdf0-#xfdfb#xfe70-#xfe74#xfe76-#xfefc#xff66-#xff6f#xff71-#xff9d]
/   [#xffa0-#xffbe#xffc2-#xffc7#xffca-#xffcf#xffd2-#xffd7#xffda-#xffdc];


// Unicode Category Nl: Number, Letter*/
        {

           return OneOf(optimizedCharset6);
		}
        public bool Nl()    /*Nl: [#x16ee-#x16f0#x2160-#x2183#x3007#x3021-#x3029#x3038-#x303a];

// Unicode Cateogry Mn: Mark, Nonspacing*/
        {

           return (In('\u16ee','\u16f0', '\u2160','\u2183', '\u3021','\u3029', '\u3038','\u303a')||OneOf("\u3007"));
		}
        public bool Mn()    /*Mn:[#x0300-#x0357#x035d-#x036f#x0483-#x0486#x0591-#x05a1#x05a3-#x05b9#x05bb-#x05bd#x05bf#x05c1-#x05c2#x05c4#x0610-#x0615#x064b-#x0658]
/   [#x0670#x06d6-#x06dc#x06df-#x06e4#x06e7-#x06e8#x06ea-#x06ed#x0711#x0730-#x074a#x07a6-#x07b0#x0901-#x0902#x093c#x0941-#x0948#x094d]
/   [#x0951-#x0954#x0962-#x0963#x0981#x09bc#x09c1-#x09c4#x09cd#x09e2-#x09e3#x0a01-#x0a02#x0a3c#x0a41-#x0a42#x0a47-#x0a48#x0a4b-#x0a4d]
/   [#x0a70-#x0a71#x0a81-#x0a82#x0abc#x0ac1-#x0ac5#x0ac7-#x0ac8#x0acd#x0ae2-#x0ae3#x0b01#x0b3c#x0b3f#x0b41-#x0b43#x0b4d#x0b56#x0b82]
/   [#x0bc0#x0bcd#x0c3e-#x0c40#x0c46-#x0c48#x0c4a-#x0c4d#x0c55-#x0c56#x0cbc#x0cbf#x0cc6#x0ccc-#x0ccd#x0d41-#x0d43#x0d4d#x0dca#x0dd2-#x0dd4]
/   [#x0dd6#x0e31#x0e34-#x0e3a#x0e47-#x0e4e#x0eb1#x0eb4-#x0eb9#x0ebb-#x0ebc#x0ec8-#x0ecd#x0f18-#x0f19#x0f35#x0f37#x0f39#x0f71-#x0f7e]
/   [#x0f80-#x0f84#x0f86-#x0f87#x0f90-#x0f97#x0f99-#x0fbc#x0fc6#x102d-#x1030#x1032#x1036-#x1037#x1039#x1058-#x1059#x1712-#x1714]
/   [#x1732-#x1734#x1752-#x1753#x1772-#x1773#x17b7-#x17bd#x17c6#x17c9-#x17d3#x17dd#x180b-#x180d#x18a9#x1920-#x1922#x1927-#x1928#x1932]
/   [#x1939-#x193b#x20d0-#x20dc#x20e1#x20e5-#x20ea#x302a-#x302f#x3099-#x309a#xfb1e#xfe20-#xfe23];

// Unicode Category Mc: Mark, Spacing Combining*/
        {

           return OneOf(optimizedCharset7);
		}
        public bool Mc()    /*Mc:[#x0903#x093e-#x0940#x0949-#x094c#x0982-#x0983#x09be-#x09c0#x09c7-#x09c8#x09cb-#x09cc#x09d7#x0a03#x0a3e-#x0a40#x0a83#x0abe-#x0ac0]
/   [#x0ac9#x0acb-#x0acc#x0b02-#x0b03#x0b3e#x0b40#x0b47-#x0b48#x0b4b-#x0b4c#x0b57#x0bbe-#x0bbf#x0bc1-#x0bc2#x0bc6-#x0bc8#x0bca-#x0bcc]
/   [#x0bd7#x0c01-#x0c03#x0c41-#x0c44#x0c82-#x0c83#x0cbe#x0cc0-#x0cc4#x0cc7-#x0cc8#x0cca-#x0ccb#x0cd5-#x0cd6#x0d02-#x0d03#x0d3e-#x0d40]
/   [#x0d46-#x0d48#x0d4a-#x0d4c#x0d57#x0d82-#x0d83#x0dcf-#x0dd1#x0dd8-#x0ddf#x0df2-#x0df3#x0f3e-#x0f3f#x0f7f#x102c#x1031#x1038#x1056-#x1057]
/   [#x17b6#x17be-#x17c5#x17c7-#x17c8#x1923-#x1926#x1929-#x192b#x1930-#x1931#x1933-#x1938];

// Unicode Category Nd: Number, Decimal Digit*/
        {

           return OneOf(optimizedCharset8);
		}
        public bool Nd()    /*Nd:[#x0030-#x0039#x0660-#x0669#x06f0-#x06f9#x0966-#x096f#x09e6-#x09ef#x0a66-#x0a6f#x0ae6-#x0aef#x0b66-#x0b6f#x0be7-#x0bef#x0c66-#x0c6f]
/   [#x0ce6-#x0cef#x0d66-#x0d6f#x0e50-#x0e59#x0ed0-#x0ed9#x0f20-#x0f29#x1040-#x1049#x1369-#x1371#x17e0-#x17e9#x1810-#x1819#x1946-#x194f#xff10-#xff19];

// Unicode Category Pc: Punctuation, Connector*/
        {

           return OneOf(optimizedCharset9);
		}
        public bool Pc()    /*Pc:  [#x005f#x203f-#x2040#x2054#x30fb#xfe33-#xfe34#xfe4d-#xfe4f#xff3f#xff65];

  
// Unicode Category Cf: Other, Format*/
        {

           return OneOf(optimizedCharset10);
		}
        public bool Cf()    /*Cf: [#x00ad#x0600-#x0603#x06dd#x070f#x17b4-#x17b5#x200b-#x200f#x202a-#x202e#x2060-#x2063#x206a-#x206f#xfeff#xfff9-#xfffb];*/
        {

           return OneOf(optimizedCharset11);
		}
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedCharset optimizedCharset1;
        internal static OptimizedCharset optimizedCharset2;
        internal static OptimizedCharset optimizedCharset3;
        internal static OptimizedCharset optimizedCharset4;
        internal static OptimizedCharset optimizedCharset5;
        internal static OptimizedCharset optimizedCharset6;
        internal static OptimizedCharset optimizedCharset7;
        internal static OptimizedCharset optimizedCharset8;
        internal static OptimizedCharset optimizedCharset9;
        internal static OptimizedCharset optimizedCharset10;
        internal static OptimizedCharset optimizedCharset11;
        
        internal static OptimizedLiterals optimizedLiterals0;
        internal static OptimizedLiterals optimizedLiterals1;
        internal static OptimizedLiterals optimizedLiterals2;
        internal static OptimizedLiterals optimizedLiterals3;
        internal static OptimizedLiterals optimizedLiterals4;
        internal static OptimizedLiterals optimizedLiterals5;
        internal static OptimizedLiterals optimizedLiterals6;
        internal static OptimizedLiterals optimizedLiterals7;
        internal static OptimizedLiterals optimizedLiterals8;
        internal static OptimizedLiterals optimizedLiterals9;
        internal static OptimizedLiterals optimizedLiterals10;
        internal static OptimizedLiterals optimizedLiterals11;
        
        static CSharp3()
        {
            {
               char[] oneOfChars = new char[]    {'\'','"','\\','0','a'
                                                  ,'b','f','n','r','t'
                                                  ,'v'};
               optimizedCharset0= new OptimizedCharset(null,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u2000','\u200a'),
                   };
               char[] oneOfChars = new char[]    {'\u0020','\u00a0','\u1680','\u180e','\u202f'
                                                  ,'\u205f','\u3000'};
               optimizedCharset1= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0041','\u005a'),
                   new OptimizedCharset.Range('\u00c0','\u00d6'),
                   new OptimizedCharset.Range('\u00d8','\u00de'),
                   new OptimizedCharset.Range('\u0178','\u0179'),
                   new OptimizedCharset.Range('\u0181','\u0182'),
                   new OptimizedCharset.Range('\u0186','\u0187'),
                   new OptimizedCharset.Range('\u0189','\u018b'),
                   new OptimizedCharset.Range('\u018e','\u0191'),
                   new OptimizedCharset.Range('\u0193','\u0194'),
                   new OptimizedCharset.Range('\u0196','\u0198'),
                   new OptimizedCharset.Range('\u019c','\u019d'),
                   new OptimizedCharset.Range('\u019f','\u01a0'),
                   new OptimizedCharset.Range('\u01a6','\u01a7'),
                   new OptimizedCharset.Range('\u01ae','\u01af'),
                   new OptimizedCharset.Range('\u01b1','\u01b3'),
                   new OptimizedCharset.Range('\u01b7','\u01b8'),
                   new OptimizedCharset.Range('\u01f6','\u01f8'),
                   new OptimizedCharset.Range('\u0388','\u038a'),
                   new OptimizedCharset.Range('\u038e','\u038f'),
                   new OptimizedCharset.Range('\u0391','\u03a1'),
                   new OptimizedCharset.Range('\u03a3','\u03ab'),
                   new OptimizedCharset.Range('\u03d2','\u03d4'),
                   new OptimizedCharset.Range('\u03f9','\u03fa'),
                   new OptimizedCharset.Range('\u0400','\u042f'),
                   new OptimizedCharset.Range('\u04c0','\u04c1'),
                   new OptimizedCharset.Range('\u0531','\u0556'),
                   new OptimizedCharset.Range('\u10a0','\u10c5'),
                   new OptimizedCharset.Range('\u1f08','\u1f0f'),
                   new OptimizedCharset.Range('\u1f18','\u1f1d'),
                   new OptimizedCharset.Range('\u1f28','\u1f2f'),
                   new OptimizedCharset.Range('\u1f38','\u1f3f'),
                   new OptimizedCharset.Range('\u1f48','\u1f4d'),
                   new OptimizedCharset.Range('\u1f68','\u1f6f'),
                   new OptimizedCharset.Range('\u1fb8','\u1fbb'),
                   new OptimizedCharset.Range('\u1fc8','\u1fcb'),
                   new OptimizedCharset.Range('\u1fd8','\u1fdb'),
                   new OptimizedCharset.Range('\u1fe8','\u1fec'),
                   new OptimizedCharset.Range('\u1ff8','\u1ffb'),
                   new OptimizedCharset.Range('\u210b','\u210d'),
                   new OptimizedCharset.Range('\u2110','\u2112'),
                   new OptimizedCharset.Range('\u2119','\u211d'),
                   new OptimizedCharset.Range('\u212a','\u212d'),
                   new OptimizedCharset.Range('\u2130','\u2131'),
                   new OptimizedCharset.Range('\u213e','\u213f'),
                   new OptimizedCharset.Range('\uff21','\uff3a'),
                   };
               char[] oneOfChars = new char[]    {'\u0100','\u0102','\u0104','\u0106','\u0108'
                                                  ,'\u010a','\u010c','\u010e','\u0110','\u0112'
                                                  ,'\u0114','\u0116','\u0118','\u011a','\u011c'
                                                  ,'\u011e','\u0120','\u0122','\u0124','\u0126'
                                                  ,'\u0128','\u012a','\u012c','\u012e','\u0130'
                                                  ,'\u0132','\u0134','\u0136','\u0139','\u013b'
                                                  ,'\u013d','\u013f','\u0141','\u0143','\u0145'
                                                  ,'\u0147','\u014a','\u014c','\u014e','\u0150'
                                                  ,'\u0152','\u0154','\u0156','\u0158','\u015a'
                                                  ,'\u015c','\u015e','\u0160','\u0162','\u0164'
                                                  ,'\u0166','\u0168','\u016a','\u016c','\u016e'
                                                  ,'\u0170','\u0172','\u0174','\u0176','\u017b'
                                                  ,'\u017d','\u0184','\u01a2','\u01a4','\u01a9'
                                                  ,'\u01ac','\u01b5','\u01bc','\u01c4','\u01c7'
                                                  ,'\u01ca','\u01cd','\u01cf','\u01d1','\u01d3'
                                                  ,'\u01d5','\u01d7','\u01d9','\u01db','\u01de'
                                                  ,'\u01e0','\u01e2','\u01e4','\u01e6','\u01e8'
                                                  ,'\u01ea','\u01ec','\u01ee','\u01f1','\u01f4'
                                                  ,'\u01fa','\u01fc','\u01fe','\u0200','\u0202'
                                                  ,'\u0204','\u0206','\u0208','\u020a','\u020c'
                                                  ,'\u020e','\u0210','\u0212','\u0214','\u0216'
                                                  ,'\u0218','\u021a','\u021c','\u021e','\u0220'
                                                  ,'\u0222','\u0224','\u0226','\u0228','\u022a'
                                                  ,'\u022c','\u022e','\u0230','\u0232','\u0386'
                                                  ,'\u038c','\u03d8','\u03da','\u03dc','\u03de'
                                                  ,'\u03e0','\u03e2','\u03e4','\u03e6','\u03e8'
                                                  ,'\u03ea','\u03ec','\u03ee','\u03f4','\u03f7'
                                                  ,'\u0460','\u0462','\u0464','\u0466','\u0468'
                                                  ,'\u046a','\u046c','\u046e','\u0470','\u0472'
                                                  ,'\u0474','\u0476','\u0478','\u047a','\u047c'
                                                  ,'\u047e','\u0480','\u048a','\u048c','\u048e'
                                                  ,'\u0490','\u0492','\u0494','\u0496','\u0498'
                                                  ,'\u049a','\u049c','\u049e','\u04a0','\u04a2'
                                                  ,'\u04a4','\u04a6','\u04a8','\u04aa','\u04ac'
                                                  ,'\u04ae','\u04b0','\u04b2','\u04b4','\u04b6'
                                                  ,'\u04b8','\u04ba','\u04bc','\u04be','\u04c3'
                                                  ,'\u04c5','\u04c7','\u04c9','\u04cb','\u04cd'
                                                  ,'\u04d0','\u04d2','\u04d4','\u04d6','\u04d8'
                                                  ,'\u04da','\u04dc','\u04de','\u04e0','\u04e2'
                                                  ,'\u04e4','\u04e6','\u04e8','\u04ea','\u04ec'
                                                  ,'\u04ee','\u04f0','\u04f2','\u04f4','\u04f8'
                                                  ,'\u0500','\u0502','\u0504','\u0506','\u0508'
                                                  ,'\u050a','\u050c','\u050e','\u1e00','\u1e02'
                                                  ,'\u1e04','\u1e06','\u1e08','\u1e0a','\u1e0c'
                                                  ,'\u1e0e','\u1e10','\u1e12','\u1e14','\u1e16'
                                                  ,'\u1e18','\u1e1a','\u1e1c','\u1e1e','\u1e20'
                                                  ,'\u1e22','\u1e24','\u1e26','\u1e28','\u1e2a'
                                                  ,'\u1e2c','\u1e2e','\u1e30','\u1e32','\u1e34'
                                                  ,'\u1e36','\u1e38','\u1e3a','\u1e3c','\u1e3e'
                                                  ,'\u1e40','\u1e42','\u1e44','\u1e46','\u1e48'
                                                  ,'\u1e4a','\u1e4c','\u1e4e','\u1e50','\u1e52'
                                                  ,'\u1e54','\u1e56','\u1e58','\u1e5a','\u1e5c'
                                                  ,'\u1e5e','\u1e60','\u1e62','\u1e64','\u1e66'
                                                  ,'\u1e68','\u1e6a','\u1e6c','\u1e6e','\u1e70'
                                                  ,'\u1e72','\u1e74','\u1e76','\u1e78','\u1e7a'
                                                  ,'\u1e7c','\u1e7e','\u1e80','\u1e82','\u1e84'
                                                  ,'\u1e86','\u1e88','\u1e8a','\u1e8c','\u1e8e'
                                                  ,'\u1e90','\u1e92','\u1e94','\u1ea0','\u1ea2'
                                                  ,'\u1ea4','\u1ea6','\u1ea8','\u1eaa','\u1eac'
                                                  ,'\u1eae','\u1eb0','\u1eb2','\u1eb4','\u1eb6'
                                                  ,'\u1eb8','\u1eba','\u1ebc','\u1ebe','\u1ec0'
                                                  ,'\u1ec2','\u1ec4','\u1ec6','\u1ec8','\u1eca'
                                                  ,'\u1ecc','\u1ece','\u1ed0','\u1ed2','\u1ed4'
                                                  ,'\u1ed6','\u1ed8','\u1eda','\u1edc','\u1ede'
                                                  ,'\u1ee0','\u1ee2','\u1ee4','\u1ee6','\u1ee8'
                                                  ,'\u1eea','\u1eec','\u1eee','\u1ef0','\u1ef2'
                                                  ,'\u1ef4','\u1ef6','\u1ef8','\u1f59','\u1f5b'
                                                  ,'\u1f5d','\u1f5f','\u2102','\u2107','\u2115'
                                                  ,'\u2124','\u2126','\u2128','\u2133','\u2145'
                                                  };
               optimizedCharset2= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0061','\u007a'),
                   new OptimizedCharset.Range('\u00df','\u00f6'),
                   new OptimizedCharset.Range('\u00f8','\u00ff'),
                   new OptimizedCharset.Range('\u0137','\u0138'),
                   new OptimizedCharset.Range('\u0148','\u0149'),
                   new OptimizedCharset.Range('\u017e','\u0180'),
                   new OptimizedCharset.Range('\u018c','\u018d'),
                   new OptimizedCharset.Range('\u0199','\u019b'),
                   new OptimizedCharset.Range('\u01aa','\u01ab'),
                   new OptimizedCharset.Range('\u01b9','\u01ba'),
                   new OptimizedCharset.Range('\u01bd','\u01bf'),
                   new OptimizedCharset.Range('\u01dc','\u01dd'),
                   new OptimizedCharset.Range('\u01ef','\u01f0'),
                   new OptimizedCharset.Range('\u0233','\u0236'),
                   new OptimizedCharset.Range('\u0250','\u02af'),
                   new OptimizedCharset.Range('\u03ac','\u03ce'),
                   new OptimizedCharset.Range('\u03d0','\u03d1'),
                   new OptimizedCharset.Range('\u03d5','\u03d7'),
                   new OptimizedCharset.Range('\u03ef','\u03f3'),
                   new OptimizedCharset.Range('\u0430','\u045f'),
                   new OptimizedCharset.Range('\u0561','\u0587'),
                   new OptimizedCharset.Range('\u1d00','\u1d2b'),
                   new OptimizedCharset.Range('\u1d62','\u1d6b'),
                   new OptimizedCharset.Range('\u1e95','\u1e9b'),
                   new OptimizedCharset.Range('\u1f00','\u1f07'),
                   new OptimizedCharset.Range('\u1f10','\u1f15'),
                   new OptimizedCharset.Range('\u1f20','\u1f27'),
                   new OptimizedCharset.Range('\u1f30','\u1f37'),
                   new OptimizedCharset.Range('\u1f40','\u1f45'),
                   new OptimizedCharset.Range('\u1f50','\u1f57'),
                   new OptimizedCharset.Range('\u1f60','\u1f67'),
                   new OptimizedCharset.Range('\u1f70','\u1f7d'),
                   new OptimizedCharset.Range('\u1f80','\u1f87'),
                   new OptimizedCharset.Range('\u1f90','\u1f97'),
                   new OptimizedCharset.Range('\u1fa0','\u1fa7'),
                   new OptimizedCharset.Range('\u1fb0','\u1fb4'),
                   new OptimizedCharset.Range('\u1fb6','\u1fb7'),
                   new OptimizedCharset.Range('\u1fc2','\u1fc4'),
                   new OptimizedCharset.Range('\u1fc6','\u1fc7'),
                   new OptimizedCharset.Range('\u1fd0','\u1fd3'),
                   new OptimizedCharset.Range('\u1fd6','\u1fd7'),
                   new OptimizedCharset.Range('\u1fe0','\u1fe7'),
                   new OptimizedCharset.Range('\u1ff2','\u1ff4'),
                   new OptimizedCharset.Range('\u1ff6','\u1ff7'),
                   new OptimizedCharset.Range('\u210e','\u210f'),
                   new OptimizedCharset.Range('\u2146','\u2149'),
                   new OptimizedCharset.Range('\ufb00','\ufb06'),
                   new OptimizedCharset.Range('\ufb13','\ufb17'),
                   new OptimizedCharset.Range('\uff41','\uff5a'),
                   };
               char[] oneOfChars = new char[]    {'\u00aa','\u00b5','\u00ba','\u0101','\u0103'
                                                  ,'\u0105','\u0107','\u0109','\u010b','\u010d'
                                                  ,'\u010f','\u0111','\u0113','\u0115','\u0117'
                                                  ,'\u0119','\u011b','\u011d','\u011f','\u0121'
                                                  ,'\u0123','\u0125','\u0127','\u0129','\u012b'
                                                  ,'\u012d','\u012f','\u0131','\u0133','\u0135'
                                                  ,'\u013a','\u013c','\u013e','\u0140','\u0142'
                                                  ,'\u0144','\u0146','\u014b','\u014d','\u014f'
                                                  ,'\u0151','\u0153','\u0155','\u0157','\u0159'
                                                  ,'\u015b','\u015d','\u015f','\u0161','\u0163'
                                                  ,'\u0165','\u0167','\u0169','\u016b','\u016d'
                                                  ,'\u016f','\u0171','\u0173','\u0175','\u0177'
                                                  ,'\u017a','\u017c','\u0183','\u0185','\u0188'
                                                  ,'\u0192','\u0195','\u019e','\u01a1','\u01a3'
                                                  ,'\u01a5','\u01a8','\u01ad','\u01b0','\u01b4'
                                                  ,'\u01b6','\u01c6','\u01c9','\u01cc','\u01ce'
                                                  ,'\u01d0','\u01d2','\u01d4','\u01d6','\u01d8'
                                                  ,'\u01da','\u01df','\u01e1','\u01e3','\u01e5'
                                                  ,'\u01e7','\u01e9','\u01eb','\u01ed','\u01f3'
                                                  ,'\u01f5','\u01f9','\u01fb','\u01fd','\u01ff'
                                                  ,'\u0201','\u0203','\u0205','\u0207','\u0209'
                                                  ,'\u020b','\u020d','\u020f','\u0211','\u0213'
                                                  ,'\u0215','\u0217','\u0219','\u021b','\u021d'
                                                  ,'\u021f','\u0221','\u0223','\u0225','\u0227'
                                                  ,'\u0229','\u022b','\u022d','\u022f','\u0231'
                                                  ,'\u0390','\u03d9','\u03db','\u03dd','\u03df'
                                                  ,'\u03e1','\u03e3','\u03e5','\u03e7','\u03e9'
                                                  ,'\u03eb','\u03ed','\u03f5','\u03f8','\u03fb'
                                                  ,'\u0461','\u0463','\u0465','\u0467','\u0469'
                                                  ,'\u046b','\u046d','\u046f','\u0471','\u0473'
                                                  ,'\u0475','\u0477','\u0479','\u047b','\u047d'
                                                  ,'\u047f','\u0481','\u048b','\u048d','\u048f'
                                                  ,'\u0491','\u0493','\u0495','\u0497','\u0499'
                                                  ,'\u049b','\u049d','\u049f','\u04a1','\u04a3'
                                                  ,'\u04a5','\u04a7','\u04a9','\u04ab','\u04ad'
                                                  ,'\u04af','\u04b1','\u04b3','\u04b5','\u04b7'
                                                  ,'\u04b9','\u04bb','\u04bd','\u04bf','\u04c2'
                                                  ,'\u04c4','\u04c6','\u04c8','\u04ca','\u04cc'
                                                  ,'\u04ce','\u04d1','\u04d3','\u04d5','\u04d7'
                                                  ,'\u04d9','\u04db','\u04dd','\u04df','\u04e1'
                                                  ,'\u04e3','\u04e5','\u04e7','\u04e9','\u04eb'
                                                  ,'\u04ed','\u04ef','\u04f1','\u04f3','\u04f5'
                                                  ,'\u04f9','\u0501','\u0503','\u0505','\u0507'
                                                  ,'\u0509','\u050b','\u050d','\u050f','\u1e01'
                                                  ,'\u1e03','\u1e05','\u1e07','\u1e09','\u1e0b'
                                                  ,'\u1e0d','\u1e0f','\u1e11','\u1e13','\u1e15'
                                                  ,'\u1e17','\u1e19','\u1e1b','\u1e1d','\u1e1f'
                                                  ,'\u1e21','\u1e23','\u1e25','\u1e27','\u1e29'
                                                  ,'\u1e2b','\u1e2d','\u1e2f','\u1e31','\u1e33'
                                                  ,'\u1e35','\u1e37','\u1e39','\u1e3b','\u1e3d'
                                                  ,'\u1e3f','\u1e41','\u1e43','\u1e45','\u1e47'
                                                  ,'\u1e49','\u1e4b','\u1e4d','\u1e4f','\u1e51'
                                                  ,'\u1e53','\u1e55','\u1e57','\u1e59','\u1e5b'
                                                  ,'\u1e5d','\u1e5f','\u1e61','\u1e63','\u1e65'
                                                  ,'\u1e67','\u1e69','\u1e6b','\u1e6d','\u1e6f'
                                                  ,'\u1e71','\u1e73','\u1e75','\u1e77','\u1e79'
                                                  ,'\u1e7b','\u1e7d','\u1e7f','\u1e81','\u1e83'
                                                  ,'\u1e85','\u1e87','\u1e89','\u1e8b','\u1e8d'
                                                  ,'\u1e8f','\u1e91','\u1e93','\u1ea1','\u1ea3'
                                                  ,'\u1ea5','\u1ea7','\u1ea9','\u1eab','\u1ead'
                                                  ,'\u1eaf','\u1eb1','\u1eb3','\u1eb5','\u1eb7'
                                                  ,'\u1eb9','\u1ebb','\u1ebd','\u1ebf','\u1ec1'
                                                  ,'\u1ec3','\u1ec5','\u1ec7','\u1ec9','\u1ecb'
                                                  ,'\u1ecd','\u1ecf','\u1ed1','\u1ed3','\u1ed5'
                                                  ,'\u1ed7','\u1ed9','\u1edb','\u1edd','\u1edf'
                                                  ,'\u1ee1','\u1ee3','\u1ee5','\u1ee7','\u1ee9'
                                                  ,'\u1eeb','\u1eed','\u1eef','\u1ef1','\u1ef3'
                                                  ,'\u1ef5','\u1ef7','\u1ef9','\u1fbe','\u2071'
                                                  ,'\u207f','\u210a','\u2113','\u212f','\u2134'
                                                  ,'\u2139','\u213d'};
               optimizedCharset3= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u1f88','\u1f8f'),
                   new OptimizedCharset.Range('\u1f98','\u1f9f'),
                   new OptimizedCharset.Range('\u1fa8','\u1faf'),
                   };
               char[] oneOfChars = new char[]    {'\u01c5','\u01c8','\u01cb','\u01f2','\u1fbc'
                                                  ,'\u1fcc','\u1ffc'};
               optimizedCharset4= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u02b0','\u02c1'),
                   new OptimizedCharset.Range('\u02c6','\u02d1'),
                   new OptimizedCharset.Range('\u02e0','\u02e4'),
                   new OptimizedCharset.Range('\u06e5','\u06e6'),
                   new OptimizedCharset.Range('\u1d2c','\u1d61'),
                   new OptimizedCharset.Range('\u3031','\u3035'),
                   new OptimizedCharset.Range('\u309d','\u309e'),
                   new OptimizedCharset.Range('\u30fc','\u30fe'),
                   new OptimizedCharset.Range('\uff9e','\uff9f'),
                   };
               char[] oneOfChars = new char[]    {'\u02ee','\u037a','\u0559','\u0640','\u0e46'
                                                  ,'\u0ec6','\u17d7','\u1843','\u3005','\u303b'
                                                  ,'\uff70'};
               optimizedCharset5= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u01c0','\u01c3'),
                   new OptimizedCharset.Range('\u05d0','\u05ea'),
                   new OptimizedCharset.Range('\u05f0','\u05f2'),
                   new OptimizedCharset.Range('\u0621','\u063a'),
                   new OptimizedCharset.Range('\u0641','\u064a'),
                   new OptimizedCharset.Range('\u066e','\u066f'),
                   new OptimizedCharset.Range('\u0671','\u06d3'),
                   new OptimizedCharset.Range('\u06ee','\u06ef'),
                   new OptimizedCharset.Range('\u06fa','\u06fc'),
                   new OptimizedCharset.Range('\u0712','\u072f'),
                   new OptimizedCharset.Range('\u074d','\u074f'),
                   new OptimizedCharset.Range('\u0780','\u07a5'),
                   new OptimizedCharset.Range('\u0904','\u0939'),
                   new OptimizedCharset.Range('\u0958','\u0961'),
                   new OptimizedCharset.Range('\u0985','\u098c'),
                   new OptimizedCharset.Range('\u098f','\u0990'),
                   new OptimizedCharset.Range('\u0993','\u09a8'),
                   new OptimizedCharset.Range('\u09aa','\u09b0'),
                   new OptimizedCharset.Range('\u09b6','\u09b9'),
                   new OptimizedCharset.Range('\u09dc','\u09dd'),
                   new OptimizedCharset.Range('\u09df','\u09e1'),
                   new OptimizedCharset.Range('\u09f0','\u09f1'),
                   new OptimizedCharset.Range('\u0a05','\u0a0a'),
                   new OptimizedCharset.Range('\u0a0f','\u0a10'),
                   new OptimizedCharset.Range('\u0a13','\u0a28'),
                   new OptimizedCharset.Range('\u0a2a','\u0a30'),
                   new OptimizedCharset.Range('\u0a32','\u0a33'),
                   new OptimizedCharset.Range('\u0a35','\u0a36'),
                   new OptimizedCharset.Range('\u0a38','\u0a39'),
                   new OptimizedCharset.Range('\u0a59','\u0a5c'),
                   new OptimizedCharset.Range('\u0a72','\u0a74'),
                   new OptimizedCharset.Range('\u0a85','\u0a8d'),
                   new OptimizedCharset.Range('\u0a8f','\u0a91'),
                   new OptimizedCharset.Range('\u0a93','\u0aa8'),
                   new OptimizedCharset.Range('\u0aaa','\u0ab0'),
                   new OptimizedCharset.Range('\u0ab2','\u0ab3'),
                   new OptimizedCharset.Range('\u0ab5','\u0ab9'),
                   new OptimizedCharset.Range('\u0ae0','\u0ae1'),
                   new OptimizedCharset.Range('\u0b05','\u0b0c'),
                   new OptimizedCharset.Range('\u0b0f','\u0b10'),
                   new OptimizedCharset.Range('\u0b13','\u0b28'),
                   new OptimizedCharset.Range('\u0b2a','\u0b30'),
                   new OptimizedCharset.Range('\u0b32','\u0b33'),
                   new OptimizedCharset.Range('\u0b35','\u0b39'),
                   new OptimizedCharset.Range('\u0b5c','\u0b5d'),
                   new OptimizedCharset.Range('\u0b5f','\u0b61'),
                   new OptimizedCharset.Range('\u0b85','\u0b8a'),
                   new OptimizedCharset.Range('\u0b8e','\u0b90'),
                   new OptimizedCharset.Range('\u0b92','\u0b95'),
                   new OptimizedCharset.Range('\u0b99','\u0b9a'),
                   new OptimizedCharset.Range('\u0b9e','\u0b9f'),
                   new OptimizedCharset.Range('\u0ba3','\u0ba4'),
                   new OptimizedCharset.Range('\u0ba8','\u0baa'),
                   new OptimizedCharset.Range('\u0bae','\u0bb5'),
                   new OptimizedCharset.Range('\u0bb7','\u0bb9'),
                   new OptimizedCharset.Range('\u0c05','\u0c0c'),
                   new OptimizedCharset.Range('\u0c0e','\u0c10'),
                   new OptimizedCharset.Range('\u0c12','\u0c28'),
                   new OptimizedCharset.Range('\u0c2a','\u0c33'),
                   new OptimizedCharset.Range('\u0c35','\u0c39'),
                   new OptimizedCharset.Range('\u0c60','\u0c61'),
                   new OptimizedCharset.Range('\u0c85','\u0c8c'),
                   new OptimizedCharset.Range('\u0c8e','\u0c90'),
                   new OptimizedCharset.Range('\u0c92','\u0ca8'),
                   new OptimizedCharset.Range('\u0caa','\u0cb3'),
                   new OptimizedCharset.Range('\u0cb5','\u0cb9'),
                   new OptimizedCharset.Range('\u0ce0','\u0ce1'),
                   new OptimizedCharset.Range('\u0d05','\u0d0c'),
                   new OptimizedCharset.Range('\u0d0e','\u0d10'),
                   new OptimizedCharset.Range('\u0d12','\u0d28'),
                   new OptimizedCharset.Range('\u0d2a','\u0d39'),
                   new OptimizedCharset.Range('\u0d60','\u0d61'),
                   new OptimizedCharset.Range('\u0d85','\u0d96'),
                   new OptimizedCharset.Range('\u0d9a','\u0db1'),
                   new OptimizedCharset.Range('\u0db3','\u0dbb'),
                   new OptimizedCharset.Range('\u0dc0','\u0dc6'),
                   new OptimizedCharset.Range('\u0e01','\u0e30'),
                   new OptimizedCharset.Range('\u0e32','\u0e33'),
                   new OptimizedCharset.Range('\u0e40','\u0e45'),
                   new OptimizedCharset.Range('\u0e81','\u0e82'),
                   new OptimizedCharset.Range('\u0e87','\u0e88'),
                   new OptimizedCharset.Range('\u0e94','\u0e97'),
                   new OptimizedCharset.Range('\u0e99','\u0e9f'),
                   new OptimizedCharset.Range('\u0ea1','\u0ea3'),
                   new OptimizedCharset.Range('\u0eaa','\u0eab'),
                   new OptimizedCharset.Range('\u0ead','\u0eb0'),
                   new OptimizedCharset.Range('\u0eb2','\u0eb3'),
                   new OptimizedCharset.Range('\u0ec0','\u0ec4'),
                   new OptimizedCharset.Range('\u0edc','\u0edd'),
                   new OptimizedCharset.Range('\u0f40','\u0f47'),
                   new OptimizedCharset.Range('\u0f49','\u0f6a'),
                   new OptimizedCharset.Range('\u0f88','\u0f8b'),
                   new OptimizedCharset.Range('\u1000','\u1021'),
                   new OptimizedCharset.Range('\u1023','\u1027'),
                   new OptimizedCharset.Range('\u1029','\u102a'),
                   new OptimizedCharset.Range('\u1050','\u1055'),
                   new OptimizedCharset.Range('\u10d0','\u10f8'),
                   new OptimizedCharset.Range('\u1100','\u1159'),
                   new OptimizedCharset.Range('\u115f','\u11a2'),
                   new OptimizedCharset.Range('\u11a8','\u11f9'),
                   new OptimizedCharset.Range('\u1200','\u1206'),
                   new OptimizedCharset.Range('\u1208','\u1246'),
                   new OptimizedCharset.Range('\u124a','\u124d'),
                   new OptimizedCharset.Range('\u1250','\u1256'),
                   new OptimizedCharset.Range('\u125a','\u125d'),
                   new OptimizedCharset.Range('\u1260','\u1286'),
                   new OptimizedCharset.Range('\u128a','\u128d'),
                   new OptimizedCharset.Range('\u1290','\u12ae'),
                   new OptimizedCharset.Range('\u12b2','\u12b5'),
                   new OptimizedCharset.Range('\u12b8','\u12be'),
                   new OptimizedCharset.Range('\u12c2','\u12c5'),
                   new OptimizedCharset.Range('\u12c8','\u12ce'),
                   new OptimizedCharset.Range('\u12d0','\u12d6'),
                   new OptimizedCharset.Range('\u12d8','\u12ee'),
                   new OptimizedCharset.Range('\u12f0','\u130e'),
                   new OptimizedCharset.Range('\u1312','\u1315'),
                   new OptimizedCharset.Range('\u1318','\u131e'),
                   new OptimizedCharset.Range('\u1320','\u1346'),
                   new OptimizedCharset.Range('\u1348','\u135a'),
                   new OptimizedCharset.Range('\u13a0','\u13f4'),
                   new OptimizedCharset.Range('\u1401','\u166c'),
                   new OptimizedCharset.Range('\u166f','\u1676'),
                   new OptimizedCharset.Range('\u1681','\u169a'),
                   new OptimizedCharset.Range('\u16a0','\u16ea'),
                   new OptimizedCharset.Range('\u1700','\u170c'),
                   new OptimizedCharset.Range('\u170e','\u1711'),
                   new OptimizedCharset.Range('\u1720','\u1731'),
                   new OptimizedCharset.Range('\u1740','\u1751'),
                   new OptimizedCharset.Range('\u1760','\u176c'),
                   new OptimizedCharset.Range('\u176e','\u1770'),
                   new OptimizedCharset.Range('\u1780','\u17b3'),
                   new OptimizedCharset.Range('\u1820','\u1842'),
                   new OptimizedCharset.Range('\u1844','\u1877'),
                   new OptimizedCharset.Range('\u1880','\u18a8'),
                   new OptimizedCharset.Range('\u1900','\u191c'),
                   new OptimizedCharset.Range('\u1950','\u196d'),
                   new OptimizedCharset.Range('\u2135','\u2138'),
                   new OptimizedCharset.Range('\u3041','\u3096'),
                   new OptimizedCharset.Range('\u30a1','\u30fa'),
                   new OptimizedCharset.Range('\u3105','\u312c'),
                   new OptimizedCharset.Range('\u3131','\u318e'),
                   new OptimizedCharset.Range('\u31a0','\u31b7'),
                   new OptimizedCharset.Range('\u31f0','\u31ff'),
                   new OptimizedCharset.Range('\ua000','\ua48c'),
                   new OptimizedCharset.Range('\ufb1f','\ufb28'),
                   new OptimizedCharset.Range('\ufb2a','\ufb36'),
                   new OptimizedCharset.Range('\ufb38','\ufb3c'),
                   new OptimizedCharset.Range('\ufb40','\ufb41'),
                   new OptimizedCharset.Range('\ufb43','\ufb44'),
                   new OptimizedCharset.Range('\ufb46','\ufbb1'),
                   new OptimizedCharset.Range('\ufbd3','\ufd3d'),
                   new OptimizedCharset.Range('\ufd50','\ufd8f'),
                   new OptimizedCharset.Range('\ufd92','\ufdc7'),
                   new OptimizedCharset.Range('\ufdf0','\ufdfb'),
                   new OptimizedCharset.Range('\ufe70','\ufe74'),
                   new OptimizedCharset.Range('\ufe76','\ufefc'),
                   new OptimizedCharset.Range('\uff66','\uff6f'),
                   new OptimizedCharset.Range('\uff71','\uff9d'),
                   new OptimizedCharset.Range('\uffa0','\uffbe'),
                   new OptimizedCharset.Range('\uffc2','\uffc7'),
                   new OptimizedCharset.Range('\uffca','\uffcf'),
                   new OptimizedCharset.Range('\uffd2','\uffd7'),
                   new OptimizedCharset.Range('\uffda','\uffdc'),
                   };
               char[] oneOfChars = new char[]    {'\u01bb','\u06d5','\u06ff','\u0710','\u07b1'
                                                  ,'\u093d','\u0950','\u09b2','\u09bd','\u0a5e'
                                                  ,'\u0abd','\u0ad0','\u0b3d','\u0b71','\u0b83'
                                                  ,'\u0b9c','\u0cbd','\u0cde','\u0dbd','\u0e84'
                                                  ,'\u0e8a','\u0e8d','\u0ea5','\u0ea7','\u0ebd'
                                                  ,'\u0f00','\u1248','\u1258','\u1288','\u12b0'
                                                  ,'\u12c0','\u1310','\u17dc','\u3006','\u303c'
                                                  ,'\u309f','\u30ff','\u3400','\u4db5','\u4e00'
                                                  ,'\u9fa5','\uac00','\ud7a3','\ufb1d','\ufb3e'
                                                  };
               optimizedCharset6= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0300','\u0357'),
                   new OptimizedCharset.Range('\u035d','\u036f'),
                   new OptimizedCharset.Range('\u0483','\u0486'),
                   new OptimizedCharset.Range('\u0591','\u05a1'),
                   new OptimizedCharset.Range('\u05a3','\u05b9'),
                   new OptimizedCharset.Range('\u05bb','\u05bd'),
                   new OptimizedCharset.Range('\u05c1','\u05c2'),
                   new OptimizedCharset.Range('\u0610','\u0615'),
                   new OptimizedCharset.Range('\u064b','\u0658'),
                   new OptimizedCharset.Range('\u06d6','\u06dc'),
                   new OptimizedCharset.Range('\u06df','\u06e4'),
                   new OptimizedCharset.Range('\u06e7','\u06e8'),
                   new OptimizedCharset.Range('\u06ea','\u06ed'),
                   new OptimizedCharset.Range('\u0730','\u074a'),
                   new OptimizedCharset.Range('\u07a6','\u07b0'),
                   new OptimizedCharset.Range('\u0901','\u0902'),
                   new OptimizedCharset.Range('\u0941','\u0948'),
                   new OptimizedCharset.Range('\u0951','\u0954'),
                   new OptimizedCharset.Range('\u0962','\u0963'),
                   new OptimizedCharset.Range('\u09c1','\u09c4'),
                   new OptimizedCharset.Range('\u09e2','\u09e3'),
                   new OptimizedCharset.Range('\u0a01','\u0a02'),
                   new OptimizedCharset.Range('\u0a41','\u0a42'),
                   new OptimizedCharset.Range('\u0a47','\u0a48'),
                   new OptimizedCharset.Range('\u0a4b','\u0a4d'),
                   new OptimizedCharset.Range('\u0a70','\u0a71'),
                   new OptimizedCharset.Range('\u0a81','\u0a82'),
                   new OptimizedCharset.Range('\u0ac1','\u0ac5'),
                   new OptimizedCharset.Range('\u0ac7','\u0ac8'),
                   new OptimizedCharset.Range('\u0ae2','\u0ae3'),
                   new OptimizedCharset.Range('\u0b41','\u0b43'),
                   new OptimizedCharset.Range('\u0c3e','\u0c40'),
                   new OptimizedCharset.Range('\u0c46','\u0c48'),
                   new OptimizedCharset.Range('\u0c4a','\u0c4d'),
                   new OptimizedCharset.Range('\u0c55','\u0c56'),
                   new OptimizedCharset.Range('\u0ccc','\u0ccd'),
                   new OptimizedCharset.Range('\u0d41','\u0d43'),
                   new OptimizedCharset.Range('\u0dd2','\u0dd4'),
                   new OptimizedCharset.Range('\u0e34','\u0e3a'),
                   new OptimizedCharset.Range('\u0e47','\u0e4e'),
                   new OptimizedCharset.Range('\u0eb4','\u0eb9'),
                   new OptimizedCharset.Range('\u0ebb','\u0ebc'),
                   new OptimizedCharset.Range('\u0ec8','\u0ecd'),
                   new OptimizedCharset.Range('\u0f18','\u0f19'),
                   new OptimizedCharset.Range('\u0f71','\u0f7e'),
                   new OptimizedCharset.Range('\u0f80','\u0f84'),
                   new OptimizedCharset.Range('\u0f86','\u0f87'),
                   new OptimizedCharset.Range('\u0f90','\u0f97'),
                   new OptimizedCharset.Range('\u0f99','\u0fbc'),
                   new OptimizedCharset.Range('\u102d','\u1030'),
                   new OptimizedCharset.Range('\u1036','\u1037'),
                   new OptimizedCharset.Range('\u1058','\u1059'),
                   new OptimizedCharset.Range('\u1712','\u1714'),
                   new OptimizedCharset.Range('\u1732','\u1734'),
                   new OptimizedCharset.Range('\u1752','\u1753'),
                   new OptimizedCharset.Range('\u1772','\u1773'),
                   new OptimizedCharset.Range('\u17b7','\u17bd'),
                   new OptimizedCharset.Range('\u17c9','\u17d3'),
                   new OptimizedCharset.Range('\u180b','\u180d'),
                   new OptimizedCharset.Range('\u1920','\u1922'),
                   new OptimizedCharset.Range('\u1927','\u1928'),
                   new OptimizedCharset.Range('\u1939','\u193b'),
                   new OptimizedCharset.Range('\u20d0','\u20dc'),
                   new OptimizedCharset.Range('\u20e5','\u20ea'),
                   new OptimizedCharset.Range('\u302a','\u302f'),
                   new OptimizedCharset.Range('\u3099','\u309a'),
                   new OptimizedCharset.Range('\ufe20','\ufe23'),
                   };
               char[] oneOfChars = new char[]    {'\u05bf','\u05c4','\u0670','\u0711','\u093c'
                                                  ,'\u094d','\u0981','\u09bc','\u09cd','\u0a3c'
                                                  ,'\u0abc','\u0acd','\u0b01','\u0b3c','\u0b3f'
                                                  ,'\u0b4d','\u0b56','\u0b82','\u0bc0','\u0bcd'
                                                  ,'\u0cbc','\u0cbf','\u0cc6','\u0d4d','\u0dca'
                                                  ,'\u0dd6','\u0e31','\u0eb1','\u0f35','\u0f37'
                                                  ,'\u0f39','\u0fc6','\u1032','\u1039','\u17c6'
                                                  ,'\u17dd','\u18a9','\u1932','\u20e1','\ufb1e'
                                                  };
               optimizedCharset7= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u093e','\u0940'),
                   new OptimizedCharset.Range('\u0949','\u094c'),
                   new OptimizedCharset.Range('\u0982','\u0983'),
                   new OptimizedCharset.Range('\u09be','\u09c0'),
                   new OptimizedCharset.Range('\u09c7','\u09c8'),
                   new OptimizedCharset.Range('\u09cb','\u09cc'),
                   new OptimizedCharset.Range('\u0a3e','\u0a40'),
                   new OptimizedCharset.Range('\u0abe','\u0ac0'),
                   new OptimizedCharset.Range('\u0acb','\u0acc'),
                   new OptimizedCharset.Range('\u0b02','\u0b03'),
                   new OptimizedCharset.Range('\u0b47','\u0b48'),
                   new OptimizedCharset.Range('\u0b4b','\u0b4c'),
                   new OptimizedCharset.Range('\u0bbe','\u0bbf'),
                   new OptimizedCharset.Range('\u0bc1','\u0bc2'),
                   new OptimizedCharset.Range('\u0bc6','\u0bc8'),
                   new OptimizedCharset.Range('\u0bca','\u0bcc'),
                   new OptimizedCharset.Range('\u0c01','\u0c03'),
                   new OptimizedCharset.Range('\u0c41','\u0c44'),
                   new OptimizedCharset.Range('\u0c82','\u0c83'),
                   new OptimizedCharset.Range('\u0cc0','\u0cc4'),
                   new OptimizedCharset.Range('\u0cc7','\u0cc8'),
                   new OptimizedCharset.Range('\u0cca','\u0ccb'),
                   new OptimizedCharset.Range('\u0cd5','\u0cd6'),
                   new OptimizedCharset.Range('\u0d02','\u0d03'),
                   new OptimizedCharset.Range('\u0d3e','\u0d40'),
                   new OptimizedCharset.Range('\u0d46','\u0d48'),
                   new OptimizedCharset.Range('\u0d4a','\u0d4c'),
                   new OptimizedCharset.Range('\u0d82','\u0d83'),
                   new OptimizedCharset.Range('\u0dcf','\u0dd1'),
                   new OptimizedCharset.Range('\u0dd8','\u0ddf'),
                   new OptimizedCharset.Range('\u0df2','\u0df3'),
                   new OptimizedCharset.Range('\u0f3e','\u0f3f'),
                   new OptimizedCharset.Range('\u1056','\u1057'),
                   new OptimizedCharset.Range('\u17be','\u17c5'),
                   new OptimizedCharset.Range('\u17c7','\u17c8'),
                   new OptimizedCharset.Range('\u1923','\u1926'),
                   new OptimizedCharset.Range('\u1929','\u192b'),
                   new OptimizedCharset.Range('\u1930','\u1931'),
                   new OptimizedCharset.Range('\u1933','\u1938'),
                   };
               char[] oneOfChars = new char[]    {'\u0903','\u09d7','\u0a03','\u0a83','\u0ac9'
                                                  ,'\u0b3e','\u0b40','\u0b57','\u0bd7','\u0cbe'
                                                  ,'\u0d57','\u0f7f','\u102c','\u1031','\u1038'
                                                  ,'\u17b6'};
               optimizedCharset8= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0030','\u0039'),
                   new OptimizedCharset.Range('\u0660','\u0669'),
                   new OptimizedCharset.Range('\u06f0','\u06f9'),
                   new OptimizedCharset.Range('\u0966','\u096f'),
                   new OptimizedCharset.Range('\u09e6','\u09ef'),
                   new OptimizedCharset.Range('\u0a66','\u0a6f'),
                   new OptimizedCharset.Range('\u0ae6','\u0aef'),
                   new OptimizedCharset.Range('\u0b66','\u0b6f'),
                   new OptimizedCharset.Range('\u0be7','\u0bef'),
                   new OptimizedCharset.Range('\u0c66','\u0c6f'),
                   new OptimizedCharset.Range('\u0ce6','\u0cef'),
                   new OptimizedCharset.Range('\u0d66','\u0d6f'),
                   new OptimizedCharset.Range('\u0e50','\u0e59'),
                   new OptimizedCharset.Range('\u0ed0','\u0ed9'),
                   new OptimizedCharset.Range('\u0f20','\u0f29'),
                   new OptimizedCharset.Range('\u1040','\u1049'),
                   new OptimizedCharset.Range('\u1369','\u1371'),
                   new OptimizedCharset.Range('\u17e0','\u17e9'),
                   new OptimizedCharset.Range('\u1810','\u1819'),
                   new OptimizedCharset.Range('\u1946','\u194f'),
                   new OptimizedCharset.Range('\uff10','\uff19'),
                   };
               optimizedCharset9= new OptimizedCharset(ranges,null);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u203f','\u2040'),
                   new OptimizedCharset.Range('\ufe33','\ufe34'),
                   new OptimizedCharset.Range('\ufe4d','\ufe4f'),
                   };
               char[] oneOfChars = new char[]    {'\u005f','\u2054','\u30fb','\uff3f','\uff65'
                                                  };
               optimizedCharset10= new OptimizedCharset(ranges,oneOfChars);
            }
            
            {
               OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]
                  {new OptimizedCharset.Range('\u0600','\u0603'),
                   new OptimizedCharset.Range('\u17b4','\u17b5'),
                   new OptimizedCharset.Range('\u200b','\u200f'),
                   new OptimizedCharset.Range('\u202a','\u202e'),
                   new OptimizedCharset.Range('\u2060','\u2063'),
                   new OptimizedCharset.Range('\u206a','\u206f'),
                   new OptimizedCharset.Range('\ufff9','\ufffb'),
                   };
               char[] oneOfChars = new char[]    {'\u00ad','\u06dd','\u070f','\ufeff'};
               optimizedCharset11= new OptimizedCharset(ranges,oneOfChars);
            }
            
            
            {
               string[] literals=
               { "abstract","as","base","bool","break","byte","case","catch",
                  "char","checked","class","const","continue","decimal","default","delegate",
                  "do","double","else","enum","event","explicit","extern","false",
                  "finally","fixed","float","for","foreach","goto","if","implicit",
                  "in","int","interface","internal","is","lock","long","namespace",
                  "new","null","object","operator","out","override","params","private",
                  "protected","public","readonly","ref","return","sbyte","sealed","short",
                  "sizeof","stackalloc","static","string","struct","switch","this","throw",
                  "true","try","typeof","uint","ulong","unchecked","unsafe","ushort",
                  "using","virtual","void","volatile","while" };
               optimizedLiterals0= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "{","}","[","]","(",")",".",",",
                  ":",";","+","-","*"," /","%","&",
                  "|","^","!","~","=","<",">","?",
                  "??","::","++","--","&&","||","->","==",
                  "!=","<=",">=","+=","-=","*="," /=","%=",
                  "&=","|=","^=","<<",">>","<<=",">>=","=>" };
               optimizedLiterals1= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "sbyte","byte","short","ushort","int","uint","long","ulong",
                  "char" };
               optimizedLiterals2= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "bool","byte","char","decimal","double","float","int","long",
                  "object","sbyte","short","string","uint","ulong","ushort" };
               optimizedLiterals3= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","abstract","sealed","static",
                  "unsafe" };
               optimizedLiterals4= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","static","readonly","volatile",
                  "unsafe" };
               optimizedLiterals5= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","static","virtual","sealed",
                  "override","abstract","extern","unsafe" };
               optimizedLiterals6= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","static","virtual","sealed",
                  "override","abstract","extern","unsafe" };
               optimizedLiterals7= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","static","virtual","sealed",
                  "override","abstract","extern","unsafe" };
               optimizedLiterals8= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "new","public","protected","internal","private","virtual","sealed","override",
                  "abstract","extern","unsafe" };
               optimizedLiterals9= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "++","--","+","-","!","~","true","false" };
               optimizedLiterals10= new OptimizedLiterals(literals);
            }

            {
               string[] literals=
               { "+","-","*","/","%","&","|","^",
                  "<<",">>","==","!=",">=","<=",">","<" };
               optimizedLiterals11= new OptimizedLiterals(literals);
            }

            
        }
        #endregion Optimization Data 
           }
}