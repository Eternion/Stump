using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Fights.Results;
using Stump.Server.WorldServer.Worlds.Fights.Results.Data;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        private short m_damageTakenBeforeFight;
        private int m_earnedExp;

        public CharacterFighter(Character character, FightTeam team)
            : base(team)
        {
            Character = character;
            Look = Character.Look.Copy();

            Cell cell;
            Fight.FindRandomFreeCell(this, out cell, false);
            Position = new ObjectPosition(character.Map, cell, character.Direction);

            FighterLeft += OnFightLeft;

            InitializeCharacterFighter();
        }

        private void InitializeCharacterFighter()
        {
            m_damageTakenBeforeFight = Stats.Health.Context;

            if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                Stats.Health.Context = 0;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return Character.Id; }
        }

        public override string Name
        {
            get { return Character.Name; }
        }

        public override EntityLook Look
        {
            get;
            protected set;
        }

        public override ObjectPosition MapPosition
        {
            get
            {
                return Character.Position; 
            }
        }

        public override byte Level
        {
            get
            {
                return Character.Level;
            }
        }

        public override StatsFields Stats
        {
            get { return Character.Stats; }
        }

        public override ObjectPosition GetLeaderBladePosition()
        {
            return Character.GetPositionBeforeMove();
        }

        public override bool CanCastSpell(Spell spell, Cell cell)
        {
            if (!IsFighterTurn())
                return false;

            if (!Character.Spells.HasSpell(spell.Id))
                return false;

            var spellLevel = spell.CurrentSpellLevel;
            var point = new MapPoint(cell);

            if (point.DistanceTo(Position.Point) > spellLevel.Range ||
                point.DistanceTo(Position.Point) < spellLevel.MinRange)
                return false;

            if (AP < spellLevel.ApCost)
                return false;

            // todo : check casts per turn
            // todo : check cooldown
            // todo : check states

            return true;
        }

        private void OnFightLeft(FightActor fightActor)
        {
            foreach (var field in Stats.Fields.Values)
            {
                if (field.Name == CaracteristicsEnum.Health)
                {
                    if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                        field.Context = m_damageTakenBeforeFight;
                    else if (field.Total <= 0)
                        field.Context = (short) ((field as StatsHealth).TotalMax - 1);
                }

                else
                {
                    field.Context = 0;
                }
            }
        }

        public void SetEarnedExperience(int experience)
        {
            m_earnedExp = experience;
        }

        public override IFightResult GetFightResult()
        {
            if (m_earnedExp > 0)
            {
                var expData = new FightExperienceData(Character)
                                  {
                                      ExperienceFightDelta = m_earnedExp,
                                      ShowExperience = true,
                                      ShowExperienceFightDelta = true,
                                      ShowExperienceLevelFloor = true,
                                      ShowExperienceNextLevelFloor = true
                                  };

                return new FightPlayerResult(this, GetFighterOutcome(), Loot, expData);
            }

            return new FightPlayerResult(this, GetFighterOutcome(), Loot);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(Id, Name, Character.Level);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations()
        {
            return new GameFightCharacterInformations(Id,
                Look, 
                GetEntityDispositionInformations(), 
                Team.Id, 
                IsAlive(), 
                GetGameFightMinimalStats(),
                Name, 
                Character.Level, 
                Character.GetActorAlignmentInformations());
        }
    }
}