

// Generated on 12/12/2013 16:56:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceListMessage : Message
    {
        public const uint Id = 6408;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.AllianceFactSheetInformations> alliances;
        
        public AllianceListMessage()
        {
        }
        
        public AllianceListMessage(IEnumerable<Types.AllianceFactSheetInformations> alliances)
        {
            this.alliances = alliances;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)alliances.Count());
            foreach (var entry in alliances)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            alliances = new Types.AllianceFactSheetInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alliances as Types.AllianceFactSheetInformations[])[i] = new Types.AllianceFactSheetInformations();
                 (alliances as Types.AllianceFactSheetInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + alliances.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}