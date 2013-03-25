
// Generated on 03/25/2013 19:24:03
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NumericWhoIsRequestMessage : Message
    {
        public const uint Id = 6298;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        
        public NumericWhoIsRequestMessage()
        {
        }
        
        public NumericWhoIsRequestMessage(int playerId)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}