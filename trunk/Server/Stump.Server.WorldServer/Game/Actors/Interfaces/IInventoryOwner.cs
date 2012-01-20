using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Actors.Interfaces
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