

// Generated on 10/30/2016 16:20:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public sbyte gameType;
        public short arenaLeaveBanTime;
        
        public ServerSettingsMessage()
        {
        }
        
        public ServerSettingsMessage(string lang, sbyte community, sbyte gameType, short arenaLeaveBanTime)
        {
            this.lang = lang;
            this.community = community;
            this.gameType = gameType;
            this.arenaLeaveBanTime = arenaLeaveBanTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(lang);
            writer.WriteSByte(community);
            writer.WriteSByte(gameType);
            writer.WriteVarShort(arenaLeaveBanTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
            gameType = reader.ReadSByte();
            arenaLeaveBanTime = reader.ReadVarShort();
            if (arenaLeaveBanTime < 0)
                throw new Exception("Forbidden value on arenaLeaveBanTime = " + arenaLeaveBanTime + ", it doesn't respect the following condition : arenaLeaveBanTime < 0");
        }
        
    }
    
}