using Microsoft.Web.XmlTransform;
using System.Linq;
using System.Xml;

namespace dotless.Tools.WebTransforms
{
    public class Merge : Transform
    {
        public Merge() : base(TransformFlags.UseParentAsTargetNode)
        {
        }

        protected override void Apply()
        {
            Apply((XmlElement)TargetNode, (XmlElement)TransformNode);
        }

        public void Apply(XmlElement targetElement, XmlElement transformElement)
        {
            var targetChildElement = targetElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(x => x.LocalName == transformElement.LocalName);
            if (targetChildElement == null)
            {
                InsertTransformElement(targetElement, transformElement);
                return;
            }

            foreach (var transformChildElement in transformElement.ChildNodes.OfType<XmlElement>())
                Apply(targetChildElement, transformChildElement);            
        }

        protected virtual void InsertTransformElement(XmlElement targetElement, XmlElement transformElement)
        {
            targetElement.AppendChild(transformElement);
        }
    }
}
