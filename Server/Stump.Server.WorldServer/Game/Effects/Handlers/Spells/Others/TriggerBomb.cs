using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerBomb)]
    public class TriggerBomb : SpellEffectHandler
    {
        public TriggerBomb(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var bomb in GetAffectedActors(x => x is SummonedBomb && ((SummonedBomb)x).Summoner == (Caster is SlaveFighter ? ((SlaveFighter)Caster).Summoner : Caster)).Where(bomb => bomb.IsAlive()))
            {
                if (Dice.Duration != 0 || Dice.Delay != 0)
                {
                    bomb.AddBuff(new EmptyBuff(bomb.PopNextBuffId(), bomb, Caster, Dice, Spell, false));
                }
                else
                {
                    ((SummonedBomb)bomb).Explode();
                }
            }

            return true;
        }
    }
}