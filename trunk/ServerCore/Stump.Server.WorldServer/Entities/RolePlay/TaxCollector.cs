using System;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities.Actors
{
    public class TaxCollector : Actor, ISpellsOwner
    {
        public TaxCollector(long id, EntityLook look, ObjectPosition position)
            : base(id, look, position)
        {
        }

        public SpellInventory SpellInventory
        {
            get;
            private set;
        }
    }
}