using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace OddsAndSods
{
    public static class XmlExtensions
    {
        public static XmlText AddText(this XmlNode node, string text)
        {
            var doc = (node is XmlDocument ? (XmlDocument)node : node.OwnerDocument);
            return (XmlText)node.AppendChild(doc.CreateTextNode(text));
        }

        public static XmlElement AddChild(this XmlNode node, string nodeName)
        {
            var doc = (node is XmlDocument ? (XmlDocument)node : node.OwnerDocument);
            return (XmlElement)node.AppendChild(doc.CreateElement(nodeName));
        }

        public static XmlAttribute AddAttribute(this XmlNode node, string name, string value)
        {
            var attr = node.Attributes[name];
            if (attr == null)
            {
                attr = (XmlAttribute)node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
            }
            attr.Value = value;
            return attr;
        }

        public static string Transform(this XmlDocument xml, XmlDocument xslt, XmlOutputMethod mode = XmlOutputMethod.AutoDetect)
        {
            var result = new StringBuilder();
            using (var buff = XmlWriter.Create(result, new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment }))
            using (var srX = new StringReader(xml.OuterXml))
            using (var xrX = XmlReader.Create(srX))
            using (var srT = new StringReader(xslt.OuterXml))
            using (var xrT = XmlReader.Create(srT))
            {
                var transformer = new System.Xml.Xsl.XslCompiledTransform();
                transformer.Load(xrT);
                transformer.Transform(xrX, buff);
            }
            return result.ToString();
        }

        public static ValidationEventArgs Validate(this XmlDocument xml, XmlDocument xsd)
        {
            ValidationEventArgs errs = null;
            using (var sr = new StringReader(xsd.OuterXml))
            {
                var schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(sr));

                var xdoc = XDocument.Parse(xml.OuterXml);
                xdoc.Validate(schemas, (ValidationEventHandler)((s, e) => errs = e));
            }
            return errs;
        }
    }
}
