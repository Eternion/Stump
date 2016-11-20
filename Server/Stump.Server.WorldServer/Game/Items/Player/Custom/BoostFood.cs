using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.NOURRITURE_BOOST)]
    public class BoostFood : BasePlayerItem
    {
        public BoostFood(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            Owner.FightEnded += OnFightEnded;
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (!unequip)
                Owner.Inventory.RemoveItem(this);
            
            return base.OnEquipItem(unequip);
        }

        public override bool OnRemoveItem()
        {
            Owner.FightEnded -= OnFightEnded;

            return base.OnRemoveItem();
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            var effect = Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_RemainingFights);

            if (effect == null)
                return;

            Invalidate();

            if (--effect.Value <= 0)
                Owner.Inventory.RemoveItem(this);
            else
                Owner.Inventory.RefreshItem(this);
        }
    }
}