

// Generated on 08/04/2015 00:37:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TreasureHuntFlagRequestAnswerMessage : Message
    {
        public const uint Id = 6507;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte questType;
        public sbyte result;
        public sbyte index;
        
        public TreasureHuntFlagRequestAnswerMessage()
        {
        }
        
        public TreasureHuntFlagRequestAnswerMessage(sbyte questType, sbyte result, sbyte index)
        {
            this.questType = questType;
            this.result = result;
            this.index = index;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteSByte(result);
            writer.WriteSByte(index);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
            index = reader.ReadSByte();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
        }
        
    }
    
}