

// Generated on 10/27/2014 19:57:42
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
            var houses_before = writer.Position;
            var houses_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in houses)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 houses_count++;
            }
            var houses_after = writer.Position;
            writer.Seek((int)houses_before);
            writer.WriteUShort((ushort)houses_count);
            writer.Seek((int)houses_after);

            var actors_before = writer.Position;
            var actors_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in actors)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 actors_count++;
            }
            var actors_after = writer.Position;
            writer.Seek((int)actors_before);
            writer.WriteUShort((ushort)actors_count);
            writer.Seek((int)actors_after);

            var interactiveElements_before = writer.Position;
            var interactiveElements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 interactiveElements_count++;
            }
            var interactiveElements_after = writer.Position;
            writer.Seek((int)interactiveElements_before);
            writer.WriteUShort((ushort)interactiveElements_count);
            writer.Seek((int)interactiveElements_after);

            var statedElements_before = writer.Position;
            var statedElements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
                 statedElements_count++;
            }
            var statedElements_after = writer.Position;
            writer.Seek((int)statedElements_before);
            writer.WriteUShort((ushort)statedElements_count);
            writer.Seek((int)statedElements_after);

            var obstacles_before = writer.Position;
            var obstacles_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
                 obstacles_count++;
            }
            var obstacles_after = writer.Position;
            writer.Seek((int)obstacles_before);
            writer.WriteUShort((ushort)obstacles_count);
            writer.Seek((int)obstacles_after);

            var fights_before = writer.Position;
            var fights_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
                 fights_count++;
            }
            var fights_after = writer.Position;
            writer.Seek((int)fights_before);
            writer.WriteUShort((ushort)fights_count);
            writer.Seek((int)fights_after);

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
            var houses_ = new Types.HouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 houses_[i] = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
                 houses_[i].Deserialize(reader);
            }
            houses = houses_;
            limit = reader.ReadUShort();
            var actors_ = new Types.GameRolePlayActorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 actors_[i] = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
                 actors_[i].Deserialize(reader);
            }
            actors = actors_;
            limit = reader.ReadUShort();
            var interactiveElements_ = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 interactiveElements_[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 interactiveElements_[i].Deserialize(reader);
            }
            interactiveElements = interactiveElements_;
            limit = reader.ReadUShort();
            var statedElements_ = new Types.StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 statedElements_[i] = new Types.StatedElement();
                 statedElements_[i].Deserialize(reader);
            }
            statedElements = statedElements_;
            limit = reader.ReadUShort();
            var obstacles_ = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 obstacles_[i] = new Types.MapObstacle();
                 obstacles_[i].Deserialize(reader);
            }
            obstacles = obstacles_;
            limit = reader.ReadUShort();
            var fights_ = new Types.FightCommonInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fights_[i] = new Types.FightCommonInformations();
                 fights_[i].Deserialize(reader);
            }
            fights = fights_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(short) + houses.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + actors.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + interactiveElements.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + statedElements.Sum(x => x.GetSerializationSize()) + sizeof(short) + obstacles.Sum(x => x.GetSerializationSize()) + sizeof(short) + fights.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}