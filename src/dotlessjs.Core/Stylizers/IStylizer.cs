namespace dotless.Stylizers
{
  public interface IStylizer
  {
    string Stylize(string str, int errorPosition);
  }
}