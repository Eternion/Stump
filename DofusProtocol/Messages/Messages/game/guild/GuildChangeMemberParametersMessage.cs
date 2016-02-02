

// Generated on 02/02/2016 14:14:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildChangeMemberParametersMessage : Message
    {
        public const uint Id = 5549;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long memberId;
        public short rank;
        public sbyte experienceGivenPercent;
        public int rights;
        
        public GuildChangeMemberParametersMessage()
        {
        }
        
        public GuildChangeMemberParametersMessage(long memberId, short rank, sbyte experienceGivenPercent, int rights)
        {
            this.memberId = memberId;
            this.rank = rank;
            this.experienceGivenPercent = experienceGivenPercent;
            this.rights = rights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(memberId);
            writer.WriteVarShort(rank);
            writer.WriteSByte(experienceGivenPercent);
            writer.WriteVarInt(rights);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9.007199254740992E15)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9.007199254740992E15");
            rank = reader.ReadVarShort();
            if (rank < 0)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0");
            experienceGivenPercent = reader.ReadSByte();
            if (experienceGivenPercent < 0 || experienceGivenPercent > 100)
                throw new Exception("Forbidden value on experienceGivenPercent = " + experienceGivenPercent + ", it doesn't respect the following condition : experienceGivenPercent < 0 || experienceGivenPercent > 100");
            rights = reader.ReadVarInt();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
        }
        
    }
    
}