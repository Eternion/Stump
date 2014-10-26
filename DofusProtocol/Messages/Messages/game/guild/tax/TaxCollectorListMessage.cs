

// Generated on 10/26/2014 23:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorListMessage : Message
    {
        public const uint Id = 5930;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte nbcollectorMax;
        public IEnumerable<Types.TaxCollectorInformations> informations;
        public IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations;
        
        public TaxCollectorListMessage()
        {
        }
        
        public TaxCollectorListMessage(sbyte nbcollectorMax, IEnumerable<Types.TaxCollectorInformations> informations, IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations)
        {
            this.nbcollectorMax = nbcollectorMax;
            this.informations = informations;
            this.fightersInformations = fightersInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(nbcollectorMax);
            var informations_before = writer.Position;
            var informations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in informations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 informations_count++;
            }
            var informations_after = writer.Position;
            writer.Seek((int)informations_before);
            writer.WriteUShort((ushort)informations_count);
            writer.Seek((int)informations_after);

            var fightersInformations_before = writer.Position;
            var fightersInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fightersInformations)
            {
                 entry.Serialize(writer);
                 fightersInformations_count++;
            }
            var fightersInformations_after = writer.Position;
            writer.Seek((int)fightersInformations_before);
            writer.WriteUShort((ushort)fightersInformations_count);
            writer.Seek((int)fightersInformations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbcollectorMax = reader.ReadSByte();
            if (nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            var limit = reader.ReadUShort();
            var informations_ = new Types.TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 informations_[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
                 informations_[i].Deserialize(reader);
            }
            informations = informations_;
            limit = reader.ReadUShort();
            var fightersInformations_ = new Types.TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightersInformations_[i] = new Types.TaxCollectorFightersInformation();
                 fightersInformations_[i].Deserialize(reader);
            }
            fightersInformations = fightersInformations_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + informations.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + fightersInformations.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}