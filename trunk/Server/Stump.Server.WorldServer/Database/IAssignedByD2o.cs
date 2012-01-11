using System.Reflection;

namespace Stump.Server.WorldServer.Database
{
    public interface IAssignedByD2O
    {
        object GenerateAssignedObject(string fieldName, object d2OObject);
    }
}