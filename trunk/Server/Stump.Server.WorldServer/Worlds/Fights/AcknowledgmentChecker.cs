using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.Worlds.Fights
{
    public class AcknowledgmentChecker
    {
        /// <summary>
        /// Delay in ms before a fighter is declared as lagger.
        /// </summary>
        [Variable]
        public const int CheckTimeout = 5000;

        public event Action<AcknowledgmentChecker, FightActor, FightSequenceAction> Success;

        private void NotifySuccess(FightSequenceAction action)
        {
            Action<AcknowledgmentChecker, FightActor,FightSequenceAction> handler = Success;
            if (handler != null)
                handler(this, Fighter, action);
        }

        public event Action<AcknowledgmentChecker, FightActor,FightSequenceAction> Timeout;

        private void NotifyTimeout(FightSequenceAction action)
        {
            Action<AcknowledgmentChecker, FightActor,FightSequenceAction> handler = Timeout;
            if (handler != null)
                handler(this, Fighter, action);
        }


        public AcknowledgmentChecker(FightActor fighter)
        {
            Fighter = fighter;
        }

        private Timer m_timer;
        public FightActor Fighter
        {
            get;
            private set;
        }

        public FightSequenceAction CurrentAction
        {
            get;
            private set;
        }

        public bool IsChecking()
        {
            return CurrentAction != FightSequenceAction.None;
        }

        public void Check(FightSequenceAction action)
        {
            if (IsChecking())
                throw new Exception(string.Format("Already checking acknowlegment of action '{0}'", CurrentAction));

            CurrentAction = action;
            m_timer = new Timer(TimerCallback, null, CheckTimeout, System.Threading.Timeout.Infinite);
        }

        public void ConfirmAcknowledgment(FightSequenceAction action)
        {
            if (action != CurrentAction)
                throw new Exception(string.Format("Attempt to send the wrong acknowledgment '{0}' instead of '{1}'", action, CurrentAction));

            EndCheck(true);
        }

        private void TimerCallback(object dummy)
        {
            EndCheck(false);
        }

        private void EndCheck(bool success)
        {
            if (m_timer == null)
                return;

            m_timer = null;

            if (success)
                NotifySuccess(CurrentAction);
            else
                NotifyTimeout(CurrentAction);

            CurrentAction = FightSequenceAction.None;
        }
    }
}