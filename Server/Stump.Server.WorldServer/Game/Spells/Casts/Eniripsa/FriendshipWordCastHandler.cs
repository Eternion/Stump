using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.MOT_D_AMITIÉ_129)]
    public class FriendshipWordCastHandler : DefaultSpellCastHandler
    {
        public FriendshipWordCastHandler(SpellCastInformations informations)
            : base(informations)
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
