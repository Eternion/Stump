using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_Double)]
    public class Double : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Double(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            // no need to check for CanSummon()

            var summon = new SummonedClone(Fight.GetNextContextualId(), Caster, TargetedCell);
            
            Caster.AddSummon(summon);
            Caster.Team.AddFighter(summon);

            ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

            Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);

            return true;
        }
    }
}