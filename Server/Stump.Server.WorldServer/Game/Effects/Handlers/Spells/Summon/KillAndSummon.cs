using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_KillAndSummon)]
    [EffectHandler(EffectsEnum.Effect_KillAndSummon_2796)]
    public class KillAndSummon : SpellEffectHandler
    {
        public KillAndSummon(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var monster = MonsterManager.Instance.GetMonsterGrade(Dice.DiceNum, Dice.DiceFace);

                if (monster == null)
                    return false;

                actor.InflictDirectDamage(actor.LifePoints, Caster);

                var summon = new SummonedMonster(Fight.GetNextContextualId(), Caster.Team, Caster, monster, actor.Cell);

                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

                Caster.AddSummon(summon);
                Caster.Team.AddFighter(summon);

                Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);
            }

            return true;
        }
    }
}
