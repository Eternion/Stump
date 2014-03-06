using System;
using System.Xml.Serialization;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Commands
{
    [Serializable]
    public class CommandInfo
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement]
        public RoleEnum RoleEnum
        {
            get;
            set;
        }

        [XmlArray]
        public string[] Aliases
        {
            get;
            set;
        }

        [XmlElement]
        public string Description
        {
            get;
            set;
        }


    }
}