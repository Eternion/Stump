using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ItemTypeAttribute : Attribute
    {
        public ItemTypeAttribute(ItemTypeEnum itemType)
        {
            ItemType = itemType;
        }

        public ItemTypeEnum ItemType
        {
            get;
            set;
        }
    }
}