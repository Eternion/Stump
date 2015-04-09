using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CIRCULEZ)]
    public class CirculezChallenge : DefaultChallenge
    {
        public CirculezChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 20;

            foreach (var fighter in fight.GetAllFighters<MonsterFighter>())
            {
                fighter.FightPointsVariation += OnFightPointsVariation;
            }
        }

        private void OnFightPointsVariation(FightActor actor, ActionsEnum action, FightActor source, FightActor target, short delta)
        {
            if (delta >= 0)
                return;

            if (!(source is CharacterFighter))
                return;

            if (action != ActionsEnum.ACTION_CHARACTER_MOVEMENT_POINTS_LOST)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, source);
        }
    }
}
