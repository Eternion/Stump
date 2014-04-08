using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.AI.Fights.Threat
{
    public class ThreatCalculator
    {
        public ThreatCalculator(AIFighter fighter)
        {
            Fighter = fighter;
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public Fight Fight
        {
            get { return Fighter.Fight; }
        }

        public float CalculateThreat(FightActor fighter)
        {
            return 0;
        }
    }
}