using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class AreaRecordConfiguration : EntityTypeConfiguration<AreaRecord>
    {
        public AreaRecordConfiguration()
        {
            ToTable("areas");
        }
    }

    [D2OClass("Area", "com.ankamagames.dofus.datacenter.world")]
    public sealed class AreaRecord : IAssignedByD2O, ISaveIntercepter
    {
        private byte[] m_boundsBin;
        private string m_name;

        public int Id
        {
            get;
            set;
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public int SuperAreaId
        {
            get;
            set;
        }

        public Boolean ContainHouses
        {
            get;
            set;
        }

        public Boolean ContainPaddocks
        {
            get;
            set;
        }

        public byte[] BoundsBin
        {
            get { return m_boundsBin; }
            set
            {
                m_boundsBin = value;
                Bounds = value.ToObject<Rectangle>();
            }
        }

        public Rectangle Bounds
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var area = (DofusProtocol.D2oClasses.Area)d2oObject;
            Id = area.id;
            NameId = area.nameId;
            SuperAreaId = area.superAreaId;
            ContainHouses = area.containHouses;
            ContainPaddocks = area.containPaddocks;
            Bounds = area.bounds;
        }

        #endregion

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_boundsBin = Bounds.ToBinary();
        }
    }
}