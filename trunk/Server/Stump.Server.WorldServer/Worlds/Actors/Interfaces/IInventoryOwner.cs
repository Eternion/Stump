using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Actors.Interfaces
{
    public interface IInventoryOwner
    {
        int Id
        {
            get;
        }

        Inventory Inventory
        {
            get;
        }
    }
}