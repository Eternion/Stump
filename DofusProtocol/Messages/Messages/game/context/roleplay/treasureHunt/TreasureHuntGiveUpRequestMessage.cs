

// Generated on 11/16/2015 14:26:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TreasureHuntGiveUpRequestMessage : Message
    {
        public const uint Id = 6487;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte questType;
        
        public TreasureHuntGiveUpRequestMessage()
        {
        }
        
        public TreasureHuntGiveUpRequestMessage(sbyte questType)
        {
            this.questType = questType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(questType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
        }
        
    }
    
}