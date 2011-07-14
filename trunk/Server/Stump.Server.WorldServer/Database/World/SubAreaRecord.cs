using System;
using System.Collections;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("subAreas")]
    [D2OClass("SubArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SubAreaRecord : WorldBaseRecord<SubAreaRecord>
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

       /// <summary>
       /// Internal Only. Do not use
       /// </summary>
       [D2OField("areaId")]
       public int AreaId
       {
           get;
           set;
       }

       private AreaRecord m_areaRecord;

       [BelongsTo("AreaId")]
       public AreaRecord Area
       {
           get
           {
               return m_areaRecord;
           }
           set
           {
               if (value != null)
                AreaId = value.Id;

               m_areaRecord = value;
           }
       }

       [D2OField("ambientSounds")]
       [Property("AmbientSounds", ColumnType="Serializable")]
       public List<AmbientSound> AmbientSounds
       {
           get;
           set;
       }

       [D2OField("mapIds")]
       [Property("MapIds", ColumnType="Serializable")]
       public List<uint> MapIds
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

       [D2OField("shape")]
       [Property("Shape", ColumnType="Serializable")]
       public List<int> Shape
       {
           get;
           set;
       }

       [D2OField("customWorldMap")]
       [Property("CustomWorldMap", ColumnType="Serializable")]
       public List<uint> CustomWorldMap
       {
           get;
           set;
       }

       [D2OField("packId")]
       [Property("PackId")]
       public int PackId
       {
           get;
           set;
       }

       protected override bool BeforeSave(IDictionary state)
       {
           // that's a hack to update SubAreaId field without setting SubArea property.
           if (Area == null && AreaId > 0)
               Area = new AreaRecord
               {
                   Id = AreaId
               };


           return base.BeforeSave(state);
       }
    }
}