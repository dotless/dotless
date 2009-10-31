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

namespace dotless.Core.engine
{
    using System.Text;
    using System.Linq;

    public class Font : Literal
    {
        public Font(string value) : base(value)
        {
        }
    }

    public class Keyword : Literal
    {
        public Keyword(string value) : base(value)
        {
        }
        public override string Inspect()
        {
            return string.Format("#{0}", this);
        }
    }

    public class FontFamily : Literal
    {
        internal Literal[] Family { get; set; }

        public FontFamily(params string[] family)
            : this(family.Select(f => new Literal(f)).ToArray())
        {
        }

        public FontFamily(params Literal[] family)
        {
            Family = family;
        }

        public override string ToCss()
        {
            var sb = new StringBuilder();
            foreach (var family in Family)
                sb.AppendFormat("{0}, ", family.ToCss());
            return sb.ToString(0, sb.Length - 2);
        }
    }
}