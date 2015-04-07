using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CRUEL)]
    public class CruelChallenge : DefaultChallenge
    {
        public CruelChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 40;

            foreach (var fighter in fight.GetAllFighters<MonsterFighter>())
            {
                fighter.Dead += OnDead;
            }
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            foreach (var f in victim.Team.GetAllFighters(x => x.IsAlive()).Where(fighter => fighter.Level > victim.Level))
            {
                UpdateStatus(ChallengeStatusEnum.FAILED);
            }

            victim.Dead -= OnDead;
        }
    }
}
