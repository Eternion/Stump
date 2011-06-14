
using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.AuthServer;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.StartupAction
{
    [Serializable]
    [ActiveRecord("startup_actions_objects")]
    public sealed class StartupActionItemRecord : WorldBaseRecord<StartupActionItemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id { get; set; }

        [BelongsTo("StartupActionId", NotNull = true)]
        public StartupActionRecord StartupAction { get; set; }

        [Property("ItemTemplate", NotNull = true)]
        public uint ItemTemplate { get; set; }

        [Property("MaxEffects", NotNull = true, Default = "1")]
        public bool MaxEffects { get; set; }


        public static StartupActionItemRecord[] FindItemsByStartupActionId(StartupActionRecord startupAction)
        {
            return FindAll(Restrictions.Eq("StartupAction", startupAction));
        }
    }
}