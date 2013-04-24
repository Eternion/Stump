using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using DofusProtocol.Messages;
using DofusProtocol.Types;
using Sniffer.Modules;

namespace ModuleExemple
{
    class Main : PacketHandlerModule
    {

        #region Infos

        public override string GetName()
        {
            return "Module d'exemple";
        }

        public override string GetAuthor()
        {
            return "Nathanael";
        }

        public override string GetVersion()
        {
            return "1.0.0";
        }
        #endregion

        private List<string> _names = new List<string>(200);

        public override void Handle(Message message, string sender)
        {
            if (message is GameRolePlayShowActorMessage)
            {
                var cMess = message as GameRolePlayShowActorMessage;

                var cInfo = cMess.informations as GameRolePlayNamedActorInformations;

                if (cInfo != null)
                {
                    if (!_names.Contains(cInfo.name))
                        _names.Add(cInfo.name);
                }
            }
            else if (message is MapComplementaryInformationsDataMessage)
            {
                var cMess = message as MapComplementaryInformationsDataMessage;

                foreach (var actor in cMess.actors)
                {
                    var cInfo = actor as GameRolePlayCharacterInformations;

                    if (cInfo != null)
                    {
                        if (!_names.Contains(cInfo.name))
                            _names.Add(cInfo.name);
                    }

                }
            }
        }

        public override void Stop()
        {
            new XmlSerializer(typeof(List<string>)).Serialize(new StreamWriter("./modules/data.xml"), _names);
        }
    }
}
