using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Punishment)]
    public class Punishment : SpellEffectHandler
    {
        private readonly List<Tuple<int, StatBuff>> m_buffs = new List<Tuple<int, StatBuff>>();
 
        public Punishment(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, true, BuffTriggerType.AFTER_ATTACKED, OnActorAttacked);
            }

            return true;
        }

        private void OnActorAttacked(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var currentBonus = m_buffs.Where(entry => entry.Item2.Target == buff.Target && entry.Item1 == Fight.TimeLine.RoundNumber).Sum(entry => entry.Item2.Value);
            var limit = Dice.DiceFace;

            // limit reached
            if (currentBonus >= limit)
                return;

            var damages = (Fights.Damage)token;
            var bonus = damages.Amount;
            
            if (bonus + currentBonus > limit)
                bonus = (short) (limit - currentBonus);

            if (bonus <= 0)
                return;

            var caracteristic = GetPunishmentBoostType(Dice.DiceNum);
            var statBuff = new StatBuff(buff.Target.PopNextBuffId(), buff.Target, Caster, Dice,
                Spell, (short)bonus, caracteristic, false, true, Dice.DiceNum) 
                {Duration = Dice.Value};

            buff.Target.AddAndApplyBuff(statBuff, true, true);
            m_buffs.Add(new Tuple<int, StatBuff>(Fight.TimeLine.RoundNumber, statBuff));
        }

        private static PlayerFields GetPunishmentBoostType(short punishementAction)
        {
            switch ((ActionsEnum)punishementAction)
            {
                case ActionsEnum.ACTION_CHARACTER_BOOST_AGILITY:
                    return PlayerFields.Agility;
                case ActionsEnum.ACTION_CHARACTER_BOOST_STRENGTH:
                    return PlayerFields.Strength;
                case ActionsEnum.ACTION_CHARACTER_BOOST_INTELLIGENCE:
                    return PlayerFields.Intelligence;
                case ActionsEnum.ACTION_CHARACTER_BOOST_CHANCE:
                    return PlayerFields.Chance;
                case ActionsEnum.ACTION_CHARACTER_BOOST_WISDOM:
                    return PlayerFields.Wisdom;
                case ActionsEnum.ACTION_CHARACTER_BOOST_DAMAGES_PERCENT:
                    return PlayerFields.DamageBonusPercent;
                case ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY:
                case (ActionsEnum)407: // **** magic numbers
                    return PlayerFields.Health;

                default:
                    throw new Exception(string.Format("PunishmentBoostType not found for action {0}", punishementAction));
            }
        }
    }
}