using System.Collections.Generic;
using System.Xml.Serialization;


namespace fyiReporting.RdlCreator
{
    public class Fields
    {
        [XmlElement(ElementName = "Field")]
        public List<Field> Field { get; set; }
    }
}