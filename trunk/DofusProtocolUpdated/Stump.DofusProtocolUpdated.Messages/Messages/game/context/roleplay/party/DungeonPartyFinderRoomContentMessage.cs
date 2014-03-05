

// Generated on 03/05/2014 20:34:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderRoomContentMessage : Message
    {
        public const uint Id = 6247;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public IEnumerable<Types.DungeonPartyFinderPlayer> players;
        
        public DungeonPartyFinderRoomContentMessage()
        {
        }
        
        public DungeonPartyFinderRoomContentMessage(short dungeonId, IEnumerable<Types.DungeonPartyFinderPlayer> players)
        {
            this.dungeonId = dungeonId;
            this.players = players;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(dungeonId);
            var players_before = writer.Position;
            var players_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in players)
            {
                 entry.Serialize(writer);
                 players_count++;
            }
            var players_after = writer.Position;
            writer.Seek((int)players_before);
            writer.WriteUShort((ushort)players_count);
            writer.Seek((int)players_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            var players_ = new Types.DungeonPartyFinderPlayer[limit];
            for (int i = 0; i < limit; i++)
            {
                 players_[i] = new Types.DungeonPartyFinderPlayer();
                 players_[i].Deserialize(reader);
            }
            players = players_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + players.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}