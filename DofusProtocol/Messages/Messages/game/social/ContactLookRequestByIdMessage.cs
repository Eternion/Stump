

// Generated on 09/01/2015 10:48:28
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
        
        public int playerId;
        
        public ContactLookRequestByIdMessage()
        {
        }
        
        public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, int playerId)
         : base(requestId, contactType)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}