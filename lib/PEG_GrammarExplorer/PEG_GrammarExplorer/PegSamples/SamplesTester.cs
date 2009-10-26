/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 * <</History>>
*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Peg.Base;
namespace Peg.Samples
{
    public enum ESampleId
    {
        eDirectCalc,eTreeCalc,
        eJson, eJsonChecker, eOptimizedJsonChecker, eJsonTree, eJsonTreeOpt,
        eEMailAddress,eBERMinimal, eBERCustom,
        eParserGenerator, eKernighanAndRitchieC, eCSharp3, eCSharp3Fast,ePython_2_5_2
    };
    public struct ParserPostProcessParams
    {
        public ParserPostProcessParams(string outputDirectory,string sourceFileTitle, string grammarFileName, PegNode root, string src, TextWriter errOut)
        {
            outputDirectory_ = outputDirectory;
            sourceFileTitle_ = sourceFileTitle;
            grammarFileName_ = grammarFileName;
            root_ = root;
            src_ = src;
            byteSrc_ = null;
            errOut_ = errOut;
            maxLineLength_ = 60;
            spacesPerTap_ = 4;
        }
        public ParserPostProcessParams(string outputDirectory,string sourceFileTitle, string grammarFileName, PegNode root, byte[] byteSrc, TextWriter errOut)
        {
            outputDirectory_ = outputDirectory;
            sourceFileTitle_ = sourceFileTitle;
            grammarFileName_ = grammarFileName;
            root_ = root;
            src_ = null;
            byteSrc_ = byteSrc;
            errOut_ = errOut;
            maxLineLength_= 60;
            spacesPerTap_ = 4;
        }

        public readonly string outputDirectory_;
        public readonly string sourceFileTitle_;
        public readonly string grammarFileName_;
        public readonly PegNode root_;
        public readonly string src_;
        public readonly byte[] byteSrc_;
        public readonly TextWriter errOut_;
        public readonly int maxLineLength_;
        public readonly int spacesPerTap_;
    }

    public interface IParserPostProcessor
    {
        string ShortDesc{get;}
        string ShortestDesc { get; }
        string DetailDesc { get; }
        void   Postprocess(ParserPostProcessParams postProcessorParams);
    }
    public struct SampleInfo
    {
        #region Constructors
        internal SampleInfo(ESampleId grammarId,PegCharParser.Matcher startRule,
            string grammarName,
            string grammarDescription,
            string samplesDirectory,
            List<IParserPostProcessor> postProcessors)
        {
            this.grammarId = grammarId;
            this.startRule = startRule;
            grammarName_ = grammarName;
            grammarDescription_ = grammarDescription;
            this.samplesDirectory = samplesDirectory;
            postProcessors_ = postProcessors;
        }
        #endregion Constructors
        #region Public Methods
        public EncodingClass GetEncodingClass()
        {
            EncodingClass encoding= EncodingClass.ascii;
            UnicodeDetection detection;
            var parser= startRule.Target as PegBaseParser;
            if( parser!=null ) parser.GetProperties(out encoding,out detection);
            return encoding;
        }
        public UnicodeDetection GetUnicodeDetection()
        {
            EncodingClass encoding;
            UnicodeDetection detection = UnicodeDetection.notApplicable;
            var parser = startRule.Target as PegBaseParser;
            if (parser != null) parser.GetProperties(out encoding, out detection);
            return detection;
        }
        #endregion Public Methods
        #region Data Members
        public readonly ESampleId grammarId;
        public readonly PegCharParser.Matcher startRule;
        public string GrammarName{
            set { grammarName_ = value; }
            get { return grammarName_; }
        }
        public readonly string grammarDescription_;
        public string samplesDirectory;
        private string grammarName_;
        public List<IParserPostProcessor> postProcessors_;
        #endregion Data Members
    }
    public class SamplesCollection
    {
        public List<SampleInfo> sampleGrammars;

        List<IParserPostProcessor> 
            AddPostprocessors(params IParserPostProcessor[] postProcessors)
        {
            var pegGrammarPostProcessor = new List<IParserPostProcessor>();
            foreach (var postProcessor in postProcessors)
            {
                pegGrammarPostProcessor.Add(postProcessor);
            }
            return pegGrammarPostProcessor;
        }
        
        public SamplesCollection()
        {
            sampleGrammars = new List<SampleInfo>();
            sampleGrammars.Add(
               new SampleInfo(
                   ESampleId.eDirectCalc,
                   new calc0_direct.calc0_direct().Expr,
                   "Calc On Parse",
                   "Calculator supporting floats, + - * / (direct evaluation)",
                   @"PegSamples\calc0_direct\input",
                   null));
            sampleGrammars.Add(
                new SampleInfo(
                    ESampleId.eTreeCalc,
                    new calc0_tree.calc0_tree().Calc,
                    "Calc On Eval",
                    "Line oriented calculator,supporting <<assigns and expression>> with <<ident,floats,+ - * />> (eval tree))",
                    @"PegSamples\calc0_tree\input",
                    AddPostprocessors(new calc0_tree.Calc0TreeEval())));
            sampleGrammars.Add(
               new SampleInfo(
                       ESampleId.eJson,
                       (new json_check.json_check()).json_text,
                       "Json Checker",
                       "Validates Json input; issues an appropriate error message",
                       @"PegSamples\Json\input",
                       null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eJsonChecker,
                        (new json_fast.json_fast()).json_text,
                        "Json Checker (fast version)",
                        "Validates Json input; issues an appropriate error message",
                        @"PegSamples\Json\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eJsonTreeOpt,
                        (new json_tree.json_tree()).json_text,
                        "Json Tree",
                        "Like Json but outputs a parse tree",
                       @"PegSamples\Json\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eEMailAddress,
                        (new EMail.EMail()).email_address,
                        "EMailAddress",
                        "EMail validation according to RFC2821",
                        @"PegSamples\EMail\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eBERMinimal,
                        (new BER.BER()).ProtocolDataUnit,
                        "Basic Encoding Rules Tree",
                        "Basic Encoding Rules (ITU-T X.690) is a binary encoding of trees (shrinkwrapped XML) (minimal solution)",
                        @"PegSamples\BasicEncodingRules\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eBERCustom,
                        (new BERTree.BERTree()).ProtocolDataUnit,
                        "Basic Encoding Rules Custom Tree",
                        "Basic Encoding Rules (ITU-T X.690) is a binary encoding of trees (shrinkwrapped XML) (custom node solution)",
                        @"PegSamples\BasicEncodingRules\input",
                        AddPostprocessors(new BERTree.BERConvertDefiniteLength(),
                                          new BERTree.BERConvertIndefiniteLength())));
                sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eParserGenerator,
                        (new PegGrammarParser()).peg_module,
                        "PEG Parser Generator",
                        "Generates a parse tree for a PEG grammar module",
                        @"PegSamples\PegGenerator\input",
                        AddPostprocessors(new PegParserGenerator())
                        ));
            sampleGrammars.Add(
               new SampleInfo(
                       ESampleId.eKernighanAndRitchieC,
                       (new C_KernighanRitchie2.C_KernighanRitchie2()).translation_unit,
                       "C Kernighan&Ritchie 2",
                       "Generates an abstract syntax tree for a preprocessed C files",
                       @"PegSamples\C_KernighanRitchie2\input",
                       null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eCSharp3,
                        (new CSharp3.CSharp3()).compilation_unit,
                        "C# 3.0",
                        "Generates an Abstract Syntax Tree for a C#3.0 file (ignores directives)",
                        @"PegSamples\CSharp3\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.eCSharp3Fast,
                        (new CSharp3Fast.CSharp3Fast()).compilation_unit,
                        "C# 3.0 (fast version)",
                        "C#3.0 Tree Generating Parser time optimized (character class recognition optimized)",
                        @"PegSamples\CSharp3\input",
                        null));
            sampleGrammars.Add(
                new SampleInfo(
                        ESampleId.ePython_2_5_2,
                        (new python_2_5_2_i.python_2_5_2_i()).file_input,
                        "Python 2.5.2 (readable grammar)",
                        "Python 2.5.2 based on readable, not optimized and heavily backtracing grammar from python 2.5.2 reference",
                        @"PegSamples\python_2_5_2\input",
                        null));
        }
        bool Exec(PegCharParser.Matcher startRule, out PegNode tree)
        {
            bool bMatches = startRule();
            tree = ((PegCharParser)startRule.Target).GetRoot();
            return bMatches;
        }
        bool RunImpl(ESampleId eSampleId, string src, TextWriter Fout,out PegNode tree)
        {
            SampleInfo sample = sampleGrammars.Find(si => si.grammarId == eSampleId);
            if (sample.startRule.Target is PegCharParser)
            {
                try
                {
                    PegCharParser pg = (PegCharParser)sample.startRule.Target;
                    pg.Construct(src, Fout);
                    pg.SetSource(src);
                    pg.SetErrorDestination(Fout);
                    bool bMatches = sample.startRule();
                    tree = ((PegCharParser)sample.startRule.Target).GetRoot();
                    return bMatches;
                }
                catch (PegException)
                {
                    tree = ((PegCharParser)sample.startRule.Target).GetRoot();
                    return false;
                }
            }
            Debug.Assert(false); tree = null; return false;
        }
        bool RunImpl(ESampleId eSampleId, byte[] src, TextWriter Fout, out PegNode tree)
        {
            SampleInfo sample = sampleGrammars.Find(si => si.grammarId == eSampleId);
            if (sample.startRule.Target is PegByteParser)
            {
                try
                {
                    PegByteParser pg = (PegByteParser)sample.startRule.Target;
                    pg.Construct(src, Fout);
                    bool bMatches = sample.startRule();
                    tree = ((PegByteParser)sample.startRule.Target).GetRoot();
                    return bMatches;
                }
                catch (PegException)
                {
                    tree = ((PegByteParser)sample.startRule.Target).GetRoot();
                    return false;
                }
            }
            Debug.Assert(false); tree = null; return false;
        }
        public bool Run(ESampleId eSampleId, string src, TextWriter Fout, out double elapsedTime, out PegNode tree)
        {
            int startTickCount = Environment.TickCount;
            bool bResult = RunImpl(eSampleId, src, Fout,out tree);
            elapsedTime = (Environment.TickCount - startTickCount);
            return bResult;
        }
        public bool Run(ESampleId eSampleId, byte[] src, TextWriter Fout, out double elapsedTime, out PegNode tree)
        {
            int startTickCount = Environment.TickCount;
            bool bResult = RunImpl(eSampleId, src, Fout, out tree);
            elapsedTime = (Environment.TickCount - startTickCount);
            return bResult;
        }
    }
}