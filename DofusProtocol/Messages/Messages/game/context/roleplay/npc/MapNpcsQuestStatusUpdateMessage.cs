

// Generated on 02/02/2016 14:14:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapNpcsQuestStatusUpdateMessage : Message
    {
        public const uint Id = 5642;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        public IEnumerable<int> npcsIdsWithQuest;
        public IEnumerable<Types.GameRolePlayNpcQuestFlag> questFlags;
        public IEnumerable<int> npcsIdsWithoutQuest;
        
        public MapNpcsQuestStatusUpdateMessage()
        {
        }
        
        public MapNpcsQuestStatusUpdateMessage(int mapId, IEnumerable<int> npcsIdsWithQuest, IEnumerable<Types.GameRolePlayNpcQuestFlag> questFlags, IEnumerable<int> npcsIdsWithoutQuest)
        {
            this.mapId = mapId;
            this.npcsIdsWithQuest = npcsIdsWithQuest;
            this.questFlags = questFlags;
            this.npcsIdsWithoutQuest = npcsIdsWithoutQuest;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
            var npcsIdsWithQuest_before = writer.Position;
            var npcsIdsWithQuest_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in npcsIdsWithQuest)
            {
                 writer.WriteInt(entry);
                 npcsIdsWithQuest_count++;
            }
            var npcsIdsWithQuest_after = writer.Position;
            writer.Seek((int)npcsIdsWithQuest_before);
            writer.WriteUShort((ushort)npcsIdsWithQuest_count);
            writer.Seek((int)npcsIdsWithQuest_after);

            var questFlags_before = writer.Position;
            var questFlags_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in questFlags)
            {
                 entry.Serialize(writer);
                 questFlags_count++;
            }
            var questFlags_after = writer.Position;
            writer.Seek((int)questFlags_before);
            writer.WriteUShort((ushort)questFlags_count);
            writer.Seek((int)questFlags_after);

            var npcsIdsWithoutQuest_before = writer.Position;
            var npcsIdsWithoutQuest_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in npcsIdsWithoutQuest)
            {
                 writer.WriteInt(entry);
                 npcsIdsWithoutQuest_count++;
            }
            var npcsIdsWithoutQuest_after = writer.Position;
            writer.Seek((int)npcsIdsWithoutQuest_before);
            writer.WriteUShort((ushort)npcsIdsWithoutQuest_count);
            writer.Seek((int)npcsIdsWithoutQuest_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            var npcsIdsWithQuest_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 npcsIdsWithQuest_[i] = reader.ReadInt();
            }
            npcsIdsWithQuest = npcsIdsWithQuest_;
            limit = reader.ReadUShort();
            var questFlags_ = new Types.GameRolePlayNpcQuestFlag[limit];
            for (int i = 0; i < limit; i++)
            {
                 questFlags_[i] = new Types.GameRolePlayNpcQuestFlag();
                 questFlags_[i].Deserialize(reader);
            }
            questFlags = questFlags_;
            limit = reader.ReadUShort();
            var npcsIdsWithoutQuest_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 npcsIdsWithoutQuest_[i] = reader.ReadInt();
            }
            npcsIdsWithoutQuest = npcsIdsWithoutQuest_;
        }
        
    }
    
}