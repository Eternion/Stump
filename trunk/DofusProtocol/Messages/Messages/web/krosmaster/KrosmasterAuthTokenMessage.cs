
// Generated on 03/25/2013 19:24:26
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KrosmasterAuthTokenMessage : Message
    {
        public const uint Id = 6351;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string token;
        
        public KrosmasterAuthTokenMessage()
        {
        }
        
        public KrosmasterAuthTokenMessage(string token)
        {
            this.token = token;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(token);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            token = reader.ReadUTF();
        }
        
    }
    
}