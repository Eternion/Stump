

// Generated on 02/02/2016 14:14:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightAttackerRemoveMessage : Message
    {
        public const uint Id = 5897;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public short fightId;
        public long fighterToRemoveId;
        
        public PrismFightAttackerRemoveMessage()
        {
        }
        
        public PrismFightAttackerRemoveMessage(short subAreaId, short fightId, long fighterToRemoveId)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(subAreaId);
            writer.WriteVarShort(fightId);
            writer.WriteVarLong(fighterToRemoveId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadVarShort();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            fighterToRemoveId = reader.ReadVarLong();
            if (fighterToRemoveId < 0 || fighterToRemoveId > 9.007199254740992E15)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0 || fighterToRemoveId > 9.007199254740992E15");
        }
        
    }
    
}