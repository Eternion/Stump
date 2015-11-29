

// Generated on 11/16/2015 14:26:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EmotePlayMessage : EmotePlayAbstractMessage
    {
        public const uint Id = 5683;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int actorId;
        public int accountId;
        
        public EmotePlayMessage()
        {
        }
        
        public EmotePlayMessage(byte emoteId, double emoteStartTime, int actorId, int accountId)
         : base(emoteId, emoteStartTime)
        {
            this.actorId = actorId;
            this.accountId = accountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(actorId);
            writer.WriteInt(accountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            actorId = reader.ReadInt();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
        }
        
    }
    
}