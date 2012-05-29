using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.Rekop)]
    public class RekopCastHandler : DefaultSpellCastHandler
    {
        public RekopCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public int CastRound
        {
            get;
            set;
        }

        public override void Initialize()
        {
            base.Initialize();

            // 0 to 3 rounds
            CastRound = new Random().Next(0, 4);
            Handlers = Handlers.Where(entry => entry.Effect.Duration == CastRound).ToArray();
        }
    }
}