

// Generated on 03/06/2014 18:50:37
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
        
        public GuildInAllianceVersatileInformations(int guildId, int leaderId, ushort guildLevel, ushort nbMembers, int allianceId)
         : base(guildId, leaderId, guildLevel, nbMembers)
        {
            this.allianceId = allianceId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(allianceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceId = reader.ReadInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}