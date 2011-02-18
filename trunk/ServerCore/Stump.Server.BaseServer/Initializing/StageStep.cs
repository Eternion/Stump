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
using System.Reflection;

namespace Stump.Server.BaseServer.Initializing
{
    public enum Stages
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        End
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class StageStep : Attribute
    {
        public Stages StageNumber;

        public readonly Action Method;

        public string LogMessage;

        public bool Executed;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public StageStep()
        {
        }

        public StageStep(Stages stageNbr, string msg)
        {
            StageNumber = stageNbr;
            LogMessage = msg;
        }

        public StageStep(Stages stageNbr, string msg, MethodInfo method)
        {
            StageNumber = stageNbr;
            LogMessage = msg;
            Method = Delegate.CreateDelegate(typeof(Action), method) as Action;
        }

        public void Execute()
        {
            if (!Executed)
            {
                Executed = true;
                DateTime startTime = DateTime.Now;
                Method.Invoke();

                StageManager.logger.Info(LogMessage + " in {0} seconds.", (DateTime.Now - startTime).TotalSeconds);
            }
        }
    }
}
