

// Generated on 09/26/2016 01:50:24
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
        public long lastTimeSlotModificationAuthorId;
        public string lastTimeSlotModificationAuthorName;
        public IEnumerable<Types.ObjectItem> modulesObjects;
        
        public AllianceInsiderPrismInformation()
        {
        }
        
        public AllianceInsiderPrismInformation(sbyte typeId, sbyte state, int nextVulnerabilityDate, int placementDate, int rewardTokenCount, int lastTimeSlotModificationDate, int lastTimeSlotModificationAuthorGuildId, long lastTimeSlotModificationAuthorId, string lastTimeSlotModificationAuthorName, IEnumerable<Types.ObjectItem> modulesObjects)
         : base(typeId, state, nextVulnerabilityDate, placementDate, rewardTokenCount)
        {
            this.lastTimeSlotModificationDate = lastTimeSlotModificationDate;
            this.lastTimeSlotModificationAuthorGuildId = lastTimeSlotModificationAuthorGuildId;
            this.lastTimeSlotModificationAuthorId = lastTimeSlotModificationAuthorId;
            this.lastTimeSlotModificationAuthorName = lastTimeSlotModificationAuthorName;
            this.modulesObjects = modulesObjects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lastTimeSlotModificationDate);
            writer.WriteVarInt(lastTimeSlotModificationAuthorGuildId);
            writer.WriteVarLong(lastTimeSlotModificationAuthorId);
            writer.WriteUTF(lastTimeSlotModificationAuthorName);
            var modulesObjects_before = writer.Position;
            var modulesObjects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in modulesObjects)
            {
                 entry.Serialize(writer);
                 modulesObjects_count++;
            }
            var modulesObjects_after = writer.Position;
            writer.Seek((int)modulesObjects_before);
            writer.WriteUShort((ushort)modulesObjects_count);
            writer.Seek((int)modulesObjects_after);

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
            lastTimeSlotModificationAuthorId = reader.ReadVarLong();
            if (lastTimeSlotModificationAuthorId < 0 || lastTimeSlotModificationAuthorId > 9007199254740990)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorId = " + lastTimeSlotModificationAuthorId + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorId < 0 || lastTimeSlotModificationAuthorId > 9007199254740990");
            lastTimeSlotModificationAuthorName = reader.ReadUTF();
            var limit = reader.ReadUShort();
            var modulesObjects_ = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 modulesObjects_[i] = new Types.ObjectItem();
                 modulesObjects_[i].Deserialize(reader);
            }
            modulesObjects = modulesObjects_;
        }
        
        
    }
    
}