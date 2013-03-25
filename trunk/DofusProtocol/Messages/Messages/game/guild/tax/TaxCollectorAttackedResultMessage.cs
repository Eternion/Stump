
// Generated on 03/25/2013 19:24:17
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorAttackedResultMessage : Message
    {
        public const uint Id = 5635;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool deadOrAlive;
        public Types.TaxCollectorBasicInformations basicInfos;
        
        public TaxCollectorAttackedResultMessage()
        {
        }
        
        public TaxCollectorAttackedResultMessage(bool deadOrAlive, Types.TaxCollectorBasicInformations basicInfos)
        {
            this.deadOrAlive = deadOrAlive;
            this.basicInfos = basicInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(deadOrAlive);
            basicInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            deadOrAlive = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
        }
        
    }
    
}