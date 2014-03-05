

// Generated on 03/05/2014 20:34:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformationsExtended : HouseInformations
    {
        public const short Id = 112;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInfo;
        
        public HouseInformationsExtended()
        {
        }
        
        public HouseInformationsExtended(int houseId, IEnumerable<int> doorsOnMap, string ownerName, short modelId, Types.GuildInformations guildInfo)
         : base(houseId, doorsOnMap, ownerName, modelId)
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