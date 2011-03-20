// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.BaseCore.Framework.Pool.Task;

namespace Stump.BaseCore.Framework.Pool
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