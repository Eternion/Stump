using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    [Flags]
    public enum BuffTriggerType
    {
        BUFF_ADDED = 0x0000001,
        TURN_BEGIN = 0x00000002,
        TURN_END = 0x00000004,
        MOVE = 0x0000008,
        BEFORE_ATTACKED = 0x00000010,
        AFTER_ATTACKED = 0x00000020,
        BEFORE_ATTACK = 0x00000040,
        AFTER_ATTACK = 0x00000080,
        BUFF_ENDED = 0x00000100,
        BUFF_ENDED_TURNEND = 0x00000200,
        BEFORE_HEALED = 0x00000400,
        AFTER_HEALED = 0x00000800,
        BEFORE_HEAL = 0x00001000,
        AFTER_HEAL = 0x00002000,
        DAMAGES_PUSHBACK = 0x00004000,
        LOST_MP = 0x00008000,
        LOST_AP = 0x00010000,
        TACKLED = 0x00020000,
        UNKNOWN = 0x7FFFFFFF,
    }
}