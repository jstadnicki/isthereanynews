using System.Xml.Serialization;

namespace Itan.Core.ImportSubscriptions
{
    [XmlRoot(ElementName = "outline")]
    public class Outline
    {
        [XmlAttribute(AttributeName = "type")] public string Type { get; set; }

        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "text")] public string Text { get; set; }

        [XmlAttribute(AttributeName = "htmlUrl")]
        public string HtmlUrl { get; set; }

        [XmlAttribute(AttributeName = "xmlUrl")]
        public string XmlUrl { get; set; }
    }
}