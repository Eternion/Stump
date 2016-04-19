

// Generated on 04/19/2016 10:17:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeInfoMessage : Message
    {
        public const uint Id = 6022;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short challengeId;
        public double targetId;
        public int xpBonus;
        public int dropBonus;
        
        public ChallengeInfoMessage()
        {
        }
        
        public ChallengeInfoMessage(short challengeId, double targetId, int xpBonus, int dropBonus)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
            this.xpBonus = xpBonus;
            this.dropBonus = dropBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(challengeId);
            writer.WriteDouble(targetId);
            writer.WriteVarInt(xpBonus);
            writer.WriteVarInt(dropBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadVarShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadDouble();
            if (targetId < -9007199254740990 || targetId > 9007199254740990)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9007199254740990 || targetId > 9007199254740990");
            xpBonus = reader.ReadVarInt();
            if (xpBonus < 0)
                throw new Exception("Forbidden value on xpBonus = " + xpBonus + ", it doesn't respect the following condition : xpBonus < 0");
            dropBonus = reader.ReadVarInt();
            if (dropBonus < 0)
                throw new Exception("Forbidden value on dropBonus = " + dropBonus + ", it doesn't respect the following condition : dropBonus < 0");
        }
        
    }
    
}