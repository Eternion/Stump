

// Generated on 12/12/2013 16:57:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MimicryObjectAssociatedMessage : Message
    {
        public const uint Id = 6462;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int hostUID;
        
        public MimicryObjectAssociatedMessage()
        {
        }
        
        public MimicryObjectAssociatedMessage(int hostUID)
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