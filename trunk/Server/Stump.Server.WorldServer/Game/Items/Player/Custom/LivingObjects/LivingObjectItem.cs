using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom.LivingObjects
{
    [ItemType(ItemTypeEnum.LIVING_OBJECTS)]
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

        public override bool AllowDropping
        {
            get { return true; }
        }

        public override bool Drop(BasePlayerItem dropOnItem)
        {
            if (dropOnItem.Template.TypeId != LivingObjectRecord.ItemType)
                return false;

            if (dropOnItem.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId))
                return false;

            // check type

            dropOnItem.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectId, (short)Template.Id));
            foreach (var effect in Effects)
            {
                dropOnItem.Effects.RemoveAll(x => x.EffectId == effect.EffectId);
                dropOnItem.Effects.Add(effect);
            }

            Owner.Inventory.RefreshItemInstance(dropOnItem);
            Owner.UpdateLook();

            return true;
        }
    }
}