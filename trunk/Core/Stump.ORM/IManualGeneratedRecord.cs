using Stump.ORM.SubSonic.DataProviders;
using Stump.ORM.SubSonic.Schema;

namespace Stump.ORM
{
    public interface IManualGeneratedRecord
    {
        ITable GetTableInformation(IDataProvider provider); 
    }
}