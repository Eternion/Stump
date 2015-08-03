

// Generated on 08/04/2015 00:37:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SymbioticObjectAssociatedMessage : Message
    {
        public const uint Id = 6527;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int hostUID;
        
        public SymbioticObjectAssociatedMessage()
        {
        }
        
        public SymbioticObjectAssociatedMessage(int hostUID)
        {
            this.hostUID = hostUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(hostUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hostUID = reader.ReadVarInt();
            if (hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + hostUID + ", it doesn't respect the following condition : hostUID < 0");
        }
        
    }
    
}