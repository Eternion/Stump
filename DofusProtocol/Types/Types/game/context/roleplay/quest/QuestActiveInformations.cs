

// Generated on 08/04/2015 00:35:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class QuestActiveInformations
    {
        public const short Id = 381;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short questId;
        
        public QuestActiveInformations()
        {
        }
        
        public QuestActiveInformations(short questId)
        {
            this.questId = questId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(questId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            questId = reader.ReadVarShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
        }
        
        
    }
    
}