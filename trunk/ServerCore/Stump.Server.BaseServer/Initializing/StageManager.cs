
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
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

            foreach (var stageStep in stageSteps)
            {
                if (stageStep.Executed)
                    continue;
                try
                {
                    StageStep step = stageStep;
                    var task = Task.Factory.StartNew(() =>
                    {
                        var localStageStep = step;

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