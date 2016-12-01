using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerBuff)]
    [EffectHandler(EffectsEnum.Effect_TriggerBuff_793)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_1160)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_1017)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_2160)]
    [EffectHandler(EffectsEnum.Effect_CastSpell_1175)]
    public class CastSpell : SpellEffectHandler
    {
        public CastSpell(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                if (Dice.Duration != 0 || Dice.Delay != 0)
                {
                    var buffId = affectedActor.PopNextBuffId();

                    var spell = new Spell(Dice.DiceNum, (byte)Dice.DiceFace);

                    var buff = new TriggerBuff(buffId, affectedActor, Caster, this, spell, Spell, false, FightDispellableEnum.DISPELLABLE_BY_DEATH, Priority, DefaultBuffTrigger);

                    affectedActor.AddBuff(buff);
                }
                else
                {
                    var spell = new Spell(Dice.DiceNum, (byte)Dice.DiceFace);

                    if (Effect.EffectId == EffectsEnum.Effect_CastSpell_1160 || Effect.EffectId == EffectsEnum.Effect_CastSpell_2160)
                    {
                        if (Spell.Id == (int)SpellIdEnum.DOFUS_ABYSSAL)
                        {
                            var ignored = new[]
                                {
                                    SpellCastResult.CANNOT_PLAY,
                                    SpellCastResult.CELL_NOT_FREE,
                                    SpellCastResult.HAS_NOT_SPELL,
                                    SpellCastResult.HISTORY_ERROR,
                                    SpellCastResult.NOT_ENOUGH_AP,
                                    SpellCastResult.NOT_IN_ZONE,
                                    SpellCastResult.STATE_FORBIDDEN,
                                    SpellCastResult.STATE_REQUIRED,
                                    SpellCastResult.UNWALKABLE_CELL
                                };

                            Caster.CastSpell(spell, affectedActor.Cell, true, true, true, this, ignored);
                        }
                        else
                        {
                            Caster.CastSpell(spell, affectedActor.Cell, true, true, true, this, new[] { SpellCastResult.OK });
                        }
                    }
                    else if (Effect.EffectId == EffectsEnum.Effect_CastSpell_1017)
                    {
                        affectedActor.CastSpell(spell, Caster.Cell, true, true, true, this, new[] { SpellCastResult.OK });
                    }
                    else
                    {
                        affectedActor.CastSpell(spell, affectedActor.Cell, true, true, true, this, new[] { SpellCastResult.OK });
                    }
                }
            }

            return true;
        }

        void DefaultBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damages = token as Fights.Damage;

            if (damages != null && damages.Spell != null && damages.Spell.Id == buff.Spell.Id)
                return;

            if (Effect.EffectId == EffectsEnum.Effect_CastSpell_1160)
            {
                buff.Caster.CastSpell(buff.Spell, buff.Target.Cell, true, true, true, this, new[] { SpellCastResult.OK });
            }
            else if (Effect.EffectId == EffectsEnum.Effect_CastSpell_1017)
            {
                buff.Target.CastSpell(buff.Spell, triggerrer.Cell, true, true, true, this, new[] { SpellCastResult.OK });
            }
            else if (buff.Spell.Id == (int)SpellIdEnum.FRIKT) // hot fix
            {
                buff.Target.CastSpell(buff.Spell, triggerrer.Cell, true, true, true, this, new[] { SpellCastResult.OK });
            }
            else
            {
                buff.Target.CastSpell(buff.Spell, buff.Target.Cell, true, true, true, this, new[] { SpellCastResult.OK });
            }
        }
    }
}