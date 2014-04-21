using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    public class ItemIdAttribute : Attribute
    {
        public ItemIdAttribute(int itemId)
        {
            ItemId = (ItemIdEnum)itemId;
        }

        public ItemIdAttribute(ItemIdEnum itemId)
        {
            ItemId = itemId;
        }

        public ItemIdEnum ItemId
        {
            get;
            set;
        }
    }
}
