using System;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class Damage
    {
        private int m_amount;

        public Damage(int amount)
        {
            Amount = amount;
        }

        public Damage(EffectDice effect)
        {
            BaseMaxDamages = Math.Max(effect.DiceFace, effect.DiceNum);
            BaseMinDamages = Math.Min(effect.DiceFace, effect.DiceNum);
        }

        public Damage(EffectDice effect, EffectSchoolEnum school, FightActor source, Spell spell)
            : this(effect)
        {
            School = school;
            Source = source;
            Spell = spell;
        }

        public EffectSchoolEnum School
        {
            get;
            set;
        }

        public FightActor Source
        {
            get;
            set;
        }

        public MarkTrigger MarkTrigger
        {
            get;
            set;
        }

        public Buff Buff
        {
            get;
            set;
        }

        public int BaseMinDamages
        {
            get;
            set;
        }

        public int BaseMaxDamages
        {
            get;
            set;
        }

        public int Amount
        {
            get { return m_amount; }
            set
            {
                if (value < 0)
                    value = 0;
                m_amount = value;
                Generated = true;
            }
        }

        public bool Generated
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            set;
        }

        public bool PvP
        {
            get { return Source is CharacterFighter; }
        }

        public bool IgnoreDamageReduction
        {
            get;
            set;
        }

        public bool IgnoreDamageBoost
        {
            get;
            set;
        }

        public EffectGenerationType EffectGenerationType
        {
            get;
            set;
        }

        public bool ReflectedDamages
        {
            get;
            set;
        }

        public void GenerateDamages()
        {
            if (Generated)
                return;

            if (EffectGenerationType == EffectGenerationType.MaxEffects)
                Amount = BaseMaxDamages;
            else if (EffectGenerationType == EffectGenerationType.MinEffects)
                Amount = BaseMinDamages;
            else
            {
                var rand = new AsyncRandom();

                Amount = rand.Next(BaseMinDamages, BaseMaxDamages + 1);
            }
        }
    }
}