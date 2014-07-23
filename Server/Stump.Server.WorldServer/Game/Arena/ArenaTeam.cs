using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaTeam : FightTeamWithLeader<CharacterFighter>
    {
        public ArenaTeam(TeamEnum id, Cell[] placementCells)
            : base(id, placementCells)
        {
        }


        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_PLAYER; }
        }
    }
}