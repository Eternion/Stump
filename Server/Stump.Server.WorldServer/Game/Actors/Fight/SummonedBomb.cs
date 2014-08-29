using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedBomb : SummonedFighter
    {
        public SummonedBomb(int id, FightTeam team, Spell summoningSpell, FightActor summoner, Cell cell)
            : base(id, team, Enumerable.Empty<Spell>(), summoner, cell)
        {
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { throw new System.NotImplementedException(); }
        }

        public override StatsFields Stats
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string GetMapRunningFighterName()
        {
            throw new System.NotImplementedException();
        }

        public override string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations();
        }
    }
}