namespace dotless.Core
{
    using System.IO;

    public class FileSource : ILessSource
    {
        private readonly string filename;

        public FileSource(string filename)
        {
            this.filename = filename;
        }

        public string Content
        {
            get { return File.ReadAllText(filename); }
        }

        public bool Cacheable
        {
            get { return true; }
        }

        public string Key
        {
            get { return filename; }
        }



        public bool Equals(FileSource other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.filename, filename);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FileSource)) return false;
            return Equals((FileSource) obj);
        }

        public override int GetHashCode()
        {
            return (filename != null ? filename.GetHashCode() : 0);
        }

        public static bool operator ==(FileSource left, FileSource right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileSource left, FileSource right)
        {
            return !Equals(left, right);
        }
    }
}