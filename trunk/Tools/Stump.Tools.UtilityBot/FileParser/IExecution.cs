
namespace Stump.Tools.UtilityBot.FileParser
{
    public enum ExecutionType
    {
        FunctionCall,
        VariableAssignation,
        ControlSequence,
        ControlSequenceEnd,
        Unknown,
    }

    public interface IExecution
    {
        ExecutionType Type
        {
            get;
        }
    }
}