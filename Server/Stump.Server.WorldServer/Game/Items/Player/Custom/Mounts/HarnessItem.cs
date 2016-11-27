using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.HARNACHEMENT)]
    public class HarnessItem : BasePlayerItem
    {
        private CharacterInventoryPositionEnum m_position;

        public HarnessItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            HarnessTemplate = MountManager.Instance.GetHarness(Template.Id);
        }

        public HarnessRecord HarnessTemplate
        {
            get;
            private set;
        }

        public override CharacterInventoryPositionEnum Position
        {
            get { return m_position; }
            set
            {
                if (value == CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS)
                    value = CharacterInventoryPositionEnum.ACCESSORY_POSITION_RIDE_HARNESS;

                m_position = value;
            }
        }

        public override bool CanEquip()
        {
            if (!Owner.HasEquippedMount())
            {
                // Vous ne pouvez pas équiper un harnachement directement, essayez plutôt de l'associer sur une monture équipée.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 491);
            }

            return base.CanEquip() && HarnessTemplate != null;
        }

        public override ActorLook UpdateItemSkin(ActorLook characterLook)
        {
            return characterLook;
        }

        public override bool OnEquipItem(bool unequip)
        {
            Owner.UpdateLook();
            Owner.EquippedMount?.RefreshMount();

            return base.OnEquipItem(unequip);
        }
    }
}