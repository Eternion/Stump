using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Handlers
{
    [ItemType(ItemTypeEnum.LIVING_OBJECTS)]
    public class LivingObjectHandler : BaseItemHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private LivingObjectRecord m_record;

        public LivingObjectHandler(PlayerItem item)
            : base(item)
        {
            var idEffect = Item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectId);

            if (idEffect == null)
                ResetLivingObject();

            m_record = ItemManager.Instance.TryGetLivingObjectRecord(Item.Template.Id);
        }

        private void ResetLivingObject()
        {
            Item.Effects.Clear();

            m_record = ItemManager.Instance.TryGetLivingObjectRecord(Item.Template.Id);

            if (m_record == null)
            {
                logger.Error("Living Object {0} has no template", Item.Template.Id);
                return;
            }

            Item.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectId, (short) m_record.Id));
            Item.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectMood, 0));
            Item.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectSkin, (short)m_record.Moods[0][0]));
            Item.Effects.Add(new EffectInteger(EffectsEnum.Effect_LivingObjectCategory, 1)); // ??
        }


        public override bool AllowDropping
        {
            get { return true; }
        }

        public override bool Drop(PlayerItem dropOnItem)
        {
            return false;
        }
    }
}