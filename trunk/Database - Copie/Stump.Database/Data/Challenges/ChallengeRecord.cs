using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Challenges
{
    [Serializable]
    [ActiveRecord("challenge")]
    [AttributeAssociatedFile("Challenge")]
    [D2OClass("Challenge", "com.ankamagames.dofus.datacenter.challenges")]
    public sealed class ChallengeRecord : DataBaseRecord<ChallengeRecord>
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

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public uint DescriptionId
       {
           get;
           set;
       }

    }
}