using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    [Flags]
    public enum BuffTriggerType : long
    {
        Instant = 0, // I

        /* DAMAGE TRIGGER */
        OnDamaged = 1 << 0, // D
        OnDamagedAir = 1 << 1, // DA
        OnDamagedEarth =  1 << 2, // DE
        OnDamagedFire = 1 << 3, // DF
        OnDamagedWater = 1 << 4, // DW
        OnDamagedNeutral = 1 << 5, // DN
        OnDamagedByAlly = 1 << 6, // DBA
        OnDamagedByEnemy = 1 << 7, // DBE
        OnDamagedByWeapon = 1 << 8, // DC
        OnDamagedBySpell = 1 << 9, // DS
        OnDamagedByGlyph = 1 << 10, // DG
        OnDamagedByTrap = 1 << 11, //DP
        OnDamagedInCloseRange = 1 << 12, //DM
        OnDamagedInLongRange = 1 << 13, //DR
        OnDamagedByPush = 1 << 14, // MD
        OnDamagedUnknown_1 = 1 << 15, // DI
        OnDamagedUnknown_2 = 1 << 16, // Dr
        OnDamagedUnknown_3 = 1 << 17, // DTB
        OnDamagedUnknown_4 = 1 << 18, // DTE
        OnDamagedUnknown_5 = 1 << 19, // MDM
        OnDamagedUnknown_6 = 1 << 20, // MDP


        /* TURN */
        OnTurnBegin = 1 << 21, // TB
        OnTurnEnd = 1 << 22, // TE

        /* AP, MP */
        OnMPLost = 1 << 23, // m
        OnAPLost = 1 << 24, // A

        /* HEAL */
        OnHealed = 1 << 25, //H

        /* STATE */
        OnStateAdded = 1 << 26, // EO
        OnSpecificStateAdded = 1 << 27, //EO#
        OnStateRemoved = 1 << 28, //Eo
        OnSpecificStateRemoved = 1 << 29, //Eo#

        /* OTHERS */
        OnCriticalHit = 1 << 30, //CC

        /* UNKNOWN */
        Unknown_1 = 1 << 31, //d
        OnPush = 1 << 32, //M
        Unknown_3 = 1 << 33, //mA
        Unknown_4 = 1 << 34, //ML
        Unknown_5 = 1 << 35, //MP
        Unknown_6 = 1 << 36, //MS
        Unknown_7 = 1 << 37, //P
        Unknown_8 = 1 << 38, //R
        Unknown_9 = 1 << 39, //tF
        OnTackle = 1 << 40, //tS
        Unknown_11 = 1 << 41, //X

        /* CUSTOM */
        AfterDamaged = 1 << 42,
        OnTackled = 1 << 42,
        OnMoved = 1 << 43,

        Unknown = 1 << 63,

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
        TACKLE = 0x00040000,
        PUSH = 0x00080000,
        UNKNOWN = 0x7FFFFFFF,

        /*
        *A=lose AP (101)
        *CC=on critical hit
        *d=
        *D=damage
        *DA=damage air
        *DBA=damage on ally
        *DBE=damage on enemy
        *DC=damaged by weapon
        *DE=damage earth
        *DF=damage fire
        *DG=damage from glyph
        *DI=
        *DM=distance between 0 and 1
        *DN=damage neutral
        *DP=damage from trap
        *Dr=
        *DR=distance > 1
        *DS=not weapon
        *DTB=
        *DTE=
        *DW=damage water
        *EO=on add state
        *EO#=on add state #
        *Eo=on state removed
        *Eo#=on state # removed
        *H=on heal
        *I=instant
        *m=lose mp (127)
        *M=
        *mA=
        *MD=push damage
        *MDM=
        *MDP=
        *ML=
        *MP=
        *MS=
        *P=
        *R=
        *TB=turn begin
        *TE=turn end
        *tF=
        *tS=
        *X*/
    }
}