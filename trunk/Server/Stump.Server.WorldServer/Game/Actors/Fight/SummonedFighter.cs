using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class SummonedFighter : AIFighter
    {
        protected SummonedFighter(int id, FightTeam team, Spell[] spells, FightActor summoner, Cell cell)
            : base(team, spells)
        {
            Id = id;
            Position = summoner.Position.Clone();
            Cell = cell;
            Summoner = summoner;
        }

        public override sealed int Id
        {
            get;
            protected set;
        }

        public FightActor Summoner
        {
            get;
            protected set;
        }


        public override int GetTackledAP()
        {
            return 0;
        }

        public override int GetTackledMP()
        {
            return 0;
        }

        protected override void OnDead(FightActor killedBy)
        {
            base.OnDead(killedBy);

            Fight.TimeLine.RemoveFighter(this);
            Delete();

            ContextHandler.SendGameFightTurnListMessage(Fight.Clients, Fight);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            Summoner.RemoveSummon(this);
        }
    }
}