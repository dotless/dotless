// ReSharper disable InconsistentNaming

namespace dotless.Core.Parser
{
    using Exceptions;
    using Importers;
    using Infrastructure;
    using Stylizers;
    using Tree;

    //
    // less.js - parser
    //
    //    A relatively straight-forward predictive parser.
    //    There is no tokenization/lexing stage, the input is parsed
    //    in one sweep.
    //
    //    To make the parser fast enough to run in the browser, several
    //    optimization had to be made:
    //
    //    - Instead of the more commonly used technique of slicing the
    //      input string on every match, we use global regexps (/g),
    //      and move the `lastIndex` pointer on match, foregoing `slice()`
    //      completely. This gives us a 3x speed-up.
    //
    //    - Matching on a huge input is often cause of slowdowns. 
    //      The solution to that is to chunkify the input into
    //      smaller strings.
    //
    //    - In many cases, we don't need to match individual tokens;
    //      for example, if a value doesn't hold any variables, operations
    //      or dynamic references, the parser can effectively 'skip' it,
    //      treating it as a literal.
    //      An example would be '1px solid #000' - which evaluates to itself,
    //      we don't need to know what the individual components are.
    //      The drawback, of course is that you don't get the benefits of
    //      syntax-checking on the CSS. This gives us a 50% speed-up in the parser,
    //      and a smaller speed-up in the code-gen.
    //
    //
    //    Token matching is done with the `Match` function, which either takes
    //    a terminal string or regexp, or a non-terminal function to call.
    //    It also takes care of moving all the indices forwards.
    //
    //
    public class Parser
    {
        public Tokenizer Tokenizer { get; set; }
        public IStylizer Stylizer { get; set; }
        public string FileName { get; set; }
        public bool Debug { get; set; }

        public string CurrentDirectory
        {
            get { return Importer.CurrentDirectory; }
            set { Importer.CurrentDirectory = value; }
        }

        private INodeProvider _nodeProvider;
        public INodeProvider NodeProvider
        {
            get { return _nodeProvider ?? (_nodeProvider = new DefaultNodeProvider()); }
            set { _nodeProvider = value; }
        }

        private IImporter _importer;
        public IImporter Importer
        {
            get { return _importer; }
            set
            {
                _importer = value;
                _importer.Parser = () => new Parser(Tokenizer.Optimization, Stylizer, _importer)
                                             {
                                                 NodeProvider = NodeProvider,
                                                 Debug = Debug,
                                                 CurrentDirectory = CurrentDirectory
                                             };
            }
        }

        public bool StrictMath { get; set; }

        private const int defaultOptimization = 1;
        private const bool defaultDebug = false;

        public Parser()
            : this(defaultOptimization, defaultDebug)
        {
        }

        public Parser(bool debug)
            : this(defaultOptimization, debug)
        {
        }

        public Parser(int optimization)
            : this(optimization, new PlainStylizer(), new Importer(), defaultDebug)
        {
        }

        public Parser(int optimization, bool debug)
            : this(optimization, new PlainStylizer(), new Importer(), debug)
        {
        }

        public Parser(IStylizer stylizer, IImporter importer)
            : this(defaultOptimization, stylizer, importer, defaultDebug)
        {
        }

        public Parser(IStylizer stylizer, IImporter importer, bool debug)
            : this(defaultOptimization, stylizer, importer, debug)
        {
        }

        public Parser(int optimization, IStylizer stylizer, IImporter importer)
            : this(optimization, stylizer, importer, defaultDebug)
        {
        }

        public Parser(int optimization, IStylizer stylizer, IImporter importer, bool debug)
        {
            Stylizer = stylizer;
            Importer = importer;
            Debug = debug;
            Tokenizer = new Tokenizer(optimization);
        }

        public Ruleset Parse(string input, string fileName)
        {
            Ruleset root;
            FileName = fileName;

            try
            {
                Tokenizer.SetupInput(input, fileName);

                var parsers = new Parsers(NodeProvider);
                root = new Root(parsers.Primary(this), GenerateParserError);
            }
            catch (ParsingException e)
            {
                throw GenerateParserError(e);
            }

            if (!Tokenizer.HasCompletedParsing())
                throw GenerateParserError(new ParsingException("Content after finishing parsing (missing opening bracket?)", Tokenizer.GetNodeLocation(Tokenizer.Location.Index)));

            return root;
        }

        private ParserException GenerateParserError(ParsingException parsingException)
        {
            var errorLocation = parsingException.Location;
            var error = parsingException.Message;
            var call = parsingException.CallLocation;

            var zone = new Zone(errorLocation, error, call != null ? new Zone(call) : null);

            var message = Stylizer.Stylize(zone);

            return new ParserException(message, parsingException);
        }
    }
}