namespace dotless.Stylizers
{
  class HtmlStylizer : IStylizer
  {
    public string Stylize(string str, string style)
    {
      if (string.IsNullOrEmpty(str))
        return "";

      return string.Format(@"<span class=""{0}"">{1}</span>", style, str);
    }
  }
}