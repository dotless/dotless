namespace dotless.Core.Stylizers
{
    using Parser;

    public interface IStylizer
    {
        string Stylize(Zone zone);
    }
}