

// Generated on 10/30/2016 16:20:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterExperienceGainMessage : Message
    {
        public const uint Id = 6321;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long experienceCharacter;
        public long experienceMount;
        public long experienceGuild;
        public long experienceIncarnation;
        
        public CharacterExperienceGainMessage()
        {
        }
        
        public CharacterExperienceGainMessage(long experienceCharacter, long experienceMount, long experienceGuild, long experienceIncarnation)
        {
            this.experienceCharacter = experienceCharacter;
            this.experienceMount = experienceMount;
            this.experienceGuild = experienceGuild;
            this.experienceIncarnation = experienceIncarnation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(experienceCharacter);
            writer.WriteVarLong(experienceMount);
            writer.WriteVarLong(experienceGuild);
            writer.WriteVarLong(experienceIncarnation);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experienceCharacter = reader.ReadVarLong();
            if (experienceCharacter < 0 || experienceCharacter > 9007199254740990)
                throw new Exception("Forbidden value on experienceCharacter = " + experienceCharacter + ", it doesn't respect the following condition : experienceCharacter < 0 || experienceCharacter > 9007199254740990");
            experienceMount = reader.ReadVarLong();
            if (experienceMount < 0 || experienceMount > 9007199254740990)
                throw new Exception("Forbidden value on experienceMount = " + experienceMount + ", it doesn't respect the following condition : experienceMount < 0 || experienceMount > 9007199254740990");
            experienceGuild = reader.ReadVarLong();
            if (experienceGuild < 0 || experienceGuild > 9007199254740990)
                throw new Exception("Forbidden value on experienceGuild = " + experienceGuild + ", it doesn't respect the following condition : experienceGuild < 0 || experienceGuild > 9007199254740990");
            experienceIncarnation = reader.ReadVarLong();
            if (experienceIncarnation < 0 || experienceIncarnation > 9007199254740990)
                throw new Exception("Forbidden value on experienceIncarnation = " + experienceIncarnation + ", it doesn't respect the following condition : experienceIncarnation < 0 || experienceIncarnation > 9007199254740990");
        }
        
    }
    
}