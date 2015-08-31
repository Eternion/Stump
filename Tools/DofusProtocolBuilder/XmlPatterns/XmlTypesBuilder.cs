using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Parsing.Elements;

namespace DofusProtocolBuilder.XmlPatterns
{
	public class XmlTypesBuilder : XmlIOBuilder
	{
		public XmlTypesBuilder(Parser parser)
			: base(parser)
		{
		}

        public override void WriteToXml(XmlWriter writer)
        {
            var xmlType = new XmlType
            {
                Name = Parser.Class.Name,
                Id = Parser.Fields.Find(entry => entry.Name == "protocolId").Value,
                Heritage = Parser.Class.Heritage,
            };
            

            xmlType.Fields = GetXmlFields().ToArray();

            var serializer = new XmlSerializer(typeof(XmlType));
            serializer.Serialize(writer, xmlType);
        }
	}
}