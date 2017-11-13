namespace dotless.Core.Test
{
    using Core.Parser;
    using Core.Stylizers;

    public class TestStylizer : IStylizer 
    {
        public string Stylize(Zone zone)
        {
          var callExtract = zone.CallZone != null ? zone.CallZone.Extract.Line : null;
          return GetErrorMessage(zone.Message, zone.Extract.Line, zone.LineNumber, zone.Position, callExtract, zone.CallZone != null ? zone.CallZone.LineNumber : 0);
        }

        public static string GetErrorMessage(string error, string line, int lineNumber, int position, string callExtract, int callLine)
        {
            var format = @"
{0}
{1}
line: {2}
position: {3};";

            if(!string.IsNullOrEmpty(callExtract))
              format += @"
call: {4}
call line: {5}";

            return string.Format(format, error, line.Split('\r', '\n')[0], lineNumber, position, callExtract, callLine);
        }
    }
}