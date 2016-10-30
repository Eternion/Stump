

// Generated on 10/30/2016 16:20:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountSterilizedMessage : Message
    {
        public const uint Id = 5977;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mountId;
        
        public MountSterilizedMessage()
        {
        }
        
        public MountSterilizedMessage(int mountId)
        {
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadVarInt();
        }
        
    }
    
}