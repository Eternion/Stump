using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.Mounts;
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
            private set;
        }

        public MapPaddock Paddock
        {
            get;
            private set;
        }

        public void EquipMount(Mount mount)
        {
            mount.Owner = Character;
            Character.Mount = mount;

            MountManager.Instance.LinkMountToCharacter(Character, mount);
            MountHandler.SendMountSetMessage(Character.Client, mount.GetMountClientData());
            MountHandler.SendMountXpRatioMessage(Character.Client, mount.GivenExperience);
        }

        public bool HasMountRight(Mount mount, bool equip = false)
        {
            if (equip && Character.HasEquipedMount())
                return false;

            if (mount.Owner != null && Character.Id != mount.OwnerId)
                return false;

            if (!equip || Character.Level >= Mount.RequiredLevel)
                return true;

            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227, Mount.RequiredLevel);
            return false;
        }

        public int GetMountByItem(BasePlayerItem item)
        {
            var effect = item.Effects.FirstOrDefault(x => x.GetEffectInstance() is EffectInstanceMount);
            if (effect == null)
                return -1;

            var effectInstance = effect.GetEffectInstance() as EffectInstanceMount;
            return (int)effectInstance.mountId;
        }

        public bool EquipToPaddock(int mountId)
        {
            if (Character.HasEquipedMount())
                return false;

            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (Character.HasEquipedMount())
                    return;

                Paddock.AddMountToPaddock(Character.Mount);
                InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, Character.Mount);

                Character.Mount.Release(Character);          
            });

            return true;
        }

        public bool EquipToStable(int mountId)
        {
            if (Character.HasEquipedMount())
                return false;

            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (Character.HasEquipedMount())
                    return;

                Paddock.AddMountToStable(Character.Mount);
                InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, Character.Mount);

                Character.Mount.Release(Character);       
            });

            return true;
        }

        public bool PaddockToEquip(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromPaddock(mount);
                EquipMount(mount);
            });

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool PaddockToStable(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromPaddock(mount);
                Paddock.AddMountToStable(mount);

                InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);
            });

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool StableToPaddock(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromStable(mount);
                Paddock.AddMountToPaddock(mount);
                InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);
            });

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);
            

            return true;
        }

        public bool StableToEquip(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount, true))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromStable(mount);
                EquipMount(mount);
            });

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool StableToInventory(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromStable(mount);
                MountManager.Instance.StoreMount(Character, mount);
            });

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool PaddockToInventory(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            if (!HasMountRight(mount))
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                Paddock.RemoveMountFromPaddock(mount);
                MountManager.Instance.StoreMount(Character, mount);
            });

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            return true;
        }

        public bool EquipToInventory(int mountId)
        {
            if (Character.HasEquipedMount())
                return false;

            if (!HasMountRight(Character.Mount))
                return false;

            if (Character.Mount.Id != mountId)
                return false;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                MountManager.Instance.StoreMount(Character, Character.Mount);
                Character.Mount.Release(Character);
            });

            return true;
        }

        public bool InventoryToStable(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId);
            var mountId = GetMountByItem(item);
            if (mountId == -1)
                return false;

            Character.Inventory.RemoveItem(item);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var mount = MountManager.Instance.GetMountById(mountId);
                if (mount == null)
                    return;

                mount.Owner = Character;

                Paddock.AddMountToStable(mount);

                InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);
            });

            return true;
        }

        public bool InventoryToPaddock(int itemId)
        {
            var item = Character.Inventory.TryGetItem(itemId);
            var mountId = GetMountByItem(item);
            if (mountId == -1)
                return false;

            Character.Inventory.RemoveItem(item);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var mount = MountManager.Instance.GetMountById(mountId);
                if (mount == null)
                    return;

                mount.Owner = Character;
                Paddock.AddMountToPaddock(mount);

                InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);
            });

            return true;
        }

        public bool InventoryToEquip(int itemId)
        {
            if (Character.HasEquipedMount())
                return false;

            var item = Character.Inventory.TryGetItem(itemId);
            var mountId = GetMountByItem(item);
            if (mountId == -1)
                return false;

            if (Character.Level < item.Template.Level)
            {
                Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227, Mount.RequiredLevel);
                return false;
            }

            Character.Inventory.RemoveItem(item);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                var mount = MountManager.Instance.GetMountById(mountId);
                if (mount == null)
                    return;

                mount.Owner = Character;
                
                EquipMount(mount);
            });

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
