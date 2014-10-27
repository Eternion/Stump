

// Generated on 10/27/2014 19:57:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpouseStatusMessage : Message
    {
        public const uint Id = 6265;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hasSpouse;
        
        public SpouseStatusMessage()
        {
        }
        
        public SpouseStatusMessage(bool hasSpouse)
        {
            this.hasSpouse = hasSpouse;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hasSpouse);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hasSpouse = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}