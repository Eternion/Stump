
// Generated on 01/04/2013 14:35:47
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountSetMessage : Message
    {
        public const uint Id = 5968;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData mountData;
        
        public MountSetMessage()
        {
        }
        
        public MountSetMessage(Types.MountClientData mountData)
        {
            this.mountData = mountData;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            mountData.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountData = new Types.MountClientData();
            mountData.Deserialize(reader);
        }
        
    }
    
}