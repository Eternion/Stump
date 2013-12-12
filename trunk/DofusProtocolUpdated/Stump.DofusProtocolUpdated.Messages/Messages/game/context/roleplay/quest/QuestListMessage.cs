

// Generated on 12/12/2013 16:57:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestListMessage : Message
    {
        public const uint Id = 5626;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> finishedQuestsIds;
        public IEnumerable<short> finishedQuestsCounts;
        public IEnumerable<Types.QuestActiveInformations> activeQuests;
        
        public QuestListMessage()
        {
        }
        
        public QuestListMessage(IEnumerable<short> finishedQuestsIds, IEnumerable<short> finishedQuestsCounts, IEnumerable<Types.QuestActiveInformations> activeQuests)
        {
            this.finishedQuestsIds = finishedQuestsIds;
            this.finishedQuestsCounts = finishedQuestsCounts;
            this.activeQuests = activeQuests;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)finishedQuestsIds.Count());
            foreach (var entry in finishedQuestsIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)finishedQuestsCounts.Count());
            foreach (var entry in finishedQuestsCounts)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)activeQuests.Count());
            foreach (var entry in activeQuests)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            finishedQuestsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedQuestsIds as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            finishedQuestsCounts = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedQuestsCounts as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            activeQuests = new Types.QuestActiveInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (activeQuests as Types.QuestActiveInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.QuestActiveInformations>(reader.ReadShort());
                 (activeQuests as Types.QuestActiveInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + finishedQuestsIds.Sum(x => sizeof(short)) + sizeof(short) + finishedQuestsCounts.Sum(x => sizeof(short)) + sizeof(short) + activeQuests.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}