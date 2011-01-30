using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class InteractiveAction : ActionBase
    {
        public abstract void Execute(InteractiveObject interactiveObject, Character executer);
    }
}