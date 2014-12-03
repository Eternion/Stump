using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.Mounts;
using MapPaddock = Stump.Server.WorldServer.Game.Maps.Paddocks.Paddock;

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
            Character.Mount = mount;

            MountHandler.SendMountSetMessage(Character.Client, mount.GetMountClientData());
            MountManager.Instance.AddMount(mount);
        }

        public void UnequipMount()
        {
            Character.Mount.Dismount();

            MountHandler.SendMountUnSetMessage(Character.Client);
            MountManager.Instance.DeleteMount(Character.Mount);
        }

        public bool EquipToPaddock(int mountId)
        {
            if (Character.Mount.Id != mountId)
                return false;

            Paddock.AddMountToPaddock(Character.Mount);
            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, Character.Mount);

            UnequipMount();

            return true;
        }

        public bool EquipToStable(int mountId)
        {
            if (Character.Mount.Id != mountId)
                return false;

            Paddock.AddMountToStable(Character.Mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, Character.Mount);

            UnequipMount();

            return true;
        }

        public bool PaddockToEquip(int mountId)
        {
            if (Character.HasEquipedMount())
                return false;

            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);

            EquipMount(mount);

            return true;
        }

        public bool PaddockToStable(int mountId)
        {
            var mount = Paddock.GetPaddockedMount(mountId);
            if (mount == null)
                return false;

            Paddock.RemoveMountFromPaddock(mount);
            Paddock.AddMountToStable(mount);

            InventoryHandler.SendExchangeMountPaddockRemoveMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountStableAddMessage(Character.Client, mount);

            return true;
        }

        public bool StableToPaddock(int mountId)
        {
            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            Paddock.RemoveMountFromStable(mount);
            Paddock.AddMountToPaddock(mount);

            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);
            InventoryHandler.SendExchangeMountPaddockAddMessage(Character.Client, mount);

            return true;
        }

        public bool StableToEquip(int mountId)
        {
            if (Character.HasEquipedMount())
                return false;

            var mount = Paddock.GetStabledMount(mountId);
            if (mount == null)
                return false;

            Paddock.RemoveMountFromStable(mount);
            InventoryHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);

            EquipMount(mount);

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
