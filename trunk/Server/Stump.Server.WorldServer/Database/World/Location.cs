using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Database.World
{
    /// <summary>
    /// Use it with [Nested]
    /// </summary>
    public class Location
    {
        [Property("Map")]
        public int Map
        {
            get;
            set;
        }

        [Property("Cell")]
        public ushort Cell
        {
            get;
            set;
        }

        [Property("Direction")]
        public DirectionsEnum Direction
        {
            get;
            set;
        }
    }
}