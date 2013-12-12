

// Generated on 12/12/2013 16:56:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceInsiderInfoMessage : Message
    {
        public const uint Id = 6403;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AllianceFactSheetInformations allianceInfos;
        public IEnumerable<Types.GuildInsiderFactSheetInformations> guilds;
        public IEnumerable<Types.PrismSubareaEmptyInfo> prisms;
        
        public AllianceInsiderInfoMessage()
        {
        }
        
        public AllianceInsiderInfoMessage(Types.AllianceFactSheetInformations allianceInfos, IEnumerable<Types.GuildInsiderFactSheetInformations> guilds, IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
        {
            this.allianceInfos = allianceInfos;
            this.guilds = guilds;
            this.prisms = prisms;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            allianceInfos.Serialize(writer);
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prisms.Count());
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allianceInfos = new Types.AllianceFactSheetInformations();
            allianceInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInsiderFactSheetInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInsiderFactSheetInformations[])[i] = new Types.GuildInsiderFactSheetInformations();
                 (guilds as Types.GuildInsiderFactSheetInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prisms as Types.PrismSubareaEmptyInfo[])[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 (prisms as Types.PrismSubareaEmptyInfo[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return allianceInfos.GetSerializationSize() + sizeof(short) + guilds.Sum(x => x.GetSerializationSize()) + sizeof(short) + prisms.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}