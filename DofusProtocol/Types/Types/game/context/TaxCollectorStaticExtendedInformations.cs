

// Generated on 10/30/2016 16:20:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorStaticExtendedInformations : TaxCollectorStaticInformations
    {
        public const short Id = 440;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.AllianceInformations allianceIdentity;
        
        public TaxCollectorStaticExtendedInformations()
        {
        }
        
        public TaxCollectorStaticExtendedInformations(short firstNameId, short lastNameId, Types.GuildInformations guildIdentity, Types.AllianceInformations allianceIdentity)
         : base(firstNameId, lastNameId, guildIdentity)
        {
            this.allianceIdentity = allianceIdentity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            allianceIdentity.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            allianceIdentity = new Types.AllianceInformations();
            allianceIdentity.Deserialize(reader);
        }
        
        
    }
    
}