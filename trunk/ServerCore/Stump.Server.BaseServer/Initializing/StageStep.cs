
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
            Method = (Action)Delegate.CreateDelegate(typeof(Action), method);
        }

        public void Execute()
        {
            if (!Executed)
            {
                Executed = true;
                DateTime startTime = DateTime.Now;
                Method.Invoke();

                StageManager.logger.Info(LogMessage + " in {0} seconds.", ( DateTime.Now - startTime ).TotalSeconds);
            }
        }
    }
}