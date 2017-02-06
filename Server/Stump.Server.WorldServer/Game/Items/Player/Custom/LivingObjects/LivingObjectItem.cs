using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom.LivingObjects
{
    [ItemType(ItemTypeEnum.OBJET_VIVANT)]
    public sealed class LivingObjectItem : CommonLivingObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public LivingObjectItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {            
            LivingObjectRecord = ItemManager.Instance.TryGetLivingObjectRecord(Template.Id);
                        
            if (LivingObjectRecord == null)
            {
                logger.Error("Living Object {0} has no template", Template.Id);
                return;
            }

            Initialize();
        }

        public override bool CanDrop(BasePlayerItem item) => true;

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            if (Owner.IsInFight())
                return false;

            if (dropOnItem.Template.TypeId != LivingObjectRecord.ItemType)
                return false;

            if (dropOnItem.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId || x.EffectId == EffectsEnum.Effect_Appearance || x.EffectId == EffectsEnum.Effect_Apparence_Wrapper))
                return false;

            // check type

            dropOnItem.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectId, (short)Template.Id));
            foreach (var effect in Effects.Where(x => x.EffectId != EffectsEnum.Effect_NonExchangeable_981 && x.EffectId != EffectsEnum.Effect_NonExchangeable_982))
            {
                dropOnItem.Effects.RemoveAll(x => x.EffectId == effect.EffectId);
                dropOnItem.Effects.Add(effect);
            }

            var newInstance = Owner.Inventory.RefreshItemInstance(dropOnItem);

            newInstance.Invalidate();
            newInstance.OnObjectModified();

            Owner.UpdateLook();

            return true;
        }

        public override bool CanEquip()
        {
            //Vous ne pouvez pas équiper un objet vivant directement, essayez plutôt de l'associer sur un objet équipé qu'il affectionne.
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 161);
            return false;
        }
    }
}