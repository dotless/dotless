namespace dotless.Core.Stylizers
{
    using Parser;

    public class PlainStylizer : IStylizer
    {
        public string Stylize(Zone zone)
        {
            var fileStr = string.IsNullOrEmpty(zone.FileName) ? "" : string.Format(" in file '{0}'", zone.FileName);

            var callStr = "";

            if(zone.CallExtract != null)
            {
                callStr = string.Format(@"
from line {0}:
{0,5:[#]}: {1}",
                                         zone.CallLine,
                                         zone.CallExtract.Line);
            }

            return string.Format(@"
{1} on line {4}{0}:
{2,5:[#]}: {3}
{4,5:[#]}: {5}
       {6}^
{7,5:[#]}: {8}{9}",
                                 fileStr,
                                 zone.Message,
                                 zone.LineNumber - 1,
                                 zone.Extract.Before,
                                 zone.LineNumber,
                                 zone.Extract.Line,
                                 new string('-', zone.Position),
                                 zone.LineNumber + 1,
                                 zone.Extract.After,
                                 callStr);
        }
    }
}