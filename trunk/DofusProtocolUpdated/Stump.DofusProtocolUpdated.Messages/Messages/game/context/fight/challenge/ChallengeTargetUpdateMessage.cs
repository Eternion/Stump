

// Generated on 03/05/2014 20:34:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeTargetUpdateMessage : Message
    {
        public const uint Id = 6123;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short challengeId;
        public int targetId;
        
        public ChallengeTargetUpdateMessage()
        {
        }
        
        public ChallengeTargetUpdateMessage(short challengeId, int targetId)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(challengeId);
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int);
        }
        
    }
    
}