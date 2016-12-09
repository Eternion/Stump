using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.MOT_D_AMITIÉ_129)]
    public class FriendshipWordCastHandler : DefaultSpellCastHandler
    {
        public FriendshipWordCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            base.Execute();

            Caster.SpellHistory.RegisterCastedSpell(new SpellHistoryEntry(Caster.SpellHistory, Spell.CurrentSpellLevel,
                Caster, Caster, Fight.TimeLine.RoundNumber, 63));
            ActionsHandler.SendGameActionFightSpellCooldownVariationMessage(Caster.Fight.Clients, Caster, Caster, Spell, 63);
        }
    }
}
