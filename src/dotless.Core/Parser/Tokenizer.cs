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
        private int _lastCommentStart = -1; // the start of the last collection of comments
        private int _lastCommentEnd = -1; // the end of the last collection of comments
        private int _inputLength;
        private readonly string _commentRegEx = @"(//[^\n]*|(/\*(.|[\r\n])*?\*/))";
        private readonly string _quotedRegEx = @"(""((?:[^""\\\r\n]|\\.)*)""|'((?:[^'\\\r\n]|\\.)*)')";
        private string _fileName;

        //Increasing throughput through tracing of Regex
        private IDictionary<string, Regex> regexCache = new Dictionary<string, Regex>();

        public Tokenizer(int optimization)
        {
            Optimization = optimization;
        }

        public void SetupInput(string input, string fileName)
        {
            _fileName = fileName;
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
                var skip = new Regex(@"\G(@\{[a-zA-Z0-9_-]+\}|[^\""'{}/\\\(\)]+)");

                var comment = GetRegex(this._commentRegEx, RegexOptions.None);
                var quotedstring = GetRegex(this._quotedRegEx, RegexOptions.None);
                var level = 0;
                var lastBlock = 0;
                var inParam = false;
                
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
                        if ((!inParam && cc == '/') || cc == '*')
                        {
                            match = comment.Match(_input, i);
                            if(match.Success)
                            {
                                i += match.Length;
                                _chunks.Add(new Chunk(match.Value, ChunkType.Comment));
                                continue;
                            } else
                            {
                                throw new ParsingException("Missing closing comment", GetNodeLocation(i));
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
                            continue;
                        } else
                        {
                            throw new ParsingException(string.Format("Missing closing quote ({0})", c), GetNodeLocation(i));
                        }
                    }
                    
                    // we are not in a quoted string or comment - process '{' level
                    if(!inParam && c == '{')
                    {
                        level++;
                        lastBlock = i;
                    }
                    else if (!inParam && c == '}')
                    {
                        level--;
                        
                        if(level < 0)
                            throw new ParsingException("Unexpected '}'", GetNodeLocation(i));
                        
                        Chunk.Append(c, _chunks, true);
                        i++;
                        continue;
                    } if (c == '(')
                    {
                        inParam = true;
                    }
                    else if (c == ')')
                    {
                        inParam = false;
                    }
                    
                    Chunk.Append(c, _chunks);
                    i++;
                }
                
                if(level > 0)
                    throw new ParsingException("Missing closing '}'", GetNodeLocation(lastBlock));

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

            string val;
            int startI = _i;
            int endI = 0;

            if  (Optimization == 0)
            {
                if (this.CurrentChar != '/')
                    return null;

                var comment = this.Match(this._commentRegEx);
                if (comment == null)
                {
                    return null;
                }
                val = comment.Value;
                endI = startI + comment.Value.Length;
            }
            else
            {
                if (_chunks[_j].Type == ChunkType.Comment)
                {
                    val = _chunks[_j].Value;
                    endI = _i + _chunks[_j].Value.Length;
                    Advance(_chunks[_j].Value.Length);
                }
                else
                {
                    return null;
                }
            }

            if (_lastCommentEnd != startI)
            {
                _lastCommentStart = startI;
            }

            _lastCommentEnd = endI;

            return val;
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
                if (_chunks[_j].Type == ChunkType.QuotedString) {
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
            if  (_i == _inputLength || _chunks[_j].Type != ChunkType.Text) {
                return null;
            }

            if (_input[_i] == tok)
            {
                var index = _i;

                Advance(1);

                return new CharMatchResult(tok) { Location = GetNodeLocation(index) };
            }

            return null;
        }


        public RegexMatchResult Match(string tok)
        {
            return Match(tok, false);
        }

        public RegexMatchResult Match(string tok, bool caseInsensitive)
        {
            if (_i == _inputLength || _chunks[_j].Type != ChunkType.Text) {
                return null;
            }

            var options = RegexOptions.None;
            if (caseInsensitive)
                options |= RegexOptions.IgnoreCase;

            var regex = GetRegex(tok, options);

            var match = regex.Match(_chunks[_j].Value, _i - _current);

            if (!match.Success)
                return null;

            var index = _i;

            Advance(match.Length);

            return new RegexMatchResult(match) {Location = GetNodeLocation(index)};
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

            if (_i > _current && _i < _current + _chunks[_j].Value.Length)
            {
                //If we absorbed the start of an inline comment then turn it into text so the rest can be absorbed
                if (_chunks[_j].Type == ChunkType.Comment && _chunks[_j].Value.StartsWith("//"))
                {
                    _chunks[_j].Type = ChunkType.Text;
                }
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

        public bool PeekAfterComments(char tok)
        {
            var memo = this.Location;

            while(GetComment() != null);

            var peekSuccess = Peek(tok);

            this.Location = memo;

            return peekSuccess;
        }

        private Regex GetRegex(string pattern, RegexOptions options)
        {
            if (!regexCache.ContainsKey(pattern))
                regexCache.Add(pattern, new Regex(@"\G" + pattern, options));

            return regexCache[pattern];
        }

        public char GetPreviousCharIgnoringComments()
        {
            if  (_i == 0) {
                return '\0';
            }

            if  (_i != _lastCommentEnd) {
                return PreviousChar;
            }

            int i = _lastCommentStart - 1;

            if  (i < 0) {
                return '\0';
            }

            return _input[i];
        }

        public char PreviousChar
        {
            get { return _i == 0 ? '\0' : _input[_i - 1]; }
        }

        public char CurrentChar
        {
            get { return _i == _inputLength ? '\0' : _input[_i]; }
        }

        public char NextChar
        {
            get { return _i + 1 == _inputLength ? '\0' : _input[_i + 1]; }
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

        public NodeLocation GetNodeLocation(int index)
        {
            return new NodeLocation(index, this._input, this._fileName);
        }

        public NodeLocation GetNodeLocation()
        {
            return GetNodeLocation(this.Location.Index);
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
                Type = ChunkType.Text;
            }

            public Chunk(string val, ChunkType type)
            {
                Value = val;
                Type = type;
            }

            public Chunk()
            {
                _builder = new StringBuilder();
                Type = ChunkType.Text;
            }

            public ChunkType Type { get; set; }

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
                if  (last == null || last.Type != ChunkType.Text || last._final == true)
                {
                    last = new Chunk();
                    chunks.Add(last);
                }
                return last;
            }

            public static void Append(char c, List<Chunk> chunks, bool final)
            {
                Chunk chunk = ReadyForText(chunks);
                chunk.Append(c);
                chunk._final = final;
            }

            public static void Append(char c, List<Chunk> chunks)
            {
                Chunk chunk = ReadyForText(chunks);
                chunk.Append(c);
            }

            public static void Append(string s, List<Chunk> chunks)
            {
                Chunk chunk = ReadyForText(chunks);
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

    public class Location 
    {
        public int Index { get; set; }
        public int CurrentChunk { get; set; }
        public int CurrentChunkIndex { get; set; }
    }
}
