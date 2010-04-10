using System;

namespace dotless.Stylizers
{
  public class HtmlStylizer : IStylizer
  {
    public string Stylize(string str, int errorPosition)
    {
      return
        string.Format(@"{0}<span class=""error"">{1}</span>{2}",
                      str.Substring(0, errorPosition),    //
                      str[errorPosition],                 // Html Encode
                      str.Substring(errorPosition + 1)    //
          );
    }
  }
}