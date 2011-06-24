using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("hint")]
    [AttributeAssociatedFile("Hints")]
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    public sealed class HintRecord : DataBaseRecord<HintRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("categoryId")]
       [Property("CategoryId")]
       public uint CategoryId
       {
           get;
           set;
       }

       [D2OField("gfx")]
       [Property("Gfx")]
       public uint Gfx
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

       [D2OField("mapId")]
       [Property("MapId")]
       public uint MapId
       {
           get;
           set;
       }

       [D2OField("realMapId")]
       [Property("RealMapId")]
       public uint RealMapId
       {
           get;
           set;
       }

    }
}