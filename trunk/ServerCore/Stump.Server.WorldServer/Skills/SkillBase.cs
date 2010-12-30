using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Skills
{
    public abstract class SkillBase
    {
        public abstract uint SkillId
        {
            get;
        }

        public uint Duration
        {
            get;
            set;
        }

        public abstract bool IsEnabled(Character executer);
        public abstract void Execute(InteractiveObject interactiveObject, Character executer);
    }
}