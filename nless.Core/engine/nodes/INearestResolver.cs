namespace nless.Core.engine
{
    public interface INearestResolver   
    {
        INode Nearest(string ident);
        T NearestAs<T>(string ident);
    }
}