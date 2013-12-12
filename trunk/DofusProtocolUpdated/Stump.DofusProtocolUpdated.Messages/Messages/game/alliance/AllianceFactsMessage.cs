

// Generated on 12/12/2013 16:56:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceFactsMessage : Message
    {
        public const uint Id = 6414;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AllianceFactSheetInformations infos;
        public IEnumerable<Types.GuildInAllianceInformations> guilds;
        public IEnumerable<short> controlledSubareaIds;
        
        public AllianceFactsMessage()
        {
        }
        
        public AllianceFactsMessage(Types.AllianceFactSheetInformations infos, IEnumerable<Types.GuildInAllianceInformations> guilds, IEnumerable<short> controlledSubareaIds)
        {
            this.infos = infos;
            this.guilds = guilds;
            this.controlledSubareaIds = controlledSubareaIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(infos.TypeId);
            infos.Serialize(writer);
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)controlledSubareaIds.Count());
            foreach (var entry in controlledSubareaIds)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            infos = Types.ProtocolTypeManager.GetInstance<Types.AllianceFactSheetInformations>(reader.ReadShort());
            infos.Deserialize(reader);
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInAllianceInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInAllianceInformations[])[i] = new Types.GuildInAllianceInformations();
                 (guilds as Types.GuildInAllianceInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            controlledSubareaIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (controlledSubareaIds as short[])[i] = reader.ReadShort();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + infos.GetSerializationSize() + sizeof(short) + guilds.Sum(x => x.GetSerializationSize()) + sizeof(short) + controlledSubareaIds.Sum(x => sizeof(short));
        }
        
    }
    
}