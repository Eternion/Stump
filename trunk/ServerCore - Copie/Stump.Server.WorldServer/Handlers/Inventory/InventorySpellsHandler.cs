
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (SpellMoveMessage))]
        public static void HandleSpellMoveMessage(WorldClient client, SpellMoveMessage message)
        {
            if (message.position < 63 || message.position > 255)
            {
                return;
            }

            client.ActiveCharacter.ModifySpellPos((SpellIdEnum) message.spellId, (int) message.position);

            SendSpellMovementMessage(client, message.spellId, message.position);
        }

        public static void SendSpellMovementMessage(WorldClient client, uint spellId, uint position)
        {
            client.Send(new SpellMovementMessage(spellId, position));
        }

        public static void SendSpellListMessage(WorldClient client, bool previsualization)
        {
            client.Send(new SpellListMessage(previsualization,
                                             client.ActiveCharacter.Spells.SpellsById.Values.Select(
                                                 entry => entry.ToNetworkSpell()).ToList()));
        }
    }
}