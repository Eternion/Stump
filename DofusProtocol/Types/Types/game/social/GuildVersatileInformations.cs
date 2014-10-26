

// Generated on 10/26/2014 23:30:21
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
        public ushort guildLevel;
        public ushort nbMembers;
        
        public GuildVersatileInformations()
        {
        }
        
        public GuildVersatileInformations(int guildId, int leaderId, ushort guildLevel, ushort nbMembers)
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
            writer.WriteUShort(guildLevel);
            writer.WriteUShort(nbMembers);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            leaderId = reader.ReadInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            guildLevel = reader.ReadUShort();
            if (guildLevel < 0 || guildLevel > 65535)
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : guildLevel < 0 || guildLevel > 65535");
            nbMembers = reader.ReadUShort();
            if (nbMembers < 0 || nbMembers > 65535)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0 || nbMembers > 65535");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(ushort) + sizeof(ushort);
        }
        
    }
    
}