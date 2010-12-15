using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global
{
    public partial class WorldSpace
    {
        public event Action<Entity> EntityAdded;
        public event Action<Entity> EntityRemoved;
    }
}
