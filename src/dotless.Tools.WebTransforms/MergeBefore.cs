using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace dotless.Tools.WebTransforms
{
    public class MergeBefore : Merge
    {
        protected override void Apply()
        {
            Apply((XmlElement)TargetNode, (XmlElement)TransformNode, Arguments);
        }

        public void Apply(XmlElement targetElement, XmlElement transformElement, IList<string> arguments)
        {
            var xpath = arguments.FirstOrDefault();

            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentException("xpath");

            var targetNode = targetElement.SelectSingleNode(xpath);
            Apply((XmlElement)targetNode.ParentNode, transformElement);
        }

        protected override void InsertTransformElement(XmlElement targetElement, XmlElement transformElement)
        {
            targetElement.InsertBefore(transformElement, targetElement.ChildNodes.OfType<XmlNode>().FirstOrDefault());
        }
    }
}
