

// Generated on 08/11/2013 11:29:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class BasicGuildInformations
    {
        public const short Id = 365;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int guildId;
        public string guildName;
        
        public BasicGuildInformations()
        {
        }
        
        public BasicGuildInformations(int guildId, string guildName)
        {
            this.guildId = guildId;
            this.guildName = guildName;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guildId);
            writer.WriteUTF(guildName);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            guildName = reader.ReadUTF();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(guildName);
        }
        
    }
    
}