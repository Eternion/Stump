

// Generated on 03/05/2014 20:34:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FriendSpouseOnlineInformations : FriendSpouseInformations
    {
        public const short Id = 93;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int mapId;
        public short subAreaId;
        
        public FriendSpouseOnlineInformations()
        {
        }
        
        public FriendSpouseOnlineInformations(int spouseAccountId, int spouseId, string spouseName, byte spouseLevel, sbyte breed, sbyte sex, Types.EntityLook spouseEntityLook, Types.BasicGuildInformations guildInfo, sbyte alignmentSide, int mapId, short subAreaId)
         : base(spouseAccountId, spouseId, spouseName, spouseLevel, breed, sex, spouseEntityLook, guildInfo, alignmentSide)
        {
            this.mapId = mapId;
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mapId);
            writer.WriteShort(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short);
        }
        
    }
    
}