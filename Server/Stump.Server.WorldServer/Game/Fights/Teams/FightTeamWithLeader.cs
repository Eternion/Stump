using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public abstract class FightTeamWithLeader<T> : FightTeam 
        where T : FightActor
    {
        public FightTeamWithLeader(TeamEnum id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightTeamWithLeader(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide) : base(id, placementCells, alignmentSide)
        {
        }

        public new T Leader
        {
            get { return (T)base.Leader; }
        }

        public override bool ChangeLeader(FightActor leader)
        {
            if (!(leader is T))
                throw new Exception(string.Format("Leader of a FightPlayerTeam must be a {0} not {1}", typeof(T), leader.GetType()));

            return base.ChangeLeader(leader);
        }

        protected override void OnFighterAdded(FightActor fighter)
        {
            if (Fighters.Count == 1 && !(Fighters[0] is T))
                throw new Exception(string.Format("Leader of a FightPlayerTeam must be a {0} not {1}", typeof(T), Fighters[0].GetType()));


            base.OnFighterAdded(fighter);
        }
    }
}