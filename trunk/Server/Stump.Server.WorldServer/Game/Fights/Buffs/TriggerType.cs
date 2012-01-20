using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    [Flags]
    public enum TriggerType
    {
        BUFF_ADDED = 0x0000001,
        TURN_BEGIN = 0x00000002,
        TURN_END = 0x00000004,
        MOVE = 0x0000008,
        BEFORE_ATTACKED = 0x00000010,
        AFTER_ATTACKED = 0x00000020,
        BEFORE_ATTACK = 0x00000040,
        AFTER_ATTACK = 0x00000080,
        UNKNOWN = 0x7FFFFFFF,
    }
}