using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using dotless.Infrastructure;

namespace dotless
{
  public class Tokenizer
  {
    public int Optimization { get; set; }

    private string _input;         // LeSS input string
    private List<string> _chunks;  // chunkified input
    private int _i;                // current index in `input`
    private int _j;                // current chunk
    private int _current;          // index of current chunk, in `input`
    private int _inputLength;

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
      if (Optimization > 0)
      {
        if (Optimization > 2)
        {
          var regex = new Regex(@"\/\*(?:[^*]|\*+[^\/*])*\*+\/");
          _input = regex.Replace(_input, "");

          regex = new Regex("^\n", RegexOptions.Multiline);
          _chunks = regex.Split(_input).ToList();
        }
        else
        {
          var buff = new List<char>(_inputLength);
          for (var k = 0; k < _input.Length; k++)
          {
            char c;
            if ((c = _input[k]) == '}' && _input[k - 1] == '\n')
            {
              buff.Add('}');
              var chunk = new string(buff.ToArray());
              _chunks.Add(chunk);
              buff = new List<char>();
            }
            else
              buff.Add(c);
          }
          _chunks.Add(new string(buff.ToArray()));
        }
      }
      else
        _chunks.Add(_input);
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
      if (_i >= _current + _chunks[_j].Length && _j < _chunks.Count - 1)
        _current += _chunks[_j++].Length;

      var options = RegexOptions.None;
      if (caseInsensitive)
        options |= RegexOptions.IgnoreCase;

      var regex = new Regex(tok, options);

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

      while (_i <= endIndex)
      {
        if (_i == _inputLength)
          break;

        var c = _input[_i];
        if (!(c == 32 || c == 10 || c == 9))
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
      var regex = new Regex(tok);

      var match = regex.Match(_input, _i);

      return match.Success && match.Index == _i;
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
    }

    public Zone GetCurrentZone()
    {
      var first = _input.Substring(0, _i);
      var second = _input.Substring(_i);

      var start = first.LastIndexOf('\n') + 1;
      var line = first.Split('\n').Length;

      var end = second.IndexOf('\n');
      end = end == -1 ? _input.Length - start + 1 : end + _i;

      return new Zone
               {
                 LineNumber = line,
                 Line = _input.Substring(start, end - start),
                 Position = _i - start
               };
    }

    public class Zone
    {
      public int LineNumber { get; set; }
      public string Line { get; set; }
      public int Position { get; set; }
    }
  }

}