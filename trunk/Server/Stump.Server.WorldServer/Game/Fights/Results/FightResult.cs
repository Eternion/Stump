using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightResult<T> : IFightResult where T : FightActor
    {
        public FightResult(T fighter, FightOutcomeEnum outcome, FightLoot loot)
        {
            Fighter = fighter;
            Outcome = outcome;
            Loot = loot;
        }

        public T Fighter
        {
            get;
            protected set;
        }

        #region IFightResult Members

        public bool Alive
        {
            get { return Fighter.IsAlive() && !Fighter.HasLeft(); }
        }

        public int Id
        {
            get { return Fighter.Id; }
        }

        public FightLoot Loot
        {
            get;
            protected set;
        }

        public FightOutcomeEnum Outcome
        {
            get;
            protected set;
        }

        public virtual FightResultListEntry GetFightResultListEntry()
        {
            return new FightResultFighterListEntry((short) Outcome, Loot.GetFightLoot(), Id, Alive);
        }

        public virtual void Apply()
        {
        }

        #endregion
    }

    public class FightResult : FightResult<FightActor>
    {
        public FightResult(FightActor fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }
    }

    public interface IFightResult
    {
        bool Alive
        {
            get;
        }

        int Id
        {
            get;
        }

        FightLoot Loot
        {
            get;
        }

        FightOutcomeEnum Outcome
        {
            get;
        }

        FightResultListEntry GetFightResultListEntry();
        void Apply();
    }
}