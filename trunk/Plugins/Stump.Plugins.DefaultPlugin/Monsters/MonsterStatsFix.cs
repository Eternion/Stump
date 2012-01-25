using NLog;
using Stump.Plugins.DefaultPlugin.World;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Plugins.DefaultPlugin.Monsters
{
    public class MonsterStatsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(MonsterManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply monster stats fix");

            foreach (var grade in MonsterManager.Instance.GetMonsterGrades())
            {
                bool extraHp = grade.LifePoints / (double)grade.Level > 10;

                grade.TackleEvade = (short) ((int) (grade.Level / 10d)  * (extraHp ? 2 : 1));
                grade.TackleBlock = grade.TackleEvade;
            }
        } 
    }
}