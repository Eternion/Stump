

// Generated on 02/19/2015 12:10:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class BasicGuildInformations : AbstractSocialGroupInfos
    {
        public const short Id = 365;
        public override short TypeId
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
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(guildId);
            writer.WriteUTF(guildName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            guildName = reader.ReadUTF();
        }
        
        
    }
    
}