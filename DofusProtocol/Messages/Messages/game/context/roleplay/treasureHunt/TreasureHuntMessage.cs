

// Generated on 02/02/2016 14:14:26
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
        public IEnumerable<Types.TreasureHuntStep> knownStepsList;
        public sbyte totalStepCount;
        public int checkPointCurrent;
        public int checkPointTotal;
        public int availableRetryCount;
        public IEnumerable<Types.TreasureHuntFlag> flags;
        
        public TreasureHuntMessage()
        {
        }
        
        public TreasureHuntMessage(sbyte questType, int startMapId, IEnumerable<Types.TreasureHuntStep> knownStepsList, sbyte totalStepCount, int checkPointCurrent, int checkPointTotal, int availableRetryCount, IEnumerable<Types.TreasureHuntFlag> flags)
        {
            this.questType = questType;
            this.startMapId = startMapId;
            this.knownStepsList = knownStepsList;
            this.totalStepCount = totalStepCount;
            this.checkPointCurrent = checkPointCurrent;
            this.checkPointTotal = checkPointTotal;
            this.availableRetryCount = availableRetryCount;
            this.flags = flags;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteInt(startMapId);
            var knownStepsList_before = writer.Position;
            var knownStepsList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in knownStepsList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 knownStepsList_count++;
            }
            var knownStepsList_after = writer.Position;
            writer.Seek((int)knownStepsList_before);
            writer.WriteUShort((ushort)knownStepsList_count);
            writer.Seek((int)knownStepsList_after);

            writer.WriteSByte(totalStepCount);
            writer.WriteVarInt(checkPointCurrent);
            writer.WriteVarInt(checkPointTotal);
            writer.WriteInt(availableRetryCount);
            var flags_before = writer.Position;
            var flags_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in flags)
            {
                 entry.Serialize(writer);
                 flags_count++;
            }
            var flags_after = writer.Position;
            writer.Seek((int)flags_before);
            writer.WriteUShort((ushort)flags_count);
            writer.Seek((int)flags_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            startMapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            var knownStepsList_ = new Types.TreasureHuntStep[limit];
            for (int i = 0; i < limit; i++)
            {
                 knownStepsList_[i] = Types.ProtocolTypeManager.GetInstance<Types.TreasureHuntStep>(reader.ReadShort());
                 knownStepsList_[i].Deserialize(reader);
            }
            knownStepsList = knownStepsList_;
            totalStepCount = reader.ReadSByte();
            if (totalStepCount < 0)
                throw new Exception("Forbidden value on totalStepCount = " + totalStepCount + ", it doesn't respect the following condition : totalStepCount < 0");
            checkPointCurrent = reader.ReadVarInt();
            if (checkPointCurrent < 0)
                throw new Exception("Forbidden value on checkPointCurrent = " + checkPointCurrent + ", it doesn't respect the following condition : checkPointCurrent < 0");
            checkPointTotal = reader.ReadVarInt();
            if (checkPointTotal < 0)
                throw new Exception("Forbidden value on checkPointTotal = " + checkPointTotal + ", it doesn't respect the following condition : checkPointTotal < 0");
            availableRetryCount = reader.ReadInt();
            limit = reader.ReadUShort();
            var flags_ = new Types.TreasureHuntFlag[limit];
            for (int i = 0; i < limit; i++)
            {
                 flags_[i] = new Types.TreasureHuntFlag();
                 flags_[i].Deserialize(reader);
            }
            flags = flags_;
        }
        
    }
    
}