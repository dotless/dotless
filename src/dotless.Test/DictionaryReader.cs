namespace dotless.Test
{
    using System.Collections.Generic;
    using System.IO;
    using Core;
    using Core.Input;


    public class DictionaryReader : IFileReader
    {
        public Dictionary<string, string> Contents;

        public DictionaryReader()
        {
            Contents = new Dictionary<string, string>();
        }

        public DictionaryReader(Dictionary<string, string> contents)
        {
            Contents = contents;
        }

        public string GetFileContents(string fileName)
        {
            if (Contents.ContainsKey(fileName))
                return Contents[fileName];

            throw new FileNotFoundException("Import not found", fileName);
        }
    }
}