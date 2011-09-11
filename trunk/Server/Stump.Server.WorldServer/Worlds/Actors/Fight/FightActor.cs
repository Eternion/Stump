using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class FightActor : ContextActor, IStatsOwner
    {
        #region Events
        public event Action<FightActor, bool> ReadyStateChanged;

        private void NotifyReadyStateChanged(bool isReady)
        {
            Action<FightActor, bool> handler = ReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, bool> TurnReadyStateChanged;

        private void NotifyTurnReadyStateChanged(bool isReady)
        {
            Action<FightActor, bool> handler = TurnReadyStateChanged;
            if (handler != null)
                handler(this, isReady);
        }

        public event Action<FightActor, Cell> CellShown;

        private void NotifyCellShown(Cell cell)
        {
            var handler = CellShown;
            if (handler != null)
                CellShown(this, cell);
        }

        public event Action<FightActor, int, FightActor> LifePointsChanged;

        private void NotifyLifePointsChanged(int delta, FightActor from)
        {
            var handler = LifePointsChanged;

            if (handler != null)
                handler(this, delta, from);
        }

        public event Action<FightActor> FighterLeft;

        internal void NotifyFightLeft()
        {
            Action<FightActor> handler = FighterLeft;
            if (handler != null)
                handler(this);
        }

        public event Action<FightActor, MovementPath> Moved;

        private void NotifyMoved(MovementPath movementPath)
        {
            Action<FightActor, MovementPath> handler = Moved;
            if (handler != null)
                handler(this, movementPath);
        }

        public event Action<FightActor, ObjectPosition> PrePlacementChanged;

        private void NotifyPrePlacementChanged(ObjectPosition position)
        {
            Action<FightActor, ObjectPosition> handler = PrePlacementChanged;
            if (handler != null)
                handler(this, position);
        }

        public event Action<FightActor> TurnPassed;

        private void NotifyTurnPassed()
        {
            Action<FightActor> handler = TurnPassed;
            if (handler != null)
                handler(this);
        }

        public event Action<FightActor, FightActor> Dead;

        private void NotifyDead(FightActor killedBy)
        {
            Action<FightActor, FightActor> handler = Dead;
            if (handler != null)
                handler(this, killedBy);
        }

        public event Action<FightActor, SequenceTypeEnum> SequenceStarted;

        private void NotifySequenceStarted(SequenceTypeEnum sequenceType)
        {
            var handler = SequenceStarted;
            if (handler != null)
                handler(this, sequenceType);
        }

        public event Action<FightActor, SequenceTypeEnum> SequenceEnded;

        private void NotifySequenceEnded(SequenceTypeEnum sequenceType)
        {
            var handler = SequenceEnded;
            if (handler != null)
                handler(this, sequenceType);
        }

        public event Action<FightActor, SequenceTypeEnum> SequenceActionEnded;

        private void NotifySequenceActionEnded(SequenceTypeEnum sequenceType)
        {
            var handler = SequenceActionEnded;
            if (handler != null)
                handler(this, sequenceType);
        }

        #endregion

        protected FightActor(FightTeam team)
        {
            Team = team;
        }

        public Fights.Fight Fight
        {
            get { return Team.Fight; }
        }

        public FightTeam Team
        {
            get;
            private set;
        }

        public override IContext Context
        {
            get
            {
                return Fight;
            }
        }

        public abstract ObjectPosition MapPosition
        {
            get;
        }

        public FightActor CarriedActor
        {
            get;
            protected set;
        }

        public SequenceTypeEnum Sequence
        {
            get;
            private set;
        }

        public bool IsSequencing
        {
            get;
            private set;
        }

        #region Stats

        public abstract StatsFields Stats
        {
            get;
        }

        public int LifePoints
        {
            get
            {
                return Stats[CaracteristicsEnum.Health].Total;
            }
        }

        public int MaxLifePoints
        {
            get
            {
                return ((StatsHealth)Stats[CaracteristicsEnum.Health]).TotalMax;
            }
        }

        public short DamageTaken
        {
            get
            {
                return Stats[CaracteristicsEnum.Health].Context;
            }
            set
            {
                Stats[CaracteristicsEnum.Health].Context = value;
            }
        }

        public int AP
        {
            get
            {
                return Stats[CaracteristicsEnum.AP].Total;
            }
        }

        public int MP
        {
            get
            {
                return Stats[CaracteristicsEnum.MP].Total;
            }
        }

        public bool UseAp(short amount)
        {
            if (Stats[CaracteristicsEnum.AP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.AP].Context -= amount;

            return true;
        }

        public bool UseMp(short amount)
        {
            if (Stats[CaracteristicsEnum.MP].Total - amount < 0)
                return false;

            Stats[CaracteristicsEnum.MP].Context -= amount;

            return true;
        }

        #endregion

        public void ToggleReady(bool ready)
        {
            IsReady = ready;

            NotifyReadyStateChanged(ready);
        }

        public bool IsReady
        {
            get;
            private set;
        }

        public void ToggleTurnReady(bool ready)
        {
            IsTurnReady = ready;

            NotifyTurnReadyStateChanged(ready);
        }

        public bool IsTurnReady
        {
            get;
            internal set;
        }

        public void ShowCell(Cell cell)
        {
            NotifyCellShown(cell);
        }

        public bool IsAlive()
        {
            return Stats[CaracteristicsEnum.Health].Total > 0;
        }

        public bool IsDead()
        {
            return !IsAlive();
        }

        public bool IsTeamLeader()
        {
            return Team.Leader == this;
        }

        public bool IsFighterTurn()
        {
            return Fight.TimeLine.Current == this;
        }

        public override bool CanMove()
        {
            return base.CanMove() && IsFighterTurn();
        }

        public virtual bool CanPlay()
        {
            return IsAlive();
        }

        public void PassTurn()
        {
            Fight.RequestTurnEnd(this);

            NotifyTurnPassed();
        }

        public void Die()
        {
            ReceiveDamage((short)LifePoints);
        }

        public short ReceiveDamage(short damage, FightActor from)
        {
            if (LifePoints - damage < 0)
                damage = (short)LifePoints;

            DamageTaken += damage;

            NotifyLifePointsChanged(-damage, from);

            if (IsDead())
                NotifyDead(from);

            return damage;
        }

        public short ReceiveDamage(short damage)
        {
            if (LifePoints - damage < 0)
                damage = (short)LifePoints;

            DamageTaken += damage;

            NotifyLifePointsChanged(-damage, null);

            if (IsDead())
                NotifyDead(null);

            return damage;
        }

        public short Heal(short healPoints)
        {
            if (LifePoints + healPoints > MaxLifePoints)
                healPoints = (short) (MaxLifePoints - LifePoints);

            DamageTaken -= healPoints;

            NotifyLifePointsChanged(healPoints, null);

            return healPoints;
        }

        public void ChangePrePlacement(Cell cell)
        {
            if (!Fight.CanChangePosition(this, cell))
                return; 

            Position.Cell = cell;

            NotifyPrePlacementChanged(Position);
        }

        public bool StartSequence(SequenceTypeEnum sequenceType)
        {
            if (IsSequencing)
                return false;

            IsSequencing = true;
            Sequence = sequenceType;

            NotifySequenceStarted(sequenceType);

            return true;
        }

        public bool EndSequence()
        {
            if (!IsSequencing)
                return false;

            IsSequencing = false;

            NotifySequenceEnded(Sequence);

            return true;
        }

        public void EndSequenceAction()
        {
            switch (Sequence)
            {
                case SequenceTypeEnum.SEQUENCE_MOVE:
                    StopMove();
                    break;
            }

            NotifySequenceActionEnded(Sequence);
        }


        public override EntityDispositionInformations GetEntityDispositionInformations()
        {
            if (CarriedActor != null)
                return new FightEntityDispositionInformations(Position.Cell.Id, (sbyte) Position.Direction, CarriedActor.Id);

            return base.GetEntityDispositionInformations();
        }

        public virtual GameFightMinimalStats GetGameFightMinimalStats()
        {
            return new GameFightMinimalStats(
                Stats[CaracteristicsEnum.Health].Total,
                ((StatsHealth)Stats[CaracteristicsEnum.Health]).TotalMax,
                Stats[CaracteristicsEnum.Health].Base,
                Stats[CaracteristicsEnum.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short) Stats[CaracteristicsEnum.AP].Total,
                (short) Stats[CaracteristicsEnum.MP].Total,
                Stats[CaracteristicsEnum.SummonLimit].Total,
                (short) Stats[CaracteristicsEnum.NeutralResistPercent].Total,
                (short) Stats[CaracteristicsEnum.EarthResistPercent].Total,
                (short) Stats[CaracteristicsEnum.WaterResistPercent].Total,
                (short) Stats[CaracteristicsEnum.AirResistPercent].Total,
                (short) Stats[CaracteristicsEnum.FireResistPercent].Total,
                (short) Stats[CaracteristicsEnum.DodgeAPProbability].Total,
                (short) Stats[CaracteristicsEnum.DodgeMPProbability].Total,
                (short) Stats[CaracteristicsEnum.TackleBlock].Total,
                (short) Stats[CaracteristicsEnum.TackleEvade].Total,
                (int) GameActionFightInvisibilityStateEnum.VISIBLE // invisibility state
                );
        }

        public virtual FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }

        public virtual GameFightFighterInformations GetGameFightFighterInformations()
        {
            return new GameFightFighterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats());
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return GetGameFightFighterInformations();
        }
    }
}