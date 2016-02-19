using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubAP)]
    public class APDebuff: SpellEffectHandler
    {
        public APDebuff(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var target = actor;
                var buff = actor.GetBestReflectionBuff();
                if (buff != null && buff.ReflectedLevel >= Spell.CurrentLevel && Spell.Template.Id != 0)
                {
                    NotifySpellReflected(actor);
                    target = Caster;

                    if (buff.Duration <= 0)
                        actor.RemoveBuff(buff);
                }

                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddStatBuff(target, (short)(-(integerEffect.Value)), PlayerFields.AP, true, (short)EffectsEnum.Effect_SubAP);
                }
                else
                {
                    target.LostAP(integerEffect.Value, Caster);
                }


                target.TriggerBuffs(target, BuffTriggerType.OnAPLost);
            }

            return true;
        }
        void NotifySpellReflected(FightActor source)
        {
            ActionsHandler.SendGameActionFightReflectSpellMessage(Fight.Clients, Caster, source);
        }
    }
}
