

// Generated on 02/02/2016 14:14:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class BasicNamedAllianceInformations : BasicAllianceInformations
    {
        public const short Id = 418;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string allianceName;
        
        public BasicNamedAllianceInformations()
        {
        }
        
        public BasicNamedAllianceInformations(int allianceId, string allianceTag, string allianceName)
         : base(allianceId, allianceTag)
        {
            this.allianceName = allianceName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(allianceName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceName = reader.ReadUTF();
        }
        
        
    }
    
}