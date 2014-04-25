

// Generated on 03/02/2014 20:43:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class MountClientData
    {
        public const short Id = 178;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool sex;
        public bool isRideable;
        public bool isWild;
        public bool isFecondationReady;
        public double id;
        public int model;
        public IEnumerable<int> ancestor;
        public IEnumerable<int> behaviors;
        public string name;
        public int ownerId;
        public double experience;
        public double experienceForLevel;
        public double experienceForNextLevel;
        public sbyte level;
        public int maxPods;
        public int stamina;
        public int staminaMax;
        public int maturity;
        public int maturityForAdult;
        public int energy;
        public int energyMax;
        public int serenity;
        public int aggressivityMax;
        public int serenityMax;
        public int love;
        public int loveMax;
        public int fecondationTime;
        public int boostLimiter;
        public double boostMax;
        public int reproductionCount;
        public int reproductionCountMax;
        public IEnumerable<Types.ObjectEffectInteger> effectList;
        
        public MountClientData()
        {
        }
        
        public MountClientData(bool sex, bool isRideable, bool isWild, bool isFecondationReady, double id, int model, IEnumerable<int> ancestor, IEnumerable<int> behaviors, string name, int ownerId, double experience, double experienceForLevel, double experienceForNextLevel, sbyte level, int maxPods, int stamina, int staminaMax, int maturity, int maturityForAdult, int energy, int energyMax, int serenity, int aggressivityMax, int serenityMax, int love, int loveMax, int fecondationTime, int boostLimiter, double boostMax, int reproductionCount, int reproductionCountMax, IEnumerable<Types.ObjectEffectInteger> effectList)
        {
            this.sex = sex;
            this.isRideable = isRideable;
            this.isWild = isWild;
            this.isFecondationReady = isFecondationReady;
            this.id = id;
            this.model = model;
            this.ancestor = ancestor;
            this.behaviors = behaviors;
            this.name = name;
            this.ownerId = ownerId;
            this.experience = experience;
            this.experienceForLevel = experienceForLevel;
            this.experienceForNextLevel = experienceForNextLevel;
            this.level = level;
            this.maxPods = maxPods;
            this.stamina = stamina;
            this.staminaMax = staminaMax;
            this.maturity = maturity;
            this.maturityForAdult = maturityForAdult;
            this.energy = energy;
            this.energyMax = energyMax;
            this.serenity = serenity;
            this.aggressivityMax = aggressivityMax;
            this.serenityMax = serenityMax;
            this.love = love;
            this.loveMax = loveMax;
            this.fecondationTime = fecondationTime;
            this.boostLimiter = boostLimiter;
            this.boostMax = boostMax;
            this.reproductionCount = reproductionCount;
            this.reproductionCountMax = reproductionCountMax;
            this.effectList = effectList;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, sex);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isRideable);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isWild);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, isFecondationReady);
            writer.WriteByte(flag1);
            writer.WriteDouble(id);
            writer.WriteInt(model);
            var ancestor_before = writer.Position;
            var ancestor_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in ancestor)
            {
                 writer.WriteInt(entry);
                 ancestor_count++;
            }
            var ancestor_after = writer.Position;
            writer.Seek((int)ancestor_before);
            writer.WriteUShort((ushort)ancestor_count);
            writer.Seek((int)ancestor_after);

            var behaviors_before = writer.Position;
            var behaviors_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in behaviors)
            {
                 writer.WriteInt(entry);
                 behaviors_count++;
            }
            var behaviors_after = writer.Position;
            writer.Seek((int)behaviors_before);
            writer.WriteUShort((ushort)behaviors_count);
            writer.Seek((int)behaviors_after);

            writer.WriteUTF(name);
            writer.WriteInt(ownerId);
            writer.WriteDouble(experience);
            writer.WriteDouble(experienceForLevel);
            writer.WriteDouble(experienceForNextLevel);
            writer.WriteSByte(level);
            writer.WriteInt(maxPods);
            writer.WriteInt(stamina);
            writer.WriteInt(staminaMax);
            writer.WriteInt(maturity);
            writer.WriteInt(maturityForAdult);
            writer.WriteInt(energy);
            writer.WriteInt(energyMax);
            writer.WriteInt(serenity);
            writer.WriteInt(aggressivityMax);
            writer.WriteInt(serenityMax);
            writer.WriteInt(love);
            writer.WriteInt(loveMax);
            writer.WriteInt(fecondationTime);
            writer.WriteInt(boostLimiter);
            writer.WriteDouble(boostMax);
            writer.WriteInt(reproductionCount);
            writer.WriteInt(reproductionCountMax);
            var effectList_before = writer.Position;
            var effectList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in effectList)
            {
                 entry.Serialize(writer);
                 effectList_count++;
            }
            var effectList_after = writer.Position;
            writer.Seek((int)effectList_before);
            writer.WriteUShort((ushort)effectList_count);
            writer.Seek((int)effectList_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            sex = BooleanByteWrapper.GetFlag(flag1, 0);
            isRideable = BooleanByteWrapper.GetFlag(flag1, 1);
            isWild = BooleanByteWrapper.GetFlag(flag1, 2);
            isFecondationReady = BooleanByteWrapper.GetFlag(flag1, 3);
            id = reader.ReadDouble();
            model = reader.ReadInt();
            if (model < 0)
                throw new Exception("Forbidden value on model = " + model + ", it doesn't respect the following condition : model < 0");
            var limit = reader.ReadUShort();
            var ancestor_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ancestor_[i] = reader.ReadInt();
            }
            ancestor = ancestor_;
            limit = reader.ReadUShort();
            var behaviors_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 behaviors_[i] = reader.ReadInt();
            }
            behaviors = behaviors_;
            name = reader.ReadUTF();
            ownerId = reader.ReadInt();
            if (ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + ownerId + ", it doesn't respect the following condition : ownerId < 0");
            experience = reader.ReadDouble();
            experienceForLevel = reader.ReadDouble();
            experienceForNextLevel = reader.ReadDouble();
            level = reader.ReadSByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            maxPods = reader.ReadInt();
            if (maxPods < 0)
                throw new Exception("Forbidden value on maxPods = " + maxPods + ", it doesn't respect the following condition : maxPods < 0");
            stamina = reader.ReadInt();
            if (stamina < 0)
                throw new Exception("Forbidden value on stamina = " + stamina + ", it doesn't respect the following condition : stamina < 0");
            staminaMax = reader.ReadInt();
            if (staminaMax < 0)
                throw new Exception("Forbidden value on staminaMax = " + staminaMax + ", it doesn't respect the following condition : staminaMax < 0");
            maturity = reader.ReadInt();
            if (maturity < 0)
                throw new Exception("Forbidden value on maturity = " + maturity + ", it doesn't respect the following condition : maturity < 0");
            maturityForAdult = reader.ReadInt();
            if (maturityForAdult < 0)
                throw new Exception("Forbidden value on maturityForAdult = " + maturityForAdult + ", it doesn't respect the following condition : maturityForAdult < 0");
            energy = reader.ReadInt();
            if (energy < 0)
                throw new Exception("Forbidden value on energy = " + energy + ", it doesn't respect the following condition : energy < 0");
            energyMax = reader.ReadInt();
            if (energyMax < 0)
                throw new Exception("Forbidden value on energyMax = " + energyMax + ", it doesn't respect the following condition : energyMax < 0");
            serenity = reader.ReadInt();
            aggressivityMax = reader.ReadInt();
            serenityMax = reader.ReadInt();
            if (serenityMax < 0)
                throw new Exception("Forbidden value on serenityMax = " + serenityMax + ", it doesn't respect the following condition : serenityMax < 0");
            love = reader.ReadInt();
            if (love < 0)
                throw new Exception("Forbidden value on love = " + love + ", it doesn't respect the following condition : love < 0");
            loveMax = reader.ReadInt();
            if (loveMax < 0)
                throw new Exception("Forbidden value on loveMax = " + loveMax + ", it doesn't respect the following condition : loveMax < 0");
            fecondationTime = reader.ReadInt();
            boostLimiter = reader.ReadInt();
            if (boostLimiter < 0)
                throw new Exception("Forbidden value on boostLimiter = " + boostLimiter + ", it doesn't respect the following condition : boostLimiter < 0");
            boostMax = reader.ReadDouble();
            reproductionCount = reader.ReadInt();
            reproductionCountMax = reader.ReadInt();
            if (reproductionCountMax < 0)
                throw new Exception("Forbidden value on reproductionCountMax = " + reproductionCountMax + ", it doesn't respect the following condition : reproductionCountMax < 0");
            limit = reader.ReadUShort();
            var effectList_ = new Types.ObjectEffectInteger[limit];
            for (int i = 0; i < limit; i++)
            {
                 effectList_[i] = new Types.ObjectEffectInteger();
                 effectList_[i].Deserialize(reader);
            }
            effectList = effectList_;
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(bool) + 0 + 0 + 0 + sizeof(double) + sizeof(int) + sizeof(short) + ancestor.Sum(x => sizeof(int)) + sizeof(short) + behaviors.Sum(x => sizeof(int)) + sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(int) + sizeof(double) + sizeof(double) + sizeof(double) + sizeof(sbyte) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(double) + sizeof(int) + sizeof(int) + sizeof(short) + effectList.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}