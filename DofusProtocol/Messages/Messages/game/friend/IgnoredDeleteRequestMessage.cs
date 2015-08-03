

// Generated on 08/04/2015 00:37:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IgnoredDeleteRequestMessage : Message
    {
        public const uint Id = 5680;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int accountId;
        public bool session;
        
        public IgnoredDeleteRequestMessage()
        {
        }
        
        public IgnoredDeleteRequestMessage(int accountId, bool session)
        {
            this.accountId = accountId;
            this.session = session;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(accountId);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            session = reader.ReadBoolean();
        }
        
    }
    
}