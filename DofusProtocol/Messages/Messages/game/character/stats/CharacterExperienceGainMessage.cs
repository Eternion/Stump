

// Generated on 10/27/2014 19:57:36
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
        
        public double experienceCharacter;
        public double experienceMount;
        public double experienceGuild;
        public double experienceIncarnation;
        
        public CharacterExperienceGainMessage()
        {
        }
        
        public CharacterExperienceGainMessage(double experienceCharacter, double experienceMount, double experienceGuild, double experienceIncarnation)
        {
            this.experienceCharacter = experienceCharacter;
            this.experienceMount = experienceMount;
            this.experienceGuild = experienceGuild;
            this.experienceIncarnation = experienceIncarnation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(experienceCharacter);
            writer.WriteDouble(experienceMount);
            writer.WriteDouble(experienceGuild);
            writer.WriteDouble(experienceIncarnation);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experienceCharacter = reader.ReadDouble();
            if (experienceCharacter < 0 || experienceCharacter > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceCharacter = " + experienceCharacter + ", it doesn't respect the following condition : experienceCharacter < 0 || experienceCharacter > 9.007199254740992E15");
            experienceMount = reader.ReadDouble();
            if (experienceMount < 0 || experienceMount > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceMount = " + experienceMount + ", it doesn't respect the following condition : experienceMount < 0 || experienceMount > 9.007199254740992E15");
            experienceGuild = reader.ReadDouble();
            if (experienceGuild < 0 || experienceGuild > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceGuild = " + experienceGuild + ", it doesn't respect the following condition : experienceGuild < 0 || experienceGuild > 9.007199254740992E15");
            experienceIncarnation = reader.ReadDouble();
            if (experienceIncarnation < 0 || experienceIncarnation > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceIncarnation = " + experienceIncarnation + ", it doesn't respect the following condition : experienceIncarnation < 0 || experienceIncarnation > 9.007199254740992E15");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(double) + sizeof(double) + sizeof(double);
        }
        
    }
    
}