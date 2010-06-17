namespace dotless.Core.Stylizers
{
    using Parser;

    internal class PlainStylizer : IStylizer
    {
        public string Stylize(Zone zone, string fileName, string error)
        {
            var fileStr = string.IsNullOrEmpty(fileName) ? "" : string.Format(" in file '{0}'", fileName);

            return string.Format(@"
{1} on line {4}{0}:
{2,5:[#]}: {3}
{4,5:[#]}: {5}
       {6}^
{7,5:[#]}: {8}",
                                 fileStr,
                                 error,
                                 zone.LineNumber - 1,
                                 zone.Extract.Before,
                                 zone.LineNumber,
                                 zone.Extract.Line,
                                 new string('-', zone.Position),
                                 zone.LineNumber + 1,
                                 zone.Extract.After);
        }
    }
}