

// Generated on 12/29/2014 21:13:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInformationsPaddocksMessage : Message
    {
        public const uint Id = 5959;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte nbPaddockMax;
        public IEnumerable<Types.PaddockContentInformations> paddocksInformations;
        
        public GuildInformationsPaddocksMessage()
        {
        }
        
        public GuildInformationsPaddocksMessage(sbyte nbPaddockMax, IEnumerable<Types.PaddockContentInformations> paddocksInformations)
        {
            this.nbPaddockMax = nbPaddockMax;
            this.paddocksInformations = paddocksInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(nbPaddockMax);
            var paddocksInformations_before = writer.Position;
            var paddocksInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in paddocksInformations)
            {
                 entry.Serialize(writer);
                 paddocksInformations_count++;
            }
            var paddocksInformations_after = writer.Position;
            writer.Seek((int)paddocksInformations_before);
            writer.WriteUShort((ushort)paddocksInformations_count);
            writer.Seek((int)paddocksInformations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbPaddockMax = reader.ReadSByte();
            if (nbPaddockMax < 0)
                throw new Exception("Forbidden value on nbPaddockMax = " + nbPaddockMax + ", it doesn't respect the following condition : nbPaddockMax < 0");
            var limit = reader.ReadUShort();
            var paddocksInformations_ = new Types.PaddockContentInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 paddocksInformations_[i] = new Types.PaddockContentInformations();
                 paddocksInformations_[i].Deserialize(reader);
            }
            paddocksInformations = paddocksInformations_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + paddocksInformations.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}