namespace dotless.Core.Stylizers
{
    using Parser;

    public class HtmlStylizer : IStylizer
    {
        public string Stylize(Zone zone)
        {
            var fileStr = string.IsNullOrEmpty(zone.FileName) ? "" : string.Format(" in '{0}'", zone.FileName);

            return string.Format(@"
<div id=""less-error-message"">
  <h3>There is an error{0}</h3>
  <p>{1} on line {3}, column {5}</p>
  <div class=""extract"">
    <pre class=""before""><span>{2}</span>{6}</pre>
    <pre class=""line""><span>{3}</span>{7}<span class=""error"">{8}</span>{9}</pre>
    <pre class=""after""><span>{4}</span>{10}</pre>
  </div>
</div>
",
                                 fileStr,
                                 zone.Message,
                                 zone.LineNumber - 1,
                                 zone.LineNumber,
                                 zone.LineNumber + 1,
                                 zone.Position,
                                 zone.Extract.Before,
                                 zone.Extract.Line.Substring(0, zone.Position),
                                 zone.Extract.Line[zone.Position],
                                 zone.Extract.Line.Substring(zone.Position + 1),
                                 zone.Extract.After);
        }
    }
}