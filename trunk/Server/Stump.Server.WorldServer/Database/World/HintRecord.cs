using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("hints")]
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    public sealed class HintRecord : WorldBaseRecord<HintRecord>
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

       private string m_name;

       public string Name
       {
           get
           {
               return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
           }
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