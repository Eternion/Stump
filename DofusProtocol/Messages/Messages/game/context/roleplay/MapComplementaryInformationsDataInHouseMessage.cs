

// Generated on 12/29/2014 21:12:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapComplementaryInformationsDataInHouseMessage : MapComplementaryInformationsDataMessage
    {
        public const uint Id = 6130;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.HouseInformationsInside currentHouse;
        
        public MapComplementaryInformationsDataInHouseMessage()
        {
        }
        
        public MapComplementaryInformationsDataInHouseMessage(short subAreaId, int mapId, IEnumerable<Types.HouseInformations> houses, IEnumerable<Types.GameRolePlayActorInformations> actors, IEnumerable<Types.InteractiveElement> interactiveElements, IEnumerable<Types.StatedElement> statedElements, IEnumerable<Types.MapObstacle> obstacles, IEnumerable<Types.FightCommonInformations> fights, Types.HouseInformationsInside currentHouse)
         : base(subAreaId, mapId, houses, actors, interactiveElements, statedElements, obstacles, fights)
        {
            this.currentHouse = currentHouse;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            currentHouse.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            currentHouse = new Types.HouseInformationsInside();
            currentHouse.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + currentHouse.GetSerializationSize();
        }
        
    }
    
}