using System;
using System.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Effects;

namespace Stump.Server.WorldServer.Game.Effects.Instances
{
    public enum EffectGenerationContext
    {
        Item,
        Spell,
    }

    public enum EffectGenerationType
    {
        Normal,
        MaxEffects,
        MinEffects,
    }

    [Serializable]
    public class EffectBase : ICloneable
    {
        [NonSerialized]
        protected EffectTemplate m_template;

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public virtual byte SerializationIdenfitier
        {
            get { return 1; }
        }

        public EffectBase()
        {
            
        }

        public EffectBase(short id)
        {
            Id = id;
            m_template = EffectManager.Instance.GetTemplate(id);
        }

        public EffectBase(short id, int targetId, int duration, int delay, int random, int group, int modificator, bool trigger, bool hidden, uint zoneSize, uint zoneShape, uint zoneMinSize)
        {
            Id = id;
            Targets = (SpellTargetType) targetId;
            Delay = delay;
            Duration = duration;
            Group = group;
            Random = random;
            Modificator = modificator;
            Trigger = trigger;
            Hidden = hidden;
            m_zoneSize = zoneSize;
            m_zoneMinSize = zoneMinSize;
            ZoneShape = (SpellShapeEnum) zoneShape;
        }

        public EffectBase(EffectInstance effect)
        {
            Id = (short) effect.effectId;
            m_template = EffectManager.Instance.GetTemplate(Id);

            Targets = (SpellTargetType) effect.targetId;
            Delay = effect.delay;
            Duration = effect.duration;
            Group = effect.group;
            Random = effect.random;
            Modificator = effect.modificator;
            Trigger = effect.trigger;
            Hidden = effect.hidden;
            RawZone = effect.rawZone;
        }

        public short Id
        {
            get;
            set;
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
            set { Id = (short) value; }
        }

        public EffectTemplate Template
        {
            get
            {
                return m_template ?? ( m_template = EffectManager.Instance.GetTemplate(Id) );
            }
            protected set { m_template = value; }
        }

        public SpellTargetType Targets
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public int Delay
        {
            get;
            set;
        }

        public int Random
        {
            get;
            set;
        }

        public int Group
        {
            get;
            set;
        }

        public int Modificator
        {
            get;
            set;
        }

        public bool Trigger
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        private uint m_zoneSize;

        public byte ZoneSize
        {
            get { return m_zoneSize >= 63 ? (byte)63 : (byte)m_zoneSize; }
            set { m_zoneSize = value; }
        }

        public SpellShapeEnum ZoneShape
        {
            get;
            set;
        }      
        
        private uint m_zoneMinSize;

        public byte ZoneMinSize
        {
            get
            {
                return m_zoneMinSize >= 63 ? (byte)63 : (byte)m_zoneMinSize;
            }
            set
            {
                m_zoneMinSize = value;
            }
        }

        private string m_rawZone;

        public string RawZone
        {
            get { return m_rawZone; }
            set
            {
                m_rawZone = value; ParseRawZone();
            }
        }

        protected void ParseRawZone()
        {
            if (string.IsNullOrEmpty(RawZone))
            {
                ZoneShape = 0;
                ZoneSize = 0;
                ZoneMinSize = 0;
                return;
            }

            var shape = (SpellShapeEnum) RawZone[0];
            byte size = 0;
            byte minSize = 0;

            var commaIndex = RawZone.IndexOf(',');

            if (commaIndex == -1 && RawZone.Length > 1)
            {
                size = byte.Parse(RawZone.Remove(0, 1));
            }
            else if (RawZone.Length > 1)
            {
                size = byte.Parse(RawZone.Substring(1, commaIndex - 1));
                minSize = byte.Parse(RawZone.Remove(0, commaIndex + 1));
            }

            ZoneShape = shape;
            ZoneSize = size;
            ZoneMinSize = minSize;
        }

        public virtual object[] GetValues()
        {
            return new object[0];
        }

        public virtual EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return this;
        }

        public virtual ObjectEffect GetObjectEffect()
        {
            return new ObjectEffect(Id);
        }

        public byte[] Serialize()
        {
            var writer = new BinaryWriter(new MemoryStream());

            writer.Write(SerializationIdenfitier);

            InternalSerialize(ref writer);

            return ( (MemoryStream) writer.BaseStream ).ToArray();
        }

        protected virtual void InternalSerialize(ref BinaryWriter writer)
        {
            if ((int)Targets == 0 &&
                Duration == 0 &&
                Delay == 0 &&
                Random == 0 &&
                Group == 0 &&
                Modificator == 0 &&
                Trigger == false &&
                Hidden == false &&
                ZoneSize == 0 &&
                ZoneShape == 0)
            {
                writer.Write('C'); // cutted object

                writer.Write(Id);
            }
            else
            {
                writer.Write((int)Targets);
                writer.Write(Id); // writer id second 'cause targets can't equals to 'C' but id can
                writer.Write(Duration);
                writer.Write(Delay);
                writer.Write(Random);
                writer.Write(Group);
                writer.Write(Modificator);
                writer.Write(Trigger);
                writer.Write(Hidden);
                writer.Write(RawZone);
            }
        }

        /// <summary>
        /// Use EffectManager.Deserialize
        /// </summary>
        internal void DeSerialize(byte[] buffer, ref int index)
        {
            var reader = new BinaryReader(new MemoryStream(buffer, index, buffer.Length - index));

            InternalDeserialize(ref reader);

            index += (int)reader.BaseStream.Position;
        }

        protected virtual void InternalDeserialize(ref BinaryReader reader)
        {
            if (reader.PeekChar() == 'C')
            {
                reader.ReadChar();
                Id = reader.ReadInt16();
            }
            else
            {
                Targets = (SpellTargetType)reader.ReadInt32();
                Id = reader.ReadInt16();
                Duration = reader.ReadInt32();
                Delay = reader.ReadInt32();
                Random = reader.ReadInt32();
                Group = reader.ReadInt32();
                Modificator = reader.ReadInt32();
                Trigger = reader.ReadBoolean();
                Hidden = reader.ReadBoolean();
                RawZone = reader.ReadString();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EffectBase)) return false;
            return Equals((EffectBase) obj);
        }

        public static bool operator ==(EffectBase left, EffectBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectBase left, EffectBase right)
        {
            return !Equals(left, right);
        }

        public bool Equals(EffectBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}