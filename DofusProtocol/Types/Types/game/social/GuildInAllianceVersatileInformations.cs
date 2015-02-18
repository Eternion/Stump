

// Generated on 02/18/2015 11:05:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GuildInAllianceVersatileInformations : GuildVersatileInformations
    {
        public const short Id = 437;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int allianceId;
        
        public GuildInAllianceVersatileInformations()
        {
        }
        
        public GuildInAllianceVersatileInformations(int guildId, int leaderId, byte guildLevel, byte nbMembers, int allianceId)
         : base(guildId, leaderId, guildLevel, nbMembers)
        {
            this.allianceId = allianceId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(allianceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceId = reader.ReadVarInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
        
    }
    
}