/* created on 21.09.2008 16:14:00 from peg generator V1.0*/

using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace CSharp3Fast
{
      
      enum ECSharp3Fast{new_line= 1, comment= 2, single_line_comment= 3, input_characters= 4, 
                         input_character= 5, new_line_character= 6, delimited_comment= 7, 
                         whitespace= 8, unicode_escape_sequence= 9, identifier= 10, available_identifier= 11, 
                         identifier_or_keyword= 12, keyword= 13, literal= 14, boolean_literal= 15, 
                         integer_literal= 16, decimal_integer_literal= 17, decimal_digits= 18, 
                         decimal_digit= 19, integer_type_suffix= 20, hexadecimal_integer_literal= 21, 
                         hex_digits= 22, hex_digit= 23, real_literal= 24, fraction= 25, 
                         exponent_part= 26, sign= 27, real_type_suffix= 28, character_literal= 29, 
                         character= 30, single_character= 31, simple_escape_sequence= 32, 
                         hexadecimal_escape_sequence= 33, string_literal= 34, regular_string_literal= 35, 
                         regular_string_literal_characters= 36, regular_string_literal_character= 37, 
                         single_regular_string_literal_character= 38, verbatim_string_literal= 39, 
                         verbatim_string_literal_characters= 40, verbatim_string_literal_character= 41, 
                         single_verbatim_string_literal_character= 42, quote_escape_sequence= 43, 
                         null_literal= 44, operator_or_punctuator= 45, right_shift= 46, 
                         right_shift_assignment= 47, pp_directive= 48, conditional_symbol= 49, 
                         symbolName= 50, pp_expression= 51, pp_or_expression= 52, pp_and_expression= 53, 
                         pp_equality_expression= 54, pp_unary_expression= 55, pp_primary_expression= 56, 
                         pp_declaration= 57, pp_new_line= 58, pp_conditional= 59, pp_if_section= 60, 
                         pp_elif_section= 61, pp_else_section= 62, pp_endif= 63, pp_diagnostic= 64, 
                         pp_message= 65, pp_region= 66, pp_start_region= 67, pp_end_region= 68, 
                         pp_line= 69, line_indicator= 70, file_name= 71, file_name_characters= 72, 
                         file_name_character= 73, pp_pragma= 74, pragma_body= 75, pragma_warning_body= 76, 
                         warning_action= 77, warning_list= 78, namespace_name= 79, type_name= 80, 
                         namespace_or_type_name= 81, name= 82, type= 83, pointer_type= 84, 
                         pointers= 85, void_pointer= 86, value_type= 87, struct_type= 88, 
                         simple_type= 89, numeric_type= 90, integral_type= 91, floating_point_type= 92, 
                         is_nullable= 93, non_nullable_type= 94, enum_type= 95, non_array_reference_type= 96, 
                         class_type= 97, interface_type= 98, array_type= 99, non_array_non_nullable_type= 100, 
                         non_array_type= 101, rank_specifiers= 102, rank_specifier= 103, 
                         dim_separators= 104, delegate_type= 105, type_argument_list= 106, 
                         type_arguments= 107, type_argument= 108, type_parameter= 109, 
                         variable_reference= 110, argument_list= 111, argument= 112, postfix_expression= 113, 
                         postfix_operation= 114, invocation= 115, member_access= 116, 
                         pointer_member_access= 117, element_access= 118, index= 119, 
                         post_incr= 120, post_decr= 121, primary_expression= 122, sizeof_expression= 123, 
                         simple_name= 124, parenthesized_expression= 125, special_member_access= 126, 
                         predefined_type= 127, expression_list= 128, this_access= 129, 
                         base_access= 130, post_increment_expression= 131, post_decrement_expression= 132, 
                         object_creation_expression= 133, object_or_collection_initializer= 134, 
                         object_initializer= 135, member_initializer_list= 136, member_initializer= 137, 
                         initializer_value= 138, collection_initializer= 139, element_initializer_list= 140, 
                         element_initializer= 141, initial_value_list= 142, array_creation_expression= 143, 
                         array_size= 144, delegate_creation_expression= 145, anonymous_object_creation_expression= 146, 
                         anonymous_object_initializer= 147, member_declarator_list= 148, 
                         member_declarator= 149, full_member_access= 150, typeof_expression= 151, 
                         unbound_type_name= 152, generic_dimension_specifier= 153, commas= 154, 
                         checked_expression= 155, unchecked_expression= 156, default_value_expression= 157, 
                         unary_expression= 158, creation_expression= 159, pre_increment_expression= 160, 
                         pre_decrement_expression= 161, cast_expression= 162, multiplicative_expression= 163, 
                         additive_expression= 164, shift_expression= 165, relational_expression= 166, 
                         equality_expression= 167, and_expression= 168, exclusive_or_expression= 169, 
                         inclusive_or_expression= 170, conditional_and_expression= 171, 
                         conditional_or_expression= 172, null_coalescing_expression= 173, 
                         conditional_expression= 174, if_else_expression= 175, lambda_expression= 176, 
                         anonymous_method_expression= 177, anonymous_function_signature= 178, 
                         explicit_anonymous_function_signature= 179, explicit_anonymous_function_parameter_list= 180, 
                         explicit_anonymous_function_parameter= 181, anonymous_function_parameter_modifier= 182, 
                         implicit_anonymous_function_signature= 183, implicit_anonymous_function_parameter_list= 184, 
                         implicit_anonymous_function_parameter= 185, anonymous_function_body= 186, 
                         query_expression= 187, from_clause= 188, query_body= 189, query_body_clauses= 190, 
                         query_body_clause= 191, let_clause= 192, where_clause= 193, join_into_clause= 194, 
                         orderby_clause= 195, orderings= 196, ordering= 197, ordering_direction= 198, 
                         select_or_group_clause= 199, select_clause= 200, group_clause= 201, 
                         query_continuation= 202, assignment= 203, assignment_operator= 204, 
                         expression= 205, non_assignment_expression= 206, constant_expression= 207, 
                         boolean_expression= 208, statement= 209, embedded_statement= 210, 
                         block= 211, statement_list= 212, empty_statement= 213, labeled_statement= 214, 
                         label= 215, declaration_statement= 216, local_variable_declaration= 217, 
                         local_variable_type= 218, local_variable_declarators= 219, local_variable_declarator= 220, 
                         variable_name= 221, local_variable_initializer= 222, stackalloc_initializer= 223, 
                         local_constant_declaration= 224, constant_declarators= 225, constant_declarator= 226, 
                         constant_name= 227, expression_statement= 228, unsafe_statement= 229, 
                         fixed_statement= 230, fixed_pointer_declarators= 231, fixed_pointer_declarator= 232, 
                         fixed_pointer_initializer= 233, statement_expression= 234, call_or_post_incr_decr= 235, 
                         selection_statement= 236, if_statement= 237, switch_statement= 238, 
                         switch_block= 239, switch_sections= 240, switch_section= 241, 
                         switch_labels= 242, switch_label= 243, iteration_statement= 244, 
                         while_statement= 245, do_statement= 246, for_statement= 247, 
                         for_initializer= 248, for_condition= 249, for_iterator= 250, 
                         statement_expression_list= 251, foreach_statement= 252, jump_statement= 253, 
                         break_statement= 254, continue_statement= 255, goto_statement= 256, 
                         return_statement= 257, throw_statement= 258, try_statement= 259, 
                         catch_clauses= 260, specific_catch_clauses= 261, specific_catch_clause= 262, 
                         general_catch_clause= 263, finally_clause= 264, checked_statement= 265, 
                         unchecked_statement= 266, lock_statement= 267, using_statement= 268, 
                         resource_acquisition= 269, yield_statement= 270, compilation_unit= 271, 
                         namespace_declaration= 272, qualified_identifier= 273, namespace_body= 274, 
                         extern_alias_directives= 275, extern_alias_directive= 276, alias_name= 277, 
                         using_directives= 278, using_directive= 279, using_alias_directive= 280, 
                         using_alias_name= 281, using_namespace_directive= 282, namespace_member_declarations= 283, 
                         namespace_member_declaration= 284, type_declaration= 285, qualified_alias_member= 286, 
                         class_declaration= 287, class_name= 288, class_modifiers= 289, 
                         class_modifier= 290, type_parameter_list= 291, type_parameters= 292, 
                         class_base= 293, interface_type_list= 294, type_parameter_constraints_clauses= 295, 
                         type_parameter_constraints_clause= 296, type_parameter_constraints= 297, 
                         primary_constraint= 298, secondary_constraints= 299, constructor_constraint= 300, 
                         class_body= 301, class_member_declarations= 302, class_member_declaration= 303, 
                         constant_declaration= 304, constant_modifiers= 305, constant_modifier= 306, 
                         field_declaration= 307, field_modifiers= 308, field_modifier= 309, 
                         variable_declarators= 310, variable_declarator= 311, variable_initializer= 312, 
                         method_declaration= 313, method_header= 314, method_modifiers= 315, 
                         method_modifier= 316, return_type= 317, interface_name_before_member= 318, 
                         method_body= 319, missing_body= 320, formal_parameter_list= 321, 
                         fixed_parameters= 322, fixed_parameter= 323, parameter_modifier= 324, 
                         parameter_array= 325, property_declaration= 326, property_modifiers= 327, 
                         property_modifier= 328, member_name= 329, accessor_declarations= 330, 
                         get_accessor_declaration= 331, set_accessor_declaration= 332, 
                         accessor_modifier= 333, accessor_body= 334, event_declaration= 335, 
                         event_modifiers= 336, event_modifier= 337, event_accessor_declarations= 338, 
                         add_accessor_declaration= 339, remove_accessor_declaration= 340, 
                         indexer_declaration= 341, indexer_modifiers= 342, indexer_modifier= 343, 
                         indexer_declarator= 344, operator_declaration= 345, operator_modifiers= 346, 
                         operator_modifier= 347, operator_declarator= 348, unary_operator_declarator= 349, 
                         overloadable_unary_operator= 350, binary_operator_declarator= 351, 
                         overloadable_binary_operator= 352, conversion_operator_declarator= 353, 
                         operator_body= 354, constructor_declaration= 355, constructor_modifiers= 356, 
                         constructor_modifier= 357, constructor_declarator= 358, constructor_initializer= 359, 
                         constructor_body= 360, static_constructor_declaration= 361, static_constructor_modifiers= 362, 
                         static_constructor_body= 363, destructor_declaration= 364, destructor_body= 365, 
                         struct_declaration= 366, struct_name= 367, struct_modifiers= 368, 
                         struct_modifier= 369, struct_interfaces= 370, struct_body= 371, 
                         struct_member_declarations= 372, struct_member_declaration= 373, 
                         fixed_size_buffer_declaration= 374, fixed_size_buffer_modifiers= 375, 
                         fixed_size_buffer_modifier= 376, buffer_element_type= 377, fixed_size_buffer_declarators= 378, 
                         fixed_size_buffer_declarator= 379, array_initializer= 380, variable_initializer_list= 381, 
                         interface_declaration= 382, interface_name= 383, interface_modifiers= 384, 
                         interface_modifier= 385, interface_base= 386, interface_body= 387, 
                         interface_member_declarations= 388, interface_member_declaration= 389, 
                         interface_method_declaration= 390, interface_property_declaration= 391, 
                         interface_accessors= 392, interface_event_declaration= 393, interface_indexer_declaration= 394, 
                         enum_declaration= 395, enum_name= 396, enum_base= 397, enum_body= 398, 
                         enum_modifiers= 399, enum_modifier= 400, enum_member_declarations= 401, 
                         enum_member_declaration= 402, enumerator_name= 403, delegate_declaration= 404, 
                         delegate_name= 405, delegate_modifiers= 406, delegate_modifier= 407, 
                         global_attributes= 408, global_attribute_sections= 409, global_attribute_section= 410, 
                         global_attribute_target_specifier= 411, global_attribute_target= 412, 
                         attributes= 413, attribute_sections= 414, attribute_section= 415, 
                         attribute_target_specifier= 416, attribute_target= 417, attribute_list= 418, 
                         attribute= 419, attribute_name= 420, attribute_arguments= 421, 
                         positional_argument_list= 422, positional_argument= 423, named_argument_list= 424, 
                         named_argument= 425, parameter_name= 426, attribute_argument_expression= 427, 
                         B= 428, S= 429, Zs= 430};
      class CSharp3Fast : PegCharParser 
      {
        
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.utf8;
        public static UnicodeDetection unicodeDetection = UnicodeDetection.notApplicable;
        #endregion Input Properties
        #region Constructors
        public CSharp3Fast()
            : base()
        {
            
        }
        public CSharp3Fast(string src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   ECSharp3Fast ruleEnum = (ECSharp3Fast)id;
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
   class _identifier_or_keyword{
  CSharp3Fast parent_;
  internal _identifier_or_keyword(CSharp3Fast parent)
  {
      parent_ = parent;
  }
  bool TryGetUnicodeFromHexString(out char uniCodeChar,out int readLength)
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      uniCodeChar='\0';
      readLength=0;
      string hexString;
      if (parent_.src_[parent_.pos_ + 1] == 'u' && parent_.pos_ + 5 < parent_.srcLen_)
      {
          hexString = parent_.src_.Substring(parent_.pos_ + 2, 4);
          readLength=6;
      }
      else if (parent_.src_[parent_.pos_ + 1] == 'U' && parent_.pos_ + 9 < parent_.srcLen_)
      {
          hexString = parent_.src_.Substring(parent_.pos_ + 2, 8);
          readLength=10;
      }
      else return false;
      try
      {
          uint val = UInt32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
          uniCodeChar = (char)val;
          return (val & 0xFFFF0000) == 0;
      }
      catch (Exception) { return false; }
  }
  bool UnicodeEscapeIsLetter()
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      char uniCodeChar;
      int readLength;
      if (!TryGetUnicodeFromHexString(out uniCodeChar, out readLength)) return false;
      if (UnicodeIsLetter(uniCodeChar))
      {
          parent_.pos_+= readLength;
          return true;
      }
      return false;
  }
  static bool UnicodeIsLetter(char c)
  {
      var cat =System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
      switch (cat)
      {
          case System.Globalization.UnicodeCategory.UppercaseLetter:
          case System.Globalization.UnicodeCategory.LowercaseLetter:
          case System.Globalization.UnicodeCategory.TitlecaseLetter:
          case System.Globalization.UnicodeCategory.ModifierLetter:
          case System.Globalization.UnicodeCategory.OtherLetter:
          case System.Globalization.UnicodeCategory.LetterNumber: return true;
          default: return false;
      }
  }
  bool UnicodeEscapeIsIdentierPartCharacter()
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      char uniCodeChar;
      int readLength;
      if (!TryGetUnicodeFromHexString(out uniCodeChar, out readLength)) return false;
      if (UnicodeIsIdentierPartCharacter(uniCodeChar))
      {
          parent_.pos_ += readLength;
          return true;
      }
      return false;
  }
  static bool UnicodeIsIdentierPartCharacter(char c)
  {
      var  cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
      switch (cat)
      {
          case System.Globalization.UnicodeCategory.UppercaseLetter:
          case System.Globalization.UnicodeCategory.LowercaseLetter:
          case System.Globalization.UnicodeCategory.TitlecaseLetter:
          case System.Globalization.UnicodeCategory.ModifierLetter:
          case System.Globalization.UnicodeCategory.OtherLetter:
          case System.Globalization.UnicodeCategory.LetterNumber: 
          case System.Globalization.UnicodeCategory.DecimalDigitNumber:
          case System.Globalization.UnicodeCategory.ConnectorPunctuation:
          case System.Globalization.UnicodeCategory.NonSpacingMark:
          case System.Globalization.UnicodeCategory.SpacingCombiningMark:
          case System.Globalization.UnicodeCategory.Format:
          return true;
          default: return false;
      }
              
  }
  internal bool ReadCSharpIdentifier_()
  {
      if( parent_.pos_ >= parent_.srcLen_ ) return false;
      if (parent_.src_[parent_.pos_] == '\\') return UnicodeEscapeIsLetter();
      if (  !(
               UnicodeIsLetter(parent_.src_[parent_.pos_])
            || parent_.src_[parent_.pos_]=='_'
            || parent_.src_[parent_.pos_] == '\\' && UnicodeEscapeIsLetter())) return false;
      while ( ++parent_.pos_ < parent_.srcLen_)
      {
          if (! (   UnicodeIsIdentierPartCharacter(parent_.src_[parent_.pos_])
                || parent_.src_[parent_.pos_] == '\\' && UnicodeEscapeIsIdentierPartCharacter())) break;
      }
      return true;
  }
}

        public bool identifier_or_keyword()    /*identifier_or_keyword
{
  CSharp3Fast parent_;
  internal _identifier_or_keyword(CSharp3Fast parent)
  {
      parent_ = parent;
  }
  bool TryGetUnicodeFromHexString(out char uniCodeChar,out int readLength)
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      uniCodeChar='\0';
      readLength=0;
      string hexString;
      if (parent_.src_[parent_.pos_ + 1] == 'u' && parent_.pos_ + 5 < parent_.srcLen_)
      {
          hexString = parent_.src_.Substring(parent_.pos_ + 2, 4);
          readLength=6;
      }
      else if (parent_.src_[parent_.pos_ + 1] == 'U' && parent_.pos_ + 9 < parent_.srcLen_)
      {
          hexString = parent_.src_.Substring(parent_.pos_ + 2, 8);
          readLength=10;
      }
      else return false;
      try
      {
          uint val = UInt32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
          uniCodeChar = (char)val;
          return (val & 0xFFFF0000) == 0;
      }
      catch (Exception) { return false; }
  }
  bool UnicodeEscapeIsLetter()
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      char uniCodeChar;
      int readLength;
      if (!TryGetUnicodeFromHexString(out uniCodeChar, out readLength)) return false;
      if (UnicodeIsLetter(uniCodeChar))
      {
          parent_.pos_+= readLength;
          return true;
      }
      return false;
  }
  static bool UnicodeIsLetter(char c)
  {
      var cat =System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
      switch (cat)
      {
          case System.Globalization.UnicodeCategory.UppercaseLetter:
          case System.Globalization.UnicodeCategory.LowercaseLetter:
          case System.Globalization.UnicodeCategory.TitlecaseLetter:
          case System.Globalization.UnicodeCategory.ModifierLetter:
          case System.Globalization.UnicodeCategory.OtherLetter:
          case System.Globalization.UnicodeCategory.LetterNumber: return true;
          default: return false;
      }
  }
  bool UnicodeEscapeIsIdentierPartCharacter()
  {
      System.Diagnostics.Debug.Assert(parent_.src_[parent_.pos_] == '\\');
      char uniCodeChar;
      int readLength;
      if (!TryGetUnicodeFromHexString(out uniCodeChar, out readLength)) return false;
      if (UnicodeIsIdentierPartCharacter(uniCodeChar))
      {
          parent_.pos_ += readLength;
          return true;
      }
      return false;
  }
  static bool UnicodeIsIdentierPartCharacter(char c)
  {
      var  cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
      switch (cat)
      {
          case System.Globalization.UnicodeCategory.UppercaseLetter:
          case System.Globalization.UnicodeCategory.LowercaseLetter:
          case System.Globalization.UnicodeCategory.TitlecaseLetter:
          case System.Globalization.UnicodeCategory.ModifierLetter:
          case System.Globalization.UnicodeCategory.OtherLetter:
          case System.Globalization.UnicodeCategory.LetterNumber: 
          case System.Globalization.UnicodeCategory.DecimalDigitNumber:
          case System.Globalization.UnicodeCategory.ConnectorPunctuation:
          case System.Globalization.UnicodeCategory.NonSpacingMark:
          case System.Globalization.UnicodeCategory.SpacingCombiningMark:
          case System.Globalization.UnicodeCategory.Format:
          return true;
          default: return false;
      }
              
  }
  internal bool ReadCSharpIdentifier_()
  {
      if( parent_.pos_ >= parent_.srcLen_ ) return false;
      if (parent_.src_[parent_.pos_] == '\\') return UnicodeEscapeIsLetter();
      if (  !(
               UnicodeIsLetter(parent_.src_[parent_.pos_])
            || parent_.src_[parent_.pos_]=='_'
            || parent_.src_[parent_.pos_] == '\\' && UnicodeEscapeIsLetter())) return false;
      while ( ++parent_.pos_ < parent_.srcLen_)
      {
          if (! (   UnicodeIsIdentierPartCharacter(parent_.src_[parent_.pos_])
                || parent_.src_[parent_.pos_] == '\\' && UnicodeEscapeIsIdentierPartCharacter())) break;
      }
      return true;
  }
}
: 	ReadCSharpIdentifier_;

//A.1.7 Keywords
//----------------------*/
        {

             var _sem= new _identifier_or_keyword(this);

           return _sem.ReadCSharpIdentifier_();
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

           return TreeAST((int)ECSharp3Fast.boolean_literal,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.decimal_integer_literal,()=>
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

           return TreeAST((int)ECSharp3Fast.hexadecimal_integer_literal,()=>
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

           return TreeAST((int)ECSharp3Fast.real_literal,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.character_literal,()=>
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

           return TreeAST((int)ECSharp3Fast.string_literal,()=>
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

           return TreeNT((int)ECSharp3Fast.symbolName,()=>
                identifier() );
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

           return TreeNT((int)ECSharp3Fast.pp_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.pp_if_section,()=>
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

           return TreeNT((int)ECSharp3Fast.pp_elif_section,()=>
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

           return TreeNT((int)ECSharp3Fast.pp_else_section,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('e','l','s','e')
                  && pp_new_line() ) );
		}
        public bool pp_endif()    /*^^pp_endif: 		whitespace*   '#'   whitespace*   'endif'   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3Fast.pp_endif,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('e','n','d','i','f')
                  && pp_new_line() ) );
		}
        public bool pp_diagnostic()    /*^^pp_diagnostic: whitespace*   '#'   whitespace*   ('error'/'warning')   pp_message;*/
        {

           return TreeNT((int)ECSharp3Fast.pp_diagnostic,()=>
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

           return TreeNT((int)ECSharp3Fast.pp_start_region,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char('r','e','g','i','o','n')
                  && pp_message() ) );
		}
        public bool pp_end_region()    /*^^pp_end_region: whitespace*   '#'   whitespace*   'endregion'   pp_message;*/
        {

           return TreeNT((int)ECSharp3Fast.pp_end_region,()=>
                And(()=>  
                     OptRepeat(()=> whitespace() )
                  && Char('#')
                  && OptRepeat(()=> whitespace() )
                  && Char("endregion")
                  && pp_message() ) );
		}
        public bool pp_line()    /*^^pp_line: whitespace*   '#'   whitespace*   'line'   whitespace+   line_indicator   pp_new_line;*/
        {

           return TreeNT((int)ECSharp3Fast.pp_line,()=>
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

           return TreeNT((int)ECSharp3Fast.pp_pragma,()=>
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

           return TreeNT((int)ECSharp3Fast.warning_action,()=>
                  
                     Char('d','i','s','a','b','l','e')
                  || Char('r','e','s','t','o','r','e') );
		}
        public bool warning_list()    /*^^warning_list: 		decimal_digits  (whitespace*   ','   whitespace+   @decimal_digits)*;
//A.2 Syntactic grammar
//---------------------
//A.2.1 Basic concepts
//----------------------*/
        {

           return TreeNT((int)ECSharp3Fast.warning_list,()=>
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

           return TreeAST((int)ECSharp3Fast.namespace_name,()=>
                namespace_or_type_name() );
		}
        public bool type_name()    /*^^type_name: 			namespace_or_type_name;*/
        {

           return TreeNT((int)ECSharp3Fast.type_name,()=>
                namespace_or_type_name() );
		}
        public bool namespace_or_type_name()    /*^namespace_or_type_name: 	( qualified_alias_member / name S type_argument_list? )  
			 	('.' S   @name S  type_argument_list?)*;*/
        {

           return TreeAST((int)ECSharp3Fast.namespace_or_type_name,()=>
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

           return TreeNT((int)ECSharp3Fast.name,()=> identifier() );
		}
        public bool type()    /*^^type: 			void_pointer / non_array_type rank_specifiers? pointers?;*/
        {

           return TreeNT((int)ECSharp3Fast.type,()=>
                  
                     void_pointer()
                  || And(()=>    
                         non_array_type()
                      && Option(()=> rank_specifiers() )
                      && Option(()=> pointers() ) ) );
		}
        public bool pointer_type()    /*^^pointer_type:			void_pointer / non_array_type rank_specifiers? pointers;*/
        {

           return TreeNT((int)ECSharp3Fast.pointer_type,()=>
                  
                     void_pointer()
                  || And(()=>    
                         non_array_type()
                      && Option(()=> rank_specifiers() )
                      && pointers() ) );
		}
        public bool pointers()    /*^^pointers:			'*' S ('*' S)*;*/
        {

           return TreeNT((int)ECSharp3Fast.pointers,()=>
                And(()=>  
                     Char('*')
                  && S()
                  && OptRepeat(()=> And(()=>    Char('*') && S() ) ) ) );
		}
        public bool void_pointer()    /*^^void_pointer:			'void' S '*' S;*/
        {

           return TreeNT((int)ECSharp3Fast.void_pointer,()=>
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

           return TreeAST((int)ECSharp3Fast.is_nullable,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_type,()=>
                type_name() );
		}
        public bool array_type()    /*^array_type: 			non_array_type   rank_specifiers;*/
        {

           return TreeAST((int)ECSharp3Fast.array_type,()=>
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

           return TreeNT((int)ECSharp3Fast.rank_specifier,()=>
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

           return TreeNT((int)ECSharp3Fast.delegate_type,()=>
                type_name() );
		}
        public bool type_argument_list()    /*^^type_argument_list:		'<'  S type_arguments   '>' S;*/
        {

           return TreeNT((int)ECSharp3Fast.type_argument_list,()=>
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

           return TreeNT((int)ECSharp3Fast.type_argument,()=> type() );
		}
        public bool type_parameter()    /*^^type_parameter: 		identifier S;
//A.2.3 Variables
//----------------------*/
        {

           return TreeNT((int)ECSharp3Fast.type_parameter,()=>
                And(()=>    identifier() && S() ) );
		}
        public bool variable_reference()    /*^variable_reference: 		expression;
//A.2.4 Expressions
//----------------------*/
        {

           return TreeAST((int)ECSharp3Fast.variable_reference,()=>
                expression() );
		}
        public bool argument_list()    /*^^argument_list: 		argument (',' S   argument)*;*/
        {

           return TreeNT((int)ECSharp3Fast.argument_list,()=>
                And(()=>  
                     argument()
                  && OptRepeat(()=>    
                      And(()=>    Char(',') && S() && argument() ) ) ) );
		}
        public bool argument()    /*^^argument: 			expression / ^'ref' B S  @variable_reference / ^'out' B S  @variable_reference;*/
        {

           return TreeNT((int)ECSharp3Fast.argument,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.invocation,()=>
                And(()=>  
                     Char('(')
                  && S()
                  && Option(()=> argument_list() )
                  && (    Char(')') || Fatal("<<')'>> expected"))
                  && S() ) );
		}
        public bool member_access()    /*^^member_access:		'.' S  @name S  type_argument_list?;*/
        {

           return TreeNT((int)ECSharp3Fast.member_access,()=>
                And(()=>  
                     Char('.')
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S()
                  && Option(()=> type_argument_list() ) ) );
		}
        public bool pointer_member_access()    /*^^pointer_member_access:	'->' S @name S;*/
        {

           return TreeNT((int)ECSharp3Fast.pointer_member_access,()=>
                And(()=>  
                     Char('-','>')
                  && S()
                  && (    name() || Fatal("<<name>> expected"))
                  && S() ) );
		}
        public bool element_access()    /*^^element_access:		'[' S  @index    S ']' S;*/
        {

           return TreeNT((int)ECSharp3Fast.element_access,()=>
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

           return TreeNT((int)ECSharp3Fast.index,()=>
                expression_list() );
		}
        public bool post_incr()    /*^post_incr:                     '++' S;*/
        {

           return TreeAST((int)ECSharp3Fast.post_incr,()=>
                And(()=>    Char('+','+') && S() ) );
		}
        public bool post_decr()    /*^post_decr:                     '--' S;*/
        {

           return TreeAST((int)ECSharp3Fast.post_decr,()=>
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

           return TreeNT((int)ECSharp3Fast.sizeof_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.simple_name,()=>
                And(()=>  
                     name()
                  && B()
                  && S()
                  && Option(()=> type_argument_list() ) ) );
		}
        public bool parenthesized_expression()    /*^parenthesized_expression: 	'(' S   expression   ')' S;*/
        {

           return TreeAST((int)ECSharp3Fast.parenthesized_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.predefined_type,()=>
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

           return TreeAST((int)ECSharp3Fast.this_access,()=>
                And(()=>    Char('t','h','i','s') && B() && S() ) );
		}
        public bool base_access()    /*^base_access: 			'base'  S ( '.'  S @name S / '[' S  @index   @']' S );*/
        {

           return TreeAST((int)ECSharp3Fast.base_access,()=>
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

           return TreeNT((int)ECSharp3Fast.post_increment_expression,()=>
                And(()=>    postfix_expression() && Char('+','+') && S() ) );
		}
        public bool post_decrement_expression()    /*^^post_decrement_expression: 	postfix_expression   '--' S;*/
        {

           return TreeNT((int)ECSharp3Fast.post_decrement_expression,()=>
                And(()=>    postfix_expression() && Char('-','-') && S() ) );
		}
        public bool object_creation_expression()    /*^^object_creation_expression: 	'new'  S type  
				( '(' S   argument_list?   ')'  S   object_or_collection_initializer? / object_or_collection_initializer );*/
        {

           return TreeNT((int)ECSharp3Fast.object_creation_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.object_initializer,()=>
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

           return TreeAST((int)ECSharp3Fast.member_initializer,()=>
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

           return TreeNT((int)ECSharp3Fast.initializer_value,()=>
                    expression() || object_or_collection_initializer() );
		}
        public bool collection_initializer()    /*^^collection_initializer: 	'{' S  element_initializer_list  ','?  S @'}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.collection_initializer,()=>
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

           return TreeNT((int)ECSharp3Fast.element_initializer_list,()=>
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

           return TreeNT((int)ECSharp3Fast.element_initializer,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.initial_value_list,()=>
                expression_list() );
		}
        public bool array_creation_expression()    /*^^array_creation_expression: 	'new' B S
				(	non_array_type   '[' S  array_size   @']' S   rank_specifiers?   array_initializer?
				/	array_type   array_initializer 
				/    	rank_specifier   array_initializer
				);*/
        {

           return TreeNT((int)ECSharp3Fast.array_creation_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.array_size,()=>
                expression_list() );
		}
        public bool delegate_creation_expression()    /*^^delegate_creation_expression:	'new' S  delegate_type   '(' S  expression  @')' S;*/
        {

           return TreeNT((int)ECSharp3Fast.delegate_creation_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.anonymous_object_creation_expression,()=>
                And(()=>  
                     Char('n','e','w')
                  && S()
                  && anonymous_object_initializer() ) );
		}
        public bool anonymous_object_initializer()    /*^^anonymous_object_initializer:  '{' S   (member_declarator_list ','? S)?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.anonymous_object_initializer,()=>
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

           return TreeNT((int)ECSharp3Fast.member_declarator_list,()=>
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

           return TreeNT((int)ECSharp3Fast.member_declarator,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.full_member_access,()=>
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

           return TreeNT((int)ECSharp3Fast.typeof_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.unbound_type_name,()=>
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

           return TreeNT((int)ECSharp3Fast.generic_dimension_specifier,()=>
                And(()=>  
                     Char('<')
                  && S()
                  && Option(()=> commas() )
                  && (    Char('>') || Fatal("<<'>'>> expected"))
                  && S() ) );
		}
        public bool commas()    /*^commas: 			(',' S)+;*/
        {

           return TreeAST((int)ECSharp3Fast.commas,()=>
                PlusRepeat(()=> And(()=>    Char(',') && S() ) ) );
		}
        public bool checked_expression()    /*^^checked_expression: 		'checked' S    @'(' S  @expression   @')' S;*/
        {

           return TreeNT((int)ECSharp3Fast.checked_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.unchecked_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.default_value_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.unary_expression,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.creation_expression,()=>
                  
                     array_creation_expression()
                  || object_creation_expression()
                  || delegate_creation_expression()
                  || anonymous_object_creation_expression() );
		}
        public bool pre_increment_expression()    /*^^pre_increment_expression: 	'++' S   unary_expression;*/
        {

           return TreeNT((int)ECSharp3Fast.pre_increment_expression,()=>
                And(()=>    Char('+','+') && S() && unary_expression() ) );
		}
        public bool pre_decrement_expression()    /*^^pre_decrement_expression: 	'--' S   unary_expression;*/
        {

           return TreeNT((int)ECSharp3Fast.pre_decrement_expression,()=>
                And(()=>    Char('-','-') && S() && unary_expression() ) );
		}
        public bool cast_expression()    /*^cast_expression: 	        ('(' S   type   ')' S &([~!(]/identifier/literal/!('as' B/'is' B) keyword B)
				/ !parenthesized_expression   '(' S type ')' )
				S   unary_expression;*/
        {

           return TreeAST((int)ECSharp3Fast.cast_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.multiplicative_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.additive_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.shift_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.relational_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.equality_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.and_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.exclusive_or_expression,()=>
                And(()=>  
                     and_expression()
                  && OptRepeat(()=>    
                      And(()=>    Char('^') && S() && and_expression() ) ) ) );
		}
        public bool inclusive_or_expression()    /*^inclusive_or_expression: 	exclusive_or_expression ('|' S exclusive_or_expression)*;*/
        {

           return TreeAST((int)ECSharp3Fast.inclusive_or_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.conditional_and_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.conditional_or_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.null_coalescing_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.conditional_expression,()=>
                And(()=>  
                     null_coalescing_expression()
                  && Option(()=> if_else_expression() ) ) );
		}
        public bool if_else_expression()    /*^if_else_expression:            ('?' S   expression   ':' S   expression);*/
        {

           return TreeAST((int)ECSharp3Fast.if_else_expression,()=>
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

           return TreeAST((int)ECSharp3Fast.lambda_expression,()=>
                And(()=>  
                     anonymous_function_signature()
                  && Char('=','>')
                  && S()
                  && anonymous_function_body() ) );
		}
        public bool anonymous_method_expression()    /*^anonymous_method_expression: 	'delegate' S   explicit_anonymous_function_signature?   block;*/
        {

           return TreeAST((int)ECSharp3Fast.anonymous_method_expression,()=>
                And(()=>  
                     Char("delegate")
                  && S()
                  && Option(()=> explicit_anonymous_function_signature() )
                  && block() ) );
		}
        public bool anonymous_function_signature()    /*^anonymous_function_signature: 	explicit_anonymous_function_signature  / implicit_anonymous_function_signature;*/
        {

           return TreeAST((int)ECSharp3Fast.anonymous_function_signature,()=>
                  
                     explicit_anonymous_function_signature()
                  || implicit_anonymous_function_signature() );
		}
        public bool explicit_anonymous_function_signature()    /*^explicit_anonymous_function_signature:
				'(' S  explicit_anonymous_function_parameter_list?   ')' S;*/
        {

           return TreeAST((int)ECSharp3Fast.explicit_anonymous_function_signature,()=>
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

           return TreeAST((int)ECSharp3Fast.explicit_anonymous_function_parameter_list,()=>
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

           return TreeAST((int)ECSharp3Fast.explicit_anonymous_function_parameter,()=>
                And(()=>  
                     Option(()=> anonymous_function_parameter_modifier() )
                  && type()
                  && parameter_name()
                  && S() ) );
		}
        public bool anonymous_function_parameter_modifier()    /*^anonymous_function_parameter_modifier: ^'ref' B S / ^'out' B S;*/
        {

           return TreeAST((int)ECSharp3Fast.anonymous_function_parameter_modifier,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.implicit_anonymous_function_signature,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.implicit_anonymous_function_parameter_list,()=>
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

           return TreeAST((int)ECSharp3Fast.anonymous_function_body,()=>
                    expression() || block() );
		}
        public bool query_expression()    /*^query_expression: 		from_clause   query_body;*/
        {

           return TreeAST((int)ECSharp3Fast.query_expression,()=>
                And(()=>    from_clause() && query_body() ) );
		}
        public bool from_clause()    /*^from_clause: 			'from' B S   (!(name S 'in' B) type)?   name S ![;=,]  'in' B S  @expression;*/
        {

           return TreeAST((int)ECSharp3Fast.from_clause,()=>
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

           return TreeAST((int)ECSharp3Fast.query_body,()=>
                And(()=>  
                     Option(()=> query_body_clauses() )
                  && select_or_group_clause()
                  && Option(()=> query_continuation() ) ) );
		}
        public bool query_body_clauses()    /*^query_body_clauses: 		query_body_clause+;*/
        {

           return TreeAST((int)ECSharp3Fast.query_body_clauses,()=>
                PlusRepeat(()=> query_body_clause() ) );
		}
        public bool query_body_clause()    /*^query_body_clause: 		from_clause / let_clause / where_clause / join_into_clause  /  orderby_clause;*/
        {

           return TreeAST((int)ECSharp3Fast.query_body_clause,()=>
                  
                     from_clause()
                  || let_clause()
                  || where_clause()
                  || join_into_clause()
                  || orderby_clause() );
		}
        public bool let_clause()    /*^let_clause: 			'let' B S   @name S  '=' S   @expression;*/
        {

           return TreeAST((int)ECSharp3Fast.let_clause,()=>
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

           return TreeAST((int)ECSharp3Fast.where_clause,()=>
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

           return TreeAST((int)ECSharp3Fast.join_into_clause,()=>
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

           return TreeNT((int)ECSharp3Fast.orderby_clause,()=>
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

           return TreeNT((int)ECSharp3Fast.ordering,()=>
                And(()=>  
                     expression()
                  && Option(()=> ordering_direction() ) ) );
		}
        public bool ordering_direction()    /*^ordering_direction: 		('ascending' / 'descending' ) B S;*/
        {

           return TreeAST((int)ECSharp3Fast.ordering_direction,()=>
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

           return TreeNT((int)ECSharp3Fast.select_clause,()=>
                And(()=>  
                     Char('s','e','l','e','c','t')
                  && B()
                  && S()
                  && (    expression() || Fatal("<<expression>> expected")) ) );
		}
        public bool group_clause()    /*^^group_clause:  		'group' B S   @expression   'by' B S   @expression;*/
        {

           return TreeNT((int)ECSharp3Fast.group_clause,()=>
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

           return TreeAST((int)ECSharp3Fast.query_continuation,()=>
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

           return TreeAST((int)ECSharp3Fast.assignment,()=>
                And(()=>  
                     unary_expression()
                  && assignment_operator()
                  && S()
                  && expression() ) );
		}
        public bool assignment_operator()    /*^assignment_operator: 		'=' !'>' / '+=' / '-=' / '*=' / '/=' / '%=' / '&=' / '|=' / '^=' / '<<=' / '>>=';*/
        {

           return TreeAST((int)ECSharp3Fast.assignment_operator,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.expression,()=>
                And(()=>  
                     Not(()=> OneOf(",)};") )
                  && (    assignment() || non_assignment_expression()) ) );
		}
        public bool non_assignment_expression()    /*^non_assignment_expression: 	query_expression / lambda_expression / conditional_expression;*/
        {

           return TreeAST((int)ECSharp3Fast.non_assignment_expression,()=>
                  
                     query_expression()
                  || lambda_expression()
                  || conditional_expression() );
		}
        public bool constant_expression()    /*^^constant_expression: 		expression;*/
        {

           return TreeNT((int)ECSharp3Fast.constant_expression,()=>
                expression() );
		}
        public bool boolean_expression()    /*^^boolean_expression: 		expression;
//A.2.5 Statements
//----------------------*/
        {

           return TreeNT((int)ECSharp3Fast.boolean_expression,()=>
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

           return TreeNT((int)ECSharp3Fast.block,()=>
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

           return TreeNT((int)ECSharp3Fast.empty_statement,()=>
                And(()=>    Char(';') && S() ) );
		}
        public bool labeled_statement()    /*^^labeled_statement: 		label S  ':' !':' S @statement;*/
        {

           return TreeNT((int)ECSharp3Fast.labeled_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.label,()=> identifier() );
		}
        public bool declaration_statement()    /*^declaration_statement: 	local_variable_declaration   ';' S  /  local_constant_declaration   ';' S;*/
        {

           return TreeAST((int)ECSharp3Fast.declaration_statement,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.local_variable_declaration,()=>
                And(()=>  
                     local_variable_type()
                  && local_variable_declarators() ) );
		}
        public bool local_variable_type()    /*^local_variable_type: 		^'var' B S / type;*/
        {

           return TreeAST((int)ECSharp3Fast.local_variable_type,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.local_variable_declarator,()=>
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

           return TreeNT((int)ECSharp3Fast.variable_name,()=>
                identifier() );
		}
        public bool local_variable_initializer()    /*^^local_variable_initializer: 	expression / array_initializer / stackalloc_initializer;*/
        {

           return TreeNT((int)ECSharp3Fast.local_variable_initializer,()=>
                  
                     expression()
                  || array_initializer()
                  || stackalloc_initializer() );
		}
        public bool stackalloc_initializer()    /*^^stackalloc_initializer:	'stackalloc' B S   type   '[' S   expression   ']' S;*/
        {

           return TreeNT((int)ECSharp3Fast.stackalloc_initializer,()=>
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

           return TreeNT((int)ECSharp3Fast.local_constant_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.constant_declarator,()=>
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

           return TreeNT((int)ECSharp3Fast.constant_name,()=>
                identifier() );
		}
        public bool expression_statement()    /*^expression_statement: 		statement_expression   ';' S;*/
        {

           return TreeAST((int)ECSharp3Fast.expression_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.call_or_post_incr_decr,()=>
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

           return TreeAST((int)ECSharp3Fast.if_statement,()=>
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

           return TreeAST((int)ECSharp3Fast.switch_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.switch_block,()=>
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

           return TreeAST((int)ECSharp3Fast.switch_section,()=>
                And(()=>    switch_labels() && statement_list() ) );
		}
        public bool switch_labels()    /*switch_labels: 			switch_label+;*/
        {

           return PlusRepeat(()=> switch_label() );
		}
        public bool switch_label()    /*^^switch_label: 		'case' B S   @constant_expression   @':' S / 'default' S   @':' S ;*/
        {

           return TreeNT((int)ECSharp3Fast.switch_label,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.while_statement,()=>
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

           return TreeAST((int)ECSharp3Fast.do_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.for_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.for_initializer,()=>
                  
                     local_variable_declaration()
                  || statement_expression_list() );
		}
        public bool for_condition()    /*^^for_condition: 		boolean_expression;*/
        {

           return TreeNT((int)ECSharp3Fast.for_condition,()=>
                boolean_expression() );
		}
        public bool for_iterator()    /*^^for_iterator: 		statement_expression_list;*/
        {

           return TreeNT((int)ECSharp3Fast.for_iterator,()=>
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

           return TreeAST((int)ECSharp3Fast.foreach_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.break_statement,()=>
                And(()=>  
                     Char('b','r','e','a','k')
                  && S()
                  && Char(';')
                  && S() ) );
		}
        public bool continue_statement()    /*^^continue_statement: 		'continue' S   ';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.continue_statement,()=>
                And(()=>    Char("continue") && S() && Char(';') && S() ) );
		}
        public bool goto_statement()    /*^^goto_statement: 		'goto'  B S   @(label S    / 'case' B S   @constant_expression    / 'default' S )  @';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.goto_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.return_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.throw_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.try_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.catch_clauses,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.specific_catch_clause,()=>
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

           return TreeNT((int)ECSharp3Fast.general_catch_clause,()=>
                And(()=>  
                     Char('c','a','t','c','h')
                  && B()
                  && S()
                  && block() ) );
		}
        public bool finally_clause()    /*^^finally_clause: 		'finally' B S   @block;*/
        {

           return TreeNT((int)ECSharp3Fast.finally_clause,()=>
                And(()=>  
                     Char('f','i','n','a','l','l','y')
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool checked_statement()    /*^^checked_statement: 		'checked' B S   @block;*/
        {

           return TreeNT((int)ECSharp3Fast.checked_statement,()=>
                And(()=>  
                     Char('c','h','e','c','k','e','d')
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool unchecked_statement()    /*^^unchecked_statement: 		'unchecked' B S   @block;*/
        {

           return TreeNT((int)ECSharp3Fast.unchecked_statement,()=>
                And(()=>  
                     Char("unchecked")
                  && B()
                  && S()
                  && (    block() || Fatal("<<block>> expected")) ) );
		}
        public bool lock_statement()    /*^^lock_statement: 		'lock' B S   @'(' S  @expression   @')' S   @embedded_statement;*/
        {

           return TreeNT((int)ECSharp3Fast.lock_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.using_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.resource_acquisition,()=>
                    local_variable_declaration() || expression() );
		}
        public bool yield_statement()    /*^^yield_statement: 		'yield' B S   ('break' S /'return' B S   @expression)   @';' S ;
//A.2.6 Namespaces
//----------------------*/
        {

           return TreeNT((int)ECSharp3Fast.yield_statement,()=>
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

           return TreeNT((int)ECSharp3Fast.compilation_unit,()=>
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

           return TreeNT((int)ECSharp3Fast.namespace_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.qualified_identifier,()=>
                And(()=>  
                     name()
                  && S()
                  && OptRepeat(()=>    
                      And(()=>    Char('.') && S() && name() && S() ) ) ) );
		}
        public bool namespace_body()    /*^^namespace_body:		'{' S  extern_alias_directives?   using_directives?   namespace_member_declarations?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.namespace_body,()=>
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

           return TreeNT((int)ECSharp3Fast.extern_alias_directives,()=>
                PlusRepeat(()=> extern_alias_directive() ) );
		}
        public bool extern_alias_directive()    /*^^extern_alias_directive: 	'extern' B S   'alias' B S   alias_name S  ';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.extern_alias_directive,()=>
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

           return TreeNT((int)ECSharp3Fast.alias_name,()=>
                identifier() );
		}
        public bool using_directives()    /*^^using_directives: 		using_directive+;*/
        {

           return TreeNT((int)ECSharp3Fast.using_directives,()=>
                PlusRepeat(()=> using_directive() ) );
		}
        public bool using_directive()    /*using_directive: 		using_alias_directive / using_namespace_directive;*/
        {

           return     using_alias_directive() || using_namespace_directive();
		}
        public bool using_alias_directive()    /*^^using_alias_directive: 	'using' B S   using_alias_name S  '=' S   @namespace_or_type_name   @';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.using_alias_directive,()=>
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

           return TreeNT((int)ECSharp3Fast.using_alias_name,()=>
                identifier() );
		}
        public bool using_namespace_directive()    /*^^using_namespace_directive: 	'using' B S   namespace_name   ';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.using_namespace_directive,()=>
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

           return TreeNT((int)ECSharp3Fast.namespace_member_declarations,()=>
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

           return TreeAST((int)ECSharp3Fast.qualified_alias_member,()=>
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

           return TreeNT((int)ECSharp3Fast.class_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.class_name,()=>
                identifier() );
		}
        public bool class_modifiers()    /*class_modifiers: 		class_modifier+;*/
        {

           return PlusRepeat(()=> class_modifier() );
		}
        public bool class_modifier()    /*^^class_modifier:		('new' / 'public' / 'protected' / 'internal' / 'private' / 'abstract' / 'sealed' / 'static' / 'unsafe') B S;*/
        {

           return TreeNT((int)ECSharp3Fast.class_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals4)
                  && B()
                  && S() ) );
		}
        public bool type_parameter_list()    /*^^type_parameter_list: 		'<' S   type_parameters   '>' S;*/
        {

           return TreeNT((int)ECSharp3Fast.type_parameter_list,()=>
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

           return TreeNT((int)ECSharp3Fast.type_parameters,()=>
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

           return TreeNT((int)ECSharp3Fast.class_base,()=>
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

           return TreeNT((int)ECSharp3Fast.type_parameter_constraints_clause,()=>
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

           return TreeNT((int)ECSharp3Fast.type_parameter_constraints,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.primary_constraint,()=>
                  
                     class_type()
                  || And(()=>    Char('c','l','a','s','s') && B() && S() )
                  || And(()=>    
                         Char('s','t','r','u','c','t')
                      && B()
                      && S() ) );
		}
        public bool secondary_constraints()    /*^^secondary_constraints: 	(interface_type / type_parameter) (',' S   (interface_type/type_parameter))*;*/
        {

           return TreeNT((int)ECSharp3Fast.secondary_constraints,()=>
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

           return TreeNT((int)ECSharp3Fast.constructor_constraint,()=>
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

           return TreeNT((int)ECSharp3Fast.class_body,()=>
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

           return TreeNT((int)ECSharp3Fast.constant_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.constant_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.field_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.field_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.variable_declarator,()=>
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

           return TreeNT((int)ECSharp3Fast.variable_initializer,()=>
                    expression() || array_initializer() );
		}
        public bool method_declaration()    /*^method_declaration: 		method_header   method_body;*/
        {

           return TreeAST((int)ECSharp3Fast.method_declaration,()=>
                And(()=>    method_header() && method_body() ) );
		}
        public bool method_header()    /*^method_header: 			attributes?   method_modifiers?   (^'partial' B S)?   return_type   member_name S  type_parameter_list?
				'(' S   formal_parameter_list?   ')' S   type_parameter_constraints_clauses?;*/
        {

           return TreeAST((int)ECSharp3Fast.method_header,()=>
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

           return TreeAST((int)ECSharp3Fast.method_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals6)
                  && B()
                  && S() ) );
		}
        public bool return_type()    /*^^return_type: 			type / 'void' B S;*/
        {

           return TreeNT((int)ECSharp3Fast.return_type,()=>
                  
                     type()
                  || And(()=>    Char('v','o','i','d') && B() && S() ) );
		}
        public bool interface_name_before_member()    /*^interface_name_before_member: (name S type_argument_list? / qualified_alias_member)  
			       ( ^'.' S   @name S !(type_parameter_list? [({]) type_argument_list?)*;*/
        {

           return TreeAST((int)ECSharp3Fast.interface_name_before_member,()=>
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

           return TreeNT((int)ECSharp3Fast.method_body,()=>
                    missing_body() || block() );
		}
        public bool missing_body()    /*^^missing_body:			';' S;*/
        {

           return TreeNT((int)ECSharp3Fast.missing_body,()=>
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

           return TreeAST((int)ECSharp3Fast.fixed_parameter,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> parameter_modifier() )
                  && type()
                  && parameter_name()
                  && S() ) );
		}
        public bool parameter_modifier()    /*^parameter_modifier: 		^('ref' / 'out' / 'this' ) B S;*/
        {

           return TreeAST((int)ECSharp3Fast.parameter_modifier,()=>
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

           return TreeAST((int)ECSharp3Fast.parameter_array,()=>
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

           return TreeAST((int)ECSharp3Fast.property_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.property_modifier,()=>
                And(()=>    OneOfLiterals(optimizedLiterals7) && S() ) );
		}
        public bool member_name()    /*^^member_name: 			interface_name_before_member '.' S  name S / name S ;*/
        {

           return TreeNT((int)ECSharp3Fast.member_name,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.get_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> accessor_modifier() )
                  && Char('g','e','t')
                  && S()
                  && accessor_body() ) );
		}
        public bool set_accessor_declaration()    /*^^set_accessor_declaration: 	attributes?   accessor_modifier?    'set' S   accessor_body;*/
        {

           return TreeNT((int)ECSharp3Fast.set_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> accessor_modifier() )
                  && Char('s','e','t')
                  && S()
                  && accessor_body() ) );
		}
        public bool accessor_modifier()    /*^^accessor_modifier: 		('protected' / 'internal' / 'private' / 'protected' B S  'internal' / 'internal' B S   'protected' )B S ;*/
        {

           return TreeNT((int)ECSharp3Fast.accessor_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.accessor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool event_declaration()    /*^event_declaration: 		attributes?   event_modifiers?   'event' B S   type   
				(	variable_declarators  ';' S
				/ 	member_name  S '{' S   event_accessor_declarations   @'}' S
				);*/
        {

           return TreeAST((int)ECSharp3Fast.event_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.event_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals8)
                  && B()
                  && S() ) );
		}
        public bool event_accessor_declarations()    /*^event_accessor_declarations: 	add_accessor_declaration   remove_accessor_declaration / remove_accessor_declaration   add_accessor_declaration;*/
        {

           return TreeAST((int)ECSharp3Fast.event_accessor_declarations,()=>
                  
                     And(()=>    
                         add_accessor_declaration()
                      && remove_accessor_declaration() )
                  || And(()=>    
                         remove_accessor_declaration()
                      && add_accessor_declaration() ) );
		}
        public bool add_accessor_declaration()    /*^^add_accessor_declaration: 	attributes?   'add' S   block;*/
        {

           return TreeNT((int)ECSharp3Fast.add_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Char('a','d','d')
                  && S()
                  && block() ) );
		}
        public bool remove_accessor_declaration()    /*^^remove_accessor_declaration: 	attributes?   'remove' S   block;*/
        {

           return TreeNT((int)ECSharp3Fast.remove_accessor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Char('r','e','m','o','v','e')
                  && S()
                  && block() ) );
		}
        public bool indexer_declaration()    /*^indexer_declaration: 		attributes?   indexer_modifiers?   indexer_declarator   '{' S   accessor_declarations   @'}' S ;*/
        {

           return TreeAST((int)ECSharp3Fast.indexer_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.indexer_modifier,()=>
                And(()=>  
                     OneOfLiterals(optimizedLiterals9)
                  && B()
                  && S() ) );
		}
        public bool indexer_declarator()    /*^indexer_declarator: 		type  (interface_type   '.' S)? 'this' S    '[' S  formal_parameter_list   @']' S;*/
        {

           return TreeAST((int)ECSharp3Fast.indexer_declarator,()=>
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

           return TreeAST((int)ECSharp3Fast.operator_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.operator_modifier,()=>
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

           return TreeAST((int)ECSharp3Fast.unary_operator_declarator,()=>
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

           return TreeAST((int)ECSharp3Fast.overloadable_unary_operator,()=>
                And(()=>    OneOfLiterals(optimizedLiterals10) && S() ) );
		}
        public bool binary_operator_declarator()    /*^binary_operator_declarator: 	type   'operator' S   overloadable_binary_operator   '(' S  type   parameter_name   ',' S   type   parameter_name  S ')' S;*/
        {

           return TreeAST((int)ECSharp3Fast.binary_operator_declarator,()=>
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

           return TreeAST((int)ECSharp3Fast.overloadable_binary_operator,()=>
                And(()=>    OneOfLiterals(optimizedLiterals11) && S() ) );
		}
        public bool conversion_operator_declarator()    /*^conversion_operator_declarator: ('implicit' / 'explicit' ) B S   'operator' B S   type   '(' S   type   parameter_name S   ')' S ;*/
        {

           return TreeAST((int)ECSharp3Fast.conversion_operator_declarator,()=>
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

           return TreeNT((int)ECSharp3Fast.operator_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool constructor_declaration()    /*^constructor_declaration: 	attributes?   constructor_modifiers?   constructor_declarator   constructor_body;*/
        {

           return TreeAST((int)ECSharp3Fast.constructor_declaration,()=>
                And(()=>  
                     Option(()=> attributes() )
                  && Option(()=> constructor_modifiers() )
                  && constructor_declarator()
                  && constructor_body() ) );
		}
        public bool constructor_modifiers()    /*^constructor_modifiers: 	constructor_modifier+;*/
        {

           return TreeAST((int)ECSharp3Fast.constructor_modifiers,()=>
                PlusRepeat(()=> constructor_modifier() ) );
		}
        public bool constructor_modifier()    /*^constructor_modifier: 		('public' / 'protected' / 'internal' / 'private' / 'extern' / 'unsafe') B S;*/
        {

           return TreeAST((int)ECSharp3Fast.constructor_modifier,()=>
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

           return TreeAST((int)ECSharp3Fast.constructor_declarator,()=>
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

           return TreeAST((int)ECSharp3Fast.constructor_initializer,()=>
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

           return TreeAST((int)ECSharp3Fast.constructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool static_constructor_declaration()    /*^static_constructor_declaration: attributes?   static_constructor_modifiers  name S  '(' S  ')' S  static_constructor_body;*/
        {

           return TreeAST((int)ECSharp3Fast.static_constructor_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.static_constructor_modifiers,()=>
                  
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

           return TreeAST((int)ECSharp3Fast.static_constructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool destructor_declaration()    /*^destructor_declaration: 	attributes?   ('extern' B S 'unsafe' S / 'unsafe' B S 'extern' S)?   '~' S   name  S '(' S  ')' S    destructor_body;*/
        {

           return TreeAST((int)ECSharp3Fast.destructor_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.destructor_body,()=>
                    block() || And(()=>    Char(';') && S() ) );
		}
        public bool struct_declaration()    /*^struct_declaration: 		attributes?   struct_modifiers?   (^'partial' B S)?   'struct' B S   struct_name S   type_parameter_list?
					struct_interfaces?   type_parameter_constraints_clauses?   @struct_body   ';'?;*/
        {

           return TreeAST((int)ECSharp3Fast.struct_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.struct_name,()=>
                identifier() );
		}
        public bool struct_modifiers()    /*struct_modifiers: 		struct_modifier+;*/
        {

           return PlusRepeat(()=> struct_modifier() );
		}
        public bool struct_modifier()    /*^struct_modifier: 		('new' / 'public' / 'protected' / 'internal' / 'private' / 'unsafe')B S;*/
        {

           return TreeAST((int)ECSharp3Fast.struct_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.struct_interfaces,()=>
                And(()=>    Char(':') && S() && interface_type_list() ) );
		}
        public bool struct_body()    /*^^struct_body:			'{' S   struct_member_declarations?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.struct_body,()=>
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

           return TreeAST((int)ECSharp3Fast.struct_member_declaration,()=>
                  
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

           return TreeNT((int)ECSharp3Fast.fixed_size_buffer_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.fixed_size_buffer_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.buffer_element_type,()=>
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

           return TreeNT((int)ECSharp3Fast.array_initializer,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.interface_base,()=>
                And(()=>    Char(':') && S() && interface_type_list() ) );
		}
        public bool interface_body()    /*^^interface_body:		'{' S   interface_member_declarations?   '}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.interface_body,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_member_declaration,()=>
                  
                     interface_method_declaration()
                  || interface_property_declaration()
                  || interface_event_declaration()
                  || interface_indexer_declaration() );
		}
        public bool interface_method_declaration()    /*^interface_method_declaration: 	attributes?   ('new' B S)?   return_type   name S  type_parameter_list?
				'('  S formal_parameter_list?   ')' S   type_parameter_constraints_clauses?  ';' S ;*/
        {

           return TreeAST((int)ECSharp3Fast.interface_method_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_property_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_accessors,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_event_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.interface_indexer_declaration,()=>
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

           return TreeAST((int)ECSharp3Fast.enum_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.enum_name,()=>
                identifier() );
		}
        public bool enum_base()    /*^^enum_base: 			':' S integral_type;*/
        {

           return TreeNT((int)ECSharp3Fast.enum_base,()=>
                And(()=>    Char(':') && S() && integral_type() ) );
		}
        public bool enum_body()    /*^^enum_body:			'{'  S (enum_member_declarations (',' S)?)?   @'}' S;*/
        {

           return TreeNT((int)ECSharp3Fast.enum_body,()=>
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

           return TreeAST((int)ECSharp3Fast.enum_modifier,()=>
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

           return TreeAST((int)ECSharp3Fast.enum_member_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.enumerator_name,()=>
                identifier() );
		}
        public bool delegate_declaration()    /*^delegate_declaration: 		attributes?   delegate_modifiers?   'delegate' B S   return_type   delegate_name  S type_parameter_list?   
				'(' S  formal_parameter_list?   ')' S   type_parameter_constraints_clauses?   ';' S;*/
        {

           return TreeAST((int)ECSharp3Fast.delegate_declaration,()=>
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

           return TreeNT((int)ECSharp3Fast.delegate_name,()=>
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

           return TreeAST((int)ECSharp3Fast.delegate_modifier,()=>
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

           return TreeNT((int)ECSharp3Fast.global_attributes,()=>
                global_attribute_sections() );
		}
        public bool global_attribute_sections()    /*global_attribute_sections: 	global_attribute_section+;*/
        {

           return PlusRepeat(()=> global_attribute_section() );
		}
        public bool global_attribute_section()    /*^global_attribute_section:	'[' S   global_attribute_target_specifier   attribute_list  (',' S)?  ']' S;*/
        {

           return TreeAST((int)ECSharp3Fast.global_attribute_section,()=>
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

           return TreeNT((int)ECSharp3Fast.global_attribute_target_specifier,()=>
                And(()=>  
                     global_attribute_target()
                  && Char(':')
                  && S() ) );
		}
        public bool global_attribute_target()    /*^^global_attribute_target: 	('assembly' / 'module') B S;*/
        {

           return TreeNT((int)ECSharp3Fast.global_attribute_target,()=>
                And(()=>  
                     (    Char("assembly") || Char('m','o','d','u','l','e'))
                  && B()
                  && S() ) );
		}
        public bool attributes()    /*^^attributes: 			attribute_sections;*/
        {

           return TreeNT((int)ECSharp3Fast.attributes,()=>
                attribute_sections() );
		}
        public bool attribute_sections()    /*attribute_sections: 		attribute_section+;*/
        {

           return PlusRepeat(()=> attribute_section() );
		}
        public bool attribute_section()    /*^^attribute_section:		'[' S   attribute_target_specifier?   attribute_list (',' S)?  ']' S;*/
        {

           return TreeNT((int)ECSharp3Fast.attribute_section,()=>
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

           return TreeNT((int)ECSharp3Fast.attribute_target_specifier,()=>
                And(()=>    attribute_target() && Char(':') && S() ) );
		}
        public bool attribute_target()    /*^attribute_target: 		('field' / 'event' / 'method' / 'param' / 'property' / 'return' / 'type') S;*/
        {

           return TreeAST((int)ECSharp3Fast.attribute_target,()=>
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

           return TreeNT((int)ECSharp3Fast.attribute,()=>
                And(()=>  
                     attribute_name()
                  && Option(()=> attribute_arguments() ) ) );
		}
        public bool attribute_name()    /*^^attribute_name:  		type_name;*/
        {

           return TreeNT((int)ECSharp3Fast.attribute_name,()=>
                type_name() );
		}
        public bool attribute_arguments()    /*^^attribute_arguments:		'('  S
				(	named_argument_list 
				/	(positional_argument_list   (',' S   named_argument_list)?)?   
				)
				')' S;*/
        {

           return TreeNT((int)ECSharp3Fast.attribute_arguments,()=>
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

           return TreeNT((int)ECSharp3Fast.positional_argument,()=>
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

           return TreeNT((int)ECSharp3Fast.named_argument,()=>
                And(()=>  
                     parameter_name()
                  && S()
                  && Char('=')
                  && S()
                  && attribute_argument_expression() ) );
		}
        public bool parameter_name()    /*^^parameter_name:               identifier;*/
        {

           return TreeNT((int)ECSharp3Fast.parameter_name,()=>
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
		#endregion Grammar Rules

        #region Optimization Data 
        internal static OptimizedCharset optimizedCharset0;
        internal static OptimizedCharset optimizedCharset1;
        
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
        
        static CSharp3Fast()
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