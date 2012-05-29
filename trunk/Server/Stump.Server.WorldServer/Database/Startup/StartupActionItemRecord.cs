using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Database.Startup
{
    [Serializable]
    [ActiveRecord("startup_actions_objects")]
    public sealed class StartupActionItemRecord : WorldBaseRecord<StartupActionItemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("StartupActionId", NotNull = true)]
        public StartupActionRecord StartupAction
        {
            get;
            set;
        }

        [Property("ItemTemplate", NotNull = true)]
        public int ItemTemplate
        {
            get;
            set;
        }

        [Property("Amount", NotNull = true)]
        public int Amount
        {
            get;
            set;
        }

        [Property("MaxEffects", NotNull = true, Default = "1")]
        public bool MaxEffects
        {
            get;
            set;
        }

        public static StartupActionItemRecord[] FindItemsByStartupActionId(StartupActionRecord startupAction)
        {
            return FindAll(Restrictions.Eq("StartupAction", startupAction));
        }
    }
}