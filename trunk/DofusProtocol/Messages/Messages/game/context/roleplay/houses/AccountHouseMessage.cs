

// Generated on 07/29/2013 23:07:56
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AccountHouseMessage : Message
    {
        public const uint Id = 6315;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.AccountHouseInformations> houses;
        
        public AccountHouseMessage()
        {
        }
        
        public AccountHouseMessage(IEnumerable<Types.AccountHouseInformations> houses)
        {
            this.houses = houses;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)houses.Count());
            foreach (var entry in houses)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            houses = new Types.AccountHouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (houses as Types.AccountHouseInformations[])[i] = new Types.AccountHouseInformations();
                 (houses as Types.AccountHouseInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + houses.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}