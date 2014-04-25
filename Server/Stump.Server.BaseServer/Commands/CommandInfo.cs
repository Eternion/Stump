using System;
using System.Xml.Serialization;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    [Serializable]
    public class CommandInfo
    {
        public CommandInfo()
        {
            
        }

        public CommandInfo(CommandBase command)
        {
            Name = command.GetType().Name;
            RequiredRole = command.RequiredRole;
            Description = command.Description;
        }


        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement]
        public RoleEnum RequiredRole
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

        [XmlElement]
        public string Usage
        {
            get;
            set;
        }

        public void ModifyCommandInfo(CommandBase command)
        {
            command.RequiredRole = RequiredRole;
            command.Description = Description;
            command.Usage = Usage;
        }
    }
}