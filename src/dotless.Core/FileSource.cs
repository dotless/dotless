/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

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