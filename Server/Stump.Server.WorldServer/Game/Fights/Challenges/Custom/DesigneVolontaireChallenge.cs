using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier(3)]
    public class DesigneVolontaireChallenge : DefaultChallenge
    {
        public DesigneVolontaireChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 25;

            Target = fight.GetRandomFighter<MonsterFighter>();

            foreach (var fighter in Target.Team.Fighters)
            {
                fighter.Dead += OnDead;
            }

            TargetId = Target.Id;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            if (victim == Target)
                UpdateStatus(ChallengeStatusEnum.SUCCESS);
            else if (victim != Target && Status == ChallengeStatusEnum.RUNNING)
                UpdateStatus(ChallengeStatusEnum.FAILED);
        }

        public MonsterFighter Target
        {
            get;
            private set;
        }
    }
}
