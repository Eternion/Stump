

// Generated on 03/05/2014 20:34:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SelectedServerDataExtendedMessage : SelectedServerDataMessage
    {
        public const uint Id = 6469;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> serverIds;
        
        public SelectedServerDataExtendedMessage()
        {
        }
        
        public SelectedServerDataExtendedMessage(short serverId, string address, ushort port, bool canCreateNewCharacter, string ticket, IEnumerable<short> serverIds)
         : base(serverId, address, port, canCreateNewCharacter, ticket)
        {
            this.serverIds = serverIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var serverIds_before = writer.Position;
            var serverIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in serverIds)
            {
                 writer.WriteShort(entry);
                 serverIds_count++;
            }
            var serverIds_after = writer.Position;
            writer.Seek((int)serverIds_before);
            writer.WriteUShort((ushort)serverIds_count);
            writer.Seek((int)serverIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var serverIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 serverIds_[i] = reader.ReadShort();
            }
            serverIds = serverIds_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + serverIds.Sum(x => sizeof(short));
        }
        
    }
    
}