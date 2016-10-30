

// Generated on 10/30/2016 16:20:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            writer.WriteVarShort(experiencePercent);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            experiencePercent = reader.ReadVarShort();
            if (experiencePercent < 0)
                throw new Exception("Forbidden value on experiencePercent = " + experiencePercent + ", it doesn't respect the following condition : experiencePercent < 0");
        }
        
    }
    
}