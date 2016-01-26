using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Parsing.Elements;
using DofusProtocolBuilder.Profiles;

namespace DofusProtocolBuilder.XmlPatterns
{
    public class XmlMessageBuilder : XmlIOBuilder
    {
        public XmlMessageBuilder(Parser parser)
            : base(parser)
        {
        }

        public override void WriteToXml(XmlWriter writer)
        {
            var xmlMessage = new XmlMessage
            {
                Name = Parser.Class.Name,
                Id = Parser.Fields.Find(entry => entry.Name == "protocolId").Value,
                Heritage = Parser.Class.Heritage,
            };

            xmlMessage.Fields = GetXmlFields().ToArray();

            var serializer = new XmlSerializer(typeof(XmlMessage));
            serializer.Serialize(writer, xmlMessage);
        }
    }
}