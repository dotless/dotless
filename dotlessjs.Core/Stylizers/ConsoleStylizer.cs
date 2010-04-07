using System.Collections.Generic;

namespace dotless.Stylizers
{
  public class ConsoleStylizer : IStylizer
  {
    private Dictionary<string, int[]> styles;

    public ConsoleStylizer()
    {
      styles = new Dictionary<string, int[]>
                 {
                   {"bold",       new[] {1, 22}},
                   {"inverse",    new[] {7, 27}},
                   {"underline",  new[] {4, 24}},
                   {"yellow",     new[] {33, 39}},
                   {"green",      new[] {32, 39}},
                   {"red",        new[] {31, 39}},
                   {"reset",      new[] {0, 0}}
                 };
    }

    public string Stylize(string str, string style)
    {
      return "\033[" + styles[style][0] + "m" + str +
             "\033[" + styles[style][1] + "m";
    }
  }
}