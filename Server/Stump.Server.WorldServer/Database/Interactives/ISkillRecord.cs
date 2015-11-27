using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Database.Interactives
{
    public interface ISkillRecord
    {
        int Id
        {
            get;
            set;
        }

        Skill GenerateSkill(int id, InteractiveObject interactiveObject);
    }
}