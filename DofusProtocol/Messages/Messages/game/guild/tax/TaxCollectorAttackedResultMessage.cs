

// Generated on 09/01/2015 10:48:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Types.BasicGuildInformations guild;
        
        public TaxCollectorAttackedResultMessage()
        {
        }
        
        public TaxCollectorAttackedResultMessage(bool deadOrAlive, Types.TaxCollectorBasicInformations basicInfos, Types.BasicGuildInformations guild)
        {
            this.deadOrAlive = deadOrAlive;
            this.basicInfos = basicInfos;
            this.guild = guild;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(deadOrAlive);
            basicInfos.Serialize(writer);
            guild.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            deadOrAlive = reader.ReadBoolean();
            basicInfos = new Types.TaxCollectorBasicInformations();
            basicInfos.Deserialize(reader);
            guild = new Types.BasicGuildInformations();
            guild.Deserialize(reader);
        }
        
    }
    
}