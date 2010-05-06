using System;

namespace dotless.Stylizers
{
  public class HtmlStylizer : IStylizer
  {
    public string Stylize(Zone zone)
    {
      var template = @"
<div id=""less-error-message"">
  <h3>There is an error in your .less file</h3>
  <p>on line {1}, column {3}</p>
  <div class=""extract"">
    <pre class=""before""><span>{0}</span>{4}</pre>
    <pre class=""line""><span>{1}</span>{5}<span class=""error"">{6}</span>{7}</pre>
    <pre class=""after""><span>{2}</span>{8}</pre>
  </div>
</div>
";

      return string.Format(template,
                           zone.LineNumber - 1,
                           zone.LineNumber,
                           zone.LineNumber + 1,
                           zone.Position,
                           zone.Extract.Before,                            // 
                           zone.Extract.Line.Substring(0, zone.Position),  //
                           zone.Extract.Line[zone.Position],               // Html Encode
                           zone.Extract.Line.Substring(zone.Position + 1), //
                           zone.Extract.After);                            //
    }
  }
}