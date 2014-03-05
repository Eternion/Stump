

// Generated on 03/05/2014 20:34:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountPaddockAddMessage : Message
    {
        public const uint Id = 6049;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData mountDescription;
        
        public ExchangeMountPaddockAddMessage()
        {
        }
        
        public ExchangeMountPaddockAddMessage(Types.MountClientData mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            mountDescription.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountDescription = new Types.MountClientData();
            mountDescription.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return mountDescription.GetSerializationSize();
        }
        
    }
    
}