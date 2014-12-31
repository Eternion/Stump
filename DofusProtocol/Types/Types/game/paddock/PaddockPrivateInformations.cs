

// Generated on 12/29/2014 21:14:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PaddockPrivateInformations : PaddockAbandonnedInformations
    {
        public const short Id = 131;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInfo;
        
        public PaddockPrivateInformations()
        {
        }
        
        public PaddockPrivateInformations(short maxOutdoorMount, short maxItems, int price, bool locked, int guildId, Types.GuildInformations guildInfo)
         : base(maxOutdoorMount, maxItems, price, locked, guildId)
        {
            this.guildInfo = guildInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + guildInfo.GetSerializationSize();
        }
        
    }
    
}