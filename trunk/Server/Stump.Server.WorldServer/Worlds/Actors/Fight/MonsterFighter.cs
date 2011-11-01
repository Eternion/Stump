using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public sealed class MonsterFighter : AIFighter
    {
        public MonsterFighter(FightTeam team, Monster monster)
            : base(team, monster.Spells)
        {
            Id = Fight.GetNextContextualId();
            Monster = monster;
            Look = monster.Look.Copy();

            Cell cell;
            Fight.FindRandomFreeCell(this, out cell, false);
            Position = new ObjectPosition(monster.Group.Map, cell, monster.Group.Direction);
        }

        public Monster Monster
        {
            get;
            private set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Monster.Group.Position; }
        }

        public override StatsFields Stats
        {
            get { return Monster.Stats; }
        }

        public override bool CanCastSpell(Spell spell, Cell cell)
        {
            if (!IsFighterTurn())
                return false;

            if (Monster.Spells.Count(entry => entry.Id == spell.Id) <= 0)
                return false;

            SpellLevelTemplate spellLevel = spell.CurrentSpellLevel;
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

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameFightMonsterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(),
                (short) Monster.Template.Id,
                (sbyte) Monster.Grade.GradeId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations()
        {
            return GetGameContextActorInformations() as GameFightFighterInformations;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte) Monster.Grade.GradeId);
        }
    }
}