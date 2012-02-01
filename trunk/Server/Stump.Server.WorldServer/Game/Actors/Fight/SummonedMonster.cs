using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class SummonedMonster : SummonedFighter
    {
        private readonly StatsFields m_stats;

        public SummonedMonster(int id, FightTeam team, FightActor summoner, MonsterGrade template, Cell cell)
            : base(id, team, template.Spells.Select(entry => new Spell(entry)).ToArray(), summoner, cell)
        {
            Monster = template;
            Look = Monster.Template.EntityLook;
            m_stats = new StatsFields(this, template);
        }

        public MonsterGrade Monster
        {
            get;
            private set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { return (byte) Monster.Level; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }


        public override bool CanCastSpell(Spell spell, Cell cell)
        {
            if (!base.CanCastSpell(spell, cell))
                return false;
            
            if (Monster.Spells.Any(entry => entry == null))
            {
                logger.Debug("Why the hell is a spell null ???");
            }

            if (Monster.Spells.Count(entry => entry.Id == spell.Id) <= 0)
                return false;

            return true;
        }

        public override string GetMapRunningFighterName()
        {
            return Monster.Id.ToString();
        }

        public override string Name
        {
            get { return Monster.Template.Name; }
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(client),
                Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client),
                (short)Monster.Template.Id,
                (sbyte)Monster.GradeId);
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.Base,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                0, // shieldsPoints = ?
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                Summoner.Id,
                true,
                (short)Stats[PlayerFields.NeutralResistPercent].Total,
                (short)Stats[PlayerFields.EarthResistPercent].Total,
                (short)Stats[PlayerFields.WaterResistPercent].Total,
                (short)Stats[PlayerFields.AirResistPercent].Total,
                (short)Stats[PlayerFields.FireResistPercent].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                (sbyte)( client == null ? VisibleState : GetVisibleStateFor(client.ActiveCharacter) ) // invisibility state
                );
        }
    }
}