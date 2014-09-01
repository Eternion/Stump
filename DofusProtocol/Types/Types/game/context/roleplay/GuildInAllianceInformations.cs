

// Generated on 09/01/2014 15:52:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GuildInAllianceInformations : GuildInformations
    {
        public const short Id = 420;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public ushort guildLevel;
        public ushort nbMembers;
        public bool enabled;
        
        public GuildInAllianceInformations()
        {
        }
        
        public GuildInAllianceInformations(int guildId, string guildName, Types.GuildEmblem guildEmblem, ushort guildLevel, ushort nbMembers, bool enabled)
         : base(guildId, guildName, guildEmblem)
        {
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
            this.enabled = enabled;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort(guildLevel);
            writer.WriteUShort(nbMembers);
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildLevel = reader.ReadUShort();
            if (guildLevel < 0 || guildLevel > 65535)
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : guildLevel < 0 || guildLevel > 65535");
            nbMembers = reader.ReadUShort();
            if (nbMembers < 0 || nbMembers > 65535)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0 || nbMembers > 65535");
            enabled = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(ushort) + sizeof(ushort) + sizeof(bool);
        }
        
    }
    
}