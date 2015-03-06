using System.Linq;
using NLog.Targets;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_SummonsBomb)]
    public class Bomb : SpellEffectHandler
    {
        public Bomb(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var bombSpell = SpellManager.Instance.GetSpellBombTemplate(Dice.DiceNum);
            var monsterTemplate = MonsterManager.Instance.GetMonsterGrade(Dice.DiceNum, Dice.DiceFace);

            var targets = GetAffectedActors();

            if (targets.Any())
            {
                var spell = new Spell(bombSpell.InstantReactionSpell, Spell.CurrentLevel);
                var cast = SpellManager.Instance.GetSpellCastHandler(Caster, spell, TargetedCell, Critical);

                cast.Initialize();
                cast.Execute();
            }
            else
            {
                if (!Caster.CanSummonBomb())
                    return false;

                var bomb = new SummonedBomb(Fight.GetNextContextualId(), Caster.Team, bombSpell, monsterTemplate, Caster,
                    TargetedCell);

                Caster.AddBomb(bomb);
                Caster.Team.AddFighter(bomb);

                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, bomb);

                Fight.TriggerMarks(bomb.Cell, bomb, TriggerType.MOVE);
            }

            return false;
        }
    }
}