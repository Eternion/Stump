using System.Linq;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace ArkalysPlugin.Commands
{
    public class IsleRateModifiers
    {
        [Initialization(typeof(World))]
        public static void Initialize()
        {
            foreach (var isle in IsleCommand.ProfilsIsle)
            {
                if(isle.RateMultiplicator == 0d ||
                    isle.RateMultiplicator == 1d)
                    continue;

                var area = World.Instance.GetMap(isle.StartMap).Area;

                foreach (var spawn in area.Maps.SelectMany(entry => entry.MonsterSpawns).Distinct())
                {
                    var monster = MonsterManager.Instance.GetTemplate(spawn.MonsterId);

                    monster.MaxDroppedKamas = (int)(monster.MaxDroppedKamas * isle.RateMultiplicator);
                    monster.MinDroppedKamas = (int)(monster.MinDroppedKamas * isle.RateMultiplicator);

                    foreach (var grade in monster.Grades)
                    {
                        grade.GradeXp = (int)(grade.GradeXp * isle.RateMultiplicator);

                        grade.Strength = (short)( grade.Strength * ( 1 / isle.RateMultiplicator ) );
                        grade.Chance = (short)( grade.Chance * ( 1 / isle.RateMultiplicator ) );
                        grade.Agility = (short)( grade.Agility * ( 1 / isle.RateMultiplicator ) );
                        grade.Intelligence = (short)( grade.Intelligence * ( 1 / isle.RateMultiplicator ) );
                    }
                }
            }
        }
    }
}