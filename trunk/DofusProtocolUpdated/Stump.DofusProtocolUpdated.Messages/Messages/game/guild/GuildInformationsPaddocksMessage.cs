

// Generated on 12/12/2013 16:57:10
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
            writer.WriteUShort((ushort)paddocksInformations.Count());
            foreach (var entry in paddocksInformations)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbPaddockMax = reader.ReadSByte();
            if (nbPaddockMax < 0)
                throw new Exception("Forbidden value on nbPaddockMax = " + nbPaddockMax + ", it doesn't respect the following condition : nbPaddockMax < 0");
            var limit = reader.ReadUShort();
            paddocksInformations = new Types.PaddockContentInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (paddocksInformations as Types.PaddockContentInformations[])[i] = new Types.PaddockContentInformations();
                 (paddocksInformations as Types.PaddockContentInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(short) + paddocksInformations.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}