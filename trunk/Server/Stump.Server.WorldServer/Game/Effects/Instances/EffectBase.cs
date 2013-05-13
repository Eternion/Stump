using System;
using System.IO;
using System.Text;
using NLog;
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private int m_delay;
        private int m_duration;
        private int m_group;
        private bool m_hidden;
        private short m_id;
        private int m_modificator;
        private int m_random;
        private SpellTargetType m_targets;

        [NonSerialized] protected EffectTemplate m_template;
        private bool m_trigger;
        private uint m_zoneMinSize;
        private SpellShapeEnum m_zoneShape;
        private uint m_zoneSize;

        public EffectBase()
        {
        }

        public EffectBase(EffectBase effect)
        {
            m_id = effect.Id;
            m_template = EffectManager.Instance.GetTemplate(effect.Id);
            m_targets = effect.Targets;
            m_delay = effect.Delay;
            m_duration = effect.Duration;
            m_group = effect.Group;
            m_random = effect.Random;
            m_modificator = effect.Modificator;
            m_trigger = effect.Trigger;
            m_hidden = effect.Hidden;
            m_zoneSize = effect.m_zoneSize;
            m_zoneMinSize = effect.m_zoneMinSize;
            m_zoneShape = effect.ZoneShape;
        }

        public EffectBase(short id, EffectBase effect)
        {
            m_id = id;
            m_template = EffectManager.Instance.GetTemplate(id);
            m_targets = effect.Targets;
            m_delay = effect.Delay;
            m_duration = effect.Duration;
            m_group = effect.Group;
            m_random = effect.Random;
            m_modificator = effect.Modificator;
            m_trigger = effect.Trigger;
            m_hidden = effect.Hidden;
            m_zoneSize = effect.m_zoneSize;
            m_zoneMinSize = effect.m_zoneMinSize;
            m_zoneShape = effect.ZoneShape;
        }

        public EffectBase(EffectInstance effect)
        {
            m_id = (short) effect.effectId;
            m_template = EffectManager.Instance.GetTemplate(Id);

            m_targets = (SpellTargetType) effect.targetId;
            m_delay = effect.delay;
            m_duration = effect.duration;
            m_group = effect.group;
            m_random = effect.random;
            m_modificator = effect.modificator;
            m_trigger = effect.trigger;
            m_hidden = effect.hidden;
            ParseRawZone(effect.rawZone);
        }

        public virtual int ProtocoleId
        {
            get { return 76; }
        }

        public virtual byte SerializationIdenfitier
        {
            get { return 1; }
        }

        public short Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                IsDirty = true;
            }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
            set
            {
                Id = (short) value;
                IsDirty = true;
            }
        }

        public EffectTemplate Template
        {
            get { return m_template ?? (m_template = EffectManager.Instance.GetTemplate(Id)); }
            protected set
            {
                m_template = value;
                IsDirty = true;
            }
        }

        public SpellTargetType Targets
        {
            get { return m_targets; }
            set
            {
                m_targets = value;
                IsDirty = true;
            }
        }

        public int Duration
        {
            get { return m_duration; }
            set
            {
                m_duration = value;
                IsDirty = true;
            }
        }

        public int Delay
        {
            get { return m_delay; }
            set
            {
                m_delay = value;
                IsDirty = true;
            }
        }

        public int Random
        {
            get { return m_random; }
            set
            {
                m_random = value;
                IsDirty = true;
            }
        }

        public int Group
        {
            get { return m_group; }
            set
            {
                m_group = value;
                IsDirty = true;
            }
        }

        public int Modificator
        {
            get { return m_modificator; }
            set
            {
                m_modificator = value;
                IsDirty = true;
            }
        }

        public bool Trigger
        {
            get { return m_trigger; }
            set
            {
                m_trigger = value;
                IsDirty = true;
            }
        }

        public bool Hidden
        {
            get { return m_hidden; }
            set
            {
                m_hidden = value;
                IsDirty = true;
            }
        }

        public uint ZoneSize
        {
            get { return m_zoneSize >= 63 ? (byte) 63 : (byte) m_zoneSize; }
            set
            {
                m_zoneSize = value;
                IsDirty = true;
            }
        }

        public SpellShapeEnum ZoneShape
        {
            get { return m_zoneShape; }
            set
            {
                m_zoneShape = value;
                IsDirty = true;
            }
        }

        public uint ZoneMinSize
        {
            get { return m_zoneMinSize >= 63 ? (byte) 63 : (byte) m_zoneMinSize; }
            set
            {
                m_zoneMinSize = value;
                IsDirty = true;
            }
        }

        public bool IsDirty
        {
            get;
            set;
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        protected void ParseRawZone(string rawZone)
        {
            if (string.IsNullOrEmpty(rawZone))
            {
                m_zoneShape = 0;
                m_zoneSize = 0;
                m_zoneMinSize = 0;
                return;
            }

            var shape = (SpellShapeEnum) rawZone[0];
            byte size = 0;
            byte minSize = 0;

            int commaIndex = rawZone.IndexOf(',');
            try
            {
                if (commaIndex == -1 && rawZone.Length > 1)
                {
                    size = byte.Parse(rawZone.Remove(0, 1));
                }
                else if (rawZone.Length > 1)
                {
                    size = byte.Parse(rawZone.Substring(1, commaIndex - 1));
                    minSize = byte.Parse(rawZone.Remove(0, commaIndex + 1));
                }
            }
            catch (Exception ex)
            {
                m_zoneShape = 0;
                m_zoneSize = 0;
                m_zoneMinSize = 0;

                logger.Error("ParseRawZone() => Cannot parse {0}", rawZone);
            }

            m_zoneShape = shape;
            m_zoneSize = size;
            m_zoneMinSize = minSize;
        }

        protected string BuildRawZone()
        {
            var builder = new StringBuilder();

            builder.Append((char) (int) ZoneShape);
            builder.Append(ZoneSize);

            if (ZoneMinSize > 0)
            {
                builder.Append(",");
                builder.Append(ZoneMinSize);
            }

            return builder.ToString();
        }

        public virtual object[] GetValues()
        {
            return new object[0];
        }

        public virtual EffectBase GenerateEffect(EffectGenerationContext context,
                                                 EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectBase(this);
        }

        public virtual ObjectEffect GetObjectEffect()
        {
            return new ObjectEffect(Id);
        }

        public virtual EffectInstance GetEffectInstance()
        {
            return new EffectInstance
                {
                    effectId = (uint) Id,
                    targetId = (int) Targets,
                    delay = Delay,
                    duration = Duration,
                    group = Group,
                    random = Random,
                    modificator = Modificator,
                    trigger = Trigger,
                    hidden = Hidden,
                    zoneMinSize = ZoneMinSize,
                    zoneSize = ZoneSize,
                    zoneShape = (uint) ZoneShape
                };
        }

        public byte[] Serialize()
        {
            var writer = new BinaryWriter(new MemoryStream());

            writer.Write(SerializationIdenfitier);

            InternalSerialize(ref writer);

            return ((MemoryStream) writer.BaseStream).ToArray();
        }

        protected virtual void InternalSerialize(ref BinaryWriter writer)
        {
            if ((int) Targets == 0 &&
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
                writer.Write((int) Targets);
                writer.Write(Id); // writer id second 'cause targets can't equals to 'C' but id can
                writer.Write(Duration);
                writer.Write(Delay);
                writer.Write(Random);
                writer.Write(Group);
                writer.Write(Modificator);
                writer.Write(Trigger);
                writer.Write(Hidden);

                string rawZone = BuildRawZone();
                if (rawZone == null)
                    writer.Write(string.Empty);
                else
                    writer.Write(rawZone);
            }
        }

        /// <summary>
        /// Use EffectManager.Deserialize
        /// </summary>
        internal void DeSerialize(byte[] buffer, ref int index)
        {
            var reader = new BinaryReader(new MemoryStream(buffer, index, buffer.Length - index));

            InternalDeserialize(ref reader);

            index += (int) reader.BaseStream.Position;
        }

        protected virtual void InternalDeserialize(ref BinaryReader reader)
        {
            if (reader.PeekChar() == 'C')
            {
                reader.ReadChar();
                m_id = reader.ReadInt16();
            }
            else
            {
                m_targets = (SpellTargetType) reader.ReadInt32();
                m_id = reader.ReadInt16();
                m_duration = reader.ReadInt32();
                m_delay = reader.ReadInt32();
                m_random = reader.ReadInt32();
                m_group = reader.ReadInt32();
                m_modificator = reader.ReadInt32();
                m_trigger = reader.ReadBoolean();
                m_hidden = reader.ReadBoolean();
                ParseRawZone(reader.ReadString());
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
    }
}