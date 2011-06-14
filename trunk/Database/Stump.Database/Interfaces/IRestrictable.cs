namespace Stump.Database.Interfaces
{
    public interface IRestrictable
    {
        bool CantBeAggressed
        {
            get;
            set;
        }

        bool CantBeChallenged
        {
            get;
            set;
        }

        bool CantTrade
        {
            get;
            set;
        }

        bool CantBeAttackedByMutant
        {
            get;
            set;
        }

        bool CantRun
        {
            get;
            set;
        }

        bool ForceSlowWalk
        {
            get;
            set;
        }

        bool CantMinimize
        {
            get;
            set;
        }

        bool CantMove
        {
            get;
            set;
        }

        bool CantAggress
        {
            get;
            set;
        }

        bool CantChallenge
        {
            get;
            set;
        }

        bool CantExchange
        {
            get;
            set;
        }

        bool CantAttack
        {
            get;
            set;
        }

        bool CantChat
        {
            get;
            set;
        }

        bool CantBeMerchant
        {
            get;
            set;
        }

        bool CantUseObject
        {
            get;
            set;
        }

        bool CantUseTaxCollector
        {
            get;
            set;
        }

        bool CantUseInteractive
        {
            get;
            set;
        }

        bool CantSpeakToNpc
        {
            get;
            set;
        }

        bool CantChangeZone
        {
            get;
            set;
        }

        bool CantAttackMonster
        {
            get;
            set;
        }

        bool CantWalk8Directions
        {
            get;
            set;
        }
    }
}