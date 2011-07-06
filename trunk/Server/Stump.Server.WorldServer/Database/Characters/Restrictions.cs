using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database.Characters
{
    public class Restrictions
    {
        [Property]
        public bool CantBeAggressed
        {
            get;
            set;
        }

        [Property]
        public bool CantBeChallenged
        {
            get;
            set;
        }

        [Property]
        public bool CantTrade
        {
            get;
            set;
        }

        [Property]
        public bool CantBeAttackedByMutant
        {
            get;
            set;
        }

        [Property]
        public bool CantRun
        {
            get;
            set;
        }

        [Property]
        public bool ForceSlowWalk
        {
            get;
            set;
        }

        [Property]
        public bool CantMinimize
        {
            get;
            set;
        }

        [Property]
        public bool CantMove
        {
            get;
            set;
        }

        [Property]
        public bool CantAggress
        {
            get;
            set;
        }

        [Property]
        public bool CantChallenge
        {
            get;
            set;
        }

        [Property]
        public bool CantExchange
        {
            get;
            set;
        }

        [Property]
        public bool CantAttack
        {
            get;
            set;
        }

        [Property]
        public bool CantChat
        {
            get;
            set;
        }

        [Property]
        public bool CantBeMerchant
        {
            get;
            set;
        }

        [Property]
        public bool CantUseObject
        {
            get;
            set;
        }

        [Property]
        public bool CantUseTaxCollector
        {
            get;
            set;
        }

        [Property]
        public bool CantUseInteractive
        {
            get;
            set;
        }

        [Property]
        public bool CantSpeakToNpc
        {
            get;
            set;
        }

        [Property]
        public bool CantChangeZone
        {
            get;
            set;
        }

        [Property]
        public bool CantAttackMonster
        {
            get;
            set;
        }

        [Property]
        public bool CantWalk8Directions
        {
            get;
            set;
        }
    }
}