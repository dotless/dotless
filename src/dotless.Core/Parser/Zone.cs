namespace dotless.Core.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Zone
    {
        public Zone(NodeLocation location)
            : this(location, null, null)
        {
        }

        public Zone(NodeLocation location, string error, Zone callZone)
        {
            var input = location.Source;
            var index = location.Index;

            if (index > input.Length)
            {
                index = input.Length;
            }

            int lineNumber, position;
            GetLineNumber(location, out lineNumber, out position);

            var lines = input.Split('\n');

            FileName = location.FileName;
            Message = error;
            CallZone = callZone;
            LineNumber = lineNumber + 1;
            Position = position;
            Extract = new Extract(lines, lineNumber);
        }

        public static int GetLineNumber(NodeLocation location)
        {
            int lineNumber, position;
            GetLineNumber(location, out lineNumber, out position);
            return lineNumber + 1;
        }

        private static void GetLineNumber(NodeLocation location, out int lineNumber, out int position)
        {
            var input = location.Source;
            var index = location.Index;

            if (location.Index > input.Length)
            {
                index = input.Length;
            }

            var first = input.Substring(0, index);

            var start = first.LastIndexOf('\n') + 1;
            lineNumber = first.Count(c => c == '\n');
            position = index - start;
        }

        public int LineNumber { get; set; }
        public int Position { get; set; }
        public Extract Extract { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public Zone CallZone { get; set; }
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
}
