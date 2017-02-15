using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.TaxCollector;

namespace Stump.Server.WorldServer.Game.Fights
{
    /// <summary>
    /// Players versus Tax Collector
    /// </summary>
    public class FightPvT : Fight<FightTaxCollectorDefenderTeam, FightTaxCollectorAttackersTeam>
    {
        [Variable]
        public static int PvTAttackersPlacementPhaseTime = 30000;

        [Variable]
        public static int PvTDefendersPlacementPhaseTime = 10000;

        [Variable]
        public static int PvTMaxFightersSlots = 5;

        bool m_isAttackersPlacementPhase;

        readonly List<Character> m_defendersQueue = new List<Character>();
        readonly Dictionary<FightActor, Map> m_defendersMaps = new Dictionary<FightActor, Map>();

        public FightPvT(int id, Map fightMap, FightTaxCollectorDefenderTeam defendersTeam,  FightTaxCollectorAttackersTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public TaxCollectorFighter TaxCollector
        {
            get;
            private set;
        }

        public ReadOnlyCollection<Character> DefendersQueue => m_defendersQueue.AsReadOnly();

        public bool IsAttackersPlacementPhase
        {
            get { return m_isAttackersPlacementPhase && (State == FightState.Placement ||State == FightState.NotStarted); }
            private set { m_isAttackersPlacementPhase = value; }
        }

        public bool IsDefendersPlacementPhase
        {
            get { return !m_isAttackersPlacementPhase && (State == FightState.Placement ||State == FightState.NotStarted); }            
            private set { m_isAttackersPlacementPhase = !value; }
        }

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_PvT;

        public override bool IsPvP => true;

        public override bool IsMultiAccountRestricted => true;

        public override void StartPlacement()
        {
            base.StartPlacement();
            
            m_isAttackersPlacementPhase = true;
            m_placementTimer = Map.Area.CallDelayed(PvTAttackersPlacementPhaseTime, StartDefendersPlacement);

            // warn guild
            TaxCollectorHandler.SendTaxCollectorAttackedMessage(TaxCollector.TaxCollectorNpc.Guild.Clients,
                TaxCollector.TaxCollectorNpc);
        }

        public void StartDefendersPlacement()
        {
             if (State != FightState.Placement)
                return;

            m_placementTimer.Dispose();
            m_placementTimer = null;
            m_isAttackersPlacementPhase = false;

            if (DefendersQueue.Count == 0)
                StartFighting();

            foreach (var defender in DefendersQueue)
            {
                var defender1 = defender;
                defender.Area.ExecuteInContext(() =>
                {
                    var lastMap = defender.Map;
                    defender1.Teleport(Map, Map.Cells[defender1.Cell.Id]);
                    defender1.ResetDefender();
                    Map.Area.ExecuteInContext(() =>
                    {
                        var fighter = defender.CreateFighter(DefendersTeam);

                        m_defendersMaps.Add(fighter, lastMap);
                        DefendersTeam.AddFighter(fighter);

                        // if all defenders have been teleported we can launch the timer
                        if (DefendersQueue.All(
                                x => DefendersTeam.Fighters.OfType<CharacterFighter>().Any(y => y.Character == x)))
                            m_placementTimer = Map.Area.CallDelayed(PvTDefendersPlacementPhaseTime, StartFighting);
                    });
                });
                
            }
        }

        protected override bool CanKickFighter(FightActor kicker, FightActor kicked) => false;

        public override void StartFighting()
        {
            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            base.StartFighting();
        }

        public FighterRefusedReasonEnum AddDefender(Character character)
        {
            if (character.TaxCollectorDefendFight != null || character.IsBusy() || character.IsInFight() || character.IsInJail())
                return FighterRefusedReasonEnum.IM_OCCUPIED;

            if (!IsAttackersPlacementPhase)
                return FighterRefusedReasonEnum.TOO_LATE;

            if (character.Guild == null || character.Guild.Id != TaxCollector.TaxCollectorNpc.Guild.Id)
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (m_defendersQueue.Count >= PvTMaxFightersSlots)
                return FighterRefusedReasonEnum.TEAM_FULL;

            if (m_defendersQueue.Any(x => x.Client.IP == character.Client.IP))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (m_defendersQueue.Contains(character))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            m_defendersQueue.Add(character);
            character.SetDefender(this);  

            TaxCollectorHandler.SendGuildFightPlayersHelpersJoinMessage(character.Guild.Clients, TaxCollector.TaxCollectorNpc, character);

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public bool RemoveDefender(Character character)
        {
            if (!m_defendersQueue.Remove(character))
                return false;

            character.ResetDefender();
            TaxCollectorHandler.SendGuildFightPlayersHelpersLeaveMessage(character.Guild.Clients, TaxCollector.TaxCollectorNpc, character);

            return true;
        }

        public int GetDefendersLeftSlot() => PvTMaxFightersSlots - m_defendersQueue.Count > 0 ? PvTMaxFightersSlots - m_defendersQueue.Count : 0;

        public override bool CanChangePosition(FightActor fighter, Cell cell) => base.CanChangePosition(fighter, cell) &&
                ((IsAttackersPlacementPhase && fighter.Team == ChallengersTeam) || (IsDefendersPlacementPhase && fighter.Team == DefendersTeam));

        protected override void OnFighterAdded(FightTeam team, FightActor actor)
        {
            if (actor is TaxCollectorFighter)
            {
                if (TaxCollector != null)
                {
                    logger.Error("There is already a tax collector in this fight !");
                }
                else
                {
                    TaxCollector = (TaxCollectorFighter) actor;
                    TaxCollector.Dead += OnTaxCollectorDeath;
                }
            }

            if (State == FightState.Placement && team == ChallengersTeam)
            {
                TaxCollectorHandler.SendGuildFightPlayersEnemiesListMessage(
                    TaxCollector.TaxCollectorNpc.Guild.Clients, TaxCollector.TaxCollectorNpc,
                    ChallengersTeam.Fighters.OfType<CharacterFighter>().Select(x => x.Character));
            }


            base.OnFighterAdded(team, actor);
        }

        void OnTaxCollectorDeath(FightActor fighter, FightActor killedBy)
        {
            if (fighter != TaxCollector)
                return;

            EndFight();
        }

        protected override void DeterminsWinners()
        {
            Winners = TaxCollector.IsDead() ? (FightTeam)ChallengersTeam : DefendersTeam;
            Losers = TaxCollector.IsDead() ? (FightTeam)DefendersTeam : ChallengersTeam;
            Draw = false;

            OnWinnersDetermined(Winners, Losers, Draw);
        }

        protected override void OnFighterRemoved(FightTeam team, FightActor actor)
        {
            if (State == FightState.Placement && team == ChallengersTeam && actor is CharacterFighter characterFighter)
            {
                TaxCollectorHandler.SendGuildFightPlayersEnemyRemoveMessage(
                    TaxCollector.TaxCollectorNpc.Guild.Clients, TaxCollector.TaxCollectorNpc, characterFighter.Character);
            }

            if (actor is TaxCollectorFighter taxCollector && taxCollector.IsAlive())
                taxCollector.TaxCollectorNpc.RejoinMap();

            base.OnFighterRemoved(team, actor);
        }

        protected override void OnWinnersDetermined(FightTeam winners, FightTeam losers, bool draw)
        {
            TaxCollectorHandler.SendTaxCollectorAttackedResultMessage(TaxCollector.TaxCollectorNpc.Guild.Clients,
                Winners != DefendersTeam && !draw, TaxCollector.TaxCollectorNpc);

            if (Winners == DefendersTeam || draw)
                TaxCollector.TaxCollectorNpc.RejoinMap();
            else
                TaxCollector.TaxCollectorNpc.Delete();

            foreach (var defender in DefendersTeam.Fighters.Where(defender => m_defendersMaps.ContainsKey(defender)).OfType<CharacterFighter>())
            {
                defender.Character.NextMap = m_defendersMaps[defender];
            }

            foreach (var defender in DefendersQueue)
            {
                defender.ResetDefender();
            }

            base.OnWinnersDetermined(winners, losers, draw);
        }

        protected override List<IFightResult> GetResults()
        {
            var results = new List<IFightResult>();

            var looters = ChallengersTeam.GetAllFightersWithLeavers().Where(entry => entry.HasResult).
                Select(entry => entry.GetFightResult()).OrderByDescending(entry => entry.Prospecting);

            results.AddRange(looters);
            results.AddRange(DefendersTeam.GetAllFightersWithLeavers().Where(entry => entry.HasResult).Select(entry => entry.GetFightResult()));

            if (Winners != ChallengersTeam)
                return results;

            var teamPP = ChallengersTeam.GetAllFighters().Where(entry => entry.HasResult).Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
            var kamas = TaxCollector.Kamas;

            foreach (var looter in looters)
            {
                looter.Loot.Kamas = kamas > 0 ? FightFormulas.AdjustDroppedKamas(looter, teamPP, kamas, false) : 0;
            }

            var i = 0;

            // dispatch loots
            foreach (var looter in looters)
            {
                var count = i + (int) Math.Ceiling(TaxCollector.Items.Count*((double) looter.Prospecting/teamPP));
                for (; i < count && i < TaxCollector.Items.Count; i++)
                {
                    looter.Loot.AddItem(TaxCollector.Items[i]);
                }
            }

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            var timer = IsStarted ? 0 : (int) GetPlacementTimeLeft(fighter).TotalMilliseconds / 100;
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), 
                (fighter.Team == ChallengersTeam && IsAttackersPlacementPhase) || (fighter.Team == DefendersTeam && IsDefendersPlacementPhase),
                IsStarted, timer, FightType);
        }

        protected override void SendGameFightSpectatorJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightSpectatorJoinMessage(spectator.Character.Client, this);
        }

        protected override bool CanCancelFight() => false;

        public TimeSpan GetAttackersPlacementTimeLeft()
        {
            if (IsAttackersPlacementPhase)
                return (m_placementTimer.NextTick - DateTime.Now);
           
            return TimeSpan.Zero;
        }

        public static TimeSpan GetDefendersWaitTimeForPlacement() => TimeSpan.FromMilliseconds(PvTAttackersPlacementPhaseTime);

        public TimeSpan GetPlacementTimeLeft(FightActor fighter)
        {
            if (State == FightState.NotStarted && fighter.Team == ChallengersTeam)
                return TimeSpan.FromMilliseconds(PvTAttackersPlacementPhaseTime);

            if (fighter.Team == DefendersTeam && m_placementTimer == null)
                return TimeSpan.FromMilliseconds(PvTDefendersPlacementPhaseTime);

            if ((fighter.Team == ChallengersTeam && IsAttackersPlacementPhase) ||
                (fighter.Team == DefendersTeam && IsDefendersPlacementPhase))
                return m_placementTimer.NextTick - DateTime.Now;

            return TimeSpan.Zero;
        }
    }
}