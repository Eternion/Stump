using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("world_map")]
    [AttributeAssociatedFile("WorldMaps")]
    [D2OClass("WorldMap", "com.ankamagames.dofus.datacenter.world")]
    public sealed class WorldMapRecord : DataBaseRecord<WorldMapRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("origineX")]
       [Property("OrigineX")]
       public int OrigineX
       {
           get;
           set;
       }

       [D2OField("origineY")]
       [Property("OrigineY")]
       public int OrigineY
       {
           get;
           set;
       }

       [D2OField("mapWidth")]
       [Property("MapWidth")]
       public double MapWidth
       {
           get;
           set;
       }

       [D2OField("mapHeight")]
       [Property("MapHeight")]
       public double MapHeight
       {
           get;
           set;
       }

       [D2OField("horizontalChunck")]
       [Property("HorizontalChunck")]
       public uint HorizontalChunck
       {
           get;
           set;
       }

       [D2OField("verticalChunck")]
       [Property("VerticalChunck")]
       public uint VerticalChunck
       {
           get;
           set;
       }

       [D2OField("viewableEverywhere")]
       [Property("ViewableEverywhere")]
       public Boolean ViewableEverywhere
       {
           get;
           set;
       }

       [D2OField("minScale")]
       [Property("MinScale")]
       public double MinScale
       {
           get;
           set;
       }

       [D2OField("maxScale")]
       [Property("MaxScale")]
       public double MaxScale
       {
           get;
           set;
       }

       [D2OField("startScale")]
       [Property("StartScale")]
       public double StartScale
       {
           get;
           set;
       }

       [D2OField("centerX")]
       [Property("CenterX")]
       public int CenterX
       {
           get;
           set;
       }

       [D2OField("centerY")]
       [Property("CenterY")]
       public int CenterY
       {
           get;
           set;
       }

       [D2OField("totalWidth")]
       [Property("TotalWidth")]
       public int TotalWidth
       {
           get;
           set;
       }

       [D2OField("totalHeight")]
       [Property("TotalHeight")]
       public int TotalHeight
       {
           get;
           set;
       }

    }
}