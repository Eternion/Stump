

// Generated on 11/16/2015 14:26:08
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
        
        public int playerId;
        
        public JobExperienceOtherPlayerUpdateMessage()
        {
        }
        
        public JobExperienceOtherPlayerUpdateMessage(Types.JobExperience experiencesUpdate, int playerId)
         : base(experiencesUpdate)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}