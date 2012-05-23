using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightTeam
    {
        #region Events
        public event Action<FightTeam, FightActor> FighterAdded;

        private void OnFightAdded(FightActor fighter)
        {
        }

        private void NotifyFighterAdded(FightActor fighter)
        {
            OnFightAdded(fighter);

            Action<FightTeam, FightActor> handler = FighterAdded;
            if (handler != null)
                handler(this, fighter);
        }

        public event Action<FightTeam, FightOptionsEnum> TeamOptionsChanged;

        private void NotifyTeamOptionsChanged(FightOptionsEnum option)
        {
            var handler = TeamOptionsChanged;
            if (handler != null)
                handler(this, option);
        }

        public event Action<FightTeam, FightActor> FighterRemoved;

        private void OnFightRemoved(FightActor fighter)
        {
        }

        private void NotifyFighterRemoved(FightActor fighter)
        {
            OnFightRemoved(fighter);

            Action<FightTeam, FightActor> handler = FighterRemoved;
            if (handler != null)
                handler(this, fighter);
        }

        #endregion

        private readonly List<FightActor> m_fighters = new List<FightActor>();
        private readonly List<FightActor> m_leavers = new List<FightActor>();
        private readonly object m_locker = new object();

        public FightTeam(sbyte id, Cell[] placementCells)
        {
            Id = id;
            PlacementCells = placementCells;
            AlignmentSide = AlignmentSideEnum.ALIGNMENT_WITHOUT;
        }

        public FightTeam(sbyte id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
        {
            Id = id;
            PlacementCells = placementCells;
            AlignmentSide = alignmentSide;
        }

        public sbyte Id
        {
            get;
            private set;
        }

        public ObjectPosition BladePosition
        {
            get;
            set;
        }

        public Cell[] PlacementCells
        {
            get;
            private set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
            private set;
        }

        public TeamTypeEnum TeamType
        {
            get
            {
                if (IsPlayerTeam())
                    return TeamTypeEnum.TEAM_TYPE_PLAYER;
                if (IsMonsterTeam())
                    return TeamTypeEnum.TEAM_TYPE_MONSTER;

                return TeamTypeEnum.TEAM_TYPE_MONSTER;
            }
        }

        public Fight Fight
        {
            get;
            internal set;
        }

        public FightActor Leader
        {
            get
            {
                return m_fighters.Count > 0 ? m_fighters.First() : null;
            }
        }

        public bool IsSecret
        {
            get;
            private set;
        }

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

        public bool IsMonsterTeam()
        {
            return Leader is MonsterFighter;
        }

        public bool IsPlayerTeam()
        {
            return Leader is CharacterFighter;
        }

        public bool ChangeLeader(FightActor leader)
        {
            if (!m_fighters.Contains(leader))
                return false;

            m_fighters.Remove(leader);
            m_fighters.Insert(0, leader);

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
                case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                    IsSecret = !IsSecret;
                    break;
                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    IsRestrictedToParty = !IsRestrictedToParty;
                    break;
            }

            NotifyTeamOptionsChanged(option);
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
                    return IsSecret;
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
            return m_fighters.Count <= 0 || m_fighters.All(entry => entry.IsDead() || entry.HasLeft());
        }

        public bool IsFull()
        {
            return Fight.State == FightState.Placement && m_fighters.Count > PlacementCells.Count();
        }

        public FighterRefusedReasonEnum CanJoin(Character character)
        {
            if (Leader is MonsterFighter)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            if (Fight.State != FightState.Placement)
                return FighterRefusedReasonEnum.TOO_LATE;

            if (IsFull())
                return FighterRefusedReasonEnum.TEAM_FULL;

            if (IsClosed)
                return FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;

            if (IsSecret)
                return FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;

            if (IsRestrictedToParty && Leader is CharacterFighter)
            {
                var leader = Leader as CharacterFighter;

                if (!leader.Character.IsInParty() || !leader.Character.Party.IsInGroup(character))
                    return FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;
            }

            if (AlignmentSide != AlignmentSideEnum.ALIGNMENT_WITHOUT &&
                character.AlignmentSide != AlignmentSide)
                return FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public bool AddFighter(FightActor actor)
        {
            if (IsFull())
                return false;

            lock (m_locker)
            {
                m_fighters.Add(actor);

                NotifyFighterAdded(actor);
                return true;
            }
        }

        public bool RemoveFighter(FightActor actor)
        {
            if (IsFull())
                return false;

            lock (m_locker)
            {
                if (!m_fighters.Remove(actor))
                    return false;

                NotifyFighterRemoved(actor);
                return true;
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
            return m_fighters.Where(entry => entry.Id == id).SingleOrDefault();
        }

        public FightActor GetOneFighter(Cell cell)
        {
            return m_fighters.Where(entry => Equals(entry.Position.Cell, cell)).SingleOrDefault();
        }

        public FightActor GetOneFighter(Predicate<FightActor> predicate)
        {
            return m_fighters.Where(entry => predicate(entry)).SingleOrDefault();
        }

        public T GetOneFighter<T>(int id) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => entry.Id == id).SingleOrDefault();
        }

        public T GetOneFighter<T>(Predicate<T> predicate) where T : FightActor
        {
            return m_fighters.OfType<T>().Where(entry => predicate(entry)).SingleOrDefault();
        }

        public IEnumerable<FightActor> GetAllFighters()
        {
            return m_fighters;
        }

        public IEnumerable<FightActor> GetAllFightersWithLeavers()
        {
            return m_fighters.Concat(m_leavers);
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

        public FightTeamInformations GetFightTeamInformations()
        {
            return new FightTeamInformations(Id,
                Leader != null ? Leader.Id : 0,
                (sbyte) AlignmentSide,
                (sbyte) TeamType,
                m_fighters.Select(entry => entry.GetFightTeamMemberInformations())
                );
        }

        public FightOptionsInformations GetFightOptionsInformations()
        {
            return new FightOptionsInformations(
                IsSecret,
                IsRestrictedToParty,
                IsClosed,
                IsAskingForHelp);
        }

        public FightTeamLightInformations GetFightTeamLightInformations()
        {
            return new FightTeamLightInformations(Id, Leader == null ? 0 : Leader.Id, (sbyte) AlignmentSide,
                (sbyte)TeamType, (sbyte) m_fighters.Count);
        }
    }
}