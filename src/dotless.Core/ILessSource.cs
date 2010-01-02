namespace dotless.Core
{
    using System;
    using System.Text;

    public interface ILessSource
    {
        string Content { get; }
        bool Cacheable { get; }
        string Key { get; }
    }

    public class StringSource : ILessSource
    {
        private readonly string content;

        public StringSource(string content)
        {
            this.content = content;
        }

        public string Content
        {
            get { return content; }
        }

        public bool Cacheable
        {
            get { return false; }
        }

        public string Key
        {
            get
            {
                byte[] inputStream = Encoding.Default.GetBytes(content);
                return BitConverter.ToString(
                    System.Security.Cryptography.SHA1.Create().ComputeHash(inputStream))
                    .Replace("-", "");
            }
        }
    }
}