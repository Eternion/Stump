using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class FightTeam
    {
        private List<FightActor> m_fighters = new List<FightActor>();


        public FightTeam(sbyte id, Cell[] placementCells, TeamEnum teamType)
        {
            Id = id;
            PlacementCells = placementCells;
            TeamType = teamType;
        }

        public sbyte Id
        {
            get;
            private set;
        }

        public Cell[] PlacementCells
        {
            get;
            private set;
        }

        public TeamEnum TeamType
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get;
            internal set;
        }

        public FightActor Leader
        {
            get
            {
                return m_fighters.Count > 0 ? m_fighters.First() : null;
            }
        }

        public void AddFighter(FightActor actor)
        {
        }

        public void RemoveFighter(FightActor actor)
        {
        }

        public FightTeamInformations GetFightTeamInformations()
        {
            return new FightTeamInformations((sbyte) Id,
                Leader != null ? Leader.Id : 0,
                (sbyte) AlignmentSideEnum.ALIGNMENT_WITHOUT,
                (sbyte) TeamType,
                m_fighters.Select(entry => entry.GetFightTeamMemberInformations())
                );
        }
    }
}