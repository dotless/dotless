namespace dotless.Core.Abstractions
{
    using System.Web;

    public interface IHttp
    {
        HttpContextBase Context { get; }
    }
}
