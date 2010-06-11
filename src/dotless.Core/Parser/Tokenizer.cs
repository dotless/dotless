namespace dotless.Core.Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Infrastructure.Nodes;

    public class Tokenizer
    {
        public int Optimization { get; set; }

        private string _input; // LeSS input string
        private List<string> _chunks; // chunkified input
        private int _i; // current index in `input`
        private int _j; // current chunk
        private int _current; // index of current chunk, in `input`
        private int _inputLength;

        //Increasing throughput through tracing of Regex
        private IDictionary<string, Regex> regexCache = new Dictionary<string, Regex>();

        public Tokenizer(int optimization)
        {
            Optimization = optimization;
        }

        public void SetupInput(string input)
        {
            _i = _j = _current = 0;
            _chunks = new List<string>();
            _input = input.Replace("\r\n", "\n");
            _inputLength = _input.Length;

            // Split the input into chunks,
            // Either delimited by /\n\n/ or
            // delmited by '\n}' (see rationale above),
            // depending on the level of optimization.

            if (Optimization == 0)
                _chunks.Add(_input);
            else
            {
                _input = Regex.Replace(_input, @"\/\*(?:[^*]|\*+[^\/*])*\*+\/",
                                       comment => Optimization == 1 ? Regex.Replace(comment.Value, @"\n\s*\n", "\n") : "");

                _input = Regex.Replace(_input, @"^\s*\n\s*(?=[^\s])", "\n", RegexOptions.Multiline);

                _chunks = Regex.Split(_input, "(?=\n\n)").ToList();

                _inputLength = _input.Length;
            }

            Advance(0); // skip any whitespace characters at the start.
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
            if (_i == _inputLength)
                return null;

            if (_input[_i] == tok)
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
            var options = RegexOptions.None;
            if (caseInsensitive)
                options |= RegexOptions.IgnoreCase;

            var regex = GetRegex(tok, options);

            var match = regex.Match(_chunks[_j], _i - _current);

            if (!match.Success)
                return null;

            if (match.Index != _i - _current)
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
            _i += length;
            var endIndex = _current + _chunks[_j].Length;

            while (true)
            {
                if(_i == _inputLength)
                    break;

                if (_i == endIndex)
                {
                    if (_j < _chunks.Count - 1)
                    {
                        _current = endIndex;
                        endIndex += _chunks[++_j].Length;
                    }
                    else
                        break;
                }

                if (!char.IsWhiteSpace(_input[_i]))
                    break;

                _i++;
            }
        }

        // Same as Match, but don't change the state of the parser,
        // just return the match.

        public bool Peek(char tok)
        {
            if (_i == _inputLength)
                return false;

            return _input[_i] == tok;
        }

        public bool Peek(string tok)
        {
            var regex = GetRegex(tok, RegexOptions.None);

            var match = regex.Match(_input, _i);

            return match.Success && match.Index == _i;
        }

        private Regex GetRegex(string pattern, RegexOptions options)
        {
            if (!regexCache.ContainsKey(pattern))
                regexCache.Add(pattern, new Regex(pattern, options));

            return regexCache[pattern];
        }

        public char PreviousChar
        {
            get { return _i == 0 ? '\0' : _input[_i - 1]; }
        }

        public char CurrentChar
        {
            get { return _i == _inputLength ? '\0' : _input[_i]; }
        }

        public bool HasCompletedParsing()
        {
            return _i == _inputLength;
        }

        public int GetLocation()
        {
            return _i;
        }

        public void SetLocation(int location)
        {
            _i = location;

            _j = 0;
            _current = 0;
            while (_current + _chunks[_j].Length < _i)
            {
                _current += _chunks[_j++].Length;
            }
        }

        public Zone GetCurrentZone()
        {
            var first = _input.Substring(0, _i);

            var start = first.LastIndexOf('\n') + 1;
            var line = first.Count(c => c == '\n');

            var lines = _input.Split('\n');

            return new Zone
                       {
                           LineNumber = line + 1,
                           Position = _i - start,
                           Extract = new Extract
                                         {
                                             Before = line > 0 ? lines[line - 1] : "/beginning of file",
                                             Line = lines[line],
                                             After = line + 1 < lines.Length ? lines[line + 1] : "/end of file",
                                         },
                       };
        }
    }

    public class Zone
    {
        public int LineNumber { get; set; }
        public int Position { get; set; }
        public Extract Extract { get; set; }
    }

    public class Extract
    {
        public string After { get; set; }
        public string Before { get; set; }
        public string Line { get; set; }
    }
}