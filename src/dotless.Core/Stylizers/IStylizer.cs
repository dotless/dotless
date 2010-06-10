namespace dotless.Core.Stylizers
{
    using Parser;

    public interface IStylizer
    {
        string Stylize(Zone zone, string fileName, string error);
    }
}