namespace dotless.Test
{
    using System.Collections.Generic;
    using System.IO;
    using Core;
    using Core.Input;


    public class DictionaryReader : IFileReader
    {
        public Dictionary<string, string> Contents;
        public List<string> DoesFileExistCalls = new List<string>();
        public List<string> GetFileContentsCalls = new List<string>();

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
            GetFileContentsCalls.Add(fileName);

            if (Contents.ContainsKey(fileName))
                return Contents[fileName];

            throw new FileNotFoundException(string.Format("Import {0} not found", fileName), fileName);
        }

        public bool DoesFileExist(string fileName)
        {
            DoesFileExistCalls.Add(fileName);

            return Contents.ContainsKey(fileName);
        }
    }
}