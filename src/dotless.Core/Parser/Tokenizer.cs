namespace dotless.Core.Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Infrastructure.Nodes;
    using Utils;

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
                var chunkParts = new List<StringBuilder> { new StringBuilder() };
                var chunkPart = chunkParts.Last();
                var skip = new Regex(@"\G[^\""'{}/\\]+");
                var comment = new Regex(@"\G(//[^\n]*|(/\*(.|[\r\n])*?\*/))");
                var level = 0;
                var lastBlock = 0;
                var lastQuote = 0;
                char? inString = null;
				
				int i = 0;
                while(i < _inputLength)
                {
                    var match = skip.Match(_input, i);
                    if(match.Success)
                    {
                        chunkPart.Append(match.Value);
                        i += match.Length;
						continue;
                    }


                    if(i < _inputLength - 1 && _input[i] == '/' && inString == null)
                    {
                        var cc = _input[i + 1];
                        if(cc == '/' || cc=='*')
                        {
                            match = comment.Match(_input, i);
                            if (match.Success)
                            {
                                i += match.Length;
                                chunkPart.Append(match.Value);
								continue;
                            }
                        }
                    }

                    var c = _input[i];

                    if (c == '"' || c == '\'')
                    {
                        if (inString == null)
                        {
                            inString = c;
                            lastQuote = i;
                        }
                        else
                            inString = inString == c ? null : inString;
                    }
//                    else if (inString != null && c == '\n')
//                    {
//                        throw new ParsingException(string.Format("Missing closing quote ({0})", inString), lastQuote);
//                    }
                    else if (inString != null && c == '\\' && i < _inputLength - 1)
                    {
                        chunkPart.Append(_input, i, 2);
                        i+=2;
                        continue;
                    }
                    else if (inString == null && c == '{')
                    {
                        level++;
                        lastBlock = i;
                    }
                    else if (inString == null && c == '}')
                    {
                        level--;

                        if (level < 0)
                            throw new ParsingException("Unexpected '}'", i);

                        chunkPart.Append(c);
                        chunkPart = new StringBuilder();
                        chunkParts.Add(chunkPart);
						i++;
                        continue;
                    }

                    chunkPart.Append(c);
					i++;
                }

                if(inString != null)
                    throw new ParsingException(string.Format("Missing closing quote ({0})", inString), lastQuote);

                if(level > 0)
                    throw new ParsingException("Missing closing '}'", lastBlock);

                _chunks = chunkParts.Select(p => p.ToString()).ToList();

                _input = _chunks.JoinStrings("");

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

            return match.Success;
        }

        private Regex GetRegex(string pattern, RegexOptions options)
        {
            if (!regexCache.ContainsKey(pattern))
                regexCache.Add(pattern, new Regex(@"\G" + pattern, options));

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

        public Location Location
        {
            get 
            {
                return new Location
                {
                    Index = _i,
                    CurrentChunk = _j,
                    CurrentChunkIndex = _current
                };
            }
            set
            {
                _i = value.Index;
                _j = value.CurrentChunk;
                _current = value.CurrentChunkIndex;
            }
        }

        public Zone GetZone(string error, int position, int call, string fileName)
        {
            var first = _input.Substring(0, System.Math.Min(position, _input.Length));

            var start = first.LastIndexOf('\n') + 1;
            var line = first.Count(c => c == '\n');

            var lines = _input.Split('\n');

            var callLine = _input.Substring(0, call).Count(c => c == '\n');

            return new Zone
                       {
                           FileName = fileName,
                           Message = error,
                           CallLine = callLine + 1,
                           CallExtract = callLine <= 0 ? null : new Extract(lines, callLine),
                           LineNumber = line + 1,
                           Position = position - start,
                           Extract = new Extract(lines, line),
                       };
        }
    }

    public class Zone
    {
        public int LineNumber { get; set; }
        public int Position { get; set; }
        public Extract Extract { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public int CallLine { get; set; }
        public Extract CallExtract { get; set; }
    }

    public class Extract
    {
        public Extract(string[] lines, int line)
        {
            Before = line > 0 ? lines[line - 1] : "/beginning of file";
            Line = lines[line];
            After = line + 1 < lines.Length ? lines[line + 1] : "/end of file";
        }

        public string After { get; set; }
        public string Before { get; set; }
        public string Line { get; set; }
    }

    public class Location 
    {
        public int Index { get; set; }
        public int CurrentChunk { get; set; }
        public int CurrentChunkIndex { get; set; }
    }
}