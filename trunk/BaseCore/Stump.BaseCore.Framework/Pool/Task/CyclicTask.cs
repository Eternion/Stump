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

namespace Stump.BaseCore.Framework.Pool
{
    public class CyclicTask
    {
        public Delegate Delegate { get; private set; }
        private TimeSpan ExecutionDelay { get; set; }
        private Condition Condition { get; set; }
        private uint? MaxExecutionNbr { get; set; }

        private DateTime LastCall { get; set; }
        private uint CurrentExecutionNbr { get; set; }

        public bool RequireExecution
        {
            get { return ReachDelay && SuitCondition; }
        }

        private bool SuitCondition
        {
            get { return Condition==null || Condition(); }
        }

        private bool ReachDelay
        {
            get { return DateTime.Now.Subtract(LastCall) >= ExecutionDelay; }
        }

        public bool ReachMaxExecutionNbr
        {
            get { return MaxExecutionNbr.HasValue && (CurrentExecutionNbr == MaxExecutionNbr); }
        }

        public CyclicTask(Delegate method, uint delay, Condition condition, uint? maxExecution)
        {
            Delegate = method;
            ExecutionDelay = TimeSpan.FromSeconds(delay);
            Condition = condition;
            MaxExecutionNbr = maxExecution;
            LastCall = DateTime.Now;
        }

        public void Execute()
        {
            Delegate.DynamicInvoke(null);
            LastCall = DateTime.Now;
            CurrentExecutionNbr++;
        }
    }
}