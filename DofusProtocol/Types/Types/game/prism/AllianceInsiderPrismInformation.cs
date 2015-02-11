

// Generated on 02/11/2015 10:21:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AllianceInsiderPrismInformation : PrismInformation
    {
        public const short Id = 431;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int lastTimeSlotModificationDate;
        public int lastTimeSlotModificationAuthorGuildId;
        public int lastTimeSlotModificationAuthorId;
        public string lastTimeSlotModificationAuthorName;
        public IEnumerable<int> modulesItemIds;
        
        public AllianceInsiderPrismInformation()
        {
        }
        
        public AllianceInsiderPrismInformation(sbyte typeId, sbyte state, int nextVulnerabilityDate, int placementDate, int rewardTokenCount, int lastTimeSlotModificationDate, int lastTimeSlotModificationAuthorGuildId, int lastTimeSlotModificationAuthorId, string lastTimeSlotModificationAuthorName, IEnumerable<int> modulesItemIds)
         : base(typeId, state, nextVulnerabilityDate, placementDate, rewardTokenCount)
        {
            this.lastTimeSlotModificationDate = lastTimeSlotModificationDate;
            this.lastTimeSlotModificationAuthorGuildId = lastTimeSlotModificationAuthorGuildId;
            this.lastTimeSlotModificationAuthorId = lastTimeSlotModificationAuthorId;
            this.lastTimeSlotModificationAuthorName = lastTimeSlotModificationAuthorName;
            this.modulesItemIds = modulesItemIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lastTimeSlotModificationDate);
            writer.WriteVarInt(lastTimeSlotModificationAuthorGuildId);
            writer.WriteVarInt(lastTimeSlotModificationAuthorId);
            writer.WriteUTF(lastTimeSlotModificationAuthorName);
            var modulesItemIds_before = writer.Position;
            var modulesItemIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in modulesItemIds)
            {
                 writer.WriteVarInt(entry);
                 modulesItemIds_count++;
            }
            var modulesItemIds_after = writer.Position;
            writer.Seek((int)modulesItemIds_before);
            writer.WriteUShort((ushort)modulesItemIds_count);
            writer.Seek((int)modulesItemIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            lastTimeSlotModificationDate = reader.ReadInt();
            if (lastTimeSlotModificationDate < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationDate = " + lastTimeSlotModificationDate + ", it doesn't respect the following condition : lastTimeSlotModificationDate < 0");
            lastTimeSlotModificationAuthorGuildId = reader.ReadVarInt();
            if (lastTimeSlotModificationAuthorGuildId < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorGuildId = " + lastTimeSlotModificationAuthorGuildId + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorGuildId < 0");
            lastTimeSlotModificationAuthorId = reader.ReadVarInt();
            if (lastTimeSlotModificationAuthorId < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorId = " + lastTimeSlotModificationAuthorId + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorId < 0");
            lastTimeSlotModificationAuthorName = reader.ReadUTF();
            var limit = reader.ReadUShort();
            var modulesItemIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 modulesItemIds_[i] = reader.ReadVarInt();
            }
            modulesItemIds = modulesItemIds_;
        }
        
        
    }
    
}