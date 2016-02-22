using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class CraftCustomer : CraftingActor
    {
        public CraftCustomer(CraftDialog trade, Character character)
            : base(trade, character)
        {
        }
    }
}