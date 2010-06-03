namespace dotless.Core.Parser.Stylizers
{
    internal class PlainStylizer : IStylizer
    {
        public string Stylize(Zone zone)
        {
            return string.Format("{0}\n{1}\n{2}^\n{3}",
                                 zone.Extract.Before,
                                 zone.Extract.Line,
                                 new string('-', zone.Position),
                                 zone.Extract.After);
        }
    }
}