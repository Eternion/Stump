using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        public CharacterFighter(Character character, FightTeam team)
            : base(team)
        {
            Character = character;
            Look = Character.Look;
            Position = Character.Position;
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

        public override ObjectPosition Position
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

        public override StatsFields Stats
        {
            get { return Character.Stats; }
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