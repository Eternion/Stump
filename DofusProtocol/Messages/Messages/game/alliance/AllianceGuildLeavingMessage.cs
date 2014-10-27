

// Generated on 10/27/2014 19:57:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceGuildLeavingMessage : Message
    {
        public const uint Id = 6399;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool kicked;
        public int guildId;
        
        public AllianceGuildLeavingMessage()
        {
        }
        
        public AllianceGuildLeavingMessage(bool kicked, int guildId)
        {
            this.kicked = kicked;
            this.guildId = guildId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteInt(guildId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kicked = reader.ReadBoolean();
            guildId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(int);
        }
        
    }
    
}