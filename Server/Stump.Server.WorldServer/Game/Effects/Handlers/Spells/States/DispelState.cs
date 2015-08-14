using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_DispelState)]
    [EffectHandler(EffectsEnum.Effect_DisableState)]
    public class DispelState : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DispelState(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                var state = SpellManager.Instance.GetSpellState((uint) Dice.Value);

                if (state == null)
                {
                    logger.Error("Spell state {0} not found", Dice.Value);
                    return false;
                }

                if (Effect.EffectId == EffectsEnum.Effect_DisableState)
                {
                    var stateBuff = affectedActor.GetBuffs(x => x is StateBuff && ((StateBuff)x).State.Id == (int)state.Id).FirstOrDefault();
                    if (stateBuff == null)
                        return false;

                    var id = Caster.PopNextBuffId();
                    var buff = new DelayBuff(id, affectedActor, Caster, Dice, Spell, false, false, StateTrigger)
                    {
                        Duration = (short) Dice.Duration,
                        Token = stateBuff
                    };

                    Caster.AddAndApplyBuff(buff);
                }


                RemoveStateBuff(affectedActor, (SpellStatesEnum)state.Id);
            }

            return true;
        }

        public void StateTrigger(DelayBuff buff, object token)
        {
            var state = (StateBuff) token;
            if (state == null)
                return;

            buff.Target.AddAndApplyBuff(state);
        }
    }
}
