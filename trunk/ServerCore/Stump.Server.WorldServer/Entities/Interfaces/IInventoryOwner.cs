
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.Entities
{
    public interface IInventoryOwner
    {
        Inventory Inventory
        {
            get;
            set;
        }
    }
}