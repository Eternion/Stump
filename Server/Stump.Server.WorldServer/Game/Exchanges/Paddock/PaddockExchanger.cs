using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Handlers.Inventory;
using MapPaddock = Stump.Server.WorldServer.Game.Maps.Paddocks.Paddock;
using Mount = Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts.Mount;

namespace Stump.Server.WorldServer.Game.Exchanges.Paddock
{
    public class PaddockExchanger : Exchanger
    {
        public PaddockExchanger(Character character, MapPaddock paddock, PaddockExchange paddockExchange)
            : base(paddockExchange)
        {
            Character = character;
            Paddock = paddock;
        }

        public Character Character
        {
            get;
        }

        public MapPaddock Paddock
        {
            get;
        }

        public void EquipMount(Mount mount)
        {
            Character.EquipMount(mount);
        }

        public Mount GetStabledMount(int mountId)
        {
            var mount = Character.GetStabledMount(mountId);
            return mount.IsInStable && mount.Paddock == Paddock ? mount : null;
        }

        public bool HasMountRight(Mount mount, bool equip = false)
        {
            if (equip && Character.HasEquippedMount())
                return false;

            if (mount.Owner != null && Character != mount.Owner)
                return false;

            if (!equip || Character.Level >= Mount.RequiredLevel)
                return true;

            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227, Mount.RequiredLevel);
            return false;
        }

        public bool EquipToPaddock(int mountId)
        {
            if (!Character.HasEquippedMount())
                return false;

            if (!HasMountRight(Character.EquippedMount))
                return false;

            if (Character.EquippedMount.Id != mountId)
                return false;

            var mount = Character.EquippedMount;
            Character.UnEquipMount();
            Paddock.AddMountToPaddock(mount);

            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);

            return true;
        }

        public bool EquipToStable(int mountId)
        {
            if (!Character.HasEquippedMount())
                return false;

            if (!HasMountRight(Character.EquippedMount))
                return false;

            if (Character.EquippedMount.Id != mountId)
                return false;

            var mount = Character.EquippedMount;
            Character.UnEquipMount();
            Character.AddStabledMount(mount);
            Paddock.AddMountToStable(mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);

            return true;
        }

        public bool PaddockToEquip(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(Character, mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            Character.SetOwnedMount(mount);
            Character.EquipMount(mount);

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool PaddockToStable(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(Character, mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            Character.AddStabledMount(mount);
            Paddock.AddMountToStable(mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool StableToPaddock(int mountId)
        {
            var mount = GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Character.RemoveStabledMount(mount);
            Paddock.RemoveMountFromStable(mount);
            Paddock.AddMountToPaddock(mount);
            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool StableToEquip(int mountId)
        {
            var mount = GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            Character.RemoveStabledMount(mount);
            Paddock.RemoveMountFromStable(mount);
            EquipMount(mount);

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool StableToInventory(int mountId)
        {
            var mount = GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Character.RemoveStabledMount(mount);
            Paddock.RemoveMountFromStable(mount);
            MountManager.Instance.StoreMount(Character, mount);
            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool PaddockToInventory(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(Character, mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            Paddock.RemoveMountFromStable(mount);
            Character.SetOwnedMount(mount);
            MountManager.Instance.StoreMount(Character, mount);
            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool EquipToInventory(int mountId)
        {
            if (!Character.HasEquippedMount())
                return false;

            if (!HasMountRight(Character.EquippedMount))
                return false;

            if (Character.EquippedMount.Id != mountId)
                return false;

            MountManager.Instance.StoreMount(Character, Character.EquippedMount);
            Character.UnEquipMount();

            return true;
        }

        public bool InventoryToStable(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId) as MountCertificate;
            if (item == null || !item.CanConvert())
                return false;

            if (item.Mount == null)
                return false;

            Character.Inventory.RemoveItem(item);
            Character.AddStabledMount(item.Mount);
            Paddock.AddMountToStable(item.Mount);

            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, item.Mount);

            return true;
        }

        public bool InventoryToPaddock(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId) as MountCertificate;
            if (item == null || !item.CanConvert())
                return false;

            if (item.Mount == null)
                return false;

            Character.Inventory.RemoveItem(item);
            Paddock.AddMountToPaddock(item.Mount);

            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, item.Mount);

            return true;
        }

        public bool InventoryToEquip(int itemId)
        {
            if (Character.HasEquippedMount())
                return false;
            var item = Character.Inventory.TryGetItem(itemId) as MountCertificate;
            if (item == null || !item.CanConvert())
                return false;

            if (item.Mount == null)
                return false;

            Character.Inventory.RemoveItem(item);
            EquipMount(item.Mount);

            return true;
        }

        public override bool MoveItem(int id, int quantity)
        {
            return false;
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}