

// Generated on 10/28/2014 16:38:04
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
        public int poiLabelId;
        
        public TreasureHuntStepFollowDirectionToPOI()
        {
        }
        
        public TreasureHuntStepFollowDirectionToPOI(sbyte direction, int poiLabelId)
        {
            this.direction = direction;
            this.poiLabelId = poiLabelId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(direction);
            writer.WriteInt(poiLabelId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            poiLabelId = reader.ReadInt();
            if (poiLabelId < 0)
                throw new Exception("Forbidden value on poiLabelId = " + poiLabelId + ", it doesn't respect the following condition : poiLabelId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(int);
        }
        
    }
    
}