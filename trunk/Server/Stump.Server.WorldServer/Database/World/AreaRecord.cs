using System;
using System.Collections;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("areas")]
    [D2OClass("Area", "com.ankamagames.dofus.datacenter.world")]
    public sealed class AreaRecord : WorldBaseRecord<AreaRecord>
    {
       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("nameId")]
       [Property("NameId")]
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

       [D2OField("superAreaId")]
       [Property("SubAreaId")]
       public int SuperAreaId
       {
           get;
           set;
       }

       [D2OField("containHouses")]
       [Property("ContainHouses")]
       public Boolean ContainHouses
       {
           get;
           set;
       }

       [D2OField("containPaddocks")]
       [Property("ContainPaddocks")]
       public Boolean ContainPaddocks
       {
           get;
           set;
       }

       [D2OField("bounds")]
       [Property("Bounds", ColumnType="Serializable")]
       public Rectangle Bounds
       {
           get;
           set;
       }
    }
}