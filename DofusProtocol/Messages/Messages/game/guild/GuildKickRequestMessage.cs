

// Generated on 04/19/2016 10:17:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildKickRequestMessage : Message
    {
        public const uint Id = 5887;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long kickedId;
        
        public GuildKickRequestMessage()
        {
        }
        
        public GuildKickRequestMessage(long kickedId)
        {
            this.kickedId = kickedId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(kickedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kickedId = reader.ReadVarLong();
            if (kickedId < 0 || kickedId > 9007199254740990)
                throw new Exception("Forbidden value on kickedId = " + kickedId + ", it doesn't respect the following condition : kickedId < 0 || kickedId > 9007199254740990");
        }
        
    }
    
}