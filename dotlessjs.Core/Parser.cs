using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Stylizers;
using dotless.Tree;

// ReSharper disable InconsistentNaming

namespace dotless
{

  //
  // less.js - parser
  //
  //    A relatively straight-forward recursive-descent parser.
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
  //    - Matching on a huge input is often cause of slowdowns,
  //      especially with the /g flag. The solution to that is to
  //      chunkify the input: we split it by /\n\n/, just to be on
  //      the safe side. The chunks are stored in the `chunks` var,
  //      `j` holds the current chunk index, and `current` holds
  //      the index of the current chunk in relation to `input`.
  //      This gives us an almost 4x speed-up.
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
    public Importer Importer { get; set; }

    public Parser()
      : this(2)
    { }

    public Parser(int optimization)
      : this(optimization, new PlainStylizer(), new FileImporter())
    { }

    public Parser(int optimization, IStylizer stylizer, Importer importer)
    {
      Stylizer = stylizer;
      Importer = importer;
      Tokenizer = new Tokenizer(optimization);
    }

    public Ruleset Parse(string input)
    {
      Tokenizer.SetupInput(input);

      ParsingException parsingException = null;
      Ruleset root = null;
      try
      {
        root = new Ruleset(new NodeList<Selector>(), Parsers.Primary(this));
        root.Root = true;
      }
      catch (ParsingException e)
      {
        parsingException = e;
      }

      CheckForParsingError(parsingException);

      return root;
    }

    private void CheckForParsingError(Exception parsingException)
    {
      if (Tokenizer.HasCompletedParsing())
        if (parsingException != null)
          throw parsingException;
        else
          return;

      var zone = Tokenizer.GetCurrentZone();

      var zoneString = Stylizer.Stylize(zone.Line, zone.Position);

      var error = "Parse Error";
      if (parsingException != null)
        error = parsingException.Message;

      var message = string.Format("{0} on line {1}:\n{2}", error, zone.LineNumber, zoneString);

      throw new ParsingException(message, parsingException);
    }
  }
}