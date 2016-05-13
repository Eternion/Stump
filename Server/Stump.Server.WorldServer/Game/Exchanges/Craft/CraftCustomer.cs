using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class CraftCustomer : CraftingActor
    {
        public CraftCustomer(BaseCraftDialog trade, Character character)
            : base(trade, character)
        {
        }
    }
}