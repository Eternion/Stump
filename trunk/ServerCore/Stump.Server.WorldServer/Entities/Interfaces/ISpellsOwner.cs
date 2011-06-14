
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public interface ISpellsOwner
    {
        SpellInventory SpellInventory
        {
            get;
        }
    }
}