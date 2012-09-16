using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class HintRecordConfiguration : EntityTypeConfiguration<HintRecord>
    {
        public HintRecordConfiguration()
        {
            ToTable("hints");
        }
    }
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    public sealed class HintRecord : IAssignedByD2O
    {
       public int Id
       {
           get;
           set;
       }

       public uint CategoryId
       {
           get;
           set;
       }

       public uint Gfx
       {
           get;
           set;
       }

       public uint NameId
       {
           get;
           set;
       }

       private string m_name;

       public string Name
       {
           get
           {
               return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
           }
       }

       public uint MapId
       {
           get;
           set;
       }

       public uint RealMapId
       {
           get;
           set;
       }

        public void AssignFields(object d2oObject)
        {
            var hint = (DofusProtocol.D2oClasses.Hint)d2oObject;
            Id = hint.id;
            NameId = hint.nameId;
            CategoryId = hint.categoryId;
            Gfx = hint.gfx;
            MapId = hint.mapId;
            RealMapId = hint.realMapId;
        }
    }
}