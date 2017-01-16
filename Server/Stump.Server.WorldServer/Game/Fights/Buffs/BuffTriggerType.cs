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
        OnDamagedBySummon, // DI
        OnDamagedByWeapon, // DC
        OnDamagedBySpell, // DS
        OnDamagedByGlyph, // DG
        OnDamagedByTrap, //DP
        OnDamagedInCloseRange, //DM
        OnDamagedInLongRange, //DR
        OnDamagedByPush, // MD
        OnDamagedByEnemyPush, //MDP
        OnDamageEnemyByPush, // MDM
        OnDamagedUnknown_2, // Dr
        OnDamagedUnknown_3, // DTB
        OnDamagedUnknown_4, // DTE


        /* TURN */
        OnTurnBegin, // TB
        OnTurnEnd, // TE

        /* AP, MP, PO */
        OnMPLost, // m
        OnAPLost, // A
        OnRangeLost, // R

        /* HEAL */
        OnHealed, //H

        /* STATE */
        OnStateAdded, // EO
        OnSpecificStateAdded, //EO#
        OnStateRemoved, //Eo
        OnSpecificStateRemoved, //Eo#

        /* BUFF */
        OnDispelled, //d

        /* OTHERS */
        OnCriticalHit, //CC
        OnDeath, //X

        /* MOVEMENT */
        OnPushed, //MP
        OnMoved, //M
        OnTackled, //tF
        OnTackle, //tS

        /* UNKNOWN */
        Unknown_3, //mA
        Unknown_4, //ML
        Unknown_6, //MS
        Unknown_7, //P

        /* CUSTOM */
        BeforeDamaged,
        AfterDamaged,
        BeforeAttack,
        AfterAttack,
        AfterHealed,
        OnHeal,
        AfterHeal,
        OnBuffEnded,
        OnBuffEndedTurnEnd,
        AfterRollCritical,

        Unknown,


        /*
        *A=lose AP (101)
        *CC=on critical hit
        *d=dispell
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
        *M=OnMoved
        *mA=
        *MD=push damage
        *MDM=receive push damages from enemy push
        *MDP=inflict push damage to enemy
        *ML=
        *MP=Pushed
        *MS=
        *P=
        *R=Lost Range
        *TB=turn begin
        *TE=turn end
        *tF=Tackled
        *tS=Tackle
        *X= Death
        */
    }
}