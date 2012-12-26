using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using Stump.ORM;

namespace Stump.Server.BaseServer.Database
{
    public abstract class BaseContext : DbContext
    {
        protected BaseContext()
        {
        }

        protected BaseContext(DbCompiledModel model) : base(model)
        {
        }

        protected BaseContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected BaseContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

        protected BaseContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        protected BaseContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected BaseContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ( (IObjectContextAdapter)this ).ObjectContext.SavingChanges += OnSavingChanges;

            base.OnModelCreating(modelBuilder);
        }

        protected virtual void OnSavingChanges(object sender, EventArgs e)
        {
            foreach (ObjectStateEntry entry in
            ( (ObjectContext)sender ).ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified))
            {
                if (!entry.IsRelationship && ( entry.Entity is ISaveIntercepter ))
                {
                    ( entry.Entity as ISaveIntercepter ).BeforeSave(entry);
                }
            }
        }
    }
}