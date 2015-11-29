

// Generated on 11/16/2015 14:26:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PortalUseRequestMessage : Message
    {
        public const uint Id = 6492;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int portalId;
        
        public PortalUseRequestMessage()
        {
        }
        
        public PortalUseRequestMessage(int portalId)
        {
            this.portalId = portalId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(portalId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            portalId = reader.ReadVarInt();
            if (portalId < 0)
                throw new Exception("Forbidden value on portalId = " + portalId + ", it doesn't respect the following condition : portalId < 0");
        }
        
    }
    
}