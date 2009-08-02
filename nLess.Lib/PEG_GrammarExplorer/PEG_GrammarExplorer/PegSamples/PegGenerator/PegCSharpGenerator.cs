/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 *  20080929;SEMBLOCK_INDENTATION;improved indentation of local classes assocated to semantic blocks
 *  20081001;//NOTIN_MISSING_PAREN;corrected code template for NotIn(...) where an opening paren was missing
 *  20081002;USING_BLOCK;added support for using in rules like'[9]   parenth_form_content  using Line_join_sem_: ...'
 * <</History>>
*/
using System;
using Peg.Base;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Peg.Samples;

namespace Peg.CSharp
{
    enum ECSharpKind{
			Project,
			MainImpl,
			ModuleHead,
			ModuleTail,
			ParserHeader,
            StaticConstructor,
			ParserImpl,
			GrammarHeader,
			InterfaceFunc,
			ErrHandler,
			ErrTable,
			And,
			Or,
			Option,
			RuleRef,
            RuleRefWithArgs,
            Literals,
            OptimizedCharset,
			String,
			StringCaseInsensitive,
			Any,
			Peek,
			Not,
			In,
            NotIn,
			OneOf,
            NotOneOf,
			TreeAnd,
			TreePeek,
			TreeNot,
			Rule,
			RuleTree,
			RuleAst,
            RuleCreaTree,
            RuleCreaAst,
			TreeChars,
			OptRepeat,
			PlusRepeat,
			ForLoop,
        //{bit operations
            Bits,
            PeekBits,
            NotBits,
            MatchingBitsInto,
            BitsInto,
            Bit,
            PeekBit,
            BitNot,
            Into,
			Fatal,
			Warning}
    enum ETemplateKind{	
        TemplNone,
		TemplNot,
		TemplPeek,
		TemplAnd,
		TemplTreeNot,
		TemplTreePeek,
		TemplTreeSafeAnd,
		TemplOr,
        TemplOptimizedCharset,
        TemplNegatedOptimizedCharset,
		TemplCharset,
		TemplNegatedCharset,
		TemplRepetition,
		TemplTreeNT,
		TemplAstNT,
		TemplTreeChars,
		TemplRule,
		TemplTreeRule,
		TemplAstRule,
        TemplTreeCreateRule,
        TemplAstCreateRule,
		TemplRuleRef,
		TemplString,
        TemplIntoVariable,
		TemplStringCaseInsensitive,
        TemplLiterals,
		TemplDots,
        TemplBitAccess,
		TemplFatal,
		TemplWarning,
        TemplIntoVar,
        TemplSemFuncCall}
    struct CodeTemplate{
        internal CodeTemplate(ECSharpKind eKind, string sCodeTemplate)
        {
            this.eKind = eKind;
            this.sCodeTemplate = sCodeTemplate;
        }
	    internal ECSharpKind	eKind;
        internal string sCodeTemplate;
    }
    public class PegCSharpGenerator
    {
        #region Data Members
        TextWriter  outFile_;
        TreeContext context_;
        string      moduleName_;
        string      outputFileName_;
        internal int literalsCount_;
        internal int optimizedCharsetCount_;
        internal StringBuilder optimizationStaticConstructor_;
       static string mainImplCSharp=	
	@"using System;
	namespace ExprRecognizeProgram
	{
		using System.IO;
		using PegGrammar;
		using ExprRecognize;
		class Program
		{
			static bool ExprRecognizeParse(string srcFile, StreamWriter FerrOut)
			{
				try
				{
					string src;
					using (StreamReader r = new StreamReader(srcFile))
					{
						src = r.ReadToEnd();
					}
					try
					{
						ExprRecognize expr = new ExprRecognize(src, FerrOut);
						bool bMatches= expr.Expr();
						return bMatches;
					}
					catch (PegException)
					{
						return false;
					}
				}
				catch (Exception)
				{
					FerrOut.WriteLine(\input file could not be opened '{0}'\, srcFile);
					FerrOut.Flush();
					return false;
				}
			}
			static void Main(string[] args)
			{
				if( args.Length<1){
					Console.WriteLine(\usage: ExprRecognize sourcefile [errorfile]\);
					return;
				}
				StreamWriter FerrOut;
				if( args.Length>=2){
					FerrOut= new StreamWriter(args[1]);
					if( FerrOut==null){
						Console.WriteLine(\ could not open error file {0}\ ,args[1]);
						return;
					}
				}else{
					FerrOut= new StreamWriter(Console.OpenStandardError());
				}
				bool bMatches = ExprRecognizeParse(args[0], FerrOut);
				if (bMatches) Console.WriteLine(\file '{0}' is matched by ExprRecognize\,args[0]);
				else Console.WriteLine(\file '{0}' is not matched by ExprRecognize\,args[0]);
			}
		}
	}";
        static string moduleHeadCSharp=
   @"
using Peg.Base;
using System;
using System.IO;
using System.Text;
namespace $(MODULE_NAME)
{
      
      enum E$(MODULE_NAME){$(ENUMERATOR)};
      class $(MODULE_NAME) : $(PARSER) 
      {
        $(SEMANTIC_BLOCKS)
         #region Input Properties
        public static EncodingClass encodingClass = EncodingClass.$(ENCODING_CLASS);
        public static UnicodeDetection unicodeDetection = UnicodeDetection.$(UNICODE_DETECTION);
        #endregion Input Properties
        #region Constructors
        public $(MODULE_NAME)()
            : base()
        {
            $(INITIALIZATION)
        }
        public $(MODULE_NAME)($(SRC_TYPE) src,TextWriter FerrOut)
			: base(src,FerrOut)
        {
            $(INITIALIZATION)
        }
        #endregion Constructors
        #region Overrides
        public override string GetRuleNameFromId(int id)
        {
            try
            {
                   E$(MODULE_NAME) ruleEnum = (E$(MODULE_NAME))id;
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
";
    static string moduleTailCSharp=
@"   }
}";
    static string staticConstructor=
        @"
        #region Optimization Data 
        $(OPTIMIZEDCHARSET_DECL)
        $(OPTIMIZEDLITERALS_DECL)
        static $(MODULE_NAME)()
        {
            $(OPTIMIZEDCHARSET_IMPL)
            $(OPTIMIZEDLITERALS_IMPL)
        }
        #endregion Optimization Data 
        ";
    static string argNReplace = "\n,()=>$(ARGN)$(ARGN1)";
    CodeTemplate[] templates = {
            new CodeTemplate(ECSharpKind.MainImpl,	    mainImplCSharp),
            new CodeTemplate(ECSharpKind.ModuleHead,   moduleHeadCSharp),
	        new CodeTemplate(ECSharpKind.ModuleTail,   moduleTailCSharp),
            new CodeTemplate(ECSharpKind.StaticConstructor,staticConstructor),
	        new CodeTemplate(ECSharpKind.And,           "And(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.Or,		    "\n$(CONDITION)"),
	        new CodeTemplate(ECSharpKind.Option,	    "Option(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.Peek,			"Peek(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.Not,			"Not(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.In,			"In($(PAIRS))"),
            new CodeTemplate(ECSharpKind.NotIn,         "NotIn($(PAIRS))"),//NOTIN_MISSING_PAREN
	        new CodeTemplate(ECSharpKind.OneOf,	        "OneOf($(CHARS))"),
            new CodeTemplate(ECSharpKind.NotOneOf,      "NotOneOf($(CHARS))"),
	        new CodeTemplate(ECSharpKind.TreeAnd,	    "And(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.TreePeek,	    "Peek(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.TreeNot,	    "Not(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.RuleRef,       "$(NAME)()"),
            new CodeTemplate(ECSharpKind.RuleRefWithArgs,"$(NAME)(()=>\n$(ARG0)$(ARGN) )"),
	        new CodeTemplate(ECSharpKind.String,	    "Char($(CHARS))"),
	        new CodeTemplate(ECSharpKind.StringCaseInsensitive,	
								                        "IChar($(CHARS))"),
            new CodeTemplate(ECSharpKind.Literals,      "OneOfLiterals($(LITERALS))"),
            new CodeTemplate(ECSharpKind.OptimizedCharset,"OneOf($(CHARSET))"),
	        new CodeTemplate(ECSharpKind.Any,		    "Any()"),
	        new CodeTemplate(ECSharpKind.RuleTree,	    "return TreeNT((int)E$(MODULE_NAME).$(ENUMERATOR),()=>\n$(CONDITION) );"),
	        new CodeTemplate(ECSharpKind.RuleAst,	    "return TreeAST((int)E$(MODULE_NAME).$(ENUMERATOR),()=>\n$(CONDITION) );"),
            new CodeTemplate(ECSharpKind.RuleCreaTree,	"return TreeNT($(CREATOR),(int)E$(MODULE_NAME).$(ENUMERATOR),()=>\n$(CONDITION) );"),
	        new CodeTemplate(ECSharpKind.RuleCreaAst,	"return TreeAST($(CREATOR),int)E$(MODULE_NAME).$(ENUMERATOR),()=>\n$(CONDITION) );"),
	        new CodeTemplate(ECSharpKind.TreeChars,     "TreeChars(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.OptRepeat,     "OptRepeat(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.PlusRepeat,    "PlusRepeat(()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.ForLoop,       "ForRepeat($(LOWER),$(UPPER),()=>\n$(CONDITION) )"),
	        new CodeTemplate(ECSharpKind.Rule,		    "return $(BODY);"),
            new CodeTemplate(ECSharpKind.Bits,          "Bits($(LOWER),$(UPPER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.PeekBits,      "PeekBits($(LOWER),$(UPPER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.NotBits,       "NotBits($(LOWER),$(UPPER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.MatchingBitsInto,      
                                                        "BitsInto($(LOWER),$(UPPER),$(BYTE),out $(INTO))"),
            new CodeTemplate(ECSharpKind.BitsInto,      "BitsInto($(LOWER),$(UPPER),out $(INTO))"),
            new CodeTemplate(ECSharpKind.Bit,           "Bit($(LOWER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.PeekBit,       "PeekBit($(LOWER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.BitNot,        "BitNot($(LOWER),$(BYTE))"),
            new CodeTemplate(ECSharpKind.Into,          "Into(()=>\n$(CONDITION),out $(INTO))"),
	        new CodeTemplate(ECSharpKind.Fatal,		    "Fatal(\"$(ERROR)\")"),
	        new CodeTemplate(ECSharpKind.Warning,		"Warning(\"$(ERROR)\")"),
        };
        #endregion Data Members
        internal class CharsetInfo{//used to break up character ranges and character sets into chunks

		   internal struct Range{internal string lower; internal string upper;}
           internal List<List<Range>> range_;
           internal List<List<string>> chars_;
        }
        #region Template Classes (Code generation support)
        internal class Template{
            internal ETemplateKind kind_;
            internal List<Template> subNodes_;
            internal string templateCode;
            internal Dictionary<string, string> replacements;       //e.g. $(CONDITION) => $0 && $1 && $2 

            internal Template(ETemplateKind kind)
            {
                kind_ = kind;
                subNodes_ = new List<Template>();
                replacements = new Dictionary<string, string>();
            }
		    Template():this(ETemplateKind.TemplNone)
		    { 
            }

        }
        internal class TemplateInt : Template
        {
            internal TemplateInt(ETemplateKind kind, int value)
                : base(kind)
            {
                value_ = value;
            }
            internal int value_;
        }
        internal class TemplateString : Template
        {
            internal TemplateString(ETemplateKind kind, string name)
                : base(kind)
            {
                name_ = name;
            }
            internal string name_;
        }
        internal class TemplateStrings : Template
        {
            internal TemplateStrings(ETemplateKind kind, List<string> strings)
                :base(kind)
            {
                strings_ = strings;
            }
            internal List<string> strings_;
        }
        internal class TemplateCharsetInfo : Template
        {
            internal TemplateCharsetInfo(ETemplateKind kind, CharsetInfo charsetInfo)
                : base(kind)
            {
                charsetInfo_ = charsetInfo;
            }
            internal CharsetInfo charsetInfo_;
        }
        internal class TemplateRepetition : Template
        {
            internal TemplateRepetition(ETemplateKind kind, PegGrammarParser.TRange repetition)
                : base(kind)
            {
                repetition_ = repetition;
            }
            internal PegGrammarParser.TRange repetition_;
        }
        internal class TemplateContainer<T> : Template
        {
            internal TemplateContainer(ETemplateKind kind,T t)
                : base(kind)
            {
                t_ = t;
            }
            internal T t_;
        }
        #endregion Template Classes (Code generation support)
        #region Code Generator Classes
        internal class TemplateGenerator
        {
            #region data members
            TreeContext context_;
            #endregion data members
            internal TemplateGenerator(TreeContext context)
            {
                context_ = context;
            }
            #region C# char encodings
            string GetEscapeValue(PegNode n)
            {
                switch (n.id_)
                {
                    case (int)EPegGrammar.escape_char:
                        Debug.Assert(n.match_.Length == 1);
                        return "\\" + n.GetAsString(context_.src_);
                    case (int)EPegGrammar.escape_int: return "\\" + n.GetAsString(context_.src_);
                    default: Debug.Assert(false); return "";
                }
            }
            char GetEscapeUnicodeValue(PegNode n)
            {
                switch (n.id_)
                {
                    case (int)EPegGrammar.escape_char:
                        Debug.Assert(n.match_.Length == 1);
                        char c= n.GetAsString(context_.src_)[0];
                        switch (c)
                        {
                            case 'n': return '\n';
                            case 'r': return '\r';
                            case 'v': return '\v';
                            default: return c;
                        }
                    case (int)EPegGrammar.escape_int:
                        {
                            string octalNumber = n.GetAsString(context_.src_);
                            return (char)Convert.ToInt32(octalNumber,8);
                            
                        }
                    default: Debug.Assert(false); return ' ';
                }
            }
            string GetCodePointValue(PegNode n)
            {
                int numericValue = 0;
                string s = n.GetAsString(context_.src_);
                switch (n.id_)
                {
                    case (int)EPegGrammar.hexadecimal_digits:
                        Int32.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out numericValue);
                        break;
                    case (int)EPegGrammar.binary_digits:
                        for (int i = 0; i < s.Length; ++i)
                        {
                            numericValue *= 2;
                            numericValue += (s[i] - '0');
                        }
                        break;
                    case (int)EPegGrammar.decimal_digits:
                        Int32.TryParse(s, out numericValue);
                        break;
                }
                return "\\u" + numericValue.ToString("x4", null);
            }
            char GetCodePointUnicodeValue(PegNode n)
            {
                int numericValue = 0;
                string s = n.GetAsString(context_.src_);
                switch (n.id_)
                {
                    case (int)EPegGrammar.hexadecimal_digits:
                        Int32.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out numericValue);
                        break;
                    case (int)EPegGrammar.binary_digits:
                        for (int i = 0; i < s.Length; ++i)
                        {
                            numericValue *= 2;
                            numericValue += (s[i] - '0');
                        }
                        break;
                    case (int)EPegGrammar.decimal_digits:
                        Int32.TryParse(s, out numericValue);
                        break;
                }
                return (char)numericValue;
            }
            string GetCSharpChar(PegNode n)
            {
                if (n== null) return "";
                switch ((EPegGrammar)n.id_)
                {
                    case EPegGrammar.escape_char:
                    case EPegGrammar.escape_int: return GetEscapeValue(n);
                    case EPegGrammar.code_point: return GetCodePointValue(n.child_);
                    case EPegGrammar.printable_char: return n.GetAsString(context_.src_);
                    default: return n.GetAsString(context_.src_);
                }
            }
            char GetCSharpUnicode(PegNode n)
            {
                if (n.child_ != null)
                {
                    switch ((EPegGrammar)n.child_.id_)
                    {
                        case EPegGrammar.escape_char:
                        case EPegGrammar.escape_int: return GetEscapeUnicodeValue(n.child_);
                        case EPegGrammar.code_point: return GetCodePointUnicodeValue(n.child_.child_);
                        case EPegGrammar.printable_char: return n.GetAsString(context_.src_)[0];
                        default: Debug.Assert(false); return ' ';
                    }
                }
                else
                {
                    return n.GetAsString(context_.src_)[0];
                }
            }
            #endregion C# char encodings
            #region Template generators
            internal Template GenTemplateForRule(PegNode rule)
            {
                string sRuleName;
                bool bIsTree, bIsAst;
                PegNode ruleIdent = PUtils.FindNode(rule.child_, EPegGrammar.rule_name);
                Debug.Assert(ruleIdent != null);
                sRuleName = ruleIdent.GetAsString(context_.src_);
                PegNode ruleId = PUtils.GetRuleId(rule, true);
                ETemplateKind kind = ETemplateKind.TemplRule;
                PUtils.TreeOrAstPresent(ruleId.next_, out bIsTree, out bIsAst);
                PegNode create= PUtils.FindNode(rule.child_, EPegGrammar.create_spec);
                Template templNode;
                if (create!=null)
                {
                    if (bIsTree) kind = ETemplateKind.TemplTreeCreateRule;
                    else if (bIsAst) kind = ETemplateKind.TemplAstCreateRule;
                    Debug.Assert(create.child_ != null && create.child_.id_ == (int)EPegGrammar.create_method);
                    templNode = new TemplateStrings(kind, new List<string>() { sRuleName, create.child_.GetAsString(context_.src_) });
                }
                else
                {
                    if (bIsTree) kind = ETemplateKind.TemplTreeRule;
                    else if (bIsAst) kind = ETemplateKind.TemplAstRule;
                    else kind = ETemplateKind.TemplRule;
                    templNode = new TemplateString(kind, sRuleName);
                }
                PegNode rhs = PUtils.GetRhs(rule);
                if (rhs != null)
                {
                    Template templChild = GenTemplateForRhs(rhs, bIsTree || bIsAst);
                    templNode.subNodes_.Add(templChild);
                }
                return templNode;
            }
            Template GenTemplateForRhs(PegNode rhs, bool bIsTreeGenerating)
            {
                PegNode choice = rhs.child_;
                Debug.Assert(choice != null && choice.id_ == (int)EPegGrammar.choice);
                return GenTemplateForAlternatives(choice, bIsTreeGenerating);
            }
            Template TryGenTemplateForLiteralAlternatives(PegNode choice)
            {
                int count;
                List<string> literals= new List<string>();
                for (count=0; choice != null; choice = choice.next_,++count)
                {
                    PegNode n= PUtils.GetByPath(choice,
                        (int)EPegGrammar.choice,
                        (int)EPegGrammar.term,
                        (int)EPegGrammar.atom,
                        (int)EPegGrammar.suffixed_literal,
                        (int)EPegGrammar.quoted_content);
                    if( n==null || n.next_!=null || n.parent_.parent_.next_!=null || n.parent_.parent_.parent_.next_!=null) break;//case or atom_postfix or term
                    string s = "";
                    for (PegNode c = n.child_; c != null; c = c.next_)
                    {
                        s += GetCSharpUnicode(c);
                    }
                    literals.Add(s);
                }
                if (count >= 8 && choice == null)
                {
                    return new TemplateStrings(ETemplateKind.TemplLiterals, literals);
                }
                return null;
            }
            Template TryGenTemplateForOptimizedCharsets(ETemplateKind kind, CharsetInfo charsetInfo)
            {
                int count = 0;
                foreach (var chars in charsetInfo.chars_)
                {
                    count += chars.Count;
                }
                foreach (var ranges in charsetInfo.range_)
                {
                    count += ranges.Count;
                }
                if (count >= 8)
                {
                    return new TemplateCharsetInfo(
                                kind == ETemplateKind.TemplCharset? ETemplateKind.TemplOptimizedCharset: ETemplateKind.TemplNegatedOptimizedCharset, 
                                charsetInfo);
                }
                return null;
            }
            Template GenTemplateForAlternatives(PegNode choice, bool bIsTreeGenerating)
            {
                Template t = TryGenTemplateForLiteralAlternatives(choice);
                if (t != null) return t;
                if (choice.next_ != null)
                {
                    Template templNode = new Template(ETemplateKind.TemplOr);
                    for (; choice != null; choice = choice.next_)
                    {
                        templNode.subNodes_.Add(GenTemplateForTerm(choice.child_));
                    }
                    return templNode;
                }
                else
                {
                    return GenTemplateForTerm(choice.child_);
                }
            }
            Template GenTemplateForTerm(PegNode term)
            {
                Debug.Assert(term != null && term.id_ == (int)EPegGrammar.term);
                if (term.next_ != null)
                {
                    Template templNode = new Template(ETemplateKind.TemplAnd);
                    for (; term != null; term = term.next_)
                    {
                        templNode.subNodes_.Add(GenTemplateForAtomInfo(term));
                    }
                    return templNode;
                }
                else
                {
                    return GenTemplateForAtomInfo(term);
                }
            }
            Template GenTemplateForAtomInfo(PegNode term)
            {
                PegNode atom = term.child_;
                Debug.Assert(term.child_ != null);
                //handle pre-atom symbol
                Template templateNode = PreAtomItem(ref atom);//&a !a ^a ^^a
                if (templateNode == null)
                    templateNode = PostAtomItem(ref atom);//a{low,high} a*  a? a+
                if (templateNode != null)
                {
                    templateNode.subNodes_.Add(AtomChildItem(atom.child_));
                 //   HandleBitAccessOptimizations(ref templateNode);
                    return templateNode;
                }
                else
                {
                    return AtomChildItem(atom.child_);
                }

            }
            Template PreAtomItem(ref PegNode atom)
            {//handles ^a ^^a &a !a and a* a+ a{low,high}
                if (atom.id_ == (int)EPegGrammar.atom_prefix)
                {
                    Debug.Assert(atom.child_ != null);
                    switch (atom.child_.id_)
                    {
                        case (int)EPegGrammar.tree_symbol:
                        case (int)EPegGrammar.ast_symbol:
                            {//currently only
                                bool bIsNt = atom.child_.next_ != null && atom.child_.next_.id_ == (int)EPegGrammar.rule_ref;
                                bool bIsTree = atom.child_.id_ == (int)EPegGrammar.tree_symbol;
                                atom = atom.next_;
                                if (bIsNt)
                                {
                                    return new Template(bIsTree ? ETemplateKind.TemplTreeNT : ETemplateKind.TemplAstNT);
                                }
                                else
                                {
                                    return new Template(ETemplateKind.TemplTreeChars);
                                }
                            }
                        case (int)EPegGrammar.peek_symbol:
                            {
                                atom = atom.next_;
                                return new Template(ETemplateKind.TemplPeek);
                            }
                        case (int)EPegGrammar.not_symbol:
                            {
                                atom = atom.next_;
                                return new Template(ETemplateKind.TemplNot);
                            }
                        default:
                            Debug.Assert(false);
                            return null;
                    }
                }
                return null;
            }
            Template PostAtomItem(ref PegNode atom)
            {
                if (atom.next_ != null && atom.next_.id_ == (int)EPegGrammar.atom_postfix)
                {
                    PegNode n = atom.next_.child_;
                    Debug.Assert(n != null);
                    switch (n.id_)
                    {
                        case (int)EPegGrammar.repetition_range:
                            PegGrammarParser.TRange rep = n as PegGrammarParser.TRange;
                            return new TemplateRepetition(ETemplateKind.TemplRepetition, rep);
                        case (int)EPegGrammar.into_variable: //provisonary implementation
                            return new TemplateString(ETemplateKind.TemplIntoVariable, n.GetAsString(context_.src_));
                        case (int)EPegGeneratorNodes.IntoVarWithContext:
                            NormalizeTree.SemanticVarOrFuncWithContext intoVarInfo = n as NormalizeTree.SemanticVarOrFuncWithContext;
                            Debug.Assert(intoVarInfo != null);
                            var templInto = new TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>(ETemplateKind.TemplIntoVar, intoVarInfo);
                            return templInto;
                        default://"not yet implemented"
                            Debug.Assert(false);
                            return null;
                    }

                }
                return null;
            }
            Template AtomChildItem(PegNode atomChild)
            { /*returns a template node describing this atom and its childs */
                switch (atomChild.id_)
                {
                    case (int)EPegGrammar.rule_ref:
                        {
                            string sRuleName = PUtils.GetRuleNameFromRuleRef(atomChild, context_.src_);
                            Template templNode= new TemplateString(ETemplateKind.TemplRuleRef, sRuleName);
                            if (atomChild.next_ != null && atomChild.next_.id_ == (int)EPegGrammar.peg_args)
                            {
                                for (PegNode rhs = atomChild.next_.child_; rhs != null; rhs = rhs.next_)
                                {
                                    Template templArg = GenTemplateForRhs(rhs, true);
                                    templNode.subNodes_.Add(templArg);
                                }
                            }
                            return templNode;
                        }
                    case (int)EPegGeneratorNodes.GenericCall:
                        {
                            NormalizeTree.GenericCall gc = atomChild as NormalizeTree.GenericCall;
                            Debug.Assert(gc != null);
                            string sRuleName = gc.GetAsString(context_.src_);
                            return new TemplateString(ETemplateKind.TemplRuleRef, sRuleName);
                        }
                    case (int)EPegGeneratorNodes.SemanticFunctionWithContext:
                            NormalizeTree.SemanticVarOrFuncWithContext semFuncCall = atomChild as NormalizeTree.SemanticVarOrFuncWithContext;
                            Debug.Assert(semFuncCall != null);
                            var templSemFuncCall = new TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>(ETemplateKind.TemplSemFuncCall, semFuncCall);
                            return templSemFuncCall;
                    case (int)EPegGrammar.suffixed_literal:
                            Debug.Assert(atomChild.child_ != null);
                            bool bCaseSensitive = context_.HasCaseSensitiveProperty() || atomChild.child_.next_ != null && atomChild.child_.next_.id_ == (int)EPegGrammar.case_insensitve;
                            List<string> strings = new List<string>();
                            for (PegNode n = atomChild.child_.child_; n != null; n = n.next_)
                            {  
                                    strings.Add(GetCSharpChar(n));
                            }
                            return new TemplateStrings(
                                            bCaseSensitive
                                                ? ETemplateKind.TemplStringCaseInsensitive
                                                : ETemplateKind.TemplString,
                                            strings);
                    case (int)EPegGrammar.into_variable:
                        return new TemplateString(ETemplateKind.TemplIntoVariable, atomChild.GetAsString(context_.src_));
                    case (int)EPegGrammar.code_point:
                            List<string> codePointValue = new List<string>();
                            codePointValue.Add(GetCodePointValue(atomChild.child_));
                            return new TemplateStrings(ETemplateKind.TemplString, codePointValue);
                    case (int)EPegGrammar.character_set:
                        { /*^^character_set:    '[' set_negation? ((char_set_range/char_set_char)+ @']'*/
                            CharsetInfo charsetInfo = new CharsetInfo();
                            ETemplateKind kind = ETemplateKind.TemplCharset;
                            charsetInfo.chars_ = new List<List<string>>();
                            charsetInfo.range_ = new List<List<CharsetInfo.Range>>();
                            for (PegNode n = atomChild.child_; n != null; n = n.next_)
                            {
                                switch (n.id_)
                                {
                                    case (int)EPegGrammar.set_negation: kind = ETemplateKind.TemplNegatedCharset;
                                        break;
                                    case (int)EPegGrammar.char_set_char:
                                        {
                                            string s = GetCSharpChar(n.child_);

                                            if (charsetInfo.chars_.Count == 0 || charsetInfo.chars_[charsetInfo.chars_.Count - 1].Count % 16 == 0)
                                            {
                                                charsetInfo.chars_.Add(new List<string>());
                                            }
                                            charsetInfo.chars_[charsetInfo.chars_.Count - 1].Add(s);
                                        }
                                        break;
                                    case (int)EPegGrammar.char_set_range:
                                        {
                                            Debug.Assert(n.child_ != null && n.child_.id_ == (int)EPegGrammar.char_set_char
                                                        && n.child_.next_ != null && n.child_.next_.id_ == (int)EPegGrammar.char_set_char);
                                            string sFirst = GetCSharpChar(n.child_.child_);
                                            string sLast = GetCSharpChar(n.child_.next_.child_);
                                            CharsetInfo.Range r;
                                            r.lower= sFirst; r.upper = sLast;
                                            if (charsetInfo.range_.Count == 0 || charsetInfo.range_[charsetInfo.range_.Count - 1].Count % 4 == 0)
                                            {
                                                charsetInfo.range_.Add(new List<CharsetInfo.Range>());
                                            }
                                            charsetInfo.range_[charsetInfo.range_.Count - 1].Add(r);
                                        }
                                        break;
                                }

                            }
                            Template template= TryGenTemplateForOptimizedCharsets(kind,charsetInfo);
                            if (template != null) return template;
                            else
                                return new TemplateCharsetInfo(kind, charsetInfo);
                        }
                    case (int)EPegGrammar.rhs_of_rule:
                            Debug.Assert(atomChild.child_ != null && atomChild.child_.id_ == (int)EPegGrammar.choice);
                            return GenTemplateForAlternatives(atomChild.child_, false);
                    case (int)EPegGrammar.hexadecimal_digits:
                            List<string> hexadecimal_digitsValue = new List<string>();
                            hexadecimal_digitsValue.Add("\\s" + atomChild.GetAsString(context_.src_));
                            return new TemplateStrings(ETemplateKind.TemplString, hexadecimal_digitsValue);
                    case (int)EPegGrammar.any_char:
                        return new TemplateInt(ETemplateKind.TemplDots, 1);
                    case (int)EPegGeneratorNodes.FatalNode:
                    case (int)EPegGeneratorNodes.WarningNode:
                            NormalizeTree.Message m = atomChild as NormalizeTree.Message;
                            return new TemplateString(
                                            atomChild.id_ == (int)EPegGeneratorNodes.FatalNode
                                            ? ETemplateKind.TemplFatal
                                            : ETemplateKind.TemplWarning,
                                            m.message_);
                    case (int)EPegGrammar.message:
                            bool isFatal = false;
                            if ((isFatal = atomChild.child_.id_ == (int)EPegGrammar.fatal) || atomChild.child_.id_ == (int)EPegGrammar.warning)
                            {
                                if (atomChild.child_.next_.id_ == (int)EPegGrammar.multiline_double_quote_literal)
                                {
                                    var multiDblQuoteNode = atomChild.child_.next_ as PegGrammarParser.MultiLineDblQuoteNode;
                                    return new
                                            TemplateString(isFatal ? ETemplateKind.TemplFatal : ETemplateKind.TemplWarning,
                                            multiDblQuoteNode.quoted_);
                                }
                            }/*
                        else if( nThrow.child_.next_.id_==(int)Epeg_generator.enumerator ){
						    PegNode nEnum= nThrow.child_.next_;
                            templNode.name = nEnum.GetAsString(context_.src_);
					    }*/
                            Debug.Assert(false);

                        break;
                    case (int)EPegGrammar.bit_access:
                            Debug.Assert(atomChild.child_ != null && atomChild.child_.id_ == (int)EPegGrammar.bit_range);
                            PegGrammarParser.TRange bitRange = atomChild.child_ as PegGrammarParser.TRange;
                            Template templBitAccess = new TemplateRepetition(ETemplateKind.TemplBitAccess, bitRange);
                            Debug.Assert(atomChild.child_.next_!=null);
                            templBitAccess.subNodes_.Add(AtomChildItem(atomChild.child_.next_));
                            if (atomChild.child_.next_.next_ != null) templBitAccess.subNodes_.Add(AtomChildItem(atomChild.child_.next_.next_));
                            return templBitAccess;
                    case (int)EPegGeneratorNodes.IntoVarWithContext:
                            NormalizeTree.SemanticVarOrFuncWithContext intoVarInfo = atomChild as NormalizeTree.SemanticVarOrFuncWithContext;
                            Debug.Assert(intoVarInfo != null);
                            var templInto = new TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>(ETemplateKind.TemplIntoVar, intoVarInfo);
                            return templInto;
                    default:
                        Debug.Assert(false);
                        break;
                }
                return null;
            }
            /*void HandleBitAccessOptimizations(ref Template templateNode)
            {
                if(     templateNode.subNodes_.Count>=1 
                    &&  templateNode.subNodes_[0].kind_==ETemplateKind.TemplBitAccess ){
                    if( templateNode.kind_==ETemplateKind.TemplNot ){
                    }
                    else if (templateNode.kind_ == ETemplateKind.TemplPeek)
                    {
                    }
                }
            }*/
            #endregion Template generators
        }
        internal class CodeFromTemplate
        {
            #region Data Members
            PegCSharpGenerator parent_;
            TreeContext context_;
            #endregion Data Members
            #region Constructors
            internal CodeFromTemplate(PegCSharpGenerator parent, TreeContext context)
            {
                parent_ = parent;
                context_ = context;
            }
            #endregion Constructors
            #region Formatting and Code Emit
            void Emit(string rule)
            {/*append rulecode to generated source code */
                rule = new string(' ', 11) + rule;
                PrefixIndent(ref rule, 16);
                parent_.outFile_.Write(rule);
            }
            string DoAlignments(Template templNode, int indent, bool parentDone)
            { //currently replacements are unsafe because macro could also be found as string
                string s = templNode.templateCode;
                if (s == null) return "";
                s = s.Replace('\n', ' ');
                foreach (KeyValuePair<string, string> kvp in templNode.replacements)
                {
                    string value = kvp.Value.Replace('\n', ' ');
                    parent_.ReplaceMacro(ref s, kvp.Key, value);
                }
                for (int i = 0; i < templNode.subNodes_.Count; ++i)
                {
                    parent_.ReplaceMacro(ref s, "$(" + i.ToString() + ")", DoAlignments(templNode.subNodes_[i], indent, false));
                }
                if (indent + s.Length > context_.generatorParams_.maxLineLength_)
                {
                    if (!parentDone && indent != 0) return s;
                    s = templNode.templateCode;
                    string sIndent = new string(' ', indent) + "\n";
                    s = s.Replace("\n", sIndent);
                    foreach (KeyValuePair<string, string> kvp in templNode.replacements)
                    {
                        parent_.ReplaceMacro(ref s, kvp.Key, kvp.Value);
                    }
                    for (int i = 0; i < templNode.subNodes_.Count; ++i)
                    {
                        string elem = DoAlignments(templNode.subNodes_[i], indent + 2, true);
                        PrefixIndent(ref elem, indent + 2);
                        parent_.ReplaceMacro(ref s, "$(" + i.ToString() + ")", elem);
                    }
                }
                return s;
            }
            #endregion Formatting and Code Emi
            #region Helper Functions
            string GetAsCharacterCode(string s)
            {
                string result;
                switch(s)
                {
                    case "'": result = @"'\''"; break;
                    case "\\": result = @"'\\'"; break;
                    case "\\]": result = "']'"; break;
                    default: result = "'" + s + "'"; break;
                }
                if (context_.IsGrammarForBinaryInput())
                {
                    if (s.Length > 2 && s.Substring(0, 2) == "\\u")
                    {
                        return "0x" + s.Substring(2);
                    }
                    else
                    {
                        return "(byte)" + result;
                    }
                }
                return result;
            }
            void PrefixIndent(ref string s, int indent)
            {
                int i, last = 0;
                string sIndent = new string(' ', indent);
                while ((i = s.IndexOf('\n', last)) != -1)
                {
                    s = s.Substring(0, i + 1) + sIndent + s.Substring(i + 1);
                    last = i + indent + 1;
                }
            }
            bool IsComposite(Template templNode)
            {
                return templNode.kind_ == ETemplateKind.TemplOr;
            }
            #endregion Helper Functions
            #region Template Generation associated C# code
            void SetRuleCode(Template templNode)
            {
                string condition;
                if (templNode.subNodes_.Count > 0)
                {
                    condition = GenMatchCodeForCSharp(templNode.subNodes_[0], 1);
                    templNode.replacements.Add("$(CONDITION)", "$(0)");
                }
                else
                {
                    condition = "true";
                    templNode.replacements.Add("$(CONDITION)", "true");
                }
            }
            internal void GenMatchCodeForRuleCSharp(Template templNode)
            {
                /*generates C# code for templNode and its chidlren and stores it in the templNode*/
                switch (templNode.kind_)
                {
                    case ETemplateKind.TemplRule:
                            string ruleCode = parent_.FindCSharpTemplateCode(ECSharpKind.Rule);
                            templNode.templateCode = ruleCode;
                            string body;
                            if (templNode.subNodes_.Count > 0)
                            {
                                body = GenMatchCodeForCSharp(templNode.subNodes_[0], 1);
                                templNode.replacements.Add("$(BODY)", "$(0)");
                            }
                            else
                            {
                                body = "true";
                                templNode.replacements.Add("$(BODY)", "true");
                            }
                            parent_.ReplaceMacro(ref ruleCode, "$(BODY)", body);
                            ruleCode = DoAlignments(templNode, 0, false);
                            Emit(ruleCode);
                        break;
                    case ETemplateKind.TemplTreeRule:
                    case ETemplateKind.TemplAstRule:
                        {
                            TemplateString templString = templNode as TemplateString;
                            string s = parent_.FindCSharpTemplateCode(templNode.kind_ == ETemplateKind.TemplTreeRule ? ECSharpKind.RuleTree : ECSharpKind.RuleAst);

                            templNode.templateCode = s;
                            SetRuleCode(templNode);
                            templNode.replacements.Add("$(MODULE_NAME)", parent_.moduleName_);
                            templNode.replacements.Add("$(ENUMERATOR)", parent_.GetCSharpPrefixed(templString.name_));
                            s = DoAlignments(templNode, 0, false);
                            Emit(s);
                        }
                        break;
                    case ETemplateKind.TemplAstCreateRule:
                    case ETemplateKind.TemplTreeCreateRule:
                        {
                            TemplateStrings templStrings = templNode as TemplateStrings;
                            string s = parent_.FindCSharpTemplateCode(templNode.kind_ == ETemplateKind.TemplTreeCreateRule ? ECSharpKind.RuleCreaTree : ECSharpKind.RuleCreaAst);
                            templNode.templateCode = s;
                            SetRuleCode(templNode);
                            templNode.replacements.Add("$(MODULE_NAME)", parent_.moduleName_);
                            templNode.replacements.Add("$(ENUMERATOR)", parent_.GetCSharpPrefixed(templStrings.strings_[0]));
                            templNode.replacements.Add("$(CREATOR)", templStrings.strings_[1]);
                            s = DoAlignments(templNode, 0, false);
                            Emit(s);
                        }
                        break;
                    default: Debug.Assert(false);//not yet implemented
                        break;
                }
            }
            string BuildConditionStringCSharp(Template templNode, string templateCode, string sOperator, int level)
            {
                bool bFirstTime = true;
                string templateCondition = "";
                for (int i = 0; i < templNode.subNodes_.Count; ++i)
                {
                    var subNode = templNode.subNodes_[i];
                    string condPart = GenMatchCodeForCSharp(subNode, level + 1);
                    if (i > 0) templateCondition += "\n";
                    if (!bFirstTime) templateCondition += sOperator + " ";
                    else templateCondition += "   ";
                    if (IsComposite(subNode)) templateCondition += "($(" + i.ToString() + "))";
                    else templateCondition += "$(" + i.ToString() + ")";
                    bFirstTime = false;
                }
                templNode.replacements.Add("$(CONDITION)", templateCondition);
                return templateCode;
            }
            string GetObjectName(PegNode semanticBlock, bool bIsLocal)
            {
                if (semanticBlock.id_ == (int)EPegGrammar.named_semantic_block)
                {
                    Debug.Assert(semanticBlock.child_.id_ == (int)EPegGrammar.sem_block_name);
                    string name = semanticBlock.child_.GetAsString(context_.src_);
                    if (name.Substring(0, 1).ToLower() == name.Substring(0, 1))
                    {
                        FatalErrOut("FATAL from <PEG_GENERATOR>: Semantic Block Name '" + name + "' must start with an uppercase");
                    }
                    string varName = name.Substring(0, 1).ToLower();
                    if (name.Length > 1) varName += name.Substring(1);
                    return varName;
                }
                else
                {
                    Debug.Assert(semanticBlock.id_ == (int)EPegGrammar.anonymous_semantic_block);
                    if (bIsLocal) return "_sem";
                    else return "_top";
                }
            }

            private void FatalErrOut(string p)
            {
                context_.errOut_.WriteLine(p);
                if (parent_.outFile_ != null) parent_.outFile_.WriteLine(p);
            }
            string GetBitAccessCode(string cSharpTemplCode, TemplateRepetition templRange)
            {
                Debug.Assert(templRange.subNodes_.Count > 0);
                Template templMatch = templRange.subNodes_[0];
                Debug.Assert(   templMatch.kind_ == ETemplateKind.TemplString
                            ||  templMatch.kind_ == ETemplateKind.TemplDots);//ETemplateKind.TemplCharset not yet handled
                Template templInto = templRange.subNodes_.Count <= 1 ? null : templRange.subNodes_[1];
                string value = "'\u0000'";
                if (templMatch.kind_ == ETemplateKind.TemplDots)
                {
                    //nothing has to be don
                }
                else if (templMatch.kind_ == ETemplateKind.TemplCharset)
                {
                    Debug.Assert(false); // not yet implemented
                }
                else
                {
                    TemplateStrings templStrings = templMatch as TemplateStrings;
                    Debug.Assert(templStrings != null && templStrings.strings_.Count > 0);
                    value = GetAsCharacterCode(templStrings.strings_[0]);
                }
                parent_.ReplaceMacro(ref cSharpTemplCode, "$(LOWER)", templRange.repetition_.lower.ToString());
                parent_.ReplaceMacro(ref cSharpTemplCode, "$(UPPER)", templRange.repetition_.upper.ToString());
                parent_.ReplaceMacro(ref cSharpTemplCode, "$(BYTE)", value);
                if (templInto!=null)
                {
                    TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext> tc;
                    if ((tc = templInto as TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>) != null)
                    {
                        NormalizeTree.SemanticVarOrFuncWithContext intoInfo = tc.t_;
                        string intoName = 
                                    GetObjectName(intoInfo.semBlock_,intoInfo.isLocal_) 
                                +   "." 
                                +   intoInfo.variableOrFunc_.GetAsString(context_.src_);
                        parent_.ReplaceMacro(ref cSharpTemplCode, "$(INTO)", intoName);
                    }
                }
                return cSharpTemplCode;
            }
            bool TryHandleBitaccessOptimization(Template templNode)
            {// try generate PeekBits,NotBits,PeekBit,NotBit ,:not yet handled: charsets as last parameter
                Debug.Assert(templNode.kind_ == ETemplateKind.TemplPeek || templNode.kind_ == ETemplateKind.TemplNot);
                if (    templNode.subNodes_.Count > 0 
                    &&  templNode.subNodes_[0].kind_ == ETemplateKind.TemplBitAccess )
                {
                    TemplateRepetition templRange = templNode.subNodes_[0] as TemplateRepetition;
                    Debug.Assert(templRange.subNodes_.Count > 0);
                    if (templRange.subNodes_.Count > 1) return false; //no optimization possible
                    string sCSharpCodeTemplate;
                    if (templRange.repetition_.lower == templRange.repetition_.upper)
                    {
                        sCSharpCodeTemplate =
                             parent_.FindCSharpTemplateCode(templNode.kind_ == ETemplateKind.TemplPeek ? ECSharpKind.PeekBit : ECSharpKind.BitNot);
                    }
                    else
                    {
                        sCSharpCodeTemplate =
                              parent_.FindCSharpTemplateCode(templNode.kind_ == ETemplateKind.TemplPeek ? ECSharpKind.PeekBits : ECSharpKind.NotBits);
                    }
                    sCSharpCodeTemplate= GetBitAccessCode(sCSharpCodeTemplate, templRange);
                    templNode.templateCode = sCSharpCodeTemplate;
                    return true;
                }
                return false;
            }
            string InRangeCode(List<List<CharsetInfo.Range>> ranges,ETemplateKind kind,ref int elemCount)
            {
                string InTemplate = kind == ETemplateKind.TemplCharset 
                                ? parent_.FindCSharpTemplateCode(ECSharpKind.In)
                                : parent_.FindCSharpTemplateCode(ECSharpKind.NotIn);
                string condition = "";
                if (kind == ETemplateKind.TemplCharset)
                {
                    foreach (var range in ranges)
                    {
                        string InTempl = InTemplate;
                        string pairs = "";
                        foreach (var pair in range)
                        {
                            if (pairs.Length > 0) pairs += ", ";
                            pairs += GetAsCharacterCode(pair.lower) + "," + GetAsCharacterCode(pair.upper);
                        }
                        parent_.ReplaceMacro(ref InTempl, "$(PAIRS)", pairs);
                        if (condition.Length > 0) condition += "||";
                        condition += InTempl;
                        ++elemCount;
                    }
                }
                else
                {
                    foreach (var range in ranges)
                    {
                        string InTempl = InTemplate;
                        string pairs = "";
                        foreach (var pair in range)
                        {
                            pairs += pair.lower + pair.upper;
                        }
                        parent_.ReplaceMacro(ref InTempl, "$(PAIRS)", "\"" + pairs + "\"");
                        if (condition.Length > 0) condition += "||";
                        condition += InTempl;
                        ++elemCount;
                    }
                }
                return condition;
            }
            string GetAsInDoubleQuotes(string s)
            {
                return s.Replace("\"", "\\\"");
            }           
            string OneOfCharsCode(List<List<string>> chars, ETemplateKind kind,ref int elemCount)
            {
                string OneOfCharsTemplate = kind == ETemplateKind.TemplCharset 
                                ? parent_.FindCSharpTemplateCode(ECSharpKind.OneOf)
                                : parent_.FindCSharpTemplateCode(ECSharpKind.NotOneOf);
                string condition = "";
                foreach (var charset in chars)
                {
                    string oneOfChar = OneOfCharsTemplate;
                    string formattedChars = "";
                    foreach (var singleChar in charset)
                    {
                        formattedChars += GetAsInDoubleQuotes(singleChar);
                    }
                    parent_.ReplaceMacro(ref oneOfChar, "$(CHARS)", "\"" + formattedChars + "\"");
                    if (condition.Length > 0) condition += "||";
                    condition += oneOfChar;
                    ++elemCount;
                }
                return condition;
            }
            string GetLimitCode(int numericLimit,PegNode variableLimit)
            {
                if (variableLimit != null)
                {
                    var intoVarInfo = variableLimit as NormalizeTree.SemanticVarOrFuncWithContext;
                    string intoName =
                                    GetObjectName(intoVarInfo.semBlock_, intoVarInfo.isLocal_)
                                + "."
                                + intoVarInfo.variableOrFunc_.GetAsString(context_.src_);
                    return intoName;
                }
                else return numericLimit.ToString();
            }
            string GenMatchCodeForCSharp(Template templNode, int level)
            {
                Debug.Assert(templNode != null);
                switch (templNode.kind_)
                {
                    case ETemplateKind.TemplNot:
                        {
                            if( TryHandleBitaccessOptimization(templNode) ) return "";
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.Not);
                            templNode.templateCode = s;
                            string sCondition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                            templNode.replacements.Add("$(CONDITION)", "$(0)");
                            return s;
                        }
                    case ETemplateKind.TemplPeek:
                        {
                            if (TryHandleBitaccessOptimization(templNode)) return "";
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.Peek);
                            templNode.templateCode = s;
                            string sCondition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                            templNode.replacements.Add("$(CONDITION)", "$(0)");
                            return s;
                        }
                    case ETemplateKind.TemplAnd:
                        {
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.And);
                            templNode.templateCode = s;
                            string sResult = BuildConditionStringCSharp(templNode, s, "&&", level + 1);
                            return sResult;
                        }

                    case ETemplateKind.TemplOr:
                        {
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.Or);
                            templNode.templateCode = s;
                            string sResult = BuildConditionStringCSharp(templNode, s, "||", level + 1);
                            return sResult;
                        }

                    case ETemplateKind.TemplNegatedCharset:
                    case ETemplateKind.TemplCharset:
                        {
                            string InTemplate = parent_.FindCSharpTemplateCode(ECSharpKind.In);
                            string OneOfCharsTemplate = parent_.FindCSharpTemplateCode(ECSharpKind.OneOf);
                            TemplateCharsetInfo charsetNode = templNode as TemplateCharsetInfo;
                            int elemCount = 0;
                            string condition = InRangeCode(charsetNode.charsetInfo_.range_, templNode.kind_,ref elemCount);
                            string condition1 = OneOfCharsCode(charsetNode.charsetInfo_.chars_, templNode.kind_, ref elemCount);
                            if( condition.Length > 0 && condition1.Length > 0) condition+= "||";
                            condition+= condition1;
                            if (elemCount > 1) condition = "(" + condition + ")";
                            templNode.templateCode = condition; 
                            return condition;
                        }
                    case ETemplateKind.TemplIntoVariable:
                        { //provisonary implementation
                            TemplateString templInto = templNode as TemplateString;
                            Debug.Assert(templInto.subNodes_.Count > 0);
                            templInto.replacements.Add("$(CONDITION)","$(0)");
                            templInto.replacements.Add("$(INTO)", templInto.name_);
                            templInto.templateCode = parent_.FindCSharpTemplateCode(ECSharpKind.Into);
                            GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                            return "";
                        }
                    case ETemplateKind.TemplIntoVar:
                        {
                            TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext> templIntoVar = templNode as TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>;
                            NormalizeTree.SemanticVarOrFuncWithContext intoInfo = templIntoVar.t_;
                             string intoName = GetObjectName(intoInfo.semBlock_, intoInfo.isLocal_)
                                             + "." + intoInfo.variableOrFunc_.GetAsString(context_.src_);
                             Debug.Assert(templIntoVar.subNodes_.Count > 0);
                             templIntoVar.replacements.Add("$(CONDITION)", "$(0)");
                             templIntoVar.replacements.Add("$(INTO)", intoName);
                             templIntoVar.templateCode = parent_.FindCSharpTemplateCode(ECSharpKind.Into);
                            GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                            return "";
                        }
                    case ETemplateKind.TemplSemFuncCall:
                        {
                            TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext> templFuncCall= templNode as TemplateContainer<NormalizeTree.SemanticVarOrFuncWithContext>;
                            NormalizeTree.SemanticVarOrFuncWithContext intoInfo = templFuncCall.t_;
                            string callSem = GetObjectName(intoInfo.semBlock_, intoInfo.isLocal_)
                                            + "." + intoInfo.variableOrFunc_.GetAsString(context_.src_);
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.RuleRef);
                            templNode.templateCode = s;
                            templNode.replacements.Add("$(NAME)", callSem);
                            return s;
                        }
                    case ETemplateKind.TemplRepetition:
                        {
                            TemplateRepetition templRep = templNode as TemplateRepetition;
                            if (templRep.repetition_.lower == 0 && templRep.repetition_.upper == 1)
                            {//option
                                string result = parent_.FindCSharpTemplateCode(ECSharpKind.Option);
                                templNode.templateCode = result;
                                string condition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                                templNode.replacements.Add("$(CONDITION)", "$(0)");
                                return result;
                            }
                            else if ((templRep.repetition_.lower == 0 || templRep.repetition_.lower == 1) && templRep.repetition_.upper == Int32.MaxValue)
                            {
                                bool bIsOptRepeat = templRep.repetition_.lower == 0;
                                string result = parent_.FindCSharpTemplateCode(bIsOptRepeat ? ECSharpKind.OptRepeat : ECSharpKind.PlusRepeat);
                                templNode.templateCode = result;
                                string condition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                                templNode.replacements.Add("$(CONDITION)", "$(0)");
                                return result;
                            }
                            else
                            { // general for loop needed
                                string result = parent_.FindCSharpTemplateCode(ECSharpKind.ForLoop);
                                templNode.templateCode = result;
                                string condition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                                string lower = GetLimitCode(templRep.repetition_.lower, templRep.repetition_.lowerIntoVar);
                                string upper = GetLimitCode(templRep.repetition_.upper, templRep.repetition_.upperIntoVar);
                                templNode.replacements.Add("$(CONDITION)", "$(0)");
                                templNode.replacements.Add("$(LOWER)", lower);
                                templNode.replacements.Add("$(UPPER)", upper);
                                return result;
                            }
                        }
                    case ETemplateKind.TemplRuleRef:
                        {
                            TemplateString templString = templNode as TemplateString;
                            string s= parent_.FindCSharpTemplateCode(templString.subNodes_.Count > 0 
                                                ? ECSharpKind.RuleRefWithArgs 
                                                : ECSharpKind.RuleRef);
                            templNode.templateCode = s;
                            string refName = parent_.GetCSharpPrefixed(templString.name_);
                            templNode.replacements.Add("$(NAME)", refName);
                            if (templString.subNodes_.Count > 0) //call rule having arguments
                            {
                                for(int i=0;i<templNode.subNodes_.Count;++i)
                                {
                                    GenMatchCodeForCSharp(templNode.subNodes_[i],level+1);
                                    templNode.replacements.Add("$(ARG" + i.ToString() + ")", "$(" + i.ToString() + ")");
                                    if (i + 1 < templNode.subNodes_.Count)
                                    {
                                        string continuation = argNReplace;
                                        string next = "$(ARG" + (i + 1).ToString() + ")";
                                        continuation= continuation.Replace("$(ARGN)", next);
                                        continuation = continuation.Replace("$(ARGN1)", "$(ARGN)");
                                        s= s.Replace("$(ARGN)", continuation);
                                    }
                                    else
                                    {
                                        s= s.Replace("$(ARGN)", "");
                                    }
                                }
                                templNode.templateCode = s;
                                return s;
                            }
                            return s;
                        }
                    case ETemplateKind.TemplLiterals:
                        {
                            TemplateStrings templateStrings = templNode as TemplateStrings;
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.Literals);
                            templNode.templateCode = s;
                            string varName = "optimizedLiterals" + parent_.literalsCount_.ToString();
                            templNode.replacements.Add("$(LITERALS)", varName);
                            ++parent_.literalsCount_;
                            AddOptimizationInitialization(templateStrings, varName);
                            return s;
                        }
                    case ETemplateKind.TemplOptimizedCharset:
                    case ETemplateKind.TemplNegatedOptimizedCharset:
                        {
                            TemplateCharsetInfo charsetNode = templNode as TemplateCharsetInfo;
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.OptimizedCharset);
                            templNode.templateCode = s;
                            string varName= "optimizedCharset" + parent_.optimizedCharsetCount_.ToString();
                            templNode.replacements.Add("$(CHARSET)",varName);
                            ++parent_.optimizedCharsetCount_;
                            AddOptimizationInitialization(charsetNode, varName,templNode.kind_);
                            return s;
                        }
                    case ETemplateKind.TemplString:
                    case ETemplateKind.TemplStringCaseInsensitive:
                        {
                            TemplateStrings templStrings = templNode as TemplateStrings;
                            string s = parent_.FindCSharpTemplateCode(templNode.kind_ == ETemplateKind.TemplString ? ECSharpKind.String : ECSharpKind.StringCaseInsensitive);
                            templNode.templateCode = s;
                            string sChars = "";
                            if (templStrings.strings_.Count >= 8)
                            {
                                 for (int i = 0; i < templStrings.strings_.Count; ++i){
                                    string c= GetAsCharacterCode(templStrings.strings_[i]);
                                    c= c.Substring(1,c.Length-2);
                                    if( c.Length==1 && c[0]=='"' ) c= "\\" + c;
                                    sChars += c;
                                 }
                                 sChars = "\"" + sChars + "\"";
                            }else{
                                for (int i = 0; i < templStrings.strings_.Count; ++i)
                                {
                                    if (i > 0) sChars += ",";
                                    sChars += GetAsCharacterCode(templStrings.strings_[i]);
                                }
                            }
                            templNode.replacements.Add("$(CHARS)", sChars);
                            return s;
                        }
                    case ETemplateKind.TemplDots:
                        {
                            string result = parent_.FindCSharpTemplateCode(ECSharpKind.Any);
                            templNode.templateCode = result;
                            return result;
                        }
                    case ETemplateKind.TemplBitAccess:
                        {
                            TemplateRepetition templRange = templNode as TemplateRepetition;
                            string cSharpTemplCode;
                            ECSharpKind kind;
                            if( templRange.subNodes_.Count>=2 ){
                                if (templRange.subNodes_[0].kind_ == ETemplateKind.TemplDots)
                                     kind = ECSharpKind.BitsInto;
                                else kind = ECSharpKind.MatchingBitsInto;
                            }else if(  templRange.repetition_.lower == templRange.repetition_.upper ){
                                kind= ECSharpKind.Bit;
                            }else{
                                kind= ECSharpKind.Bits;
                            }
                            cSharpTemplCode= parent_.FindCSharpTemplateCode(kind);
                            templNode.templateCode = GetBitAccessCode(cSharpTemplCode, templRange);
                            return cSharpTemplCode;
                        }
                    case ETemplateKind.TemplWarning:
                    case ETemplateKind.TemplFatal:
                        {
                            TemplateString templString = templNode as TemplateString;
                            string result = parent_.FindCSharpTemplateCode(
                                templNode.kind_ == ETemplateKind.TemplFatal ? ECSharpKind.Fatal : ECSharpKind.Warning);
                            templNode.templateCode = result;
                            templNode.replacements.Add("$(PREFIX)", "");
                            templNode.replacements.Add("$(ERROR)", GetAsInDoubleQuotes(templString.name_));
                            return result;
                        }
                    case ETemplateKind.TemplTreeChars:
                        {
                            string s = parent_.FindCSharpTemplateCode(ECSharpKind.TreeChars);
                            templNode.templateCode = s;
                            string condition = GenMatchCodeForCSharp(templNode.subNodes_[0], level + 1);
                            templNode.replacements.Add("$(CONDITION)", "$(0)");
                            return s;
                        }
                    default: Debug.Assert(false);
                        return "";
                }
            }
            #endregion Template Generation associated C# code
            #region Code Optimizations
            void CheckAddStaticConstructor()
            {
                if (parent_.optimizationStaticConstructor_.Length == 0)
                {
                    string s = parent_.FindCSharpTemplateCode(ECSharpKind.StaticConstructor);
                    parent_.ReplaceMacro(ref s, "$(MODULE_NAME)", parent_.moduleName_);
                    parent_.optimizationStaticConstructor_.Append(s);
                }
            }
            void AddOptimizationInitialization(TemplateStrings literals, string varName)
            {
                CheckAddStaticConstructor();
                StringBuilder s = new StringBuilder();
                s.Append("{\n               string[] literals=\n               ");
                s.Append("{ ");
                for (int i = 0; i < literals.strings_.Count; ++i)
                {
                    if (i > 0)
                    {
                        s.Append(",");
                        if (i % 8 == 0) s.Append("\n                  ");
                    }
                    s.Append("\"");
                    s.Append(literals.strings_[i]);
                    s.Append("\"");
                }
                s.Append(" };\n               ");
                s.Append(varName);
                s.Append("= new OptimizedLiterals(literals);\n            }\n");
                parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,
                            "$(OPTIMIZEDLITERALS_DECL)",
                            "internal static OptimizedLiterals " + varName + ";\n        " + "$(OPTIMIZEDLITERALS_DECL)");
                parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,
                            "$(OPTIMIZEDLITERALS_IMPL)",
                            s.ToString() + "\n            $(OPTIMIZEDLITERALS_IMPL)");
            }
            void AddOptimizationInitialization(TemplateCharsetInfo charset, string varName,ETemplateKind kind)
            {
                CheckAddStaticConstructor();
                StringBuilder s = new StringBuilder();
                s.Append("{\n               ");

                if (charset.charsetInfo_.range_.Count > 0)
                {
                    s.Append("OptimizedCharset.Range[] ranges = new OptimizedCharset.Range[]\n                  {");
                    foreach(var r in charset.charsetInfo_.range_)
                    {
                        foreach (var pair in r)
                        {
                            s.Append("new OptimizedCharset.Range(");
                            s.Append(GetAsCharacterCode(pair.lower) + "," + GetAsCharacterCode(pair.upper));
                            s.Append("),\n                   ");
                        }
                    }
                    s.Append("};\n               ");
                }
                if (charset.charsetInfo_.chars_.Count > 0)
                {
                    s.Append("char[] oneOfChars = new char[]    {");
                    bool bFirstTime = true;
                    int count = 0;
                    foreach (var chars in charset.charsetInfo_.chars_)
                    {
                        foreach (var singleChar in chars)
                        {
                            if (!bFirstTime) s.Append(",");
                            s.Append(GetAsCharacterCode(singleChar));
                            if (++count % 5 == 0)
                            {
                                s.Append("\n                                                  ");
                            }
                            bFirstTime = false;
                        }
                    }
                    s.Append("};\n               ");
                }
                s.Append(varName);
                s.Append("= new OptimizedCharset(");
                s.Append(charset.charsetInfo_.range_.Count > 0 ? "ranges,":"null,");
                s.Append(charset.charsetInfo_.chars_.Count > 0 ? "oneOfChars":"null");
                if (kind == ETemplateKind.TemplNegatedOptimizedCharset) s.Append(", true");
                s.Append(");");
                s.Append("\n            }\n            ");
                parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,
                            "$(OPTIMIZEDCHARSET_DECL)",
                            "internal static OptimizedCharset " + varName + ";\n        " + "$(OPTIMIZEDCHARSET_DECL)");
                parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,
                            "$(OPTIMIZEDCHARSET_IMPL)",
                            s.ToString() + "\n            $(OPTIMIZEDCHARSET_IMPL)");
                

            }
            #endregion Code Optimizations
        }
        internal class TopLevelCode
        {
            #region Local Types
            struct InitializationTermination{public string initialization;public string termination;}
            #endregion Local Types
            #region Data Members
            PegCSharpGenerator parent_;
            TreeContext context_;
            Dictionary<string, InitializationTermination> dictUsing_ = new Dictionary<string, InitializationTermination>();

            #endregion Data Members
            #region Constructors
            internal TopLevelCode(PegCSharpGenerator parent, TreeContext context)
            {
                parent_ = parent;
                context_ = context;
            }
            #endregion Constructors
            #region Internal Functions
            internal void GenCodeCSharpForThisModule()
            {
                if (!OpenOutFile("C#", ".cs")) return;
                try
                {
                    GenCodeForModuleHeadCSharp();
                    GenCodeForRulesCSharp();
                    GenCodeForOptimizations();
                    GenCodeForModuleEndCSharp();
                    
                }
                catch (Exception )
                {
                    throw;
                }
                finally
                {
                    context_.generatorParams_.errOut_.WriteLine("INFO  from <PEG_GENERATOR> {0} bytes written to '{1}'",
                                                ((StreamWriter)parent_.outFile_).BaseStream.Position,
                                                parent_.outputFileName_);
                    parent_.outFile_.Close();
                }
            }
            #endregion Internal Functions
            #region Private Functions
            bool OpenOutFile(string sGenSubDirectory, string fileEnding)
            {
                try
                {
                    string cSharpDir =
                        PUtils.MakeFileName("",context_.generatorParams_.outputDirectory_, sGenSubDirectory);
                    parent_.outputFileName_ = PUtils.MakeFileName(
                            parent_.moduleName_ + fileEnding,cSharpDir);
                    if (!Directory.Exists(cSharpDir))
                    {
                        Directory.CreateDirectory(cSharpDir);
                    }
                    parent_.outFile_ = new StreamWriter(parent_.outputFileName_);
                    parent_.outFile_.WriteLine("/* created on {0} from peg generator V1.0 using '{1}' as input*/", DateTime.Now.ToString(),context_.generatorParams_.sourceFileTitle_);
                }
                catch (Exception e)
                {
                    context_.errOut_.WriteLine("FATAL from <PEG_GENERATOR> FILE:'{0}' could not be opened (%s)", parent_.outputFileName_, e.Message);
                    return false;
                }
                return true;
            }
            string GenCodeEnumForRuleCSharp(PegNode n)
            {
                PegNode ruleIdent = PUtils.FindNode(n.child_, EPegGrammar.rule_name);
                Debug.Assert(ruleIdent != null);
                PegNode ruleId = PUtils.GetRuleId(n, true);
                string sEnum;
                sEnum =
                        parent_.GetCSharpPrefixed(ruleIdent.GetAsString(context_.src_)) + "= " + ruleId.GetAsString(context_.src_);
                return sEnum;
            }
            bool IsGrammarForBinaryInput()
            {
                return context_.dictProperties_.ContainsKey("encoding_class")
                    && context_.dictProperties_["encoding_class"].Equals("binary", StringComparison.InvariantCultureIgnoreCase);
            }
            bool GetEncoding(out string  encoding_class,out string  encoding_detection)
            {
                encoding_class = EncodingClass.ascii.ToString();
                encoding_detection = UnicodeDetection.notApplicable.ToString();
                if (!context_.dictProperties_.ContainsKey("encoding_class")) return false;
                encoding_class = context_.dictProperties_["encoding_class"];
                encoding_detection = UnicodeDetection.notApplicable.ToString();
                if (context_.dictProperties_.ContainsKey("encoding_detection"))
                {
                    encoding_detection = context_.dictProperties_["encoding_detection"];
                }
                return true;
            }
            string GenEnumeratorDefinition()
            {
                string sEnumerators = "";
                PegNode firstRule = PUtils.GetRuleFromRoot(context_.root_);
                for (PegNode q = firstRule; q != null; q = q.next_)
                {
                    if (q != firstRule) sEnumerators += ", ";
                    int NLpos = sEnumerators.LastIndexOf('\n');
                    if (sEnumerators.Substring(NLpos + 1).Length > context_.generatorParams_.maxLineLength_)
                    {
                        sEnumerators += "\n";
                    }
                    sEnumerators += GenCodeEnumForRuleCSharp(q);
                }
                return sEnumerators;
            }
           
            string GenSemanticBlockContent(PegNode semBlock, string className,bool isLocal,
                                                out string initialization,out string termination)
            {
                string blockSrc="";
                int begPos = semBlock.match_.posBeg_;
                PegNode content = PUtils.FindNode(semBlock, EPegGrammar.semantic_block_content);
                Debug.Assert(content != null);
                bool constructorFound = false;
                bool disposeFound;
                bool destructorOrDisposableFound = FindDispose(content,out disposeFound);
                if (isLocal && destructorOrDisposableFound)
                {
                    if (semBlock.id_ == (int)EPegGrammar.anonymous_semantic_block) blockSrc += " : IDisposable";
                    else if (semBlock.id_ == (int)EPegGrammar.named_semantic_block)
                    {
                        blockSrc += context_.src_.Substring(begPos, semBlock.child_.match_.posEnd_ - begPos);
                        begPos = semBlock.child_.match_.posEnd_;
                        blockSrc += " : IDisposable";
                    }
                } 
                for (PegNode member = content.child_; member != null; member = member.next_)
                {
                    switch ((EPegGrammar)member.id_)
                    {
                        case EPegGrammar.into_declaration:{
                            for( PegNode variable = PUtils.FindNode(member, EPegGrammar.variable);
                                 variable!=null;
                                 variable= PUtils.FindNodeNext(variable,EPegGrammar.variable))
                            {
                                if( IsUsedMember(variable)||(!isLocal&&IsAccessedInLocalClass(variable)))
                                {
                                    if( !AccessModifierPresent(member.child_))
                                    {
                                        AddInternalModifier(ref blockSrc,ref begPos,member);
                                        break;
                                    }
                                }
                            }
                        }
                            break;
                        case EPegGrammar.field_declaration:{
                            for (PegNode variable = PUtils.FindNode(member, EPegGrammar.variable);
                                 variable != null;
                                 variable = PUtils.FindNodeNext(variable, EPegGrammar.variable))
                            {
                                if ((!isLocal && IsAccessedInLocalClass(variable)) && !AccessModifierPresent(member.child_))
                                {
                                    AddInternalModifier(ref blockSrc, ref begPos, member);
                                    break;
                                }
                            }
                        }
                            break;
                        case EPegGrammar.sem_func_declaration:
                        case EPegGrammar.creator_func_declaration:
	                    {
                            PegNode memberName= PUtils.FindNode(member, EPegGrammar.member_name);
                            if (    IsUsedMember(memberName) ||  (!isLocal&&IsAccessedInLocalClass(memberName)))
                            {
                                if (!AccessModifierPresent(member.child_.child_))
                                {
                                    AddInternalModifier(ref blockSrc,ref begPos,member);
                                }
                            }
                            if (isLocal)
                            {
                                QualifyTopLevelMemberAccessInMethodBody(member.child_.next_, ref blockSrc, ref begPos);
                            } 
	                    }
                        break;
                        case EPegGrammar.func_declaration:{
                            PegNode memberName= PUtils.FindNode(member,EPegGrammar.member_name);
                            if( memberName.GetAsString(context_.src_)=="Dispose" && !AccessModifierPresent(member.child_.child_))
                            {
                                AddPublicModifier(ref blockSrc,ref begPos,member);
                            }else{
                                if( (!isLocal&&IsAccessedInLocalClass(memberName)) && !AccessModifierPresent(member.child_.child_))
                                {
                                    AddInternalModifier(ref blockSrc,ref begPos,member);
                                }
                            }
                             if (isLocal)
                            {
                                QualifyTopLevelMemberAccessInMethodBody(member.child_.next_, ref blockSrc, ref begPos);
                            } 
                        }
                            break;
                        case EPegGrammar.destructor_decl:
                            {
                                if (!disposeFound && isLocal)
                                {
                                    blockSrc += context_.src_.Substring(begPos, member.match_.posBeg_ - begPos);
                                    blockSrc += GetMinimumIndentation(blockSrc)+"public void Dispose()";
                                    begPos = member.child_.next_.match_.posBeg_;
                                    QualifyTopLevelMemberAccessInMethodBody(member.child_.next_, ref blockSrc, ref begPos);
                                }
                            }
                            break;
                        case EPegGrammar.constructor_decl:
                            {
                                constructorFound = true;
                                if (!AccessModifierPresent(member.child_.child_))
                                {
                                    AddInternalModifier(ref blockSrc, ref begPos, member);
                                }
                                PegNode memberName = PUtils.FindNode(member, EPegGrammar.member_name);
                                blockSrc += context_.src_.Substring(begPos, memberName.match_.posBeg_ - begPos);
                                blockSrc += className;
                                begPos = memberName.match_.posEnd_;
                                if (isLocal)
                                {
                                    PegNode f = PUtils.FindNode(member.child_, EPegGrammar.formal_pars);
                                    if (f!=null  && f.child_ == null)
                                    {
                                        AddParentParameterToConstructor(ref blockSrc,ref begPos,f);
                                       
                                    }
                                    QualifyTopLevelMemberAccessInMethodBody(member.child_.next_, ref blockSrc, ref begPos);
                                    blockSrc += context_.src_.Substring(begPos, member.match_.posEnd_ - begPos);
                                    begPos = member.match_.posEnd_;
                                    blockSrc += "\n" + GetMinimumIndentation(blockSrc) + parent_.moduleName_ + " parent_;\n";

                                }
                            }
                            break;
                    }
                }
                if (isLocal && !constructorFound)
                {
                    blockSrc += context_.src_.Substring(begPos, content.match_.posEnd_ - begPos);
                    begPos = content.match_.posEnd_;
                    AddConstructorForLocalClass(ref blockSrc, className);
                }
                blockSrc += context_.src_.Substring(begPos, semBlock.match_.posEnd_ - begPos);
                termination = "";
                initialization = "";
                if (className.Length > 0)
                {
                    if (isLocal)
                    {
                        initialization = "var _sem= new " + className + "(this);\n";
                        if (destructorOrDisposableFound)
                        {
                            initialization = "using(var _sem= new " + className +"(this)){";
                            termination = "\n      }";
                        }
                    }
                    else
                    {
                        string objName = GetSingletonName(className);
                        blockSrc += className + " " + objName + ";\n";
                        initialization = GetSingletonName(className) + "= new " + className + "();\n";
                    }
                }
                return blockSrc;
            }
            private void AddParentParameterToConstructor(ref string blockSrc,ref int begPos,PegNode formalPars)
            {
                AddSource(ref blockSrc, ref begPos,formalPars.match_.posBeg_+1);
                blockSrc += parent_.moduleName_ + " parent";
                PegNode methodBody = formalPars.parent_.next_;
                AddSource(ref blockSrc, ref begPos, methodBody.match_.posBeg_ + 1);
                blockSrc+= "parent_= parent; ";
            }

            private void AddSource(ref string src,ref int startPos,int endPos)
            {
                src += context_.src_.Substring(startPos, endPos - startPos);
                startPos = endPos;
            }

            private bool FindDispose(PegNode content,out bool disposeFound)
            {
                bool destructorFound = false;
                disposeFound = false;
                for (PegNode member = content.child_; member != null; member = member.next_)
                {
                    switch ((EPegGrammar)member.id_)
                    {
                        case EPegGrammar.constructor_decl: destructorFound = true; break;
                        case EPegGrammar.func_declaration:
                            PegNode memberName = PUtils.FindNode(member, EPegGrammar.member_name);
                            if (memberName.GetAsString(context_.src_) == "Dispose") disposeFound = true;
                            break;
                    }
                }
                return disposeFound||destructorFound;
            }

            private string GetSingletonName(string className)
            {
                Debug.Assert(className.Length>1);
                if (className[0] == '_') return "_" + className.Substring(1, 1).ToLower() + className.Substring(2);
                else return className.Substring(0, 1).ToLower() + className.Substring(1);
            }

            private bool IsAccessedInLocalClass(PegNode memberName)
            {
                return context_.referencedMembers_.Contains(memberName.GetAsString(context_.src_));
            }

            private void AddConstructorForLocalClass(ref string blockSrc, string className)
            {
                string indentation = GetMinimumIndentation(blockSrc);
                string constructorTemplate =
@"internal $(CLASSNAME)($(MODULE_NAME) grammarClass){ parent_ = grammarClass; }
$(MODULE_NAME) parent_;
";
                parent_.ReplaceMacro(ref constructorTemplate, "$(CLASSNAME)", className);
                parent_.ReplaceMacro(ref constructorTemplate, "$(MODULE_NAME)", parent_.moduleName_);
                constructorTemplate = indentation + constructorTemplate;
                constructorTemplate = constructorTemplate.Replace("\n", "\n" + indentation);
                blockSrc += constructorTemplate;
            }

            private string GetMinimumIndentation(string blockSrc)//SEMBLOCK_INDENTATION
            {
                int minIndentation=blockSrc.Length;
                for (int pos = 0; pos < blockSrc.Length && (pos = blockSrc.IndexOf('\n', pos)) != -1; ++pos)
                {
                    int indent;
                    for (indent = 1; pos + indent < blockSrc.Length && Char.IsWhiteSpace(blockSrc[pos+indent]); )
                    {
                        if (blockSrc[pos + indent] == '\t')
                            indent += context_.generatorParams_.spacesPerTap_;
                        else
                            indent += 1;
                    }
                    if(--indent>0 && indent<minIndentation ) minIndentation=indent;
                }
                return new string(' ', minIndentation == blockSrc.Length ? 0 : minIndentation);
            }
            /// <summary>
            /// Determines whether pegNode is referenced from an into-variable or a called semantic function
            /// </summary>
            /// <param name="pegNode"></param>
            /// <returns></returns>
            private bool IsUsedMember(PegNode pegNode)
            {
                return context_.semanticInfoNodes_.Contains(pegNode);
            }
            /// <summary>
            /// Determines whether one of the children of pegNode is referenced from an into-variable or a called semantic function
            /// </summary>
            /// <param name="pegNode"></param>
            /// <returns></returns>
            private bool HasUsedMemberInChildren(PegNode pegNode)
            {
                if (pegNode == null) return false;
                for (PegNode p = pegNode.child_; p != null; p = p.next_)
                {
                    if (IsUsedMember(p)||HasUsedMemberInChildren(p)) return true;
                }
                return false;
            }
            /// <summary>
            /// If a member of a top level class is used in this method body then qualify the access to this top level member
            /// </summary>
            /// <param name="pegNode"></param>
            /// <param name="blockSrc"></param>
            /// <param name="begPos"></param>
            private void QualifyTopLevelMemberAccessInMethodBody(PegNode pegNode, ref string blockSrc, ref int begPos)
            {
                if (pegNode == null) return;
                if (pegNode.id_ == (int)EPegGrammar.designator)
                {
                    string desigIdent = pegNode.child_.GetAsString(context_.src_);
                    if (context_.dictSemanticInfo_.ContainsKey(desigIdent))
                    {
                        var semNode = context_.dictSemanticInfo_[desigIdent];
                        string qualification = null;
                        if (semNode.id_ == (int)EPegGrammar.named_semantic_block)
                        {
                            if( semNode.child_.match_.GetAsString(context_.src_).Equals("CREATE") ){
                                qualification= "parent_.";
                            }else{
                                string className = semNode.child_.GetAsString(context_.src_);
                                qualification= "parent_." + className.Substring(0,1).ToLower() + className.Substring(1) + ".";
                            }
                        }
                        else if (semNode.id_ == (int)EPegGrammar.anonymous_semantic_block)
                        {
                            qualification= "parent_._top.";
                        }
                        if(qualification!=null )
                        {
                            blockSrc += context_.src_.Substring(begPos, pegNode.child_.match_.posBeg_ - begPos);
                            blockSrc += qualification;
                            begPos= pegNode.child_.match_.posBeg_;
                        }
                         
                    }
                }
                QualifyTopLevelMemberAccessInMethodBody(pegNode.child_, ref blockSrc, ref begPos);
                QualifyTopLevelMemberAccessInMethodBody(pegNode.next_, ref blockSrc, ref begPos);
            }

            private void AddInternalModifier(ref string blockSrc, ref int begPos, PegNode member)
            {
                blockSrc += context_.src_.Substring(begPos, member.match_.posBeg_ - begPos);
                blockSrc += "internal ";
                begPos = member.match_.posBeg_;
            }
            private void AddPublicModifier(ref string blockSrc, ref int begPos, PegNode member)
            {
                blockSrc += context_.src_.Substring(begPos, member.match_.posBeg_ - begPos);
                blockSrc += "public ";
                begPos = member.match_.posBeg_;
            }
            private bool HasUsedMember(PegNode node)
            {
                return true;
            }

            private bool AccessModifierPresent(PegNode pegNode)
            {
                if (pegNode == null ) return false;
                for( 
                    ;pegNode.id_==(int)EPegGrammar.field_modifier
                    || pegNode.id_==(int)EPegGrammar.method_modifier;
                    pegNode= pegNode.next_)
                {
                    string s= pegNode.GetAsString(context_.src_);
                    s= s.Trim();
                    switch(s)
                    {
                        case "protected":
                        case "private":
                        case "internal":
                        case "public": return true;
                    }
                }
                return false;
            }
            string GenSemanticBlockInfo(string indent,out string initialization)
            {
                PegNode blockInfo = PUtils.FindNode(context_.root_, EPegGrammar.toplevel_semantic_blocks);
                string srcCode = "";
                initialization="";
                string termination="";
                if (blockInfo!=null )
                {
                    for (PegNode semanticBlock = blockInfo.child_; semanticBlock != null; semanticBlock = semanticBlock.next_)
                    {
                        if (semanticBlock.id_ == (int)EPegGrammar.anonymous_semantic_block)
                        {
                            string blockSrcCode= "class _Top";
                            string blockInitialization;
                            blockSrcCode += GenSemanticBlockContent(semanticBlock,"_Top",false,
                                                                        out blockInitialization, out termination);
                            initialization += blockInitialization + "\n";
                            srcCode = blockSrcCode.Replace("\n", "\n" + indent);
                        }
                        else if (semanticBlock.id_ == (int)EPegGrammar.named_semantic_block)
                        {
                            Debug.Assert(semanticBlock.child_ != null && semanticBlock.child_.id_ == (int)EPegGrammar.sem_block_name);
                            string className = semanticBlock.child_.GetAsString(context_.src_);
                            if( semanticBlock.child_.match_.GetAsString(context_.src_).Equals("CREATE") ){
                               srcCode+= "#region CREATE\n";
                               string blockInitialization;
                               srcCode += GenSemanticBlockContent(semanticBlock.child_.next_.child_,"",false,
                                                                                    out blockInitialization, out termination);
                               srcCode+= "#endregion CREATE\n";
                            }else if( IsUsedAsLocalBlock(className) ){
                               string blockCode= "class ";
                               string blockInitialization;
                               blockCode+= GenSemanticBlockContent(semanticBlock, className, true,
                                                        out blockInitialization, out termination);
                               dictUsing_.Add(className, new InitializationTermination { initialization = blockInitialization, termination = termination });
                               srcCode+= blockCode.Replace("\n", "\n" + indent);
                            }else{
                               string blockCode= "class ";
                               blockCode+= GenSemanticBlockContent(semanticBlock, className, false,
                                                        out initialization,out termination);
                               srcCode+= blockCode.Replace("\n", "\n" + indent);
                            }
                        }
                    }
                }
                return srcCode;
            }

            private bool IsUsedAsLocalBlock(string className)
            {
                for (PegNode rule = PUtils.GetRuleFromRoot(context_.root_); rule != null; rule = rule.next_)
                {
                    PegNode using_block = PUtils.FindNode(rule.child_, EPegGrammar.sem_block_name);
                    if (using_block != null && using_block.GetAsString(context_.src_) == className) return true;
                }
                return false;
            }
            void GenCodeForModuleHeadCSharp()
            { 
                string moduleHead = parent_.FindCSharpTemplateCode(ECSharpKind.ModuleHead);
                string encoding_class, encoding_detection;
                GetEncoding(out encoding_class,out encoding_detection);
                parent_.ReplaceMacro(ref moduleHead, "$(ENCODING_CLASS)", encoding_class);
                parent_.ReplaceMacro(ref moduleHead, "$(UNICODE_DETECTION)", encoding_detection);
                parent_.ReplaceMacro(ref moduleHead, "$(MODULE_NAME)", parent_.moduleName_);
                string enumerators = GenEnumeratorDefinition();
                parent_.ReplaceMacro(ref moduleHead, "$(ENUMERATOR)", enumerators,true);
                string parserName = IsGrammarForBinaryInput() ? "PegByteParser" : "PegCharParser";
                parent_.ReplaceMacro(ref moduleHead, "$(PARSER)", parserName);
                string srcType = context_.IsGrammarForBinaryInput() ? "byte[]" : "string";
                parent_.ReplaceMacro(ref moduleHead, "$(SRC_TYPE)", srcType);
                string initialization;
                string semanticBlockInfo = GenSemanticBlockInfo("        ",out initialization);
                parent_.ReplaceMacro(ref moduleHead, "$(SEMANTIC_BLOCKS)", semanticBlockInfo);
                parent_.ReplaceMacro(ref moduleHead, "$(INITIALIZATION)", initialization);
                parent_.outFile_.Write(moduleHead);
            }
            void GenCodeForRulesCSharp()
            {
                parent_.outFile_.WriteLine("		#region Grammar Rules");
                PegNode firstRule = PUtils.GetRuleFromRoot(context_.root_);
                for (PegNode q = firstRule; q != null; q = q.next_)
                {
                   
                    GenCodeForRuleCSharp(q);
                }
                parent_.outFile_.WriteLine("		#endregion Grammar Rules");
            }
            string GetOriginalRuleString(PegNode rule)
            {
                string s = rule.GetAsString(context_.src_);
                return s.Trim().Replace("*/", "* /");
            }
            void GenLocalSemanticBlock(string sRuleName, PegNode semBlock, out string initialization, out string termination)
            {
                if (semBlock.id_ == (int)EPegGrammar.anonymous_semantic_block)
                {
                    string className= "_" + sRuleName;
                    string srcCode = "class " + className;
                    srcCode += GenSemanticBlockContent(semBlock,className,true,out initialization,out termination);
                    srcCode = srcCode.Replace("\n", "\n   ");
                    parent_.outFile_.WriteLine("   {0}", srcCode);
                    
                }
                else if (semBlock.id_ == (int)EPegGrammar.named_semantic_block)
                {
                    string srcCode = "class ";
                    string className = semBlock.child_.GetAsString(context_.src_);
                    string blockCode = GenSemanticBlockContent(semBlock, className,true,out initialization,out termination);
                    srcCode += blockCode;
                    srcCode = srcCode.Replace("\n", "\n   ");
                    parent_.outFile_.WriteLine("   {0}", srcCode);
                }
                else
                {
                    Debug.Assert(false);
                    initialization = "";
                    termination = "";
                }
            }
            string GetRuleParams(PegNode rule)
            {
                string paramCode = "";
                PegNode @params = PUtils.FindNode(rule.child_.child_, EPegGrammar.peg_params);
                if (@params != null)
                {
                    for (PegNode param = @params.child_; param != null; param = param.next_)
                    {
                        if (paramCode != "") paramCode += ", ";
                        paramCode += "Matcher " + parent_.GetCSharpPrefixed(param.GetAsString(context_.src_));
                    }
                }
                return paramCode;
            }
            void GenCodeForRuleCSharp(PegNode rule)
            {
                PegNode ruleIdent = PUtils.FindNode(rule.child_, EPegGrammar.rule_name);
                string sRuleFunc = parent_.GetCSharpPrefixed(ruleIdent.GetAsString(context_.src_));
                PegNode sem_block = PUtils.FindNode(rule.child_, EPegGrammar.named_semantic_block,EPegGrammar.anonymous_semantic_block);
                string initialization="";
                string termination="";
                if (sem_block != null) GenLocalSemanticBlock(sRuleFunc, sem_block, out initialization, out termination);
                else
                {
                    PegNode using_block = PUtils.FindNode(rule.child_, EPegGrammar.sem_block_name);
                    if (using_block != null)
                    {
                        string name = using_block.GetAsString(context_.src_);
                        GenLocalBlockInitializationAndTermination(name,out initialization, out termination);
                        if (initialization == "")
                        {
                            FatalErrorOut("FATAL from <PEG_GENERATOR>: using class "+name+"; '"+name+"' not found");
                        }
                    }
                }
                string originalRuleString = GetOriginalRuleString(rule);
                string ruleParams = GetRuleParams(rule);
                parent_.outFile_.WriteLine("        public bool {0}({1})    /*{2}*/\n        {{\n",
                    sRuleFunc,
                    ruleParams,
                    originalRuleString);
                if (initialization != "")
                {
                    parent_.outFile_.WriteLine("             {0}", initialization);
                }
                Template templNode = (new TemplateGenerator(context_)).GenTemplateForRule(rule);
                (new CodeFromTemplate(parent_, context_)).GenMatchCodeForRuleCSharp(templNode);
                if (termination != "")
                {
                    parent_.outFile_.WriteLine("            {0}", termination);
                }
                parent_.outFile_.WriteLine("\n\t\t}");
            }

            private void FatalErrorOut(string p)
            {
                context_.generatorParams_.errOut_.WriteLine(p);
                if (parent_.outFile_ != null) parent_.outFile_.WriteLine(p);
            }

            private void GenLocalBlockInitializationAndTermination(string className,out string initialization, out string termination)
            {//retrieve initialization and termination from map
                if(dictUsing_.ContainsKey(className))
                {
                    initialization= dictUsing_[className].initialization;
                    termination= dictUsing_[className].termination;
                }else{
                    initialization="";
                    termination="";
                }
            }
            void GenCodeForModuleEndCSharp()
            {
                string moduleTrailer = parent_.FindCSharpTemplateCode(ECSharpKind.ModuleTail);
                parent_.outFile_.Write(moduleTrailer);
            }
            void GenCodeForOptimizations()
            {
                if (parent_.optimizationStaticConstructor_.Length > 0)
                {
                    parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,"$(OPTIMIZEDCHARSET_DECL)","");
                    parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,"$(OPTIMIZEDLITERALS_DECL)","");
                    parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,"$(OPTIMIZEDCHARSET_IMPL)","");
                    parent_.ReplaceMacro(parent_.optimizationStaticConstructor_,"$(OPTIMIZEDLITERALS_IMPL)","");
                    parent_.outFile_.Write(parent_.optimizationStaticConstructor_);
                }
            }
            #endregion Private Functions
        }
        #endregion Code Generator Classes
        #region Constructors
        internal PegCSharpGenerator(TreeContext context)
        {
            context_=   context;
            literalsCount_ = 0;
            optimizedCharsetCount_ = 0;
            optimizationStaticConstructor_ = new StringBuilder();
            moduleName_= context.GetModuleName();
            if (moduleName_.Length == 0)
            {
                context_.generatorParams_.errOut_.WriteLine("FATAL from <PEG_GENERATOR>: grammarName in <<Grammar Name=\"<grammarName>\" ..>> missing");
                return;
            }
            else if (!IsCSharpIdentifier(moduleName_))
            {
                context_.generatorParams_.errOut_.WriteLine("FATAL from <PEG_GENERATOR>: {0} in <<Grammar Name=\"{1}\" ..>> is not a correct identifier", moduleName_,moduleName_);
                return;
            }
            (new TopLevelCode(this, context_)).GenCodeCSharpForThisModule();
        }
        #endregion Constructors
        #region Helper functions
        bool IsCSharpIdentifier(string name)
        {
            Regex regex = new Regex("^[A-Za-z_][A-Za-z_0-9]*$");
            return regex.Match(name).Success;
        }
        string FindCSharpTemplateCode(ECSharpKind eKind)
		{
            for (int i = 0; i < templates.Length; ++i)
            {
                if (templates[i].eKind == eKind) return templates[i].sCodeTemplate;
			}
            Debug.Assert(false);
			return "";
		}
        void ReplaceMacro(ref string s, string macro, string replacement, bool doAlignement)
        {
            int i= s.IndexOf(macro);
            if (i == -1) return;
            int lineBreak= s.Substring(0, i).LastIndexOf('\n');
            if (lineBreak == -1) lineBreak = 0;
            string align = new string(' ', i-lineBreak);
            align=  "\n" + align;
            replacement= replacement.Replace("\n", align);
            s= s.Replace(macro,replacement);
        }
        void ReplaceMacro(ref string s, string macro, string replacement)
		{
            s= s.Replace(macro, replacement);
		}
        void ReplaceMacro(StringBuilder s, string macro, string replacement)
        {
            s.Replace(macro, replacement);
        }
        string GetCSharpPrefixed(string s)
		{
			string[] keywords={
				"abstract",
				"as",
				"base",
				"bool",
				"break",
				"byte",
				"case",
				"catch",
				"char",
				"checked",
				"class",
				"const",
				"continue",
				"decimal",
				"default",
				"delegate",
				"do",
				"double",
				"else",
				"enum",
				"event",
				"explicit",
				"extern",
				"false",
				"finally",
				"fixed",
				"float",
				"for",
				"foreach",
				"goto",
				"if",
				"implicit",
				"in",
				"int",
				"interface",
				"internal",
				"is",
				"lock",
				"long",
				"namespace",
				"new",
				"null",
				"object",
				"operator",
				"out",
				"override",
				"params",
				"private",
				"protected",
				"public",
				"readonly",
				"ref",
				"return",
				"sbyte",
				"sealed",
				"short",
				"sizeof",
				"stackalloc",
				"static",
				"string",
				"struct",
				"switch",
				"this",
				"throw",
				"true",
				"try",
				"typeof",
				"uint",
				"ulong",
				"unchecked",
				"unsafe",
				"ushort",
				"using",
				"virtual",
				"void",
				"volatile",
				"while"
            };
				for(int i=0;i<keywords.Length;++i){
					if( keywords[i]==s ) return "@"+s;
				}
				return s;
        }
        #endregion Helper functions
    }   
}