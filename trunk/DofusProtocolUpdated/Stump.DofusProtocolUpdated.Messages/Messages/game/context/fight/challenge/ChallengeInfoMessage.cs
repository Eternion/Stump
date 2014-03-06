

// Generated on 03/06/2014 18:50:09
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
        public int targetId;
        public int xpBonus;
        public int dropBonus;
        
        public ChallengeInfoMessage()
        {
        }
        
        public ChallengeInfoMessage(short challengeId, int targetId, int xpBonus, int dropBonus)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
            this.xpBonus = xpBonus;
            this.dropBonus = dropBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(challengeId);
            writer.WriteInt(targetId);
            writer.WriteInt(xpBonus);
            writer.WriteInt(dropBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
            xpBonus = reader.ReadInt();
            if (xpBonus < 0)
                throw new Exception("Forbidden value on xpBonus = " + xpBonus + ", it doesn't respect the following condition : xpBonus < 0");
            dropBonus = reader.ReadInt();
            if (dropBonus < 0)
                throw new Exception("Forbidden value on dropBonus = " + dropBonus + ", it doesn't respect the following condition : dropBonus < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(int) + sizeof(int);
        }
        
    }
    
}