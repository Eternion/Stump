

// Generated on 10/26/2014 23:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightJoinRequestMessage : Message
    {
        public const uint Id = 5717;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int taxCollectorId;
        
        public GuildFightJoinRequestMessage()
        {
        }
        
        public GuildFightJoinRequestMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(taxCollectorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            taxCollectorId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}