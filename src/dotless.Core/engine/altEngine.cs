namespace dotless.Core.engine
{

    public interface ILessParser
    {
        IPegNode ParseAST(string Src);
    }

    public interface IPegNode
    {
    }
}
