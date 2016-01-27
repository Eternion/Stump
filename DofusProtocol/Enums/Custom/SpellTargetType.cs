using System;

namespace Stump.DofusProtocol.Enums
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF_ONLY = 0x80000, // C
        SELF = 0x1, // c

        ALLY_MONSTER_SUMMON = 0x2, // s
        ALLY_SUMMON = 0x4, // j
        ALLY_NON_MONSTER_SUMMON = 0x8, // i
        ALLY_COMPANION = 0x10, // d
        ALLY_MONSTER = 0x20, // m
        ALLY_SUMMONER = 0x40, // h
        ALLY_PLAYER = 0x80, // l
        ALLY_BOMB = 0x100, // P

        ALLY_ALL_EXCEPT_SELF = ALLY_MONSTER_SUMMON | ALLY_SUMMON | ALLY_NON_MONSTER_SUMMON |
            ALLY_COMPANION | ALLY_COMPANION | ALLY_MONSTER | ALLY_PLAYER | ALLY_BOMB, // g

        ALLY_ALL = SELF | ALLY_MONSTER_SUMMON | ALLY_SUMMON | ALLY_NON_MONSTER_SUMMON |
            ALLY_COMPANION | ALLY_COMPANION | ALLY_MONSTER | ALLY_PLAYER | ALLY_BOMB, // a

        ENEMY_MONSTER_SUMMON = 0x200, // S
        ENEMY_SUMMON = 0x400, // J
        ENEMY_NON_MONSTER_SUMMON = 0x800, // I
        ENEMY_COMPANION = 0x1000, // D
        ENEMY_MONSTER = 0x2000, // M
        ENEMY_UNKN_1 = 0x8000, // H
        ENEMY_PLAYER = 0x10000, // L
        ENEMY_BOMB = 0x20000, // p

        ENEMY_ALL = ENEMY_MONSTER_SUMMON | ENEMY_SUMMON | ENEMY_NON_MONSTER_SUMMON |
            ENEMY_COMPANION | ENEMY_COMPANION | ENEMY_MONSTER | ENEMY_UNKN_1 | ENEMY_PLAYER | ENEMY_BOMB // A
    }
}