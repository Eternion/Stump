

// Generated on 09/26/2016 01:50:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobExperienceOtherPlayerUpdateMessage : JobExperienceUpdateMessage
    {
        public const uint Id = 6599;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long playerId;
        
        public JobExperienceOtherPlayerUpdateMessage()
        {
        }
        
        public JobExperienceOtherPlayerUpdateMessage(Types.JobExperience experiencesUpdate, long playerId)
         : base(experiencesUpdate)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
        }
        
    }
    
}