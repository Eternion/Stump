

// Generated on 09/26/2016 01:49:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameActionFightKillMessage : AbstractGameActionMessage
    {
        public const uint Id = 5571;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double targetId;
        
        public GameActionFightKillMessage()
        {
        }
        
        public GameActionFightKillMessage(short actionId, double sourceId, double targetId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadDouble();
            if (targetId < -9007199254740990 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
        }
        
    }
    
}