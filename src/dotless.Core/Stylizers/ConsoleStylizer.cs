namespace dotless.Core.Stylizers
{
    using System.Collections.Generic;
    using Parser;

    public class ConsoleStylizer : IStylizer
    {
        private Dictionary<string, int[]> styles;

        public ConsoleStylizer()
        {
            styles = new Dictionary<string, int[]>
                         {
                             {"bold", new[] {1, 22}},
                             {"inverse", new[] {7, 27}},
                             {"underline", new[] {4, 24}},
                             {"yellow", new[] {33, 39}},
                             {"green", new[] {32, 39}},
                             {"red", new[] {31, 39}},
                             {"grey", new[] {90, 39}},
                             {"reset", new[] {0, 0}}
                         };
        }

        private string Stylize(string str, string style)
        {
            return "\x1b[" + styles[style][0] + "m" + str +
                   "\x1b[" + styles[style][1] + "m";
        }

        public string Stylize(Zone zone)
        {
            var extract = zone.Extract;
            var errorPosition = zone.Position;

            var errorBefore = extract.Line.Substring(0, errorPosition);
            var errorAfter = extract.Line.Substring(errorPosition + 1);

            return Stylize(extract.Before, "grey") +
                   Stylize(errorBefore, "green") +
                   Stylize(
                       Stylize(extract.Line[errorPosition].ToString(), "inverse") + errorAfter, "yellow") +
                   Stylize(extract.After, "grey") +
                   Stylize("", "reset");
        }
    }
}