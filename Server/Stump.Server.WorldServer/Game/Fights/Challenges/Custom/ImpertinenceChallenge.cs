using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.HARDI)]
    [ChallengeIdentifier((int)ChallengeEnum.COLLANT)]
    public class ImpertinenceChallenge : DefaultChallenge
    {
        private readonly FightTeam m_team;

        public ImpertinenceChallenge(int id, IFight fight)
            : base(id, fight)
        {
            if (id == (int)ChallengeEnum.HARDI)
            {
                BonusMin = 25;
                BonusMax = 25;
            }
            else
            {
                BonusMin = 40;
                BonusMax = 40;
            }

            m_team = Fight.DefendersTeam is FightMonsterTeam ? Fight.DefendersTeam : Fight.ChallengersTeam;
            if (id == (int) ChallengeEnum.COLLANT)
                m_team = m_team.OpposedTeam;
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.BeforeTurnStopped += OnBeforeTurnStopped;
        }

        public override bool IsEligible()
        {
            return m_team.GetAllFighters().Count() > 1;
        }

        private void OnBeforeTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.Position.Point.GetAdjacentCells(x => m_team.GetOneFighter(Fight.Map.GetCell(x)) != null).Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnBeforeTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnBeforeTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
