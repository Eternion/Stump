

// Generated on 02/02/2016 14:14:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderRoomContentUpdateMessage : Message
    {
        public const uint Id = 6250;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        public IEnumerable<Types.DungeonPartyFinderPlayer> addedPlayers;
        public IEnumerable<long> removedPlayersIds;
        
        public DungeonPartyFinderRoomContentUpdateMessage()
        {
        }
        
        public DungeonPartyFinderRoomContentUpdateMessage(short dungeonId, IEnumerable<Types.DungeonPartyFinderPlayer> addedPlayers, IEnumerable<long> removedPlayersIds)
        {
            this.dungeonId = dungeonId;
            this.addedPlayers = addedPlayers;
            this.removedPlayersIds = removedPlayersIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(dungeonId);
            var addedPlayers_before = writer.Position;
            var addedPlayers_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in addedPlayers)
            {
                 entry.Serialize(writer);
                 addedPlayers_count++;
            }
            var addedPlayers_after = writer.Position;
            writer.Seek((int)addedPlayers_before);
            writer.WriteUShort((ushort)addedPlayers_count);
            writer.Seek((int)addedPlayers_after);

            var removedPlayersIds_before = writer.Position;
            var removedPlayersIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in removedPlayersIds)
            {
                 writer.WriteVarLong(entry);
                 removedPlayersIds_count++;
            }
            var removedPlayersIds_after = writer.Position;
            writer.Seek((int)removedPlayersIds_before);
            writer.WriteUShort((ushort)removedPlayersIds_count);
            writer.Seek((int)removedPlayersIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            dungeonId = reader.ReadVarShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            var addedPlayers_ = new Types.DungeonPartyFinderPlayer[limit];
            for (int i = 0; i < limit; i++)
            {
                 addedPlayers_[i] = new Types.DungeonPartyFinderPlayer();
                 addedPlayers_[i].Deserialize(reader);
            }
            addedPlayers = addedPlayers_;
            limit = reader.ReadUShort();
            var removedPlayersIds_ = new long[limit];
            for (int i = 0; i < limit; i++)
            {
                 removedPlayersIds_[i] = reader.ReadVarLong();
            }
            removedPlayersIds = removedPlayersIds_;
        }
        
    }
    
}