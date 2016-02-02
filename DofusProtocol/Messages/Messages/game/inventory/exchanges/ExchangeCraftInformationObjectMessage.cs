

// Generated on 02/02/2016 14:14:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftInformationObjectMessage : ExchangeCraftResultWithObjectIdMessage
    {
        public const uint Id = 5794;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long playerId;
        
        public ExchangeCraftInformationObjectMessage()
        {
        }
        
        public ExchangeCraftInformationObjectMessage(sbyte craftResult, short objectGenericId, long playerId)
         : base(craftResult, objectGenericId)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9.007199254740992E15)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9.007199254740992E15");
        }
        
    }
    
}