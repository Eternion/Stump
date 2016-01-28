using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Teleport)]
    public class Teleportation : SpellEffectHandler
    {
        public Teleportation(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var carryingActor = Caster.GetCarryingActor();

            if (carryingActor != null)
                carryingActor.ThrowActor(TargetedCell);
            else
            {
                Caster.Position.Cell = TargetedCell;

                Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster, TargetedCell), true);
            }

            return true;
        }
    }
}