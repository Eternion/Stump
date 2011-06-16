using System;

namespace Stump.Core.Pool.Task
{
    public class CyclicTask
    {
        public CyclicTask(Action method, int delay, Condition condition, uint? maxExecution)
        {
            Action = method;
            ExecutionDelay = TimeSpan.FromSeconds(delay);
            Condition = condition;
            MaxExecutionNbr = maxExecution;
            LastCall = DateTime.Now;
        }

        public Action Action
        {
            get;
            private set;
        }

        private TimeSpan ExecutionDelay
        {
            get;
            set;
        }

        private Condition Condition
        {
            get;
            set;
        }

        private uint? MaxExecutionNbr
        {
            get;
            set;
        }

        private DateTime LastCall
        {
            get;
            set;
        }

        private uint CurrentExecutionNbr
        {
            get;
            set;
        }

        public bool RequireExecution
        {
            get { return ReachDelay && SuitCondition; }
        }

        private bool SuitCondition
        {
            get { return Condition == null || Condition(); }
        }

        private bool ReachDelay
        {
            get { return DateTime.Now.Subtract(LastCall) >= ExecutionDelay; }
        }

        public bool ReachMaxExecutionNbr
        {
            get { return MaxExecutionNbr.HasValue && (CurrentExecutionNbr == MaxExecutionNbr); }
        }

        public void Execute()
        {
            Action.Invoke();
            LastCall = DateTime.Now;
            CurrentExecutionNbr++;
        }
    }
}