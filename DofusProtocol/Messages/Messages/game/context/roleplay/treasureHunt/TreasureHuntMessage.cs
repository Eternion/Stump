

// Generated on 09/01/2014 15:52:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TreasureHuntMessage : Message
    {
        public const uint Id = 6486;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte questType;
        public int startMapId;
        public IEnumerable<Types.TreasureHuntStep> stepList;
        public int checkPointCurrent;
        public int checkPointTotal;
        public int availableRetryCount;
        
        public TreasureHuntMessage()
        {
        }
        
        public TreasureHuntMessage(sbyte questType, int startMapId, IEnumerable<Types.TreasureHuntStep> stepList, int checkPointCurrent, int checkPointTotal, int availableRetryCount)
        {
            this.questType = questType;
            this.startMapId = startMapId;
            this.stepList = stepList;
            this.checkPointCurrent = checkPointCurrent;
            this.checkPointTotal = checkPointTotal;
            this.availableRetryCount = availableRetryCount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteInt(startMapId);
            var stepList_before = writer.Position;
            var stepList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in stepList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 stepList_count++;
            }
            var stepList_after = writer.Position;
            writer.Seek((int)stepList_before);
            writer.WriteUShort((ushort)stepList_count);
            writer.Seek((int)stepList_after);

            writer.WriteInt(checkPointCurrent);
            writer.WriteInt(checkPointTotal);
            writer.WriteInt(availableRetryCount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            startMapId = reader.ReadInt();
            if (startMapId < 0)
                throw new Exception("Forbidden value on startMapId = " + startMapId + ", it doesn't respect the following condition : startMapId < 0");
            var limit = reader.ReadUShort();
            var stepList_ = new Types.TreasureHuntStep[limit];
            for (int i = 0; i < limit; i++)
            {
                 stepList_[i] = Types.ProtocolTypeManager.GetInstance<Types.TreasureHuntStep>(reader.ReadShort());
                 stepList_[i].Deserialize(reader);
            }
            stepList = stepList_;
            checkPointCurrent = reader.ReadInt();
            if (checkPointCurrent < 0)
                throw new Exception("Forbidden value on checkPointCurrent = " + checkPointCurrent + ", it doesn't respect the following condition : checkPointCurrent < 0");
            checkPointTotal = reader.ReadInt();
            if (checkPointTotal < 0)
                throw new Exception("Forbidden value on checkPointTotal = " + checkPointTotal + ", it doesn't respect the following condition : checkPointTotal < 0");
            availableRetryCount = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(int) + sizeof(short) + stepList.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(int) + sizeof(int) + sizeof(int);
        }
        
    }
    
}