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

namespace dotless.Core.engine.CssNodes
{
    public class CssProperty 
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public CssProperty(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(CssProperty other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Key, Key) && Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CssProperty)) return false;
            return Equals((CssProperty) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Key.GetHashCode()*397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(CssProperty left, CssProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CssProperty left, CssProperty right)
        {
            return !Equals(left, right);
        }
    }
}