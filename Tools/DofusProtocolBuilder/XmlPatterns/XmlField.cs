using System.Xml.Serialization;

namespace DofusProtocolBuilder.XmlPatterns
{
    public class XmlField
    {
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Type
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ArrayLength
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Condition
        {
            get;
            set;
        }
    }
}