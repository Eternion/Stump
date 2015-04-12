using Stump.DofusProtocol.Enums.Custom;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.LE_CHEAT_DES_DEVS)]
    public class DodgyDevsChallenge : DefaultChallenge
    {
        public DodgyDevsChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 1;
            BonusMax = 1;
        }
    }
}
