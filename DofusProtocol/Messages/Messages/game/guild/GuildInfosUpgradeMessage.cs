

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildInfosUpgradeMessage : Message
    {
        public const uint Id = 5636;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte maxTaxCollectorsCount;
        public sbyte taxCollectorsCount;
        public short taxCollectorLifePoints;
        public short taxCollectorDamagesBonuses;
        public short taxCollectorPods;
        public short taxCollectorProspecting;
        public short taxCollectorWisdom;
        public short boostPoints;
        public IEnumerable<short> spellId;
        public IEnumerable<sbyte> spellLevel;
        
        public GuildInfosUpgradeMessage()
        {
        }
        
        public GuildInfosUpgradeMessage(sbyte maxTaxCollectorsCount, sbyte taxCollectorsCount, short taxCollectorLifePoints, short taxCollectorDamagesBonuses, short taxCollectorPods, short taxCollectorProspecting, short taxCollectorWisdom, short boostPoints, IEnumerable<short> spellId, IEnumerable<sbyte> spellLevel)
        {
            this.maxTaxCollectorsCount = maxTaxCollectorsCount;
            this.taxCollectorsCount = taxCollectorsCount;
            this.taxCollectorLifePoints = taxCollectorLifePoints;
            this.taxCollectorDamagesBonuses = taxCollectorDamagesBonuses;
            this.taxCollectorPods = taxCollectorPods;
            this.taxCollectorProspecting = taxCollectorProspecting;
            this.taxCollectorWisdom = taxCollectorWisdom;
            this.boostPoints = boostPoints;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(maxTaxCollectorsCount);
            writer.WriteSByte(taxCollectorsCount);
            writer.WriteVarShort(taxCollectorLifePoints);
            writer.WriteVarShort(taxCollectorDamagesBonuses);
            writer.WriteVarShort(taxCollectorPods);
            writer.WriteVarShort(taxCollectorProspecting);
            writer.WriteVarShort(taxCollectorWisdom);
            writer.WriteVarShort(boostPoints);
            var spellId_before = writer.Position;
            var spellId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in spellId)
            {
                 writer.WriteVarShort(entry);
                 spellId_count++;
            }
            var spellId_after = writer.Position;
            writer.Seek((int)spellId_before);
            writer.WriteUShort((ushort)spellId_count);
            writer.Seek((int)spellId_after);

            var spellLevel_before = writer.Position;
            var spellLevel_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in spellLevel)
            {
                 writer.WriteSByte(entry);
                 spellLevel_count++;
            }
            var spellLevel_after = writer.Position;
            writer.Seek((int)spellLevel_before);
            writer.WriteUShort((ushort)spellLevel_count);
            writer.Seek((int)spellLevel_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            maxTaxCollectorsCount = reader.ReadSByte();
            if (maxTaxCollectorsCount < 0)
                throw new Exception("Forbidden value on maxTaxCollectorsCount = " + maxTaxCollectorsCount + ", it doesn't respect the following condition : maxTaxCollectorsCount < 0");
            taxCollectorsCount = reader.ReadSByte();
            if (taxCollectorsCount < 0)
                throw new Exception("Forbidden value on taxCollectorsCount = " + taxCollectorsCount + ", it doesn't respect the following condition : taxCollectorsCount < 0");
            taxCollectorLifePoints = reader.ReadVarShort();
            if (taxCollectorLifePoints < 0)
                throw new Exception("Forbidden value on taxCollectorLifePoints = " + taxCollectorLifePoints + ", it doesn't respect the following condition : taxCollectorLifePoints < 0");
            taxCollectorDamagesBonuses = reader.ReadVarShort();
            if (taxCollectorDamagesBonuses < 0)
                throw new Exception("Forbidden value on taxCollectorDamagesBonuses = " + taxCollectorDamagesBonuses + ", it doesn't respect the following condition : taxCollectorDamagesBonuses < 0");
            taxCollectorPods = reader.ReadVarShort();
            if (taxCollectorPods < 0)
                throw new Exception("Forbidden value on taxCollectorPods = " + taxCollectorPods + ", it doesn't respect the following condition : taxCollectorPods < 0");
            taxCollectorProspecting = reader.ReadVarShort();
            if (taxCollectorProspecting < 0)
                throw new Exception("Forbidden value on taxCollectorProspecting = " + taxCollectorProspecting + ", it doesn't respect the following condition : taxCollectorProspecting < 0");
            taxCollectorWisdom = reader.ReadVarShort();
            if (taxCollectorWisdom < 0)
                throw new Exception("Forbidden value on taxCollectorWisdom = " + taxCollectorWisdom + ", it doesn't respect the following condition : taxCollectorWisdom < 0");
            boostPoints = reader.ReadVarShort();
            if (boostPoints < 0)
                throw new Exception("Forbidden value on boostPoints = " + boostPoints + ", it doesn't respect the following condition : boostPoints < 0");
            var limit = reader.ReadUShort();
            var spellId_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellId_[i] = reader.ReadVarShort();
            }
            spellId = spellId_;
            limit = reader.ReadUShort();
            var spellLevel_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 spellLevel_[i] = reader.ReadSByte();
            }
            spellLevel = spellLevel_;
        }
        
    }
    
}