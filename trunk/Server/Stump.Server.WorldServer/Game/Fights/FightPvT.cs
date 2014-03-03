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
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.TaxCollector;

namespace Stump.Server.WorldServer.Game.Fights
{
    /// <summary>
    /// Players versus Tax Collector
    /// </summary>
    public class FightPvT : Fight
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable] public static int PvTAttackersPlacementPhaseTime = 30000;
        [Variable] public static int PvTDefendersPlacementPhaseTime = 10000;
        private bool m_isAttackersPlacementPhase;

        private readonly List<Character> m_defendersQueue = new List<Character>();
        private readonly Dictionary<Character, Map> m_defendersMaps = new Dictionary<Character, Map>(); 

        public FightPvT(int id, Map fightMap, FightTeam blueTeam, FightTeam redTeam)
            : base(id, fightMap, blueTeam, redTeam)
        {
        }

        public TaxCollectorFighter TaxCollector
        {
            get;
            private set;
        }

        public FightTeam AttackersTeam
        {
            get;
            private set;
        }

        public FightTeam DefendersTeam
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
            get { return m_isAttackersPlacementPhase && State == FightState.Placement; }
            private set { m_isAttackersPlacementPhase = value; }
        }

        public bool IsDefendersPlacementPhase
        {
            get { return !m_isAttackersPlacementPhase && State == FightState.Placement; }            
            private set { m_isAttackersPlacementPhase = !value; }
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PvT; }
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

            m_isAttackersPlacementPhase = false;

            foreach (var defender in DefendersQueue)
            {
                m_defendersMaps.Add(defender, defender.Map);

                var defender1 = defender;
                defender.Area.ExecuteInContext(() =>
                {
                    defender1.Teleport(Map, defender1.Cell);
                    Map.Area.ExecuteInContext(() =>
                    {
                        DefendersTeam.AddFighter(defender.CreateFighter(DefendersTeam));

                        // if all defenders have been teleported we can launch the timer
                        if (DefendersQueue.All(
                                x => DefendersTeam.Fighters.OfType<CharacterFighter>().Any(y => y.Character == x)))
                            m_placementTimer = Map.Area.CallDelayed(PvTDefendersPlacementPhaseTime, StartFighting);
                    });
                });
                
            }
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        public FighterRefusedReasonEnum AddDefender(Character character)
        {
            if (!IsAttackersPlacementPhase)
                return FighterRefusedReasonEnum.TOO_LATE;

            if (character.Guild == null || character.Guild != TaxCollector.TaxCollectorNpc.Guild)
                return FighterRefusedReasonEnum.WRONG_GUILD;

            if (m_defendersQueue.Count >= 7)
                return FighterRefusedReasonEnum.TEAM_FULL;

            if (m_defendersQueue.Any(x => x.Client.IP == character.Client.IP))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            if (m_defendersQueue.Contains(character))
                return FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;

            m_defendersQueue.Add(character);
              
            TaxCollectorHandler.SendGuildFightPlayersHelpersJoinMessage(character.Guild.Clients, this, character);

            return FighterRefusedReasonEnum.FIGHTER_ACCEPTED;
        }

        public bool RemoveDefender(Character character)
        {
            if (!m_defendersQueue.Remove(character))
                return false;

            TaxCollectorHandler.SendGuildFightPlayersHelpersLeaveMessage(character.Guild.Clients, this, character);

            return true;
        }

        public int GetDefendersLeftSlot()
        {
            return 7 - m_defendersQueue.Count > 0 ? 7 - m_defendersQueue.Count : 0;
        }

        public override bool CanChangePosition(FightActor fighter, Cell cell)
        {
            return base.CanChangePosition(fighter, cell) && 
                ((IsAttackersPlacementPhase && fighter.Team == AttackersTeam) || (IsDefendersPlacementPhase && fighter.Team == DefendersTeam));
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
                    DefendersTeam = TaxCollector.Team;
                    AttackersTeam = TaxCollector.OpposedTeam;
                }
            }

            if (State == FightState.Placement)
            {
                if (team == AttackersTeam)
                {
                    TaxCollectorHandler.SendGuildFightPlayersEnemiesListMessage(
                        TaxCollector.TaxCollectorNpc.Guild.Clients, this,
                        AttackersTeam.Fighters.OfType<CharacterFighter>().Select(x => x.Character));
                }
            }

            base.OnFighterAdded(team, actor);
        }

        protected override void OnFighterRemoved(FightTeam team, FightActor actor)
        {
            if (State == FightState.Placement)
            {
                if (team == AttackersTeam && actor is CharacterFighter)
                {
                    TaxCollectorHandler.SendGuildFightPlayersEnemyRemoveMessage(
                        TaxCollector.TaxCollectorNpc.Guild.Clients, this, (actor as CharacterFighter).Character);
                }
            }

            if (actor is TaxCollectorFighter && actor.IsAlive())
                (actor as TaxCollectorFighter).TaxCollectorNpc.RejoinMap();

            base.OnFighterRemoved(team, actor);
        }

        protected override void OnFightEnded()
        {
            TaxCollectorHandler.SendTaxCollectorAttackedResultMessage(TaxCollector.TaxCollectorNpc.Guild.Clients,
                Winners == DefendersTeam, TaxCollector.TaxCollectorNpc);

            if (Winners == DefendersTeam)
            {
                TaxCollector.TaxCollectorNpc.RejoinMap();

                foreach (var defender in m_defendersQueue)
                {
                    defender.NextMap = m_defendersMaps[defender];
                }
            }


            base.OnFightEnded();
        }

        protected override IEnumerable<IFightResult> GenerateResults()
        {
            // todo
            yield break;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), 
                (fighter.Team == AttackersTeam && IsAttackersPlacementPhase) || (fighter.Team == DefendersTeam && IsDefendersPlacementPhase), false,
                IsStarted, (int)GetPlacementTimeLeft(fighter).TotalMilliseconds, FightType);
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
            if ((fighter.Team == AttackersTeam && IsAttackersPlacementPhase) || (fighter.Team == DefendersTeam && IsDefendersPlacementPhase))
                return m_placementTimer.NextTick - DateTime.Now;

            return TimeSpan.Zero;
        }

    }
}