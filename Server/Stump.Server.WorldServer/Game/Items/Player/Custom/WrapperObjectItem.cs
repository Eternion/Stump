using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Inventory;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.OBJET_D_APPARAT)]
    public class WrapperObjectItem : BasePlayerItem
    {
        public WrapperObjectItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override bool CanDrop(BasePlayerItem item)
        {
            return true;
        }

        public override bool CanEquip()
        {
            //Vous ne pouvez pas équiper un objet d'apparat directement, essayez plutôt de l'associer à un objet équipé compatible.
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 445);
            return false;
        }

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            if (Owner.IsInFight())
                return false;

            var compatibleEffect = Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Compatible) as EffectInteger;

            if (compatibleEffect == null)
                return false;

            if (dropOnItem.Template.TypeId != compatibleEffect.Value)
                return false;

            if (dropOnItem.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId || x.EffectId == EffectsEnum.Effect_Appearance || x.EffectId == EffectsEnum.Effect_Apparence_Wrapper))
                return false;

            dropOnItem.Effects.Add(new EffectInteger(EffectsEnum.Effect_Apparence_Wrapper, (short)Template.Id));

            dropOnItem.Invalidate();
            Owner.Inventory.RefreshItem(dropOnItem);
            dropOnItem.OnObjectModified();

            Owner.UpdateLook();

            InventoryHandler.SendWrapperObjectAssociatedMessage(Owner.Client, dropOnItem);

            return true;
        }
    }
}
