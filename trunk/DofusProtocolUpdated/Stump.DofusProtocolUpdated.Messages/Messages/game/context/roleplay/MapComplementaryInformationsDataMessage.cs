

// Generated on 12/12/2013 16:56:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapComplementaryInformationsDataMessage : Message
    {
        public const uint Id = 226;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public int mapId;
        public IEnumerable<Types.HouseInformations> houses;
        public IEnumerable<Types.GameRolePlayActorInformations> actors;
        public IEnumerable<Types.InteractiveElement> interactiveElements;
        public IEnumerable<Types.StatedElement> statedElements;
        public IEnumerable<Types.MapObstacle> obstacles;
        public IEnumerable<Types.FightCommonInformations> fights;
        
        public MapComplementaryInformationsDataMessage()
        {
        }
        
        public MapComplementaryInformationsDataMessage(short subAreaId, int mapId, IEnumerable<Types.HouseInformations> houses, IEnumerable<Types.GameRolePlayActorInformations> actors, IEnumerable<Types.InteractiveElement> interactiveElements, IEnumerable<Types.StatedElement> statedElements, IEnumerable<Types.MapObstacle> obstacles, IEnumerable<Types.FightCommonInformations> fights)
        {
            this.subAreaId = subAreaId;
            this.mapId = mapId;
            this.houses = houses;
            this.actors = actors;
            this.interactiveElements = interactiveElements;
            this.statedElements = statedElements;
            this.obstacles = obstacles;
            this.fights = fights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteInt(mapId);
            writer.WriteUShort((ushort)houses.Count());
            foreach (var entry in houses)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)actors.Count());
            foreach (var entry in actors)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)interactiveElements.Count());
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)statedElements.Count());
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)obstacles.Count());
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fights.Count());
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            var limit = reader.ReadUShort();
            houses = new Types.HouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (houses as Types.HouseInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
                 (houses as Types.HouseInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            actors = new Types.GameRolePlayActorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (actors as Types.GameRolePlayActorInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
                 (actors as Types.GameRolePlayActorInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            interactiveElements = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (interactiveElements as Types.InteractiveElement[])[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 (interactiveElements as Types.InteractiveElement[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            statedElements = new Types.StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (statedElements as Types.StatedElement[])[i] = new Types.StatedElement();
                 (statedElements as Types.StatedElement[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            obstacles = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 (obstacles as Types.MapObstacle[])[i] = new Types.MapObstacle();
                 (obstacles as Types.MapObstacle[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fights = new Types.FightCommonInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fights as Types.FightCommonInformations[])[i] = new Types.FightCommonInformations();
                 (fights as Types.FightCommonInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(short) + houses.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + actors.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + interactiveElements.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + statedElements.Sum(x => x.GetSerializationSize()) + sizeof(short) + obstacles.Sum(x => x.GetSerializationSize()) + sizeof(short) + fights.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}