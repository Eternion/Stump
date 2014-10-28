

// Generated on 10/28/2014 16:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntStepFollowDirectionToHint : TreasureHuntStep
    {
        public const short Id = 472;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte direction;
        public short npcId;
        
        public TreasureHuntStepFollowDirectionToHint()
        {
        }
        
        public TreasureHuntStepFollowDirectionToHint(sbyte direction, short npcId)
        {
            this.direction = direction;
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(direction);
            writer.WriteShort(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
            npcId = reader.ReadShort();
            if (npcId < 0)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(short);
        }
        
    }
    
}