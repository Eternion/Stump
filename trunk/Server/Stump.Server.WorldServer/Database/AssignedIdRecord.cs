using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database
{
    public class AssignedIdRecord<T> : WorldBaseRecord<T> where T : ActiveRecordBase
    {
        public RecordState State
        {
            get;
            set;
        }

        public bool IsNew
        {
            get { return State == RecordState.New; }
        }

        public bool IsDirty
        {
            get { return State == RecordState.New || State == RecordState.Dirty; }
        }

        public bool IsDeleted
        {
            get { return State == RecordState.Deleted; }
        }

        #region Overrides
        public override void Save()
        {
            if (IsNew)
            {
                Create();
            }
            else
            {
                Update();
            }
        }

        public override void Create()
        {
            State = RecordState.Undefined;

            base.Create();
        }

        public override void SaveAndFlush()
        {
            if (IsNew)
            {
                CreateAndFlush();
            }
            else
            {
                UpdateAndFlush();
            }
        }

        public override void CreateAndFlush()
        {
            State = RecordState.Undefined;

            base.CreateAndFlush();
        }

        public override void Delete()
        {
            if (!IsNew)
            {
                base.Delete();
            }
        }

        public override void DeleteAndFlush()
        {
            if (!IsDeleted && !IsNew)
            {
                State = RecordState.Deleted;

                base.DeleteAndFlush();
            }
        }
        #endregion
    }
}