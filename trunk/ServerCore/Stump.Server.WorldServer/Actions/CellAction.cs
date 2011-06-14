using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class CellAction : ActionBase
    {
        public abstract void Execute(CellLinked cell, Character executer);
    }
}