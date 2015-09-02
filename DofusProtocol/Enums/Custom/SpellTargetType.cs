using System;

namespace Stump.DofusProtocol.Enums
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF_ONLY = 0x80000, // C
        SELF = 0x1, // c

        ALLY_PLAYER = 0x2, // g
        ALLY_MONSTER_SUMMON = 0x4, // s
        ALLY_SUMMON = 0x8, // j
        ALLY_NON_MONSTER_SUMMON = 0x10, // i
        ALLY_COMPANION = 0x20, // d
        ALLY_MONSTER = 0x40, // m
        ALLY_UNKN_1 = 0x80, // h
        ALLY_UNKN_2 = 0x100, // l
        ALLY_BOMB = 0x200, // P

        ALLY_ALL = ALLY_PLAYER | ALLY_MONSTER_SUMMON | ALLY_SUMMON | ALLY_NON_MONSTER_SUMMON |
            ALLY_COMPANION | ALLY_COMPANION | ALLY_MONSTER | ALLY_UNKN_1 | ALLY_UNKN_2 | ALLY_BOMB, // a

        ENEMY_PLAYER = 0x400, // G
        ENEMY_MONSTER_SUMMON = 0x800, // S
        ENEMY_SUMMON = 0x1000, // J
        ENEMY_NON_MONSTER_SUMMON = 0x2000, // I
        ENEMY_COMPANION = 0x4000, // D
        ENEMY_MONSTER = 0x8000, // M
        ENEMY_UNKN_1 = 0x10000, // H
        ENEMY_UNKN_2 = 0x20000, // L
        ENEMY_BOMB = 0x40000, // p

        ENEMY_ALL = ENEMY_PLAYER | ENEMY_MONSTER_SUMMON | ENEMY_SUMMON | ENEMY_NON_MONSTER_SUMMON |
            ENEMY_COMPANION | ENEMY_COMPANION | ENEMY_MONSTER | ENEMY_UNKN_1 | ENEMY_UNKN_2 | ENEMY_BOMB // A
    }
}