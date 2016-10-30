

// Generated on 10/30/2016 16:20:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightDefenderLeaveMessage : Message
    {
        public const uint Id = 5892;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public short fightId;
        public long fighterToRemoveId;
        
        public PrismFightDefenderLeaveMessage()
        {
        }
        
        public PrismFightDefenderLeaveMessage(short subAreaId, short fightId, long fighterToRemoveId)
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
            if (fighterToRemoveId < 0 || fighterToRemoveId > 9007199254740990)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0 || fighterToRemoveId > 9007199254740990");
        }
        
    }
    
}