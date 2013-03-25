
// Generated on 03/25/2013 19:24:02
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerSettingsMessage : Message
    {
        public const uint Id = 6340;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string lang;
        public sbyte community;
        
        public ServerSettingsMessage()
        {
        }
        
        public ServerSettingsMessage(string lang, sbyte community)
        {
            this.lang = lang;
            this.community = community;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(lang);
            writer.WriteSByte(community);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
        }
        
    }
    
}