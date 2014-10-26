

// Generated on 10/26/2014 23:30:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorGuildInformations : TaxCollectorComplementaryInformations
    {
        public const short Id = 446;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.BasicGuildInformations guild;
        
        public TaxCollectorGuildInformations()
        {
        }
        
        public TaxCollectorGuildInformations(Types.BasicGuildInformations guild)
        {
            this.guild = guild;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guild.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guild = new Types.BasicGuildInformations();
            guild.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + guild.GetSerializationSize();
        }
        
    }
    
}