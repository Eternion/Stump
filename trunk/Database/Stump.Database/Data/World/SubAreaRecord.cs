using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Classes.ambientSounds;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("sub_area")]
    [AttributeAssociatedFile("SubAreas")]
    [D2OClass("SubArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SubAreaRecord : DataBaseRecord<SubAreaRecord>
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
       public AreaRecord SubArea
       {
           get
           {
               return m_areaRecord;
           }
           set
           {
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

    }
}