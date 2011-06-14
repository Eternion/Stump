
namespace Stump.Tools.UtilityBot.FileParser
{
    public class Unknown : IExecution
    {
        public string Execution
        {
            get;
            set;
        }

        #region IExecution Members

        public ExecutionType Type
        {
            get { return ExecutionType.Unknown; }
        }

        #endregion
    }
}