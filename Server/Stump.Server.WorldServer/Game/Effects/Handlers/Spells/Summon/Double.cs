using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_Double)]
    public class Double : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Double(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var summon = new SummonedClone(Fight.GetNextContextualId(), Caster, TargetedCell)
            {SummoningEffect=this};
            
            Caster.AddSummon(summon);
            Caster.Team.AddFighter(summon);

            ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

            Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);

            return true;
        }
    }
}