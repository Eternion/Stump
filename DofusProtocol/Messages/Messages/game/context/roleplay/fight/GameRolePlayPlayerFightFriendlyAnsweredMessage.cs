

// Generated on 10/30/2016 16:20:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayPlayerFightFriendlyAnsweredMessage : Message
    {
        public const uint Id = 5733;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public long sourceId;
        public long targetId;
        public bool accept;
        
        public GameRolePlayPlayerFightFriendlyAnsweredMessage()
        {
        }
        
        public GameRolePlayPlayerFightFriendlyAnsweredMessage(int fightId, long sourceId, long targetId, bool accept)
        {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteVarLong(sourceId);
            writer.WriteVarLong(targetId);
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            sourceId = reader.ReadVarLong();
            if (sourceId < 0 || sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < 0 || sourceId > 9007199254740990");
            targetId = reader.ReadVarLong();
            if (targetId < 0 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0 || targetId > 9007199254740990");
            accept = reader.ReadBoolean();
        }
        
    }
    
}