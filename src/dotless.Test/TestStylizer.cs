namespace dotless.Test
{
    using Core.Parser;
    using Core.Stylizers;

    public class TestStylizer : IStylizer 
    {
        public string Stylize(Zone zone, string fileName, string error)
        {
            return GetErrorMessage(error, zone.Extract.Line, zone.LineNumber, zone.Position);
        }

        public static string GetErrorMessage(string error, string line, int lineNumber, int position)
        {
            var format = @"{0}
{1}
line: {2}
position: {3};";

            return string.Format(format, error, line, lineNumber, position);
        }
    }
}