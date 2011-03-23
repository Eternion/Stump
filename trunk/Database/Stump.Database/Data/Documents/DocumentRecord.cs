using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Documents
{
    [Serializable]
    [ActiveRecord("document")]
    [AttributeAssociatedFile("Documents")]
    [D2OClass("Document", "com.ankamagames.dofus.datacenter.documents")]
    public sealed class DocumentRecord : DataBaseRecord<DocumentRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("typeId")]
       [Property("TypeId")]
       public uint TypeId
       {
           get;
           set;
       }

       [D2OField("titleId")]
       [Property("TitleId")]
       public uint TitleId
       {
           get;
           set;
       }

       [D2OField("authorId")]
       [Property("AuthorId")]
       public uint AuthorId
       {
           get;
           set;
       }

       [D2OField("subTitleId")]
       [Property("SubTitleId")]
       public uint SubTitleId
       {
           get;
           set;
       }

       [D2OField("contentId")]
       [Property("ContentId")]
       public uint ContentId
       {
           get;
           set;
       }

    }
}