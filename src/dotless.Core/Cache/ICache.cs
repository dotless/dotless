namespace dotless.Core.Cache
{
    using System.Collections.Generic;

    public interface ICache
    {
        void Insert(string fileName, IEnumerable<string> imports, string css);
        bool Exists(string filename);
        string Retrieve(string filename);
    }
}