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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable] public static int PvTAttackersPlacementPhaseTime = 30000;
        [Variable] public static int PvTDefendersPlacementPhaseTime = 10000;
        private bool m_isAttackersPlacementPhase;

        private readonly List<Character> m_defendersQueue = new List<Character>();
        private readonly Dictionary<FightActor, Map> m_defendersMaps = new Dictionary<FightActor, Map>();

        public FightPvT(int id, Map fightMap, FightTaxCollectorDefenderTeam defendersTeam,  FightTaxCollectorAttackersTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public TaxCollectorFighter TaxCollector
        {
            get;
            private set;
        }

        public ReadOnlyCollection<Character> DefendersQueue
        {
            get
            {
                return m_defendersQueue.AsReadOnly();
            }
        }

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

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PvT; }
        }

        public override bool IsPvP
        {
            get { return true; }
        }
        public override bool IsMultiAccountRestricted
        {
            get { return true; }
        }

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

        protected override bool CanKickFighter(FightActor kicker, FightActor kicked)
        {
            return false;
        }

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

            if (m_defendersQueue.Count >= 7)
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

        public int GetDefendersLeftSlot()
        {
            return 7 - m_defendersQueue.Count > 0 ? 7 - m_defendersQueue.Count : 0;
        }

        public override bool CanChangePosition(FightActor fighter, Cell cell)
        {
            return base.CanChangePosition(fighter, cell) && 
                ((IsAttackersPlacementPhase && fighter.Team == ChallengersTeam) || (IsDefendersPlacementPhase && fighter.Team == DefendersTeam));
        }

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
                    TaxCollector = actor as TaxCollectorFighter;
                    TaxCollector.Dead += OnTaxCollectorDeath;
                }
            }

            if (State == FightState.Placement)
            {
                if (team == ChallengersTeam)
                {
                    TaxCollectorHandler.SendGuildFightPlayersEnemiesListMessage(
                        TaxCollector.TaxCollectorNpc.Guild.Clients, TaxCollector.TaxCollectorNpc,
                        ChallengersTeam.Fighters.OfType<CharacterFighter>().Select(x => x.Character));
                }
            }

            base.OnFighterAdded(team, actor);
        }

        private void OnTaxCollectorDeath(FightActor fighter, FightActor killedBy)
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
            if (State == FightState.Placement)
            {
                if (team == ChallengersTeam && actor is CharacterFighter)
                {
                    TaxCollectorHandler.SendGuildFightPlayersEnemyRemoveMessage(
                        TaxCollector.TaxCollectorNpc.Guild.Clients, TaxCollector.TaxCollectorNpc, ((CharacterFighter) actor).Character);
                }
            }

            if (actor is TaxCollectorFighter && actor.IsAlive())
                (actor as TaxCollectorFighter).TaxCollectorNpc.RejoinMap();

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

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            var results = new List<IFightResult>();

            var looters = ChallengersTeam.GetAllFightersWithLeavers<CharacterFighter>().Select(entry => entry.GetFightResult()).OrderByDescending(entry => entry.Prospecting);

            results.AddRange(looters);
            results.AddRange(DefendersTeam.GetAllFightersWithLeavers().Where(entry => !(entry is SummonedFighter) && !(entry is SummonedBomb) && !(entry is SlaveFighter)).Select(entry => entry.GetFightResult()));

            if (Winners != ChallengersTeam)
                return results;

            var teamPP = ChallengersTeam.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
            var kamas = TaxCollector.Kamas;

            foreach (var looter in looters)
            {
                looter.Loot.Kamas = kamas > 0 ? FightFormulas.AdjustDroppedKamas(looter, teamPP, kamas) : 0;
            }

            var i = 0;

            // dispatch loots
            foreach (var looter in looters)
            {
                var count = (int) Math.Ceiling(TaxCollector.Items.Count*((double) looter.Prospecting/teamPP));
                for (; i < count && i < TaxCollector.Items.Count; i++)
                {
                    looter.Loot.AddItem(TaxCollector.Items[i]);
                }
            }

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            var timer = (int) GetPlacementTimeLeft(fighter).TotalMilliseconds;
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), 
                (fighter.Team == ChallengersTeam && IsAttackersPlacementPhase) || (fighter.Team == DefendersTeam && IsDefendersPlacementPhase), false,
                IsStarted, timer, FightType);
        }

        protected override void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, IsStarted, 0, FightType);
        }

        protected override bool CanCancelFight()
        {
            return false;
        }

        public TimeSpan GetAttackersPlacementTimeLeft()
        {
            if (IsAttackersPlacementPhase)
                return (m_placementTimer.NextTick - DateTime.Now);
           
            return TimeSpan.Zero;
        }

        public TimeSpan GetDefendersWaitTimeForPlacement()
        {
            return TimeSpan.FromMilliseconds(PvTAttackersPlacementPhaseTime);
        }

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