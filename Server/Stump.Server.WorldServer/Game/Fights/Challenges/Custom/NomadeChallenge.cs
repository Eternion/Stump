using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.NOMADE)]
    public class NomadeChallenge : DefaultChallenge
    {
        public NomadeChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 20;

            Fight.BeforeTurnStopped += OnTurnStopped;
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.MP <= 0)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnTurnStopped;
        }
    }
}
