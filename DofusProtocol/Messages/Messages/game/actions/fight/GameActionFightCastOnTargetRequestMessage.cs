

// Generated on 09/26/2016 01:49:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightCastOnTargetRequestMessage : Message
    {
        public const uint Id = 6330;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        public double targetId;
        
        public GameActionFightCastOnTargetRequestMessage()
        {
        }
        
        public GameActionFightCastOnTargetRequestMessage(short spellId, double targetId)
        {
            this.spellId = spellId;
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(spellId);
            writer.WriteDouble(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            targetId = reader.ReadDouble();
            if (targetId < -9007199254740990 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
        }
        
    }
    
}