/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 *  20080930;FIND_IN_SEMANTIC_BLOCK;corrected search in global semantic blocks: helper function <<PUtils.FindNode(<startnode>,int params[] ids)>> did not return closest node
 *  20081002;USING_BLOCK;added support for using in rules like'[9]   parenth_form_content  using Line_join_sem_: ...'
 * <</History>>
*/
using System;
using Peg.Base;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Peg.Samples
{
    enum EPegGeneratorNodes{FatalNode=1001,WarningNode=1002,EnumeratorNode=1003,
                                IntoVarWithContext=500,SemanticFunctionWithContext=501,
                                GenericCall=600};
    class PUtils
    {
        public static void Trim(StringBuilder s)
        {
            string sTrimChars = " \t\r\v\n";
            int i, j;
            for (i = 0; i < s.Length; ++i)
            {
                if (sTrimChars.IndexOf(s[i]) == -1) break;
            }
            for (j = s.Length; j > i; j--)
            {
                if (sTrimChars.IndexOf(s[j - 1]) == -1) break;
            }
            s.Remove(j, s.Length - j);
            s.Remove(0, i);
        }
        public static PegNode GetByPath(PegNode node, params int[] path)
        {
            for (int i = 0; i < path.Length; ++i)
            {
                if (node == null || node.id_ != path[i]) return null;
                if (i == path.Length - 1) return node; else node = node.child_;
            }
            return node;
        }
        
        public static PegNode FindNode(PegNode node, EPegGrammar id, int nodeDistance)
        {
            if (node == null || nodeDistance<=0) return null;
            if (node.id_ == (int)id) return node;
            PegNode foundNode = FindNode(node.child_, id, nodeDistance - 1);
            if (foundNode!=null) return foundNode;
            foundNode = FindNode(node.next_, id, nodeDistance - 1);
            if (foundNode != null) return foundNode;
            return null;
        }
        public static PegNode FindNode(PegNode node, EPegGrammar id)
        {
            return FindNode(node, id, 8);
        }
        public static PegNode FindNode(PegNode node, int nodeDistance, params EPegGrammar[] ids)/*FIND_IN_SEMANTIC_BLOCK*/
        {
            if (node == null || nodeDistance<=0) return null;
            foreach (EPegGrammar id in ids)
            {
                if (node.id_ == (int)id) return node;
            }
            PegNode foundNode = FindNode(node.child_, nodeDistance - 1,ids );
            if (foundNode!=null) return foundNode;
            foundNode = FindNode(node.next_, nodeDistance - 1,ids );
            if (foundNode != null) return foundNode;
            return null;
        }
        public static PegNode FindNode(PegNode node, params EPegGrammar[] ids)/*FIND_IN_SEMANTIC_BLOCK*/
        {
            return FindNode(node, 8,ids );
        }
        public static PegNode FindNodeInParents(PegNode node, EPegGrammar id)
        {
            for(; node!=null;node=node.parent_)
            {
                if( node.id_ == (int) id ) return node;
            }
            return null;
        }
        public static PegNode FindNodeInParents(PegNode node, params EPegGrammar[] ids)
        {
            foreach (EPegGrammar id in ids)
            {
                PegNode foundNode = FindNodeInParents(node, id);
                if (foundNode != null) return foundNode;
            }
            return null;
        }
        public static PegNode FindNodeNext(PegNode node, EPegGrammar id)
        {
            for (node = node.next_; node != null; node = node.next_)
            {
                if (node.id_ == (int)id) return node;
            }
            return null;
        }
        public static PegNode GetPegSpecification(PegNode root)
        {
            return root.child_.next_;
        }
        public static PegNode GetRuleFromRoot(PegNode root)
        {
            PegNode s= GetPegSpecification(root);
            return s.child_.next_.child_;
        }
       
        public static PegNode GetRuleId(PegNode rule, bool bMustExist)
        {
            Debug.Assert(		rule!=null
			&&	rule.id_== (int)EPegGrammar.peg_rule
            && rule.child_ != null && rule.child_.id_ == (int)EPegGrammar.lhs_of_rule);
            PegNode ruleId= rule.child_.child_;
	        if( bMustExist ){
                Debug.Assert(ruleId != null && ruleId.id_ == (int)EPegGrammar.rule_id); //either user defined or generated in preprocessing
                return ruleId;
	        }else{
                if (ruleId != null && ruleId.id_ == (int)EPegGrammar.rule_id)
                {
                    return ruleId;
		        }
		        return null;
	        }
        }
        public static string GetRuleNameFromRule(PegNode rule, string src)
        {
            PegNode ruleIdent = PUtils.FindNode(rule, EPegGrammar.rule_name, 8);
            string ruleName = ruleIdent.match_.GetAsString(src);
            return ruleName;
        }
        public static string GetRuleNameFromRuleRef(PegNode ruleRef,string src)
        {
            Debug.Assert(ruleRef.id_== (int)EPegGrammar.rule_ref );
            return ruleRef.match_.GetAsString(src);
        }
        public static string GetAsString(string src,PegNode n)
        {
            return n.match_.GetAsString(src);
        }
        public static void ReplaceNode( PegNode  toReplace, PegNode replacement)
        {	
            Debug.Assert(replacement.next_==null);
	        replacement.next_= toReplace.next_;
	        replacement.parent_= toReplace.parent_;
	        if( toReplace.parent_.child_==toReplace ){
		        toReplace.parent_.child_= replacement;
		        replacement.parent_= toReplace.parent_;
	        }else{
		        PegNode n;
		        for(n= toReplace.parent_.child_;
			        n.next_!=toReplace;
			        n= n.next_){
			    }
		        n.next_= replacement;
		        replacement.parent_= toReplace.parent_;
	        }
        }
        public static bool IsTemplateCall(PegNode ruleRef)
        {
	        PegNode n;
	        if( ruleRef.id_== (int)EPegGrammar.rule_ref){
                n= ruleRef.child_;
		        if( n!=null || n.id_!= (int) EPegGrammar.rule_name) return false;
		        return n.next_!=null && n.next_.id_== (int)EPegGrammar.rhs_of_rule;
	        }else return false;
        }
        public static string MakeFileName(string sFileTitle, params string[] directories)
        {
            string path="";
            foreach (string dir in directories)
            {
                path += dir;
                if (dir.Length>0 && dir[dir.Length - 1] != '\\') path += '\\';
            }
            return path + sFileTitle;
        }
        public static bool TreeOrAstPresent(PegNode n,out bool bIsTree,out bool bIsAst)
        {
	        bIsTree=false; bIsAst=false;
	        for( ;n!=null;n=n.next_){
		        switch(n.id_){
		        case (int)EPegGrammar.tree_symbol: bIsTree=true;return true;
                case (int)EPegGrammar.ast_symbol: bIsAst = true; return true;
		        default:break;
		        }
	        }
            return false;
        }
        public static PegNode GetRhs(PegNode rule)
        {	
            Debug.Assert( rule.id_== (int)EPegGrammar.peg_rule); 
	        PegNode n= rule.child_.next_;
	        Debug.Assert(n==null || n.id_== (int)EPegGrammar.rhs_of_rule);
	        return n;
        }
    }
    class NormalizeTree
    {
        #region Data Types
        delegate void NormalizeInRule(PegNode n);
        delegate bool RuleIsCandidate(PegNode rule);
        #endregion
        #region Data Members
        internal TreeContext c_;
        internal bool bOk_;
        HashSet<string> setRules_;
        #endregion Data Members
        #region Constructors
        internal NormalizeTree(TreeContext c, HashSet<string> setRules)
        {
            c_ = c;
            bOk_ = true;
            setRules_ = setRules;
            PegNode rule = PUtils.GetRuleFromRoot(c_.root_);
            PegNode specification = PUtils.GetPegSpecification(c_.root_);
            TranslateMandatoryToFatal(rule);
            InsertMissingRuleIds(rule);
            new TryFuseCharsets(this, rule);
            LinkIntoVariablesToSemanticBlocks(rule);
            new GenericParameters(this, rule);
            new SemanticFunctions(this, rule);
            //InsertMissingEnumerations(rule,specification);
        }
        #endregion
        PegNode NewNode(
                            EPegGrammar id,
                            PegBegEnd match,
                            PegNode child,
                            PegNode next)
        {
            PegNode n = new PegNode(null, (int)id);
            n.match_ = match;
            n.child_ = child;
            n.next_ = next;
            for (PegNode node = n.child_; node != null; node = node.next_) node.parent_ = n;
            return n;
        }
        PegNode NewNode(
                            EPegGrammar id,
                            PegBegEnd match)
        {
            return NewNode(id, match, null, null);
        }
        PegNode NewNode(
                            EPegGrammar id,
                            PegBegEnd match,
                            PegNode child)
        {
            return NewNode(id, match, child, null);
        }
        public class IdNode : PegGrammarParser.PGParserNode
        {
            internal IdNode(PegNode parent, int id, int ownId, PegBegEnd match, PegNode next)
                : base(parent, id)
            {
                ownId_ = ownId;
                base.match_ = match;
                next_ = next;
            }
            internal override string TreeNodeToString(string src)
            {
                return base.ToString() + ": " + ownId_.ToString();
            }
            public override string GetAsString(string s)
            {
                return ownId_.ToString();
            }
            public override PegNode Clone()
            {
                IdNode clone = new IdNode(parent_, id_, ownId_, match_, null);
                CloneSubTrees(clone);
                return clone;
            }
            public int ownId_;
        }
        public class Message : PegGrammarParser.PGParserNode
        {
            internal Message(PegNode parent, int id, string message, PegBegEnd match)
                : base(parent, id)
            {
                message_ = message;
                base.match_ = match;
            }
            internal override string TreeNodeToString(string src)
            {
                return message_;
            }
            public override PegNode Clone()
            {
                Message clone = new Message(parent_, id_, message_, match_);
                CloneSubTrees(clone);
                return clone;
            }
            public string message_;
        }
        public class TRepetition : PegGrammarParser.PGParserNode
        {
            internal TRepetition(int nLowerLimit, int nUpperLimit, EPegGrammar id, PegBegEnd begEnd)
                : base(null, (int)id)
            {
                base.match_ = begEnd;
                lower = nLowerLimit;
                upper = nUpperLimit;
            }
            internal override string TreeNodeToString(string src)
            {
                return "{" + lower.ToString() + ", " + upper.ToString() + "}";
            }
            public override PegNode Clone()
            {
                TRepetition clone = new TRepetition(lower, upper, (EPegGrammar)id_, match_);
                CloneSubTrees(clone);
                return clone;
            }
            public int lower;
            public int upper;
        }
        public class SemanticVarOrFuncWithContext : PegGrammarParser.PGParserNode
        {
            public SemanticVarOrFuncWithContext(int id, PegNode semBlock, PegNode variableOrFunc, PegNode intoVariableOrCall, bool isLocal)
                : base(intoVariableOrCall.parent_, id)
            {
                semBlock_ = semBlock;
                variableOrFunc_ = variableOrFunc;
                intoVariableOrCall_ = intoVariableOrCall;
                isLocal_ = isLocal;
            }
            internal override string TreeNodeToString(string src)
            {
                if (semBlock_.id_ == (int)EPegGrammar.named_semantic_block)
                {
                    return semBlock_.child_.GetAsString(src) + "::" + intoVariableOrCall_.GetAsString(src);
                }
                else
                {
                    Debug.Assert(semBlock_.id_ == (int)EPegGrammar.anonymous_semantic_block);
                    if (semBlock_.parent_.id_ == (int)EPegGrammar.toplevel_semantic_blocks)
                    {
                        return "::" + intoVariableOrCall_.GetAsString(src);
                    }
                    else
                    {
                        return "sem." + intoVariableOrCall_.GetAsString(src);
                    }
                }
            }
            public override PegNode Clone()
            {
                SemanticVarOrFuncWithContext clone = new SemanticVarOrFuncWithContext(id_, semBlock_, variableOrFunc_, intoVariableOrCall_, isLocal_);
                CloneSubTrees(clone);
                return clone;
            }
            public PegNode semBlock_;
            public PegNode variableOrFunc_;
            public PegNode intoVariableOrCall_;
            public bool isLocal_;
        }
        public class GenericCall : PegGrammarParser.PGParserNode
        {
            public GenericCall(PegNode genericParam, PegNode genericCall)
                : base(genericCall.parent_, (int)EPegGeneratorNodes.GenericCall)
            {
                match_ = genericCall.match_;
                genericParam_ = genericParam;
                genericCall_ = genericCall;
            }
            internal override string TreeNodeToString(string src)
            {
                return genericCall_.GetAsString(src);
            }
            public override string GetAsString(string s)
            {
                return genericCall_.GetAsString(s);
            }

            public override PegNode Clone()
            {
                GenericCall clone = new GenericCall(genericParam_, genericCall_);
                CloneSubTrees(clone);
                return clone;
            }
            public PegNode genericParam_, genericCall_;
        }
        #region MandatoryToFatal: translate @<item>  to (item/FATAL("item expected"))
        PegNode NewFatal(string sMsg, PegBegEnd match)
        {
            return new Message(null, (int)EPegGeneratorNodes.FatalNode, sMsg, match);
        }

        void RemoveComments(StringBuilder s)
        {
            string sContent = s.ToString();
            int ind = sContent.IndexOf("//");
            if (ind >= 0)
            {
                int ind1 = sContent.IndexOf('\n', ind);
                if (ind1 == -1) ind1 = sContent.Length - 1;
                s.Remove(ind, ind1 + 1 - ind);
                RemoveComments(s);
            }
        }
        void TrimInnerWhiteSpace(StringBuilder s)
        {
            RemoveComments(s);

            for (int i = s.Length; i > 0; --i)
            {
                if (Char.IsWhiteSpace(s[i - 1]) && i > 2)
                {
                    int end = i;
                    for (--i; i > 1; --i)
                    {
                        if (!Char.IsWhiteSpace(s[i - 1])) break;
                    }
                    if (end - i > 2)
                    {
                        s.Remove(i, end - i - 1);
                    }
                }
            }
        }
        string ExtractFatalMsg(PegNode n)
        {
            string s = n.match_.GetAsString(c_.src_);
            StringBuilder sb = new StringBuilder(s);
            PUtils.Trim(sb);
            TrimInnerWhiteSpace(sb);
            sb.Replace("\r\n", " ");
            sb.Replace("\n", " ");
            sb.Replace("\t", " ");

            sb.Replace("/", " or ");
            sb.Replace("&", "");

            return "<<" + sb.ToString() + ">> expected";
        }
        void RemoveSubTree(PegNode n)
        {
            Debug.Assert(n != null && n.parent_ != null);
            if (n.parent_.child_ == n)
            {
                n.parent_.child_ = n.next_;
            }
            else
            {
                Debug.Assert(n.parent_.child_ != null);
                PegNode cur;
                for (cur = n.parent_.child_; cur.next_ != n; cur = cur.next_)
                {
                    Debug.Assert(cur.next_ != null);
                }
                Debug.Assert(cur.next_ == n);
                cur.next_ = cur.next_.next_;

            }
        }
        void MandatoryToFatal(PegNode n)
        {	// example: <<@"}" >> is translated to << ("}"/Fatal<msg>) S>>
            //  corresponds to the tree transformation 
            //  (term atom_prefix(mandatory_symbol) atom) => term  atom(rhs(choice(term(atom("{"))) choice(term(atom(FATAL(" ..."))))
            Debug.Assert(n.id_ == (int)EPegGrammar.mandatory_symbol && n.parent_ != null && n.parent_.next_ != null && n.parent_.parent_ != null);
            string msg = ExtractFatalMsg(n.parent_.next_);
            PegNode term = n.parent_.parent_;
            PegNode atom = n.parent_.next_;
            PegBegEnd match = atom.match_;
            PegNode nodeFatalRoot =
                NewNode(EPegGrammar.atom, match,
                    NewNode(
                        EPegGrammar.rhs_of_rule,
                        match,
                        NewNode(EPegGrammar.choice,
                                    match,
                                    NewNode(EPegGrammar.term, match, atom),
                                    NewNode(EPegGrammar.choice, match,
                                        NewNode(EPegGrammar.term, match,
                                            NewNode(EPegGrammar.atom, match, NewFatal(msg, match))))
                                )
                        ));
            RemoveSubTree(n.parent_);
            term.child_ = nodeFatalRoot;
            nodeFatalRoot.parent_ = term;
        }
        void NormalizeMandatory(PegNode n)
        {
            if (n == null) return;
            switch (n.id_)
            {
                case (int)EPegGrammar.mandatory_symbol:
                    MandatoryToFatal(n);
                    break;
                default:
                    NormalizeMandatory(n.next_);
                    NormalizeMandatory(n.child_);
                    break;
            }
        }
        #endregion MandatoryToFatal
        string ExtractMsg(PegNode dblQuoteNode)
        {
            StringBuilder s = new StringBuilder();
            Debug.Assert(dblQuoteNode.id_ == (int)EPegGrammar.double_quote_literal);
            for (; dblQuoteNode != null; dblQuoteNode = dblQuoteNode.next_)
            {
                Debug.Assert(dblQuoteNode.match_.Length >= 2);
                s.Append(c_.src_.Substring(dblQuoteNode.match_.posBeg_ + 1, dblQuoteNode.match_.Length - 2));
            }
            return s.ToString();
        }
        string BuildEnumeratorFromDesc(string sMsg, int n)
        {
            StringBuilder s = new StringBuilder("e_");
            bool bFirstWhite = false;
            for (int i = 0; i < 30 && i < sMsg.Length; ++i)
            {
                if (char.IsLetterOrDigit(sMsg[i]))
                {
                    bFirstWhite = true;
                    s.Append(sMsg[i]);
                }
                else
                {
                    if (bFirstWhite)
                    {
                        bFirstWhite = false;
                        s.Append('_');
                    }
                }
            }
            if (sMsg.Length > 30)
            {
                s.Append("___");
            }
            s.Append(n.ToString());
            return s.ToString();
        }
        void TryInsertMissingEnumerationDesc(PegNode specification, PegNode n)
        {//creates an enumeration node for each fatal message
            Debug.Assert(specification.child_ != null
                            && specification.child_.id_ == (int)EPegGrammar.toplevel_semantic_blocks
                            && specification.child_.next_ != null
                            && specification.child_.next_.id_ == (int)EPegGrammar.peg_rule
                            && n != null
                            && n.id_ == (int)EPegGrammar.multiline_double_quote_literal);
            PegNode enumerationsNode = specification.child_.next_.next_;
            if (enumerationsNode == null)
            {
                enumerationsNode = specification.child_.next_.next_ = NewNode(EPegGrammar.enumeration_definitions, new PegBegEnd());
                enumerationsNode.parent_ = specification;
            }
            PegNode enumNode = enumerationsNode.child_;
            string sFatalMsg = n.child_.GetAsString(c_.src_);
            int i;
            for (i = 0; enumNode != null; enumNode = enumNode.next_, ++i)
            {
                Debug.Assert((enumNode != null) && enumNode.child_ != null && enumNode.child_.next_ != null && enumNode.child_.next_.id_ == (int)EPegGrammar.multiline_double_quote_literal);
                string s = enumNode.child_.next_.child_.GetAsString(c_.src_);
                if (s == sFatalMsg) return;
            }
            string sEnumerator = BuildEnumeratorFromDesc(sFatalMsg, i);
            int len = sEnumerator.Length;
            string sMsg = sEnumerator;
            PegBegEnd match, match0;
            match.posBeg_ = 0; match.posEnd_ = len;
            match0.posBeg_ = match0.posEnd_ = 0;
            enumNode = NewNode(EPegGrammar.enumeration_definition, match0,
                                NewNode(EPegGrammar.enumerator, match, null,
                                    NewNode(EPegGrammar.multiline_double_quote_literal, n.child_.match_,
                                        NewNode(EPegGrammar.double_quote_literal, n.child_.match_))));
            enumNode.parent_ = enumerationsNode;
        }
        void TryInsertMissingEnumeration(PegNode specification, PegNode n)
        {
            Debug.Assert(specification.child_ != null
                        && specification.child_.id_ == (int)EPegGrammar.toplevel_semantic_blocks
                        && specification.child_.next_ != null &&
                        specification.child_.next_.id_ == (int)EPegGrammar.peg_rule);
            PegNode enumerationsNode = specification.child_.next_.next_;
            PegBegEnd match;
            match.posBeg_ = match.posEnd_ = 0;
            if (enumerationsNode == null)
            { /* no enumerations present */
                enumerationsNode = specification.child_.next_.next_ = NewNode(EPegGrammar.enumeration_definitions, match);
                enumerationsNode.parent_ = specification;
                specification.child_.next_.next_ = enumerationsNode;
            }
            PegNode enumNode = enumerationsNode.child_, lastEnumNode;
            for (lastEnumNode = enumNode; enumNode != null; lastEnumNode = enumNode, enumNode = enumNode.next_)
            {
                Debug.Assert(enumNode != null && enumNode.child_ != null && enumNode.child_.id_ == (int)EPegGrammar.enumerator);
                if (enumNode.child_.match_.GetAsString(c_.src_) == n.match_.GetAsString(c_.src_)) return;
            }
            int nSize = n.match_.posEnd_ - n.match_.posBeg_;
            string sMsg = n.match_.GetAsString(c_.src_);
            PegBegEnd match0;
            match.posBeg_ = 0; match.posEnd_ = sMsg.Length;
            match0.posBeg_ = match0.posEnd_ = 0;
            enumNode = NewNode(EPegGrammar.enumeration_definition, match0,
                                NewNode(EPegGrammar.enumerator, n.match_, null,
                                    new Message(null, (int)EPegGeneratorNodes.EnumeratorNode, sMsg, match)));
            enumNode.parent_ = enumerationsNode;
            if (lastEnumNode == null) enumerationsNode.child_ = enumNode; else lastEnumNode.next_ = enumNode;
        }
        void InsertMissingEnumeration(PegNode n, PegNode specification)
        {
            if (n == null) return;
            switch (n.id_)
            {
                case (int)EPegGrammar.enumeration_definition: return;
                case (int)EPegGrammar.enumerator:
                    {
                        if (n.parent_ != null && n.parent_.id_ == (int)EPegGrammar.message)
                        {
                            TryInsertMissingEnumeration(specification, n);
                        }
                    }
                    break;
                case (int)EPegGrammar.multiline_double_quote_literal:
                    {
                        if (n.parent_ != null && n.parent_.id_ == (int)EPegGrammar.message)
                        {
                            TryInsertMissingEnumerationDesc(specification, n);
                        }
                    }
                    break;
                default:
                    InsertMissingEnumeration(n.next_, specification);
                    InsertMissingEnumeration(n.child_, specification);
                    break;
            }
        }
        void InsertMissingEnumerations(PegNode rule, PegNode specification)
        {
            for (; rule != null; rule = rule.next_)
            {
                InsertMissingEnumeration(rule.child_.next_, specification);
            }
        }
        void TranslateMandatoryToFatal(PegNode rule)
        {
            for (; rule != null; rule = rule.next_)
            {
                NormalizeMandatory(rule.child_.next_);
            }
        }
        #region GenericNormalizer: traverses all the nodes and applies normalizer in case of match with one of idNodes
        void NormalizeEachRule(PegNode n, RuleIsCandidate isCandidate, NormalizeInRule normalizer, params int[] idNodes)
        {
            if (n == null) return;
            if (n.id_ == (int)EPegGrammar.peg_rule && (isCandidate != null && !isCandidate(n)))
            {
                NormalizeEachRule(n.next_, isCandidate, normalizer, idNodes);
                return;
            }
            else
            {
                foreach (int id in idNodes)
                {
                    if (id == n.id_)
                    {
                        normalizer(n);
                        break;
                    }
                }
            }
            NormalizeEachRule(n.child_, isCandidate, normalizer, idNodes);
            NormalizeEachRule(n.next_, isCandidate, normalizer, idNodes);
        }
        #endregion GenericNormalizer
        #region MissingRuleIds: Assign arbitrary rule id to rules without a user defined rule id
        int SearchGap(SortedList<int, int> setRuleIds, int nNofIdsRequired)
        {
            int nLastKey = 0;
            foreach (var elem in setRuleIds)
            {
                if (elem.Key - nLastKey > nNofIdsRequired + 1) return nLastKey + 1;
                nLastKey = elem.Key;
            }
            return nLastKey + 1;
        }
        void CollectRuleIds(PegNode rule, SortedList<int, int> setRuleIds, ref int nNofIdsRequired)
        {
            int idMax = -1;
            for (; rule != null; rule = rule.next_)
            {
                PegNode ruleIdNode = PUtils.GetRuleId(rule, false);
                if (ruleIdNode != null)
                {
                    int id = Int32.Parse(ruleIdNode.GetAsString(c_.src_));
                    if (setRuleIds.IndexOfKey(id) == -1)
                    {
                        setRuleIds.Add(id, id);
                    }
                    else
                    {
                        c_.errOut_.WriteLine(c_.sErrorPrefix + "WARNING: rule <{0}> has id <{1}> which is alread used by one of previous rules",
                            PUtils.GetRuleNameFromRule(rule, c_.src_), id);
                    }
                    if (id > idMax) idMax = id;
                }
                else nNofIdsRequired++;
            }
        }
        void InsertRuleId(PegNode rule, int nRuleId)
        {
            Debug.Assert(rule != null
                        && rule.child_ != null
                        && rule.child_.child_ != null
                        && rule.child_.child_.id_ != (int)EPegGrammar.rule_id);
            PegNode next = rule.child_.child_;
            rule.child_.child_ =
                new IdNode(rule.child_, (int)EPegGrammar.rule_id, nRuleId, rule.child_.child_.match_, next);
        }
        void InsertRuleIds(PegNode rule, int nGapStartId)
        {
            for (; rule != null; rule = rule.next_)
            {
                PegNode ruleId = PUtils.GetRuleId(rule, false);
                if (ruleId == null)
                {
                    InsertRuleId(rule, nGapStartId++);
                }
            }
        }
        void InsertMissingRuleIds(PegNode rule)
        {
            SortedList<int, int> setRuleIds = new SortedList<int, int>();
            int nNofIdsRequired = 0;
            CollectRuleIds(rule, setRuleIds, ref nNofIdsRequired);
            if (nNofIdsRequired == 0) return;
            int nGapStartId = SearchGap(setRuleIds, nNofIdsRequired);
            InsertRuleIds(rule, nGapStartId);
        }
        #endregion MissingRuleIds

        class TryFuseCharsets //fuse alternative charsets to one charset
        {
            internal TryFuseCharsets(NormalizeTree normalizeTree, PegNode rule)
            {
                normalizeTree_ = normalizeTree;
                FuseAlternativeCharsets(rule);
            }
            void FuseCharsets(PegNode first, PegNode end, List<PegNode> fuseList)
            {
                Debug.Assert(fuseList.Count > 0 && fuseList[0].child_ != null);
                PegNode fused = fuseList[0];
                PegNode last;
                for (last = fused.child_; last.next_ != null; last = last.next_)
                    ;
                for (int i = 1; i < fuseList.Count; ++i)
                {
                    last.next_ = fuseList[i].child_;
                    for (last = last.next_, last.parent_ = fused; last.next_ != null; last = last.next_)
                    {
                        last.parent_ = fused;
                    }

                }
                first.next_ = end;
            }
            void FuseCharsets(PegNode n)
            {
                Debug.Assert(n.id_ == (int)EPegGrammar.rhs_of_rule);
                PegNode first = null;
                List<PegNode> fuseList = new List<PegNode>();
                for (PegNode choice = n.child_; choice != null; choice = choice.next_)
                {
                    PegNode charsetNode =
                        PUtils.GetByPath(choice, (int)EPegGrammar.choice,
                                                (int)EPegGrammar.term,
                                                (int)EPegGrammar.atom,
                                                (int)EPegGrammar.character_set);
                    bool canBeFused =
                                charsetNode != null
                            && charsetNode.child_ != null
                            && charsetNode.child_.id_ != (int)EPegGrammar.set_negation
                            && charsetNode.parent_.next_==null //no quantor (e.g. * + {l,h})
                            && choice.child_.next_ == null;//term must not have sibling term
                    if (canBeFused)
                    {
                        if (first == null) first = choice;
                        fuseList.Add(charsetNode);
                    }
                    else
                    {
                        if (first != null)
                        {
                            FuseCharsets(first, choice, fuseList);
                            first = null;
                            fuseList = new List<PegNode>();
                        }

                    }
                }
                if (first != null) FuseCharsets(first, null, fuseList);
            }
            void FuseAlternativeCharsets(PegNode rule)
            {
                normalizeTree_.NormalizeEachRule(rule, null, FuseCharsets, (int)EPegGrammar.rhs_of_rule);
            }
            NormalizeTree normalizeTree_;
        }

        #region IntoVariable: search context
        bool FindIntoVariableInTree(PegNode n, string intoVariable, ref PegNode variable)
        {
            if (n == null) return false;
            if (n.id_ == (int)EPegGrammar.variable)
            {
                string varName = n.GetAsString(c_.src_);
                Debug.Assert(varName.Length > 0);
                if (
                    varName == intoVariable
                || varName[0] == '@' && varName.Substring(1) == intoVariable)
                {
                    variable = n; return true;
                }
            }
            return
                FindIntoVariableInTree(n.child_, intoVariable, ref variable)
            || FindIntoVariableInTree(n.next_, intoVariable, ref variable);
        }
        delegate bool FindSemanticVariableOrFunction(PegNode n, string variableOrFunction, ref PegNode varOrFunc);
        PegNode FindSemanticBlock(FindSemanticVariableOrFunction findVarOrFunc, string intoVarOrFunc, PegNode n, out PegNode varOrFunc, out bool isLocal)
        {
            varOrFunc = null;
            isLocal = true;
            #region Search in Local Scope
            PegNode rule = PUtils.FindNodeInParents(n, EPegGrammar.peg_rule);
            Debug.Assert(rule != null);
            PegNode semBlock = PUtils.FindNode(rule.child_, EPegGrammar.named_semantic_block, EPegGrammar.anonymous_semantic_block);
            if (semBlock != null && findVarOrFunc(semBlock.child_, intoVarOrFunc, ref varOrFunc)) return semBlock;
            #endregion Search in Local Scope
            isLocal = false;
            #region Search in grammar scope
            PegNode pegSpec = PUtils.FindNodeInParents(rule, EPegGrammar.peg_specification);
            Debug.Assert(pegSpec != null && pegSpec.child_.id_ == (int)EPegGrammar.toplevel_semantic_blocks);
            for (
                semBlock = PUtils.FindNode(pegSpec.child_.child_, EPegGrammar.named_semantic_block, EPegGrammar.anonymous_semantic_block);
                semBlock != null && (semBlock.id_ == (int)EPegGrammar.named_semantic_block || semBlock.id_ == (int)EPegGrammar.anonymous_semantic_block);
                semBlock = semBlock.next_)
            {
                if (findVarOrFunc(semBlock.child_, intoVarOrFunc, ref varOrFunc)) return semBlock;
            }
            #endregion Search in grammar scope
            return null;
        }
        void LinkIntoVariables(PegNode n)
        {
            Debug.Assert(n.id_ == (int)EPegGrammar.into_variable);
            string intoVariable = n.GetAsString(c_.src_);
            PegNode variable;
            bool isLocal;
            PegNode semBlock = FindSemanticBlock(FindIntoVariableInTree, intoVariable, n, out variable, out isLocal);
            if (semBlock != null)
            {
                var intoVarWithContext = new NormalizeTree.SemanticVarOrFuncWithContext((int)EPegGeneratorNodes.IntoVarWithContext, semBlock, variable, n, isLocal);
                c_.semanticInfoNodes_.Add(variable);
                PUtils.ReplaceNode(n, intoVarWithContext);
                Debug.Assert(intoVarWithContext.parent_ != null);
                if (intoVarWithContext.parent_.id_ == (int)EPegGrammar.lower_limit
                    && intoVarWithContext.parent_.parent_.id_ == (int)EPegGrammar.repetition_range)
                {
                    var r = intoVarWithContext.parent_.parent_ as PegGrammarParser.TRange;
                    Debug.Assert(r != null);
                    r.lowerIntoVar = intoVarWithContext;
                }
                else if (intoVarWithContext.parent_.id_ == (int)EPegGrammar.upper_limit
                        && intoVarWithContext.parent_.parent_.id_ == (int)EPegGrammar.repetition_range)
                {
                    var r = intoVarWithContext.parent_.parent_ as PegGrammarParser.TRange;
                    Debug.Assert(r != null);
                    r.upperIntoVar = intoVarWithContext;
                }
            }
            else
            {
                c_.errOut_.WriteLine(c_.sErrorPrefix + "ERROR: into variable <" + intoVariable + "> not found in semantic block candidates");
                bOk_ = false;
            }
        }
        void LinkIntoVariablesToSemanticBlocks(PegNode rule)
        {
            NormalizeEachRule(rule, null, LinkIntoVariables, (int)EPegGrammar.into_variable);
        }
        #endregion

        class GenericParameters
        {
            internal GenericParameters(NormalizeTree normalizeTree, PegNode rule)
            {
                normalizeTree_ = normalizeTree;
                genericParam_ = null;
                LinkToGenericParams(rule);
            }
            PegNode FindGenericParam(string refName)
            {
                Debug.Assert(genericParam_ != null);
                for (PegNode param = genericParam_.child_; param != null; param = param.next_)
                {
                    Debug.Assert(param.id_ == (int)EPegGrammar.rule_param);
                    if (param.GetAsString(normalizeTree_.c_.src_) == refName) return param;
                }
                return null;
            }
            void LinkToGenericParam(PegNode n)
            {
                Debug.Assert(n.id_ == (int)EPegGrammar.rule_ref);
                string refName = n.GetAsString(normalizeTree_.c_.src_);
                PegNode genericParam = FindGenericParam(refName);
                if (genericParam != null)
                {
                    PegNode genericCall = new NormalizeTree.GenericCall(genericParam, n);
                    PUtils.ReplaceNode(n, genericCall);
                }
            }
            bool IsGenericRule(PegNode rule)
            {
                PegNode param = PUtils.FindNode(rule.child_.child_, EPegGrammar.peg_params);
                if (param != null)
                {
                    genericParam_ = param;
                    return true;
                }
                return false;
            }
            void LinkToGenericParams(PegNode rule)
            {
                normalizeTree_.NormalizeEachRule(rule, IsGenericRule, LinkToGenericParam, (int)EPegGrammar.rule_ref);
            }
            PegNode genericParam_;
            NormalizeTree normalizeTree_;
        }
        class SemanticFunctions
        {
            internal SemanticFunctions(NormalizeTree normalizeTree, PegNode rule)
            {
                normalizeTree_ = normalizeTree;
                LinkSemanticFunctionsToSemanticBlocks(rule);

            }
            bool FindSemanticFunctionInTree(PegNode n, string semanticFunction, ref PegNode memberName)
            {
                if (n == null) return false;
                if (n.id_ == (int)EPegGrammar.member_name && n.parent_.id_ == (int)EPegGrammar.sem_func_header)
                {
                    string thisMemberName = n.GetAsString(normalizeTree_.c_.src_);
                    Debug.Assert(thisMemberName.Length > 0);
                    if (
                        thisMemberName == semanticFunction
                    || thisMemberName[0] == '@' && thisMemberName.Substring(1) == semanticFunction)
                    {
                        memberName = n; return true;
                    }
                }
                return
                    FindSemanticFunctionInTree(n.child_, semanticFunction, ref memberName)
                || FindSemanticFunctionInTree(n.next_, semanticFunction, ref memberName);
            }
            void LinkSemanticFunctions(PegNode n)
            {
                Debug.Assert(n.id_ == (int)EPegGrammar.rule_ref);
                string semanticFunction = n.GetAsString(normalizeTree_.c_.src_);
                if (normalizeTree_.setRules_.Contains(semanticFunction)) return; //was a rule reference
                PegNode method;
                bool isLocal;
                PegNode semBlock = normalizeTree_.FindSemanticBlock(FindSemanticFunctionInTree, semanticFunction, n, out method, out isLocal);
                if (semBlock != null)
                {
                    var semanticFuncWithContext = new NormalizeTree.SemanticVarOrFuncWithContext((int)EPegGeneratorNodes.SemanticFunctionWithContext, semBlock, method, n, isLocal);
                    normalizeTree_.c_.semanticInfoNodes_.Add(method);
                    PUtils.ReplaceNode(n, semanticFuncWithContext);
                }
                else
                {
                    normalizeTree_.c_.errOut_.WriteLine(normalizeTree_.c_.sErrorPrefix + "ERROR: no rule found and no semantic function <" + semanticFunction + "> found in semantic block candidates");
                    normalizeTree_.bOk_ = false;
                }
            }
            void LinkSemanticFunctionsToSemanticBlocks(PegNode rule)
            {
                normalizeTree_.NormalizeEachRule(rule, null, LinkSemanticFunctions, (int)EPegGrammar.rule_ref);
            }
            NormalizeTree normalizeTree_;
        }
    }
    internal class TreeContext
    {
        public PegNode                      root_;
        public string                       src_;
        public TextWriter                   errOut_;
        public ParserPostProcessParams      generatorParams_;
        public Dictionary<string, PegNode>  dictNameRuleObj_;
        public Dictionary<string, string>   dictProperties_;
        public HashSet<PegNode>             semanticInfoNodes_;
        public Dictionary<string, PegNode>  dictSemanticInfo_;
        public HashSet<string>              referencedMembers_;
        public string                       sErrorPrefix;

        public static string caseSensitivity = "caseSensitivity";
        public static string encoding_class = "encoding_class";
        public static string Name = "name";

        public TreeContext(ParserPostProcessParams generatorParams)
        {
            generatorParams_ = generatorParams;
            root_ = generatorParams.root_;
            src_ = generatorParams.src_;
            errOut_ = generatorParams.errOut_;
            dictProperties_ = new Dictionary<string, string>();
            dictNameRuleObj_ = new Dictionary<string, PegNode>();
            semanticInfoNodes_ = new HashSet<PegNode>();
            dictSemanticInfo_ = new Dictionary<string, PegNode>();//member_name -> named/anonym-semanticblock
            referencedMembers_ = new HashSet<string>();
            sErrorPrefix = "<PEG_PARSER> FILE:'" + generatorParams_.grammarFileName_ + "' ";
            RegisterRules();
            RegisterProperties();
            RegisterSemanticMembers();
        }
        void RegisterRules()
        {
            PegNode rule = PUtils.FindNode(root_, EPegGrammar.peg_rule);
		    if( rule == null ) return;
		    for(PegNode cur= rule;cur!=null;cur=cur.next_){
			    string sRuleIdent= PUtils.GetAsString(src_,PUtils.FindNode(cur,EPegGrammar.rule_name,8));
                if( !dictNameRuleObj_.ContainsKey(sRuleIdent) ) dictNameRuleObj_.Add(sRuleIdent,cur);
		    }
        }
        public bool HasCaseSensitiveProperty()
        {
            string sCaseSensitive;
            if( dictProperties_.TryGetValue(caseSensitivity,out sCaseSensitive) ) return sCaseSensitive.ToUpper()=="YES";
            return false;
        }
        public bool IsGrammarForBinaryInput()
        {
            return dictProperties_.ContainsKey(encoding_class)
                && dictProperties_[encoding_class].Equals("binary", StringComparison.InvariantCultureIgnoreCase);
        }
        public string NameProperty()
        {
            string sName;
            return dictProperties_.TryGetValue(Name, out sName) ? sName : ""; 
        }
        void RegisterProperties()
        {
            PegNode attribute = PUtils.FindNode(root_, EPegGrammar.attribute);
            if (attribute == null) return;
            for (PegNode cur = attribute; cur != null; cur = cur.next_)
            {
                string key =   PUtils.FindNode(cur, EPegGrammar.attribute_key).GetAsString(src_).ToLower();
                string value = PUtils.FindNode(cur, EPegGrammar.attribute_value).child_.GetAsString(src_);
                string prevValue;
                if (dictProperties_.TryGetValue(key, out prevValue))
                {
                    value = prevValue + "\n" + value;
                    dictProperties_.Remove(key);
                }
                dictProperties_.Add(key, value);
            }
        }
        /// <summary>
        /// Register all members of top level non-local semantic blocks in the dictionary 'dictSemanticInfo_'
        /// </summary>
        void RegisterSemanticMembers()
        {
            PegNode topSemanticBlocks = PUtils.FindNode(root_, EPegGrammar.toplevel_semantic_blocks);
            List<PegNode> topLevelBlocksUsedAsLocalClasses = new List<PegNode> { };
            for(PegNode semBlock= topSemanticBlocks.child_;semBlock!=null;semBlock=semBlock.next_)
            {
                PegNode content=null;
                if( semBlock.id_==(int)EPegGrammar.named_semantic_block){
                    string className = semBlock.child_.GetAsString(src_);
                    if (className == "CREATE") continue; //a create block does not correspond to a local class
                    if (UsedAsLocalClass(className)){
                        topLevelBlocksUsedAsLocalClasses.Add(semBlock);
                        continue;
                    }
                    content = semBlock.child_.next_.child_;
                }
                else if (semBlock.id_ == (int)EPegGrammar.anonymous_semantic_block)
                {
                    content = semBlock.child_;
                }
                if (content == null) continue;
                RegisterSemBlockMembers(semBlock, content, dictSemanticInfo_,src_);

            }
            //register use of top level members in methods of local classes
            foreach(PegNode localSemBlock in topLevelBlocksUsedAsLocalClasses)
            {
                RegisterTopLevelMembersInUse(GetContent(localSemBlock));
            }
            for (PegNode rule = PUtils.FindNode(root_, EPegGrammar.peg_rule); rule != null; rule = rule.next_)
            {
                PegNode semBlock = PUtils.FindNode(rule.child_, EPegGrammar.named_semantic_block, EPegGrammar.anonymous_semantic_block);
                if (semBlock == null) continue;

                RegisterTopLevelMembersInUse(GetContent(semBlock));
            }
        }

        private PegNode GetContent(PegNode semBlock)
        {
            switch ((EPegGrammar)semBlock.id_)
            {
                case EPegGrammar.named_semantic_block: return semBlock.child_.next_.child_;
                case EPegGrammar.anonymous_semantic_block: return semBlock.child_;
                default: Debug.Assert(false); return null;
            }
        }

        private bool UsedAsLocalClass(string className)
        {
            for (PegNode rule = PUtils.GetRuleFromRoot(root_); rule != null; rule = rule.next_)
            {
                PegNode using_block = PUtils.FindNode(rule.child_, EPegGrammar.sem_block_name);
                if (using_block != null && using_block.GetAsString(src_) == className) return true;
            }
            return false;
        }

        private void RegisterTopLevelMembersInUse(PegNode content)
        {
            for (PegNode member = content.child_; member != null; member = member.next_)
            {
                switch ((EPegGrammar)member.id_)
                {
                    case EPegGrammar.constructor_decl:
                    case EPegGrammar.destructor_decl:
                    case EPegGrammar.sem_func_declaration:
                    case EPegGrammar.func_declaration:
                        RegisterTopLevelMembersInUseInBody(PUtils.FindNode(member.child_,EPegGrammar.method_body));
                        break;
                }
            }
        }
        private void RegisterTopLevelMembersInUseInBody(PegNode pegNode)
        {
            if (pegNode == null) return;
            if (pegNode.id_ == (int)EPegGrammar.designator)
            {
                string desigIdent = pegNode.child_.GetAsString(src_);
                if (dictSemanticInfo_.ContainsKey(desigIdent))
                {
                    if (!referencedMembers_.Contains(desigIdent)) referencedMembers_.Add(desigIdent);
                }
            }
            RegisterTopLevelMembersInUseInBody(pegNode.child_);
            RegisterTopLevelMembersInUseInBody(pegNode.next_);
        }

        private static void RegisterSemBlockMembers(PegNode semBlock, PegNode content, Dictionary<string, PegNode> dictSemanticInfo, string src)
        {
            for (PegNode member = content.child_; member != null; member = member.next_)
            {
                switch ((EPegGrammar)member.id_)
                {
                    case EPegGrammar.into_declaration:
                    case EPegGrammar.field_declaration:
                        for (PegNode variable = PUtils.FindNode(member.child_,EPegGrammar.variable); variable != null; variable = variable.next_)
                        {
                            dictSemanticInfo[variable.GetAsString(src)] = semBlock;
                        }
                        break;
                    case EPegGrammar.sem_func_declaration:
                    case EPegGrammar.func_declaration:
                    case EPegGrammar.creator_func_declaration:
                        PegNode memberName = PUtils.FindNode(member.child_, EPegGrammar.member_name);
                        dictSemanticInfo[memberName.GetAsString(src)] = semBlock;
                        break;
                    case EPegGrammar.outer_ident:
                        break;
                    default: continue;
                }
            }
        }
        public string GetModuleName()
        {
            return NameProperty();
        }
    }
    
    class PegParserGenerator : IParserPostProcessor
    {
        
        class CheckGrammar
        {
            #region Data Members
            internal HashSet<string> setRules_;
            internal TreeContext c_;
            internal bool bOk_;
            #endregion Data Members
            internal CheckGrammar(TreeContext c)
            {
                c_ = c;
                bOk_ = true;
                setRules_ = new HashSet<string>();
                PegNode n = PUtils.GetRuleFromRoot(c.root_);
                if (n == null) return;
                Debug.Assert(n.id_ == (int)EPegGrammar.peg_rule);
                CheckDoubleDefinitions(c);
                /*
                m_bIsTopModule = true;
                CheckReferenceDefined(c.m_context);
                m_bIsTopModule = true;
                CheckLeftRecursiveness(c.m_context);
                 * */
            }
            void CheckDoubleDefinitions(TreeContext c)
            {
                CheckDoubleDefinitions(c, PUtils.GetRuleFromRoot(c.root_));
            }
            void CheckDoubleDefinitions(TreeContext c, PegNode n)
            {
                for (; n != null; n = n.next_)
                {
                    Debug.Assert(n.id_ == (int)EPegGrammar.peg_rule);
                    string ruleName = PUtils.GetRuleNameFromRule(n, c_.src_);
                    if (setRules_.Contains(ruleName))
                    {
                        bOk_ = false;
                        c.errOut_.WriteLine(c.sErrorPrefix + "ERRROR: rule <" + ruleName + "> is duplicate");
                    }
                    else
                    {
                        setRules_.Add(ruleName);
                    }
                }
            }
        }
        #region Constructors
        #endregion Constructors
        #region IParserPostProcessor functions
        string IParserPostProcessor.ShortDesc { get { return "Create C# parser"; } }
        string IParserPostProcessor.ShortestDesc { get { return "Create C#"; } }
        string IParserPostProcessor.DetailDesc
        {
            get
            {
                return "Creates a C# parser for the given PEG grammar syntax tree";
            }
        }
        void IParserPostProcessor.Postprocess(ParserPostProcessParams postProcessorParams)
        {
            TreeContext context = new TreeContext(postProcessorParams);
            var checker = new CheckGrammar(context);
            if (!checker.bOk_) return;
            var normalize = new NormalizeTree(context,checker.setRules_);
            if (!normalize.bOk_) return;
            Peg.CSharp.PegCSharpGenerator cSharpGenerator = new Peg.CSharp.PegCSharpGenerator(context);
        }
        #endregion IParserPostProcessor functions
    }
}