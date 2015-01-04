

// Generated on 01/04/2015 11:54:06
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
            var guilds_before = writer.Position;
            var guilds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
                 guilds_count++;
            }
            var guilds_after = writer.Position;
            writer.Seek((int)guilds_before);
            writer.WriteUShort((ushort)guilds_count);
            writer.Seek((int)guilds_after);

            var prisms_before = writer.Position;
            var prisms_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 prisms_count++;
            }
            var prisms_after = writer.Position;
            writer.Seek((int)prisms_before);
            writer.WriteUShort((ushort)prisms_count);
            writer.Seek((int)prisms_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            allianceInfos = new Types.AllianceFactSheetInformations();
            allianceInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            var guilds_ = new Types.GuildInsiderFactSheetInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guilds_[i] = new Types.GuildInsiderFactSheetInformations();
                 guilds_[i].Deserialize(reader);
            }
            guilds = guilds_;
            limit = reader.ReadUShort();
            var prisms_ = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 prisms_[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 prisms_[i].Deserialize(reader);
            }
            prisms = prisms_;
        }
        
    }
    
}