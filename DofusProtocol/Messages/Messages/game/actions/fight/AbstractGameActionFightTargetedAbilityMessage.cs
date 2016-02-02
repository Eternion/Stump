

// Generated on 02/02/2016 14:14:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AbstractGameActionFightTargetedAbilityMessage : AbstractGameActionMessage
    {
        public const uint Id = 6118;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double targetId;
        public short destinationCellId;
        public sbyte critical;
        public bool silentCast;
        
        public AbstractGameActionFightTargetedAbilityMessage()
        {
        }
        
        public AbstractGameActionFightTargetedAbilityMessage(short actionId, double sourceId, double targetId, short destinationCellId, sbyte critical, bool silentCast)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.destinationCellId = destinationCellId;
            this.critical = critical;
            this.silentCast = silentCast;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(targetId);
            writer.WriteShort(destinationCellId);
            writer.WriteSByte(critical);
            writer.WriteBoolean(silentCast);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadDouble();
            if (targetId < -9.007199254740992E15 || targetId > 9.007199254740992E15)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9.007199254740992E15 || targetId > 9.007199254740992E15");
            destinationCellId = reader.ReadShort();
            if (destinationCellId < -1 || destinationCellId > 559)
                throw new Exception("Forbidden value on destinationCellId = " + destinationCellId + ", it doesn't respect the following condition : destinationCellId < -1 || destinationCellId > 559");
            critical = reader.ReadSByte();
            if (critical < 0)
                throw new Exception("Forbidden value on critical = " + critical + ", it doesn't respect the following condition : critical < 0");
            silentCast = reader.ReadBoolean();
        }
        
    }
    
}