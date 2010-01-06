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

namespace dotless.Core.configuration
{
    using System;
    using System.Xml;

    public class XmlConfigurationInterpreter
    {
        public DotlessConfiguration Process(XmlNode section)
        {
            var dotlessConfiguration = DotlessConfiguration.Default;
            //Minify
            XmlAttribute attribute = section.Attributes["minifyCss"];
            if (attribute != null && attribute.Value == "true")
                dotlessConfiguration.MinifyOutput = true;
            //Cache
            XmlAttribute cacheAttribute = section.Attributes["cache"];
            if (cacheAttribute != null && cacheAttribute.Value == "false")
                dotlessConfiguration.CacheEnabled = false;

            //Source
            XmlAttribute sourceAttribute = section.Attributes["source"];
            if (sourceAttribute != null)
                dotlessConfiguration.LessSource = Type.GetType(sourceAttribute.Value);

            return dotlessConfiguration;
        }
    }
}