

// Generated on 12/29/2014 21:13:51
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
            writer.WriteInt(hostUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hostUID = reader.ReadInt();
            if (hostUID < 0)
                throw new Exception("Forbidden value on hostUID = " + hostUID + ", it doesn't respect the following condition : hostUID < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}