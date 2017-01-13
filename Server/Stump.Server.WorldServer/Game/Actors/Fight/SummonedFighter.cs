using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class SummonedFighter : AIFighter
    {
        protected SummonedFighter(int id, FightTeam team, IEnumerable<Spell> spells, FightActor summoner, Cell cell)
            : base(team, spells)
        {
            Id = id;
            Position = summoner.Position.Clone();
            Cell = cell;
            Summoner = summoner;

            FightStartPosition = Position.Clone();
            MovementHistory.RegisterEntry(FightStartPosition.Cell);
        }

        protected SummonedFighter(int id, FightTeam team, IEnumerable<Spell> spells, FightActor summoner, Cell cell, int identifier)
            : base(team, spells, identifier)
        {
            Id = id;
            Position = summoner.Position.Clone();
            Cell = cell;
            Summoner = summoner;

            FightStartPosition = Position.Clone();
            MovementHistory.RegisterEntry(FightStartPosition.Cell);
        }

        public override sealed int Id
        {
            get;
            protected set;
        }

        private CharacterFighter m_controller;
        public CharacterFighter Controller
        {
            get { return m_controller; }
            protected set
            {
                m_controller = value;

                if (value == null)
                    Fight.TurnStopped -= OnTurnStopped;
                else
                    Fight.TurnStopped += OnTurnStopped;
            }
        }

        public void SetController(CharacterFighter controller)
        {
            Controller = controller;
        }

        public bool IsControlled() => Controller != null;

        protected override void OnTurnStarted(IFight fight, FightActor currentfighter)
        {
            if (IsControlled())
                return;

            base.OnTurnStarted(fight, currentfighter);
        }

        public override bool HasResult => false;

        public override int GetTackledAP(int mp, Cell cell) => 0;

        public override int GetTackledMP(int mp, Cell cell) => 0;

        public CharacterCharacteristicsInformations GetSlaveCharacteristicsInformations()
        {
            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return new CharacterCharacteristicsInformations();

            var slaveStats = characterFighter.Character.GetCharacterCharacteristicsInformations();

            slaveStats.actionPoints = Stats.AP;
            slaveStats.actionPointsCurrent = (short)Stats.AP.Total;
            slaveStats.movementPoints = Stats.MP;
            slaveStats.movementPointsCurrent = (short)Stats.MP.Total;
            slaveStats.lifePoints = Stats.Health.Total;
            slaveStats.maxLifePoints = Stats.Health.TotalMax;

            slaveStats.tackleEvade = Stats[PlayerFields.TackleEvade];
            slaveStats.intelligence = Stats[PlayerFields.Intelligence];
            slaveStats.strength = Stats[PlayerFields.Strength];
            slaveStats.chance = Stats[PlayerFields.Chance];
            slaveStats.wisdom = Stats[PlayerFields.Wisdom];
            slaveStats.agility = Stats[PlayerFields.Agility];

            return slaveStats;
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Controller = null;
            base.OnDead(killedBy, passTurn);
            Summoner.RemoveSummon(this);
        }

        void OnTurnStopped(IFight fight, FightActor player)
        {
            if (fight.TimeLine.Next != this)
                return;

            var characterFighter = Summoner as CharacterFighter;
            if (characterFighter == null)
                return;

            ContextHandler.SendSlaveSwitchContextMessage(characterFighter.Character.Client, this);
        }
    }
}