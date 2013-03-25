
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerExperienceModificatorMessage : Message
    {
        public const uint Id = 6237;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short experiencePercent;
        
        public ServerExperienceModificatorMessage()
        {
        }
        
        public ServerExperienceModificatorMessage(short experiencePercent)
        {
            this.experiencePercent = experiencePercent;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(experiencePercent);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experiencePercent = reader.ReadShort();
            if (experiencePercent < 0)
                throw new Exception("Forbidden value on experiencePercent = " + experiencePercent + ", it doesn't respect the following condition : experiencePercent < 0");
        }
        
    }
    
}