using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.NOURRITURE_BOOST)]
    public class FoodBoostItem : BasePlayerItem
    {
        private bool m_removed;

        public FoodBoostItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
       {
            Owner.FightEnded += OnFightEnded;
            Owner.ContextChanged += OnContextChanged;
        }

        private void OnContextChanged(Character character, bool infight)
        {
            if (infight && character.Fight.IsPvP)
            {
                Owner.Inventory.ApplyItemEffects(this, force: ItemEffectHandler.HandlerOperation.UNAPPLY);
            }
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (unequip && !m_removed)
                Owner.Inventory.RemoveItem(this);
            
            return base.OnEquipItem(unequip);
        }

        public override bool OnRemoveItem()
        {
            m_removed = true;
            Owner.FightEnded -= OnFightEnded;

            return base.OnRemoveItem();
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            var effect = Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_RemainingFights);

            if (effect == null)
                return;

            if (fighter.Fight.IsPvP)
            {
                Owner.Inventory.ApplyItemEffects(this, force: ItemEffectHandler.HandlerOperation.APPLY);
            }

            Invalidate();
            
            if (!fighter.Fight.IsPvP)
            {
                if (--effect.Value <= 0)
                    Owner.Inventory.RemoveItem(this);
                else
                    Owner.Inventory.RefreshItem(this);
            }
        }
    }
}