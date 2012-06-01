namespace dotless.Core.Stylizers
{
    using Parser;

    public class PlainStylizer : IStylizer
    {
        public string Stylize(Zone zone)
        {
            var fileStr = string.IsNullOrEmpty(zone.FileName) ? "" : string.Format(" in file '{0}'", zone.FileName);

            var callStr = "";

            if(zone.CallZone != null)
            {
                var callFile = "";

                if (zone.CallZone.FileName != zone.FileName && !string.IsNullOrEmpty(zone.CallZone.FileName))
                {
                    callFile = string.Format(@" in file '{0}'", zone.CallZone.FileName);
                }

                callStr = string.Format(@"
from line {0}{2}:
{0,5:[#]}: {1}",
                                         zone.CallZone.LineNumber,
                                         zone.CallZone.Extract.Line,
                                         callFile);
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