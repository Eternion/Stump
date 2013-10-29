using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    public class BaseItemHandler
    {
        public BaseItemHandler(PlayerItem item)
        {
            Character = item.Owner;
            Item = item;
        }

        public PlayerItem Item
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public virtual bool AddItem()
        {
            return true;
        }

        public virtual bool RemoveItem()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="targetCell"></param>
        /// <param name="target"></param>
        /// <returns>Returns the amount of items to remove</returns>
        public virtual uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            uint removed = 0;
            foreach (var effect in Item.Effects)
            {
                var handler = EffectManager.Instance.GetUsableEffectHandler(effect, target ?? Character, Item);
                handler.NumberOfUses = amount;
                handler.TargetCell = targetCell;

                if (handler.Apply())
                    removed = Math.Max(handler.UsedItems, removed);
            }

            return removed;
        }

        public virtual bool EquipItem(bool unequip)
        {
            return true;
        }

        public virtual bool AllowFeeding
        {
            get
            {
                return false;
            }
        }

        public virtual bool Feed(PlayerItem food)
        {
            return false;
        }

        public virtual bool AllowDropping
        {
            get
            {
                return false;
            }
        }

        public virtual bool Drop(PlayerItem dropOnItem)
        {
            return false;
        }
    }
}