using NLog;
using NLog.Targets;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Double
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

            var summon = new SummonedClone(Fight.GetNextContextualId(), (NamedFighter)Caster, TargetedCell);
            
            ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

            Caster.AddSummon(summon);
            Caster.Team.AddFighter(summon);

            return true;
        }
    }
}