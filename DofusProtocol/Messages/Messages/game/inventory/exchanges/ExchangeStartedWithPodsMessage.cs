

// Generated on 02/02/2016 14:14:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartedWithPodsMessage : ExchangeStartedMessage
    {
        public const uint Id = 6129;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double firstCharacterId;
        public int firstCharacterCurrentWeight;
        public int firstCharacterMaxWeight;
        public double secondCharacterId;
        public int secondCharacterCurrentWeight;
        public int secondCharacterMaxWeight;
        
        public ExchangeStartedWithPodsMessage()
        {
        }
        
        public ExchangeStartedWithPodsMessage(sbyte exchangeType, double firstCharacterId, int firstCharacterCurrentWeight, int firstCharacterMaxWeight, double secondCharacterId, int secondCharacterCurrentWeight, int secondCharacterMaxWeight)
         : base(exchangeType)
        {
            this.firstCharacterId = firstCharacterId;
            this.firstCharacterCurrentWeight = firstCharacterCurrentWeight;
            this.firstCharacterMaxWeight = firstCharacterMaxWeight;
            this.secondCharacterId = secondCharacterId;
            this.secondCharacterCurrentWeight = secondCharacterCurrentWeight;
            this.secondCharacterMaxWeight = secondCharacterMaxWeight;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(firstCharacterId);
            writer.WriteVarInt(firstCharacterCurrentWeight);
            writer.WriteVarInt(firstCharacterMaxWeight);
            writer.WriteDouble(secondCharacterId);
            writer.WriteVarInt(secondCharacterCurrentWeight);
            writer.WriteVarInt(secondCharacterMaxWeight);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstCharacterId = reader.ReadDouble();
            if (firstCharacterId < -9.007199254740992E15 || firstCharacterId > 9.007199254740992E15)
                throw new Exception("Forbidden value on firstCharacterId = " + firstCharacterId + ", it doesn't respect the following condition : firstCharacterId < -9.007199254740992E15 || firstCharacterId > 9.007199254740992E15");
            firstCharacterCurrentWeight = reader.ReadVarInt();
            if (firstCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on firstCharacterCurrentWeight = " + firstCharacterCurrentWeight + ", it doesn't respect the following condition : firstCharacterCurrentWeight < 0");
            firstCharacterMaxWeight = reader.ReadVarInt();
            if (firstCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on firstCharacterMaxWeight = " + firstCharacterMaxWeight + ", it doesn't respect the following condition : firstCharacterMaxWeight < 0");
            secondCharacterId = reader.ReadDouble();
            if (secondCharacterId < -9.007199254740992E15 || secondCharacterId > 9.007199254740992E15)
                throw new Exception("Forbidden value on secondCharacterId = " + secondCharacterId + ", it doesn't respect the following condition : secondCharacterId < -9.007199254740992E15 || secondCharacterId > 9.007199254740992E15");
            secondCharacterCurrentWeight = reader.ReadVarInt();
            if (secondCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on secondCharacterCurrentWeight = " + secondCharacterCurrentWeight + ", it doesn't respect the following condition : secondCharacterCurrentWeight < 0");
            secondCharacterMaxWeight = reader.ReadVarInt();
            if (secondCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on secondCharacterMaxWeight = " + secondCharacterMaxWeight + ", it doesn't respect the following condition : secondCharacterMaxWeight < 0");
        }
        
    }
    
}