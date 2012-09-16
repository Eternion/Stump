using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Database
{
    public class SpellLevelTemplateConfiguration : EntityTypeConfiguration<SpellLevelTemplate>
    {
        public SpellLevelTemplateConfiguration()
        {
            ToTable("spells_level");
        }
    }
    [D2OClass("SpellLevel", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellLevelTemplate : IAssignedByD2O, ISaveIntercepter
    {
        private List<EffectDice> m_criticalEffects;
        private List<EffectDice> m_effects;
        private SpellTemplate m_spell;

        public uint Id
        {
            get;
            set;
        }

        public uint SpellId
        {
            get;
            set;
        }

        public SpellTemplate Spell
        {
            get { return m_spell ?? (m_spell = SpellManager.Instance.GetSpellTemplate(SpellId)); }
        }

        public uint SpellBreed
        {
            get;
            set;
        }

        public uint ApCost
        {
            get;
            set;
        }

        public uint Range
        {
            get;
            set;
        }

        public Boolean CastInLine
        {
            get;
            set;
        }

        public Boolean CastInDiagonal
        {
            get;
            set;
        }

        public Boolean CastTestLos
        {
            get;
            set;
        }

        public uint CriticalHitProbability
        {
            get;
            set;
        }

        private byte[] m_statesRequiredBin;

        public byte[] StatesRequiredBin
        {
            get { return m_statesRequiredBin; }
            set
            {
                m_statesRequiredBin = value;
                StatesRequired = m_statesRequiredBin.ToObject<List<int>>();
            }
        }

        public List<int> StatesRequired
        {
            get;
            set;
        }

        public uint CriticalFailureProbability
        {
            get;
            set;
        }

        public Boolean NeedFreeCell
        {
            get;
            set;
        }

        public Boolean NeedFreeTrapCell
        {
            get;
            set;
        }

        public Boolean NeedTakenCell
        {
            get;
            set;
        }

        public Boolean RangeCanBeBoosted
        {
            get;
            set;
        }

        public int MaxStack
        {
            get;
            set;
        }

        public uint MaxCastPerTurn
        {
            get;
            set;
        }

        public uint MaxCastPerTarget
        {
            get;
            set;
        }

        public uint MinCastInterval
        {
            get;
            set;
        }

        public uint InitialCooldown
        {
            get;
            set;
        }

        public int GlobalCooldown
        {
            get;
            set;
        }

        public uint MinPlayerLevel
        {
            get;
            set;
        }

        public Boolean CriticalFailureEndsTurn
        {
            get;
            set;
        }

        public Boolean HideEffects
        {
            get;
            set;
        }

        public Boolean Hidden
        {
            get;
            set;
        }

        public uint MinRange
        {
            get;
            set;
        }

        private byte[] m_statesForbiddenBin;

        public byte[] StatesForbiddenBin
        {
            get { return m_statesForbiddenBin; }
            set { m_statesForbiddenBin = value;
                StatesForbidden = value.ToObject<List<int>>();
            }
        }

        public List<int> StatesForbidden
        {
            get;
            set;
        }

        private byte[] EffectsBin
        {
            get;
            set;
        }

        public List<EffectDice> Effects
        {
            get
            {
                return m_effects ?? ( m_effects = EffectManager.Instance.DeserializeEffects(EffectsBin).Cast<EffectDice>().ToList() );
            }
            set { m_effects = value; }
        }

        private byte[] CriticalEffectsBin
        {
            get;
            set;
        }

        public List<EffectDice> CriticalEffects
        {
            get
            {
                return m_criticalEffects ?? ( m_criticalEffects = EffectManager.Instance.DeserializeEffects(CriticalEffectsBin).Cast<EffectDice>().ToList() );
            }
            set { m_criticalEffects = value; }
        }

        public void AssignFields(object d2oObject)
        {
            var spell = (DofusProtocol.D2oClasses.SpellLevel)d2oObject;
            Id = spell.id;
            SpellId = spell.spellId;
            SpellBreed = spell.spellBreed;
            ApCost = spell.apCost;
            Range = spell.range;
            CastInLine = spell.castInLine;
            CastInDiagonal = spell.castInDiagonal;
            CastTestLos = spell.castTestLos;
            CriticalHitProbability = spell.criticalHitProbability;
            StatesRequired = spell.statesRequired;
            CriticalFailureProbability = spell.criticalFailureProbability;
            NeedFreeCell = spell.needFreeCell;
            NeedFreeTrapCell = spell.needFreeTrapCell;
            NeedTakenCell = spell.needTakenCell;
            RangeCanBeBoosted = spell.rangeCanBeBoosted;
            MaxStack = spell.maxStack;
            MaxCastPerTarget = spell.maxCastPerTarget;
            MinCastInterval = spell.minCastInterval;
            InitialCooldown = spell.initialCooldown;
            GlobalCooldown = spell.globalCooldown;
            MinPlayerLevel = spell.minPlayerLevel;
            CriticalFailureEndsTurn = spell.criticalFailureEndsTurn;
            HideEffects = spell.hideEffects;
            Hidden = spell.hidden;
            MinRange = spell.minRange;
            StatesForbidden = spell.statesForbidden;
            EffectsBin = EffectManager.Instance.SerializeEffects(spell.effects);
            CriticalEffectsBin = EffectManager.Instance.SerializeEffects(spell.criticalEffect);
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_statesForbiddenBin = StatesForbidden.ToBinary();
            m_statesRequiredBin = StatesRequired.ToBinary();
            EffectsBin = EffectManager.Instance.SerializeEffects(Effects);
            CriticalEffectsBin = EffectManager.Instance.SerializeEffects(CriticalEffects);
        }
    }
}