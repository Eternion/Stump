

// Generated on 10/30/2016 16:20:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementDetailedListRequestMessage : Message
    {
        public const uint Id = 6357;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short categoryId;
        
        public AchievementDetailedListRequestMessage()
        {
        }
        
        public AchievementDetailedListRequestMessage(short categoryId)
        {
            this.categoryId = categoryId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(categoryId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            categoryId = reader.ReadVarShort();
            if (categoryId < 0)
                throw new Exception("Forbidden value on categoryId = " + categoryId + ", it doesn't respect the following condition : categoryId < 0");
        }
        
    }
    
}