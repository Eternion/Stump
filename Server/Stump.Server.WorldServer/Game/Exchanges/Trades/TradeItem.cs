using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Item = Stump.Server.WorldServer.Game.Items.Item;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades
{
    public abstract class TradeItem : Item
    {
        public abstract CharacterInventoryPositionEnum Position
        {
            get;
        }

        public override ObjectItem GetObjectItem()
        {
            return new ObjectItem(
                (byte)Position,
                (short)Template.Id,
                0,
                false,
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                (int)Stack);
        }
    }
}