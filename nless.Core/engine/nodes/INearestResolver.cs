namespace dotless.Core.engine
{
    public interface INearestResolver   
    {
        INode Nearest(string ident);
        T NearestAs<T>(string ident);
    }
}