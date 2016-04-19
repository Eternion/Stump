

// Generated on 04/19/2016 10:17:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapComplementaryInformationsDataInHavenBagMessage : MapComplementaryInformationsDataMessage
    {
        public const uint Id = 6622;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterMinimalInformations ownerInformations;
        public sbyte theme;
        public sbyte roomId;
        public sbyte maxRoomId;
        
        public MapComplementaryInformationsDataInHavenBagMessage()
        {
        }
        
        public MapComplementaryInformationsDataInHavenBagMessage(short subAreaId, int mapId, IEnumerable<Types.HouseInformations> houses, IEnumerable<Types.GameRolePlayActorInformations> actors, IEnumerable<Types.InteractiveElement> interactiveElements, IEnumerable<Types.StatedElement> statedElements, IEnumerable<Types.MapObstacle> obstacles, IEnumerable<Types.FightCommonInformations> fights, Types.CharacterMinimalInformations ownerInformations, sbyte theme, sbyte roomId, sbyte maxRoomId)
         : base(subAreaId, mapId, houses, actors, interactiveElements, statedElements, obstacles, fights)
        {
            this.ownerInformations = ownerInformations;
            this.theme = theme;
            this.roomId = roomId;
            this.maxRoomId = maxRoomId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            ownerInformations.Serialize(writer);
            writer.WriteSByte(theme);
            writer.WriteSByte(roomId);
            writer.WriteSByte(maxRoomId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            ownerInformations = new Types.CharacterMinimalInformations();
            ownerInformations.Deserialize(reader);
            theme = reader.ReadSByte();
            roomId = reader.ReadSByte();
            if (roomId < 0)
                throw new Exception("Forbidden value on roomId = " + roomId + ", it doesn't respect the following condition : roomId < 0");
            maxRoomId = reader.ReadSByte();
            if (maxRoomId < 0)
                throw new Exception("Forbidden value on maxRoomId = " + maxRoomId + ", it doesn't respect the following condition : maxRoomId < 0");
        }
        
    }
    
}