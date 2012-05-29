using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Punishment)]
    public class Punishment : SpellEffectHandler
    {
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
            var buffs = buff.Target.GetBuffs(entry => entry.Spell == Spell).OfType<StatBuff>();

            var currentBonus = buffs.Where(entry => entry.Duration == Effect.Duration).Sum(entry => entry.Value);
            var limit = Dice.DiceFace;
            // limit reached
            if (currentBonus >= limit)
                return;

            var bonus = (short)token;

            if (bonus + currentBonus > limit)
                bonus = (short) (limit - currentBonus);

            var caracteristic = GetPunishmentBoostType(Dice.DiceNum);
            var statBuff = new StatBuff(buff.Target.PopNextBuffId(), buff.Target, Caster, Dice,
                Spell, bonus, caracteristic, false, true, (short)GetBuffEffectId(caracteristic)) 
                {Duration = Dice.Value};

            buff.Target.AddAndApplyBuff(statBuff);
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
                case ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY:
                case ActionsEnum.ACTION_CHARACTER_BOOST_VITALITY_407: // **** magic numbers
                    return PlayerFields.Vitality;

                default:
                    throw new Exception(string.Format("PunishmentBoostType not found for action {0}", punishementAction));
            }
        }

        private static EffectsEnum GetBuffEffectId(PlayerFields caracteristic)
        {
            switch (caracteristic)
            {
                case PlayerFields.Agility:
                    return EffectsEnum.Effect_AddAgility;
                case PlayerFields.Chance:
                    return EffectsEnum.Effect_AddChance;
                case PlayerFields.Strength:
                    return EffectsEnum.Effect_AddStrength;
                case PlayerFields.Intelligence:
                    return EffectsEnum.Effect_AddIntelligence;
                case PlayerFields.Vitality:
                    return EffectsEnum.Effect_AddVitality;
                case PlayerFields.Wisdom:
                    return EffectsEnum.Effect_AddWisdom;
                default:
                    throw new Exception(string.Format("Buff Effect not found for caracteristic {0}", caracteristic));
            }
        }
    }
}