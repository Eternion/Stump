using System;

namespace Stump.DofusProtocol.Enums.Extensions
{
    public static class DirectionsEnumExtensions
    {
        public static DirectionsEnum GetOpposedDirection(this DirectionsEnum direction)
        {
            return (DirectionsEnum)Math.Abs((int)direction - 4);
        }
    }
}