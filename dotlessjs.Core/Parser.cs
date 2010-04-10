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
  //    Token matching is done with the `$` function, which either takes
  //    a terminal string or regexp, or a non-terminal function to call.
  //    It also takes care of moving all the indices forwards.
  //
  //
  public class Parser {
    private string input;         // LeSS input string
    private List<string> chunks;  // chunkified input
    private int i;                // current index in `input`
    private int j;                // current chunk
    private int current;          // index of current chunk, in `input`
    private int inputLength;

    public int Optimization { get; set; }
    public IStylizer Stylizer { get; set; }

    public Importer Importer { get; set; }

    public Parser() : this(2)
    {}
    public Parser(int optimization) : this(optimization, new PlainStylizer(), new FileImporter())
    {}

    public Parser(int optimization, IStylizer stylizer, Importer importer)
    {
      Stylizer = stylizer;
      Optimization = optimization;
      Importer = importer;
    }

    public Ruleset Parse(string str)
    {
      var buff = new List<char>();
      
      i = j = current = 0;
      chunks = new List<string>();
      input = str.Replace("\r\n", "\n");
      inputLength = input.Length;

      // Split the input into chunks,
      // Either delimited by /\n\n/ or
      // delmited by '\n}' (see rationale above),
      // depending on the level of optimization.
      if (Optimization > 0)
      {
        if (Optimization > 2)
        {
          var regex = new Regex(@"\/\*(?:[^*]|\*+[^\/*])*\*+\/");
          input = regex.Replace(input, "");

          regex = new Regex("^\n", RegexOptions.Multiline);
          chunks = regex.Split(input).ToList();
        }
        else
        {
          for (var k = 0; k < input.Length; k++)
          {
            char c;
            if ((c = input[k]) == '}' && input[k - 1] == '\n')
            {
              buff.Add('}');
              var chunk = new string(buff.ToArray());
              chunks.Add(chunk);
              buff = new List<char>();
            }
            else
              buff.Add(c);
          }
          chunks.Add(new string(buff.ToArray()));
        }
      }
      else
        chunks.Add(input);

      // Start with the primary rule.
      // The whole syntax tree is held under a Ruleset node,
      // with the `root` property set to true, so no `{}` are
      // output.
      var root = new Ruleset(new NodeList<Selector>(), Parsers.primary(this));
      root.Root = true;

      // If `i` is smaller than the `input.length - 1`,
      // it means the parser wasn't able to parse the whole
      // string, so we've got a parsing error.
      //
      // We try to extract a \n delimited string,
      // showing the line where the parse error occured.
      // We split it up into two parts (the part which parsed,
      // and the part which didn't), so we can color them differently.
      if (i < input.Length - 1) {
        int start;
        for (start = i; start > 0; start--)
        {
         if (input[start] == '\n')
           break;
        }
        start++;
        
        var regex = new Regex(@"\n", RegexOptions.Multiline);
        var line = regex.Matches(input.Substring(0, i)).Count + 1;

        var end = input.Substring(i).IndexOf('\n');
        end = end == -1 ? input.Length - start + 1 : end + i;

        var zone = Stylizer.Stylize(input.Substring(start, end - start), i - start);

        var message = "Parse Error on line " + line + ":\n" + zone;
        throw new ParsingException(message);
      }
      return root;
    }

    public string MatchString(char tok)
    {
      var c = Match(tok);

      return c == null ? null : c.Value;
    }

    public string MatchString(string tok)
    {
      var match = Match(tok);

      return match == null ? null : match.Value;
    }

    //
    // Parse from a token, regexp or string, and move forward if match
    //

    public CharMatchResult Match(char tok)
    {
      if (i == inputLength)
        return null;

      if (input[i] == tok)
      {
        Advance(1);

        return new CharMatchResult(tok);
      }

      return null;
    }

    
    public RegexMatchResult Match(string tok)
    {
      return Match(tok, false);
    }

    public RegexMatchResult Match(string tok, bool caseInsensitive)
    {
      if (i >= current + chunks[j].Length && j < chunks.Count - 1)
        current += chunks[j++].Length;

      var options = RegexOptions.None;
      if (caseInsensitive)
        options |= RegexOptions.IgnoreCase;

      var regex = new Regex(tok, options);

      var match = regex.Match(chunks[j], i - current);

      if (!match.Success)
        return null;

      if (match.Index != i - current)
        return null;

      Advance(match.Length);

      return new RegexMatchResult(match);
    }

    public void Advance(int length)
    {
      // The match is confirmed, add the match length to `i`,
      // and consume any extra white-space characters (' ' || '\n')
      // which come after that. The reason for this is that LeSS's
      // grammar is mostly white-space insensitive.
      i += length;
      var endIndex = current + chunks[j].Length;

      while (i <= endIndex)
      {
        if (i == inputLength)
          break;

        var c = input[i];
        if (! (c == 32 || c == 10 || c == 9))
          break;
        i++;
      }
    }

    // Same as Match, but don't change the state of the parser,
    // just return the match.

    public bool Peek(char tok)
    {
      if(i == inputLength)
        return false;

      return input[i] == tok;
    }

    public bool Peek(string tok)
    {
      var regex = new Regex(tok);

      var match = regex.Match(input, i);

      return match.Success && match.Index == i;
    }

    public char PreviousChar
    {
      get { return i == 0 ? '\0' : input[i - 1]; }
    }

    public char CurrentChar
    {
      get { return i == inputLength ? '\0' : input[i]; }
    }
  }
}