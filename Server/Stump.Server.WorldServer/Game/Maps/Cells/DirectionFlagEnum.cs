using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Maps.Cells
{
    [Flags]
    public enum DirectionFlagEnum
    {
        NONE = 0,
        DIRECTION_EAST = 0x1,
        DIRECTION_SOUTH_EAST = 0x2,
        DIRECTION_SOUTH = 0x4,
        DIRECTION_SOUTH_WEST = 0x8,
        DIRECTION_WEST = 0x10,
        DIRECTION_NORTH_WEST = 0x20,
        DIRECTION_NORTH = 0x40,
        DIRECTION_NORTH_EAST = 0x80,
        ALL_DIRECTIONS = 0xFF
    }

    public static class DirectionConversion
    {
        public static DirectionFlagEnum GetFlag(this DirectionsEnum direction)
        {
            switch (direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    return DirectionFlagEnum.DIRECTION_EAST;
                case DirectionsEnum.DIRECTION_WEST:
                    return DirectionFlagEnum.DIRECTION_WEST;
                case DirectionsEnum.DIRECTION_NORTH:
                    return DirectionFlagEnum.DIRECTION_NORTH;
                case DirectionsEnum.DIRECTION_SOUTH:
                    return DirectionFlagEnum.DIRECTION_SOUTH;
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    return DirectionFlagEnum.DIRECTION_NORTH_EAST;
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    return DirectionFlagEnum.DIRECTION_NORTH_WEST;
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    return DirectionFlagEnum.DIRECTION_SOUTH_EAST;
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    return DirectionFlagEnum.DIRECTION_SOUTH_WEST;
                default:
                    return DirectionFlagEnum.NONE;
            }
        }
    }
}