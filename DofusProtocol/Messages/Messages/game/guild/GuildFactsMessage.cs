

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFactsMessage : Message
    {
        public const uint Id = 6415;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GuildFactSheetInformations infos;
        public int creationDate;
        public short nbTaxCollectors;
        public bool enabled;
        public IEnumerable<Types.CharacterMinimalInformations> members;
        
        public GuildFactsMessage()
        {
        }
        
        public GuildFactsMessage(Types.GuildFactSheetInformations infos, int creationDate, short nbTaxCollectors, bool enabled, IEnumerable<Types.CharacterMinimalInformations> members)
        {
            this.infos = infos;
            this.creationDate = creationDate;
            this.nbTaxCollectors = nbTaxCollectors;
            this.enabled = enabled;
            this.members = members;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(infos.TypeId);
            infos.Serialize(writer);
            writer.WriteInt(creationDate);
            writer.WriteVarShort(nbTaxCollectors);
            writer.WriteBoolean(enabled);
            var members_before = writer.Position;
            var members_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
                 members_count++;
            }
            var members_after = writer.Position;
            writer.Seek((int)members_before);
            writer.WriteUShort((ushort)members_count);
            writer.Seek((int)members_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            infos = Types.ProtocolTypeManager.GetInstance<Types.GuildFactSheetInformations>(reader.ReadShort());
            infos.Deserialize(reader);
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
            nbTaxCollectors = reader.ReadVarShort();
            if (nbTaxCollectors < 0)
                throw new Exception("Forbidden value on nbTaxCollectors = " + nbTaxCollectors + ", it doesn't respect the following condition : nbTaxCollectors < 0");
            enabled = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var members_ = new Types.CharacterMinimalInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 members_[i] = new Types.CharacterMinimalInformations();
                 members_[i].Deserialize(reader);
            }
            members = members_;
        }
        
    }
    
}