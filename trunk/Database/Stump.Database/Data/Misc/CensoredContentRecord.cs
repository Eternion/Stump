using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("censored_content")]
    [AttributeAssociatedFile("CensoredContents")]
    [D2OClass("CensoredContent", "com.ankamagames.dofus.datacenter.misc")]
    public sealed class CensoredContentRecord : DataBaseRecord<CensoredContentRecord>
    {

       [D2OField("MODULE")]
       [Property("MODULE")]
       public String MODULE
       {
           get;
           set;
       }

    }
}