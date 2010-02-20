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
    using System;
    using System.Collections.Generic;

    public class Selectors : Dictionary<string, Func<Selector>> 
    {
        public Selectors()
        {
            Add("", () => new Descendant());
            Add(">", () => new Child());
            Add("+", () => new Adjacent());
            Add(":", () => new PseudoClass());
            Add("::", () => new PseudoElement());
            Add("~", () => new Sibling());
        }
    }

    public class Selector : Entity
    {
        private static readonly Selectors Selectors = new Selectors();

        public static Selector Get(string key)
        {
            return Selectors[key].Invoke();
        }
    }

    public class Descendant : Selector
    {
        public override string ToCss()
        {
            return " ";
        }
    }

    public class Child : Selector
    {
        public override string ToCss()
        {
            return " > ";
        }
    }

    public class Adjacent : Selector
    {
        public override string ToCss()
        {
            return " + ";
        }
    }

    public class Sibling : Selector
    {
        public override string ToCss()
        {
            return " ~ ";
        }
    }

    public class PseudoClass : Selector
    {
        public override string ToCss()
        {
            return ":";
        }
    }

    public class PseudoElement : Selector
    {
        public override string ToCss()
        {
            return "::";
        }
    }
}