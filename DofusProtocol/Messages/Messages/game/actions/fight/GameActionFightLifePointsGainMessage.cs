

// Generated on 10/27/2014 19:57:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightLifePointsGainMessage : AbstractGameActionMessage
    {
        public const uint Id = 6311;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short delta;
        
        public GameActionFightLifePointsGainMessage()
        {
        }
        
        public GameActionFightLifePointsGainMessage(short actionId, int sourceId, int targetId, short delta)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(delta);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadShort();
            if (delta < 0)
                throw new Exception("Forbidden value on delta = " + delta + ", it doesn't respect the following condition : delta < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short);
        }
        
    }
    
}