using System.Xml.Serialization;

namespace Itan.Core.ImportSubscriptions
{
    [XmlRoot(ElementName = "opml")]
    public class Opml
    {
        [XmlElement(ElementName = "head")] public Head Head { get; set; }
        [XmlElement(ElementName = "body")] public Body Body { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }
}