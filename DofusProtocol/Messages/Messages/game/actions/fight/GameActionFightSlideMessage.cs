

// Generated on 10/30/2016 16:20:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightSlideMessage : AbstractGameActionMessage
    {
        public const uint Id = 5525;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double targetId;
        public short startCellId;
        public short endCellId;
        
        public GameActionFightSlideMessage()
        {
        }
        
        public GameActionFightSlideMessage(short actionId, double sourceId, double targetId, short startCellId, short endCellId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.startCellId = startCellId;
            this.endCellId = endCellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(targetId);
            writer.WriteShort(startCellId);
            writer.WriteShort(endCellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadDouble();
            if (targetId < -9007199254740990 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            startCellId = reader.ReadShort();
            if (startCellId < -1 || startCellId > 559)
                throw new Exception("Forbidden value on startCellId = " + startCellId + ", it doesn't respect the following condition : startCellId < -1 || startCellId > 559");
            endCellId = reader.ReadShort();
            if (endCellId < -1 || endCellId > 559)
                throw new Exception("Forbidden value on endCellId = " + endCellId + ", it doesn't respect the following condition : endCellId < -1 || endCellId > 559");
        }
        
    }
    
}