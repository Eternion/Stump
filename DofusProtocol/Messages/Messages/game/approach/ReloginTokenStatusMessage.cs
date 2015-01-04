

// Generated on 01/04/2015 11:54:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ReloginTokenStatusMessage : Message
    {
        public const uint Id = 6539;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool validToken;
        public string token;
        
        public ReloginTokenStatusMessage()
        {
        }
        
        public ReloginTokenStatusMessage(bool validToken, string token)
        {
            this.validToken = validToken;
            this.token = token;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(validToken);
            writer.WriteUTF(token);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            validToken = reader.ReadBoolean();
            token = reader.ReadUTF();
        }
        
    }
    
}