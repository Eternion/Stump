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

namespace Stump.BaseCore.Framework.Xml
{
    public class CyclicTask
    {
        public Delegate Delegate { get; private set; }
        public TimeSpan ExecutionDelay { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public uint? MaxExecutionNbr { get; private set; }

        public DateTime LastCall { get; private set; }
        public uint CurrentExecutionNbr { get; private set; }

        public bool RequireExecution
        {
            get { return IsBegin && ReachDelay; }
        }

        public bool IsObsolete
        {
            get { return IsEnded || ReachMaxExecutionNbr; }
        }

        private bool ReachDelay
        {
            get { return DateTime.Now.Subtract(LastCall) >= ExecutionDelay; }
        }

        private bool IsBegin
        {
            get { return !StartDate.HasValue || (DateTime.Now > StartDate); }
        }

        private bool IsEnded
        {
            get { return EndDate.HasValue && (DateTime.Now > EndDate); }
        }

        private bool ReachMaxExecutionNbr
        {
            get { return MaxExecutionNbr.HasValue && (CurrentExecutionNbr == MaxExecutionNbr); }
        }

        public CyclicTask(Delegate method, uint delay, DateTime? startDate, DateTime? endDate, uint? maxExecution)
        {
            Delegate = method;
            ExecutionDelay = TimeSpan.FromMilliseconds(delay);
            StartDate = startDate;
            EndDate = endDate;
            MaxExecutionNbr = maxExecution;
        }

        public void Execute()
        {
            Delegate.DynamicInvoke(null);
            LastCall = DateTime.Now;
            CurrentExecutionNbr++;
        }
    }
}