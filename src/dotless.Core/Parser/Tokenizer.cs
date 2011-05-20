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
        private List<Chunk> _chunks; // chunkified input
        private int _i; // current index in `input`
        private int _j; // current chunk
        private int _current; // index of current chunk, in `input`
        private int _inputLength;
        private readonly string _commentRegEx = @"(//[^\n]*|(/\*(.|[\r\n])*?\*/))";
        private readonly string _quotedRegEx = @"(""((?:[^""\\\r\n]|\\.)*)""|'((?:[^'\\\r\n]|\\.)*)')";

        //Increasing throughput through tracing of Regex
        private IDictionary<string, Regex> regexCache = new Dictionary<string, Regex>();

        public Tokenizer(int optimization)
        {
            Optimization = optimization;
        }

        public void SetupInput(string input)
        {
            _i = _j = _current = 0;
            _chunks = new List<Chunk>();
            _input = input.Replace("\r\n", "\n");
            _inputLength = _input.Length;

            // Split the input into chunks,
            // Either delimited by /\n\n/ or
            // delmited by '\n}' (see rationale above),
            // depending on the level of optimization.

            if(Optimization == 0)
                _chunks.Add(new Chunk(_input));
            else
            {
                var skip = new Regex(@"\G[^\""'{}/\\]+");

                var comment = GetRegex(this._commentRegEx, RegexOptions.None);
                var quotedstring = GetRegex(this._quotedRegEx, RegexOptions.None);
                var level = 0;
                var lastBlock = 0;
                
                int i = 0;
                while(i < _inputLength)
                {
                    var match = skip.Match(_input, i);
                    if(match.Success)
                    {
                        Chunk.Append(match.Value, _chunks);
                        i += match.Length;
                        continue;
                    }
                    
                    var c = _input[i];
                    
                    if(i < _inputLength - 1 && c == '/')
                    {
                        var cc = _input[i + 1];
                        if(cc == '/' || cc == '*')
                        {
                            match = comment.Match(_input, i);
                            if(match.Success)
                            {
                                i += match.Length;
                                _chunks.Add(new Chunk(match.Value, ChunkType.Comment));
                                //Chunk.Append(match.Value, _chunks);
                                continue;
                            } else
                            {
                                throw new ParsingException("Missing closing comment", i);
                            }
                        }
                    }
                    
                    if(c == '"' || c == '\'')
                    {
                        match = quotedstring.Match(_input, i);
                        if(match.Success)
                        {
                            i += match.Length;
                            _chunks.Add(new Chunk(match.Value, ChunkType.QuotedString));
                            //Chunk.Append(match.Value, _chunks);
                            continue;
                        } else
                        {
                            throw new ParsingException(string.Format("Missing closing quote ({0})", c), i);
                        }
                    }
                    
                    // we are not in a quoted string or comment - process '{' level
                    if(c == '{')
                    {
                        level++;
                        lastBlock = i;
                    } else if(c == '}')
                    {
                        level--;
                        
                        if(level < 0)
                            throw new ParsingException("Unexpected '}'", i);
                        
                        Chunk.Append(c, _chunks, true);
                        i++;
                        continue;
                    }
                    
                    Chunk.Append(c, _chunks);
                    i++;
                }
                
                if(level > 0)
                    throw new ParsingException("Missing closing '}'", lastBlock);

                _input =  Chunk.CommitAll(_chunks);

                _inputLength = _input.Length;
            }

            Advance(0); // skip any whitespace characters at the start.
        }

        public string GetComment()
        {
            // if we've hit the end we might still be looking at a valid chunk, so return early
            if (_i == _inputLength) {
                return null;
            }

            if  (Optimization == 0)
            {
                if (this.CurrentChar != '/')
                    return null;

                //Once CSS Hacks are supported, implement this exception
                //if (comment.Value.EndsWith(@"\*/")) {
                //    throw new ParsingException("The IE6 comment hack is not supported", parser.Tokenizer.Location.Index);
                //}

                var comment = this.Match(this._commentRegEx);
                return comment.Value;
            }
            else
            {
                if (_chunks[_j].Type == Tokenizer.ChunkType.Comment)
                {
                    string val = _chunks[_j].Value;
                    Advance(_chunks[_j].Value.Length);
                    return val;
                }
            }
            return null;
        }

        public string GetQuotedString()
        {
            // if we've hit the end we might still be looking at a valid chunk, so return early
            if (_i == _inputLength) {
                return null;
            }
            
            if (Optimization == 0) {
                if (this.CurrentChar != '"' && this.CurrentChar != '\'')
                    return null;
                
                var quotedstring = this.Match(this._quotedRegEx);
                return quotedstring.Value;
            } else {
                if (_chunks[_j].Type == Tokenizer.ChunkType.QuotedString) {
                    string val = _chunks[_j].Value;
                    Advance(_chunks[_j].Value.Length);
                    return val;
                }
            }
            return null;
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
            if  (_i == _inputLength || _chunks[_j].Type != Tokenizer.ChunkType.Text) {
                return null;
            }

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
            if (_i == _inputLength || _chunks[_j].Type != Tokenizer.ChunkType.Text) {
                return null;
            }

            var options = RegexOptions.None;
            if (caseInsensitive)
                options |= RegexOptions.IgnoreCase;

            var regex = GetRegex(tok, options);

            var match = regex.Match(_chunks[_j].Value, _i - _current);

            if (!match.Success)
                return null;

            Advance(match.Length);

            return new RegexMatchResult(match);
        }

        // Match a string, but include the possibility of matching quoted and comments
        public RegexMatchResult MatchAny(string tok)
        {
            if (_i == _inputLength) {
                return null;
            }

            var regex = GetRegex(tok, RegexOptions.None);
            
            var match = regex.Match(_input, _i);
            
            if (!match.Success)
                return null;

            Advance(match.Length);

            //If we absorbed the start of a quote/comment then turn it into text so the rest can be absorbed
            if  (_i != _inputLength && _i > _current && _i < _current + _chunks[_j].Value.Length) {
                _chunks[_j].Type = Tokenizer.ChunkType.Text;
            }

            return new RegexMatchResult(match);
        }

        public void Advance(int length)
        {
            if (_i == _inputLength) //only for empty cases as there may not be any chunks
                return;

            // The match is confirmed, add the match length to `i`,
            // and consume any extra white-space characters (' ' || '\n')
            // which come after that. The reason for this is that LeSS's
            // grammar is mostly white-space insensitive.
            _i += length;
            var endIndex = _current + _chunks[_j].Value.Length;

            while (true)
            {
                if(_i == _inputLength)
                    break;

                if (_i >= endIndex)
                {
                    if (_j < _chunks.Count - 1)
                    {
                        _current = endIndex;
                        endIndex += _chunks[++_j].Value.Length;
                        continue; // allow skipping multiple chunks
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

        private enum ChunkType
        {
            Text,
            Comment,
            QuotedString
        }

        private class Chunk
        {
            private StringBuilder _builder;

            public Chunk(string val)
            {
                Value = val;
                Type = Tokenizer.ChunkType.Text;
            }

            public Chunk(string val, ChunkType type)
            {
                Value = val;
                Type = type;
            }

            public Chunk()
            {
                _builder = new StringBuilder();
                Type = Tokenizer.ChunkType.Text;
            }

            public Tokenizer.ChunkType Type { get; set; }

            public string Value { get; set; }

            private bool _final;

            public void Append(string str)
            {
                _builder.Append(str);
            }

            public void Append(char c)
            {
                _builder.Append(c);
            }

            private static Chunk ReadyForText(List<Chunk> chunks)
            {
                Chunk last = chunks.LastOrDefault();
                if  (last == null || last.Type != Tokenizer.ChunkType.Text || last._final == true)
                {
                    last = new Chunk();
                    chunks.Add(last);
                }
                return last;
            }

            public static void Append(char c, List<Chunk> chunks, bool final)
            {
                Chunk chunk = Chunk.ReadyForText(chunks);
                chunk.Append(c);
                chunk._final = final;
            }

            public static void Append(char c, List<Chunk> chunks)
            {
                Chunk chunk = Chunk.ReadyForText(chunks);
                chunk.Append(c);
            }

            public static void Append(string s, List<Chunk> chunks)
            {
                Chunk chunk = Chunk.ReadyForText(chunks);
                chunk.Append(s);
            }

            public static string CommitAll(List<Chunk> chunks)
            {
                StringBuilder all = new StringBuilder();
                foreach(Chunk chunk in chunks)
                {
                    if  (chunk._builder != null)
                    {
                        string val = chunk._builder.ToString();
                        chunk._builder = null;
                        chunk.Value = val;
                    }

                    all.Append(chunk.Value);
                }
                return all.ToString();
            }
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
