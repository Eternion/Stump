

// Generated on 12/12/2013 16:57:03
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
            writer.WriteUShort((ushort)npcsIdsWithQuest.Count());
            foreach (var entry in npcsIdsWithQuest)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)questFlags.Count());
            foreach (var entry in questFlags)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)npcsIdsWithoutQuest.Count());
            foreach (var entry in npcsIdsWithoutQuest)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            npcsIdsWithQuest = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (npcsIdsWithQuest as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            questFlags = new Types.GameRolePlayNpcQuestFlag[limit];
            for (int i = 0; i < limit; i++)
            {
                 (questFlags as Types.GameRolePlayNpcQuestFlag[])[i] = new Types.GameRolePlayNpcQuestFlag();
                 (questFlags as Types.GameRolePlayNpcQuestFlag[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            npcsIdsWithoutQuest = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (npcsIdsWithoutQuest as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + npcsIdsWithQuest.Sum(x => sizeof(int)) + sizeof(short) + questFlags.Sum(x => x.GetSerializationSize()) + sizeof(short) + npcsIdsWithoutQuest.Sum(x => sizeof(int));
        }
        
    }
    
}