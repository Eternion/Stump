

// Generated on 10/26/2014 23:29:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TreasureHuntDigRequestAnswerFailedMessage : TreasureHuntDigRequestAnswerMessage
    {
        public const uint Id = 6509;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte wrongFlagCount;
        
        public TreasureHuntDigRequestAnswerFailedMessage()
        {
        }
        
        public TreasureHuntDigRequestAnswerFailedMessage(sbyte questType, sbyte result, sbyte wrongFlagCount)
         : base(questType, result)
        {
            this.wrongFlagCount = wrongFlagCount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(wrongFlagCount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            wrongFlagCount = reader.ReadSByte();
            if (wrongFlagCount < 0)
                throw new Exception("Forbidden value on wrongFlagCount = " + wrongFlagCount + ", it doesn't respect the following condition : wrongFlagCount < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}