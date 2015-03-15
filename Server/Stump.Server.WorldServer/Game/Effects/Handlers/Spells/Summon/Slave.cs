using NLog;
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
    [EffectHandler(EffectsEnum.Effect_SummonSlave)]
    public class Slave : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Slave(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var monster = MonsterManager.Instance.GetMonsterGrade(Dice.DiceNum, Dice.DiceFace);

            if (monster == null)
            {
                logger.Error("Cannot summon monster {0} grade {1} (not found)", Dice.DiceNum, Dice.DiceFace);
                return false;
            }

            if (!Caster.CanSummon())
                return false;

            var stateRooted = SpellManager.Instance.GetSpellState((int) SpellStatesEnum.Rooted);
            var stateUnlockable = SpellManager.Instance.GetSpellState((int)SpellStatesEnum.Unlockable);

            var slave = new SlaveFighter(Fight.GetNextContextualId(), Caster.Team, Caster, monster, TargetedCell);

            AddStateBuff(slave, false, true, stateRooted);
            AddStateBuff(slave, false, true, stateUnlockable);

            ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, slave);

            Caster.AddSlave(slave);
            Caster.Team.AddFighter(slave);

            Fight.TriggerMarks(slave.Cell, slave, TriggerType.MOVE);

            return true;
        }
    }
}
