
using Stump.Server.WorldServer.Database.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Actors.Interfaces
{
    interface ICreature
    {
        MonsterGrade MonsterGrade { get; }
    }
}
