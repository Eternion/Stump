using System;
using Stump.Core.Reflection;
using Stump.Core.Sql;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.AuthServer.Database;
using Stump.Server.WorldServer.Database;

namespace Stump.Tools.CacheManager
{
    public class D2OTable
    {
        private Func<IAssignedByD2O> m_constructor; 

        public D2OTable(Type tableType)
        {
            TableType = tableType;

            if (!tableType.HasInterface(typeof(IAssignedByD2O)))
                throw new Exception(string.Format("{0} must implement IAssignedByD2O", tableType));

            ClassAttribute = tableType.GetCustomAttribute<D2OClassAttribute>();

            if (ClassAttribute == null)
                throw new Exception(string.Format("{0} must have the D2OClass attribute", tableType));

            TableName = tableType.GetCustomAttribute<TableNameAttribute>().TableName;
            var ctor = tableType.GetConstructor(new Type[0]);

            if (ctor == null)
                throw new Exception(string.Format("{0} has no default ctor", tableType));
            m_constructor = ctor.CreateDelegate<Func<IAssignedByD2O>>();
        }

        public string TableName
        {
            get;
            set;
        }

        public D2OClassAttribute ClassAttribute
        {
            get;
            set;
        }

        public Type TableType
        {
            get;
            set;
        }

        public object GenerateRow(object obj)
        {
            var petaObj = m_constructor();
            petaObj.AssignFields(obj);
            return petaObj;
        }
    }
}