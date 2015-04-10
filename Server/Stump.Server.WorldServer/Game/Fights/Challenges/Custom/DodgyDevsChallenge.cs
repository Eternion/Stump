namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    public class DodgyDevsChallenge : DefaultChallenge
    {
        public DodgyDevsChallenge(IFight fight)
            : base(fight)
        {
        }

        public DodgyDevsChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 1;
        }
    }
}
