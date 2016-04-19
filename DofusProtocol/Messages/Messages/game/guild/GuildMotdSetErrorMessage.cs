

// Generated on 04/19/2016 10:17:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMotdSetErrorMessage : Message
    {
        public const uint Id = 6591;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public GuildMotdSetErrorMessage()
        {
        }
        
        public GuildMotdSetErrorMessage(sbyte reason)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}