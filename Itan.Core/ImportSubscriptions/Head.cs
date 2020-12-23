using System.Xml.Serialization;

namespace Itan.Core.ImportSubscriptions
{
    [XmlRoot(ElementName = "head")]
    public class Head
    {
        [XmlElement(ElementName = "title")] public string Title { get; set; }
    }
}