

// Generated on 10/30/2016 16:20:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountRenameRequestMessage : Message
    {
        public const uint Id = 5987;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public int mountId;
        
        public MountRenameRequestMessage()
        {
        }
        
        public MountRenameRequestMessage(string name, int mountId)
        {
            this.name = name;
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteVarInt(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            mountId = reader.ReadVarInt();
        }
        
    }
    
}