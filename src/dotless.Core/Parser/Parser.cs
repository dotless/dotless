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

        private INodeProvider _nodeProvider;
        public INodeProvider NodeProvider
        {
            get { return _nodeProvider??(new DefaultNodeProvider()); }
            set { _nodeProvider = value; }
        }

        private IImporter _importer;
        public IImporter Importer
        {
            get { return _importer; }
            set
            {
                _importer = value;
                _importer.Parser = () => new Parser(Tokenizer.Optimization, Stylizer, _importer) {NodeProvider = this.NodeProvider};
            }
        }

        private const int defaultOptimization = 1;

        public Parser()
            : this(defaultOptimization)
        {
        }

        public Parser(int optimization)
            : this(optimization, new PlainStylizer(), new Importer())
        {
        }

        public Parser(IStylizer stylizer, IImporter importer)
            : this(defaultOptimization, stylizer, importer)
        {
        }

        public Parser(int optimization, IStylizer stylizer, IImporter importer)
        {
            Stylizer = stylizer;
            Importer = importer;
            Tokenizer = new Tokenizer(optimization);
        }

        public Ruleset Parse(string input,  string fileName)
        {
            ParsingException parsingException = null;
            Ruleset root = null;

            try
            {
                Tokenizer.SetupInput(input);

                var parsers = new Parsers(NodeProvider);
                root = new Root(parsers.Primary(this), e => GenerateParserError(e, fileName));
            }
            catch (ParsingException e)
            {
                parsingException = e;
            }

            if (Tokenizer.HasCompletedParsing() && parsingException == null)
                return root;

            throw GenerateParserError(parsingException, fileName);
        }

        private ParserException GenerateParserError(ParsingException parsingException, string fileName)
        {
            var errorLocation = Tokenizer.Location.Index;
            var error = "Parse Error";
            var call = 0;

            if(parsingException != null)
            {
                errorLocation = parsingException.Index;
                error = parsingException.Message;
                call = parsingException.Call;
            }

            var zone = Tokenizer.GetZone(error, errorLocation, call, fileName);

            var message = Stylizer.Stylize(zone);

            return new ParserException(message, parsingException);
        }
    }
}