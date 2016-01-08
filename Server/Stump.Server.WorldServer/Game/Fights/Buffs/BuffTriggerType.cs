using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public enum BuffTriggerType
    {
        Instant, // I

        /* DAMAGE TRIGGER */
        OnDamaged, // D
        OnDamagedAir, // DA
        OnDamagedEarth, // DE
        OnDamagedFire, // DF
        OnDamagedWater, // DW
        OnDamagedNeutral, // DN
        OnDamagedByAlly, // DBA
        OnDamagedByEnemy, // DBE
        OnDamagedByWeapon, // DC
        OnDamagedBySpell, // DS
        OnDamagedByGlyph, // DG
        OnDamagedByTrap, //DP
        OnDamagedInCloseRange, //DM
        OnDamagedInLongRange, //DR
        OnDamagedByPush, // MD
        OnDamagedUnknown_1, // DI
        OnDamagedUnknown_2, // Dr
        OnDamagedUnknown_3, // DTB
        OnDamagedUnknown_4, // DTE
        OnDamagedUnknown_5, // MDM
        OnDamagedUnknown_6, // MDP


        /* TURN */
        OnTurnBegin, // TB
        OnTurnEnd, // TE

        /* AP, MP */
        OnMPLost, // m
        OnAPLost, // A

        /* HEAL */
        OnHealed, //H

        /* STATE */
        OnStateAdded, // EO
        OnSpecificStateAdded, //EO#
        OnStateRemoved, //Eo
        OnSpecificStateRemoved, //Eo#

        /* OTHERS */
        OnCriticalHit, //CC

        /* UNKNOWN */
        Unknown_1, //d
        OnPush, //M
        Unknown_3, //mA
        Unknown_4, //ML
        Unknown_5, //MP
        Unknown_6, //MS
        Unknown_7, //P
        Unknown_8, //R
        Unknown_9, //tF
        OnTackle, //tS
        Unknown_11, //X

        /* CUSTOM */
        BeforeDamaged,
        AfterDamaged,
        OnTackled,
        OnMoved,
        BeforeAttack,
        AfterAttack,
        AfterHealed,
        OnHeal,
        AfterHeal,
        OnBuffEnded,
        OnBuffEndedTurnEnd,
        BeforeRollCritical,
        AfterRollCritical,

        Unknown,


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