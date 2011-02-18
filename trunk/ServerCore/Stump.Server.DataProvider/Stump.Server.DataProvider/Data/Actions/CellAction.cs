using Stump.Server.DataProvider.Data.Actions;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class CellAction : ActionBase
    {
        public abstract void Execute(CellData cell, Character executer);
    }
}