using System;

namespace Stump.DofusProtocol.Enums
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF = 0x1,
        ALLY_1 = 0x2,
        ALLY_2 = 0x4,
        ALLY_SUMMONS = 0x8,
        ALLY_STATIC_SUMMONS = 0x10,
        ALLY_BOMBS = 0x20, // not sure about that
        ALLY_SUMMONER = 0x40,
        ALLY_5 = 0x80,
        ALLY_ALL = 0x2 | 0x4 | 0x8 | 0x10 | 0x20 | 0x40 | 0x80,
        ENEMY_1 = 0x100,
        ENEMY_2 = 0x200,
        ENEMY_SUMMONS = 0x400,
        ENEMY_STATIC_SUMMONS = 0x800,
        ENEMY_BOMBS = 0x1000,
        ENEMY_SUMMONER = 0x2000,
        ENEMY_5 = 0x4000,
        ENEMY_ALL = 0x100 | 0x200 | 0x400 | 0x800 | 0x1000 | 0x2000 | 0x4000,
        ALL = 0x7FFF,
        ALL_SUMMONS = 0x8 | 0x10 | 0x400 | 0x800,
        ONLY_SELF = 0x8000,
    }
}