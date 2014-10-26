

// Generated on 10/26/2014 23:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildPaddockTeleportRequestMessage : Message
    {
        public const uint Id = 5957;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int paddockId;
        
        public GuildPaddockTeleportRequestMessage()
        {
        }
        
        public GuildPaddockTeleportRequestMessage(int paddockId)
        {
            this.paddockId = paddockId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(paddockId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paddockId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}