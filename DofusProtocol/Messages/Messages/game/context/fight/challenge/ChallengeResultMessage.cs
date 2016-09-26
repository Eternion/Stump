

// Generated on 09/26/2016 01:49:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeResultMessage : Message
    {
        public const uint Id = 6019;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short challengeId;
        public bool success;
        
        public ChallengeResultMessage()
        {
        }
        
        public ChallengeResultMessage(short challengeId, bool success)
        {
            this.challengeId = challengeId;
            this.success = success;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(challengeId);
            writer.WriteBoolean(success);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            challengeId = reader.ReadVarShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            success = reader.ReadBoolean();
        }
        
    }
    
}