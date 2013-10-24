using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    public class BaseItemHandler
    {
        public virtual void ItemAdded(Character character, PlayerItem item)
        {
        }

        public virtual void ItemRemoved(Character character, PlayerItem item)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <returns>True whenever the item has been used and must be removed</returns>
        public virtual bool UseItem(Character character, PlayerItem item)
        {
            var remove = false;
            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetUsableEffectHandler(effect, character, item);

                if (handler.Apply())
                    remove = true;
            }

            return remove;
        }

        public virtual void EquipItem(Character character, PlayerItem item, bool unequip)
        {
        }

        public virtual bool AllowFeeding
        {
            get
            {
                return false;
            }
        }

        public virtual void Feed(Character character, PlayerItem item, PlayerItem food)
        {
        }
    }
}