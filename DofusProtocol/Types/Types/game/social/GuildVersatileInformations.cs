

// Generated on 12/29/2014 21:14:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GuildVersatileInformations
    {
        public const short Id = 435;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int guildId;
        public int leaderId;
        public byte guildLevel;
        public byte nbMembers;
        
        public GuildVersatileInformations()
        {
        }
        
        public GuildVersatileInformations(int guildId, int leaderId, byte guildLevel, byte nbMembers)
        {
            this.guildId = guildId;
            this.leaderId = leaderId;
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guildId);
            writer.WriteInt(leaderId);
            writer.WriteByte(guildLevel);
            writer.WriteByte(nbMembers);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            leaderId = reader.ReadInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            guildLevel = reader.ReadByte();
            if (guildLevel < 1 || guildLevel > 200)
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : guildLevel < 1 || guildLevel > 200");
            nbMembers = reader.ReadByte();
            if (nbMembers < 1 || nbMembers > 240)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 1 || nbMembers > 240");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(byte) + sizeof(byte);
        }
        
    }
    
}