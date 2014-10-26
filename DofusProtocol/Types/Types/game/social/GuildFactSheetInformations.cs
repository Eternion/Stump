

// Generated on 10/26/2014 23:30:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GuildFactSheetInformations : GuildInformations
    {
        public const short Id = 424;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int leaderId;
        public byte guildLevel;
        public short nbMembers;
        
        public GuildFactSheetInformations()
        {
        }
        
        public GuildFactSheetInformations(int guildId, string guildName, Types.GuildEmblem guildEmblem, int leaderId, byte guildLevel, short nbMembers)
         : base(guildId, guildName, guildEmblem)
        {
            this.leaderId = leaderId;
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(leaderId);
            writer.WriteByte(guildLevel);
            writer.WriteShort(nbMembers);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            leaderId = reader.ReadInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            guildLevel = reader.ReadByte();
            if (guildLevel < 0 || guildLevel > 255)
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : guildLevel < 0 || guildLevel > 255");
            nbMembers = reader.ReadShort();
            if (nbMembers < 0)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(byte) + sizeof(short);
        }
        
    }
    
}