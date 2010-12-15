/*************************************************************************
 *
 *  Copyright (C) 2009 - 2010  Tesla Team
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 *************************************************************************/

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using Stump.BaseCore.Framework.Utils;
using NLog;

namespace Stump.Server.BaseServer.Initializing
{
    public static class StageManager
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Time to wait a task after it's has been started. (ms)
        /// </summary>
        private const int WaitTime = 100;
        /// <summary>
        /// Array for our differents stages.
        /// </summary>
        private static readonly List<StageStep> StagesSteps = new List<StageStep>();

        public static void Initialize(Assembly asm)
        {
            Type[] types = asm.GetTypes();

            foreach (Type asmType in types)
            {
                RegisterStage(asmType);
            }

            for (var stg = Stages.One; stg <= Stages.End; stg++)
            {
                List<StageStep> stages = StagesSteps.FindAll(o => o.StageNumber == stg);
                ExecuteStageSteps(stages);          
            }
        }

        private static void ExecuteStageSteps(List<StageStep> stageSteps)
        {
            if (stageSteps.Count == 0)
                return;

            logger.Info("-> Performing Stage {0}", Enum.GetName(typeof(Stages), stageSteps[0].StageNumber));

            var tasks = new Task[(uint)stageSteps.Count];
            var i = 0;

            foreach(var stageStep in stageSteps)
            {
                if (stageStep.Executed)
                    continue;
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        var localStageStep = stageStep;

                        try
                        {
                            localStageStep.Execute();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Exception occured when executing task '{0}'", localStageStep.LogMessage);
                            logger.Error(ex.ToString());
                            throw;
                        }
                    });

                    task.Wait(WaitTime);

                    tasks[i] = task;
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions)
                    {
                        logger.Error(e.Message);
                    }

                }
                finally
                {
                    i++;
                }
            }

            Task.WaitAll(tasks);
            logger.Info("-> Performed Stage {0}.", Enum.GetName(typeof(Stages), stageSteps[0].StageNumber));
        }

        private static void RegisterStage(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attribute = (StageStep)method.GetCustomAttributes(typeof(StageStep), false).FirstOrDefault();

                if (attribute == null)
                    continue;

                try
                {
                    var stage = new StageStep(attribute.StageNumber, attribute.LogMessage, method);

                    StagesSteps.Add(stage);
                }
                catch (Exception e)
                {
                    var handlerStr = type.FullName + "." + method.Name;
                    throw new Exception("Unable to register StageStep " + handlerStr + ".\n" + e.Message);
                }
            
            }
        }
    }
}
