using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

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
            get { return Fighter.IsAlive(); }
        }

        public bool HasLeft
        {
            get { return Fighter.HasLeft(); }
        }

        public int Id
        {
            get { return Fighter.Id; }
        }

        public int Prospecting
        {
            get { return Fighter.Stats[PlayerFields.Prospecting].Total; }
        }

        public int Wisdom
        {
            get { return Fighter.Stats[PlayerFields.Wisdom].Total; }
        }

        public int Level
        {
            get { return Fighter.Level; }
        }

        public virtual bool CanLoot(FightTeam team)
        {
            return false;
        }

        public IFight Fight
        {
            get { return Fighter.Fight; }
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
        bool HasLeft
        {
            get;
        }

        int Id
        {
            get;
        }

        int Prospecting
        {
            get;
        }

        int Wisdom
        {
            get;
        }

        int Level
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

        IFight Fight
        {
            get;
        }

        bool CanLoot(FightTeam looters);

        FightResultListEntry GetFightResultListEntry();
        void Apply();
    }
}