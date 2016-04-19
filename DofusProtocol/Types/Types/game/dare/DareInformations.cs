

// Generated on 04/19/2016 10:17:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class DareInformations
    {
        public const short Id = 502;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public double dareId;
        public Types.CharacterBasicMinimalInformations creator;
        public int subscriptionFee;
        public int jackpot;
        public ushort maxCountWinners;
        public double endDate;
        public bool isPrivate;
        public int guildId;
        public int allianceId;
        public IEnumerable<Types.DareCriteria> criterions;
        public double startDate;
        
        public DareInformations()
        {
        }
        
        public DareInformations(double dareId, Types.CharacterBasicMinimalInformations creator, int subscriptionFee, int jackpot, ushort maxCountWinners, double endDate, bool isPrivate, int guildId, int allianceId, IEnumerable<Types.DareCriteria> criterions, double startDate)
        {
            this.dareId = dareId;
            this.creator = creator;
            this.subscriptionFee = subscriptionFee;
            this.jackpot = jackpot;
            this.maxCountWinners = maxCountWinners;
            this.endDate = endDate;
            this.isPrivate = isPrivate;
            this.guildId = guildId;
            this.allianceId = allianceId;
            this.criterions = criterions;
            this.startDate = startDate;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(dareId);
            creator.Serialize(writer);
            writer.WriteInt(subscriptionFee);
            writer.WriteInt(jackpot);
            writer.WriteUShort(maxCountWinners);
            writer.WriteDouble(endDate);
            writer.WriteBoolean(isPrivate);
            writer.WriteVarInt(guildId);
            writer.WriteVarInt(allianceId);
            var criterions_before = writer.Position;
            var criterions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in criterions)
            {
                 entry.Serialize(writer);
                 criterions_count++;
            }
            var criterions_after = writer.Position;
            writer.Seek((int)criterions_before);
            writer.WriteUShort((ushort)criterions_count);
            writer.Seek((int)criterions_after);

            writer.WriteDouble(startDate);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            dareId = reader.ReadDouble();
            if (dareId < 0 || dareId > 9007199254740990)
                throw new Exception("Forbidden value on dareId = " + dareId + ", it doesn't respect the following condition : dareId < 0 || dareId > 9007199254740990");
            creator = new Types.CharacterBasicMinimalInformations();
            creator.Deserialize(reader);
            subscriptionFee = reader.ReadInt();
            if (subscriptionFee < 0)
                throw new Exception("Forbidden value on subscriptionFee = " + subscriptionFee + ", it doesn't respect the following condition : subscriptionFee < 0");
            jackpot = reader.ReadInt();
            if (jackpot < 0)
                throw new Exception("Forbidden value on jackpot = " + jackpot + ", it doesn't respect the following condition : jackpot < 0");
            maxCountWinners = reader.ReadUShort();
            if (maxCountWinners < 0 || maxCountWinners > 65535)
                throw new Exception("Forbidden value on maxCountWinners = " + maxCountWinners + ", it doesn't respect the following condition : maxCountWinners < 0 || maxCountWinners > 65535");
            endDate = reader.ReadDouble();
            if (endDate < 0 || endDate > 9007199254740990)
                throw new Exception("Forbidden value on endDate = " + endDate + ", it doesn't respect the following condition : endDate < 0 || endDate > 9007199254740990");
            isPrivate = reader.ReadBoolean();
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            allianceId = reader.ReadVarInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
            var limit = reader.ReadUShort();
            var criterions_ = new Types.DareCriteria[limit];
            for (int i = 0; i < limit; i++)
            {
                 criterions_[i] = new Types.DareCriteria();
                 criterions_[i].Deserialize(reader);
            }
            criterions = criterions_;
            startDate = reader.ReadDouble();
            if (startDate < 0 || startDate > 9007199254740990)
                throw new Exception("Forbidden value on startDate = " + startDate + ", it doesn't respect the following condition : startDate < 0 || startDate > 9007199254740990");
        }
        
        
    }
    
}