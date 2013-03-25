
// Generated on 03/25/2013 19:24:21
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage
    {
        public const uint Id = 5979;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.MountClientData> paddockedMountsDescription;
        
        public ExchangeStartOkMountMessage()
        {
        }
        
        public ExchangeStartOkMountMessage(IEnumerable<Types.MountClientData> stabledMountsDescription, IEnumerable<Types.MountClientData> paddockedMountsDescription)
         : base(stabledMountsDescription)
        {
            this.paddockedMountsDescription = paddockedMountsDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)paddockedMountsDescription.Count());
            foreach (var entry in paddockedMountsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            paddockedMountsDescription = new Types.MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 (paddockedMountsDescription as Types.MountClientData[])[i] = new Types.MountClientData();
                 (paddockedMountsDescription as Types.MountClientData[])[i].Deserialize(reader);
            }
        }
        
    }
    
}