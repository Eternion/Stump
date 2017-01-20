using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Parties;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public abstract class FightTeam
    {
        #region Events

        public event Action<FightTeam, FightActor> FighterAdded;

        protected virtual void OnFighterAdded(FightActor fighter)
        {
            fighter.Dead += OnFighterDead;
            fighter.FighterLeft += OnFighterLeave;

            FighterAdded?.Invoke(this, fighter);
        }

        void OnFighterDead(FightActor actor, FightActor killer)
        {
            m_deadFighters.Add(actor);
        }

        void OnFighterLeave(FightActor actor)
        {
            m_deadFighters.Remove(actor);
        }

        public event Action<FightTeam, FightOptionsEnum> TeamOptionsChanged;

        protected virtual void OnTeamOptionsChanged(FightOptionsEnum option)
        {
            TeamOptionsChanged?.Invoke(this, option);
        }

        public event Action<FightTeam, FightActor> FighterRemoved;

        protected virtual void OnFighterRemoved(FightActor fighter)
        {
            fighter.Dead -= OnFighterDead;
            fighter.FighterLeft -= OnFighterLeave;

            FighterRemoved?.Invoke(this, fighter);
        }

        #endregion

        readonly List<FightActor> m_fighters = new List<FightActor>();
        readonly List<FightActor> m_leavers = new List<FightActor>();  
        readonly List<FightActor> m_deadFighters = new List<FightActor>();
        readonly object m_locker = new object();
                       
        protected FightTeam(TeamEnum id, Cell[] placementCells)
        {
            Id = id;
            PlacementCells = placementCells;
            AlignmentSide = AlignmentSideEnum.ALIGNMENT_WITHOUT;
        }

        protected FightTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
        {
            Id = id;
            PlacementCells = placementCells;
            AlignmentSide = alignmentSide;
        }

        public TeamEnum Id
        {
            get;
        }

        public ObjectPosition BladePosition
        {
            get;
            set;
        }

        public Cell[] PlacementCells
        {
            get;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
        }

        public abstract TeamTypeEnum TeamType
        {
            get;
        }

        public IFight Fight
        {
            get;
            internal set;
        }

        public FightTeam OpposedTeam => Fight.DefendersTeam == this ? Fight.ChallengersTeam : Fight.DefendersTeam;

        public virtual FightActor Leader => m_fighters.Count > 0 ? m_fighters.First() : null;

        public bool IsRestrictedToParty
        {
            get;
            private set;
        }

        public bool IsClosed
        {
            get;
            private set;
        }

        public bool IsAskingForHelp
        {
            get;
            private set;
        }

        public ReadOnlyCollection<FightActor> Fighters => m_fighters.AsReadOnly();

        public ReadOnlyCollection<FightActor> Leavers => m_leavers.AsReadOnly();

        public virtual bool ChangeLeader(FightActor leader)
        {
            if (leader == null)
                throw new ArgumentNullException("leader");

            if (!m_fighters.Contains(leader))
                return false;

            if (m_fighters.Count > 1)
            {
                m_fighters.Remove(leader);
                m_fighters.Insert(0, leader);
            }
            else
            {
                m_fighters.Add(leader);
            }

            return true;
        }

        public void ToggleOption(FightOptionsEnum option)
        {
            switch (option)
            {
                case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                    IsClosed = !IsClosed;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                    IsAskingForHelp = !IsAskingForHelp;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    IsRestrictedToParty = !IsRestrictedToParty;
                    break;
                default:
                    return;
            }

            OnTeamOptionsChanged(option);
        }

        public bool GetOptionState(FightOptionsEnum option)
        {
            switch (option)
            {
                case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                    return IsClosed;
                case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                    return IsAskingForHelp;
                case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                    return Fight.SpectatorClosed;
                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    return IsRestrictedToParty;
                default:
                    return false;
            }
        }

        public bool AreAllReady()
        {
            return m_fighters.All(entry => entry.IsReady);
        }

        public bool AreAllDead()
        {
            return m_fighters.Count <= 0 || m_fighters.Where(x => !(x is SummonedFighter) && !(x is SummonedBomb)).
                All(entry => entry.IsDead() || (entry.HasLeft() && (!(entry is CharacterFighter) || !((CharacterFighter)entry).IsDisconnected)));
        }

        public bool IsFull() => Fight.State == FightState.Placement && (m_fighters.Count > PlacementCells.Count() || m_fighters.Count >= 8);

        public virtual FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (Fight.State != FightState.Placement)
                return FighterRefusedReasonEnum.TOO_LATE;

            if (IsFull())
                return FighterRefusedReasonEnum.TEAM_FULL;

            if (IsClosed)
                return FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;

            if (AlignmentSide != AlignmentSideEnum.ALIGNMENT_WITHOUT && character.AlignmentSide != AlignmentSide)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (AlignmentSide != AlignmentSideEnum.ALIGNMENT_WITHOUT && !character.PvPEnabled)
                return FighterRefusedReasonEnum.INSUFFICIENT_RIGHTS;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public bool AddFighter(FightActor actor)
        {
            if (actor == null)
                throw new ArgumentNullException("actor");

            if (IsFull())
                return false;

            lock (m_locker)
            {
                m_fighters.Add(actor);

                OnFighterAdded(actor);
                return true;
            }
        }

        public bool RemoveFighter(FightActor actor)
        {
            lock (m_locker)
            {
                if (!m_fighters.Remove(actor))
                    return false;

                OnFighterRemoved(actor);
                return true;
            }
        }

        public void RemoveAllFighters()
        {
            lock (m_locker)
            {
                foreach (var fighter in m_fighters)
                    OnFighterRemoved(fighter);

                m_fighters.Clear();
            }
        }

        public void AddLeaver(FightActor leaver)
        {
            m_leavers.Add(leaver);
        }

        public bool RemoveLeaver(FightActor leaver)
        {
            return m_leavers.Remove(leaver);
        }

        public FightActor GetOneFighter(int id)
        {
            return m_fighters.FirstOrDefault(entry => entry.Id == id);
        }

        public FightActor GetOneFighter(Cell cell)
        {
            return m_fighters.FirstOrDefault(entry => Equals(entry.Position.Cell, cell));
        }

        public FightActor GetOneFighter(Predicate<FightActor> predicate)
        {
            return m_fighters.FirstOrDefault(entry => predicate(entry));
        }

        public T GetOneFighter<T>(int id) where T : FightActor
        {
            return m_fighters.OfType<T>().FirstOrDefault(entry => entry.Id == id);
        }

        public T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().FirstOrDefault(entry => predicate(entry));
        }

        public IEnumerable<FightActor> GetAllFighters()
        {
            return m_fighters;
        }

        public IEnumerable<FightActor> GetAllFightersWithLeavers()
        {
            return m_fighters.Concat(m_leavers);
        }

        public IEnumerable<T> GetAllFightersWithLeavers<T>() where T : FightActor
        {
            return m_fighters.Concat(m_leavers).OfType<T>();
        }

        public IEnumerable<FightActor> GetAllFighters(Cell[] cells)
        {
            return GetAllFighters<FightActor>(entry => cells.Contains(entry.Position.Cell));
        }

        public IEnumerable<FightActor> GetAllFighters(Predicate<FightActor> predicate)
        {
            return GetAllFighters().Where(entry => predicate(entry));
        }

        public IEnumerable<FightActor> GetAllFightersWithLeavers(Predicate<FightActor> predicate)
        {
            return GetAllFightersWithLeavers().Where(entry => predicate(entry));
        }

        public IEnumerable<T> GetAllFighters<T>() where T : FightActor
        {
            return m_fighters.OfType<T>();
        }

        public IEnumerable<T> GetAllFighters<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => predicate(entry));
        }

        public FightActor GetLastDeadFighter(bool onlyCharacter = false)
        {
            if (onlyCharacter)
                return m_deadFighters.LastOrDefault(x => x.IsDead() && x is CharacterFighter);
            else
                return m_deadFighters.LastOrDefault(x => x.IsDead());
        }

        public Party GetTeamParty()
        {
            var party = Fighters.OfType<CharacterFighter>().FirstOrDefault()?.Character.Party;
            return party != null && Fighters.OfType<CharacterFighter>().Count(x => x.Character.Party == party) == party.MembersCount ? party : null;
        }

        public FightOutcomeEnum GetOutcome()
        {
            var teamDead = AreAllDead();
            var opposedTeamDead = OpposedTeam.AreAllDead();

            if (!teamDead && opposedTeamDead)
                return FightOutcomeEnum.RESULT_VICTORY;

            if (teamDead && !opposedTeamDead)
                return FightOutcomeEnum.RESULT_LOST;

            return FightOutcomeEnum.RESULT_DRAW;
        }


        public FightTeamInformations GetFightTeamInformations()
        {
            return new FightTeamInformations((sbyte)Id,
                                             Leader?.Id ?? 0,
                                             (sbyte) AlignmentSide,
                                             (sbyte) TeamType,
                                             0,
                                             m_fighters.Select(entry => entry.GetFightTeamMemberInformations()));
        }

        public FightOptionsInformations GetFightOptionsInformations()
            => new FightOptionsInformations(
                Fight.SpectatorClosed,
                IsRestrictedToParty,
                IsClosed,
                IsAskingForHelp);

        public FightTeamLightInformations GetFightTeamLightInformations(Character character)
            => new FightTeamLightInformations((sbyte)Id, Leader?.Id ?? 0, (sbyte)AlignmentSide,
                                                  (sbyte)TeamType, 0, 
                                                  m_fighters.OfType<CharacterFighter>().Any(x => character.FriendsBook.IsFriend(x.Character.Account.Id)),
                                                  character.Guild != null && m_fighters.OfType<CharacterFighter>().Any(x => character.Guild == x.Character.Guild),
                                                  false,
                                                  character.Party != null && m_fighters.OfType<CharacterFighter>().Any(x => character.Parties.Any(y => x.Character.Parties.Contains(y))),
                                                  false,
                                                  (sbyte)m_fighters.Count(x => !x.IsSummoned()),
                                                  (int)m_fighters.Where(x => !x.IsSummoned()).Average(x => x.Level));

    }
}