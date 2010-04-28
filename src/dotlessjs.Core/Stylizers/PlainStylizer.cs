namespace dotless.Stylizers
{
  class PlainStylizer : IStylizer
  {
    public string Stylize(string str, int errorPosition)
    {
      return string.Format("{0}\n{1}^", str, new string('-', errorPosition));
    }
  }
}