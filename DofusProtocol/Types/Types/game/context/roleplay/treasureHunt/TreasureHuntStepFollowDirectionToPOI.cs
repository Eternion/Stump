

// Generated on 09/26/2016 01:50:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntStepFollowDirectionToPOI : TreasureHuntStep
    {
        public const short Id = 461;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte direction;
        public short poiLabelId;
        
        public TreasureHuntStepFollowDirectionToPOI()
        {
        }
        
        public TreasureHuntStepFollowDirectionToPOI(sbyte direction, short poiLabelId)
        {
            this.direction = direction;
            this.poiLabelId = poiLabelId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(direction);
            writer.WriteVarShort(poiLabelId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            poiLabelId = reader.ReadVarShort();
            if (poiLabelId < 0)
                throw new Exception("Forbidden value on poiLabelId = " + poiLabelId + ", it doesn't respect the following condition : poiLabelId < 0");
        }
        
        
    }
    
}