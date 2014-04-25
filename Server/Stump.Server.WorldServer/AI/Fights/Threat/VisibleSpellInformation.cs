using System;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.AI.Fights.Threat
{
    public class VisibleSpellInformation
    {
        public AIFighter Sighter
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public SpellLevelTemplate SpellLevel
        {
            get { return Spell.CurrentSpellLevel; }
        }

        public uint Cost
        {
            get { return Spell.CurrentSpellLevel.ApCost; }
        }

        public int InflictedDamage
        {
            get;
            private set;
        }

        public int HealedPoints
        {
            get;
            private set;
        }

        public bool CanHeal
        {
            get { return HealedPoints > 0; }
        }

        public bool CanInflictDamage
        {
            get { return InflictedDamage > 0; }
        }

        public void AddInflictedDamage(int value, FightActor target)
        {
            InflictedDamage += value;
        }

        public void AddHealedPoints(int value, FightActor target)
        {
            HealedPoints += value;
        }

        public float GetThreat()
        {
            var times = (int) Math.Ceiling((Caster.AP + Caster.UsedAP) / (double)Cost);

            return (InflictedDamage + HealedPoints) * times;
        }
    }
}