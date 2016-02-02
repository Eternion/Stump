

// Generated on 02/02/2016 14:14:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ContactLookRequestByIdMessage : ContactLookRequestMessage
    {
        public const uint Id = 5935;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long playerId;
        
        public ContactLookRequestByIdMessage()
        {
        }
        
        public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, long playerId)
         : base(requestId, contactType)
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