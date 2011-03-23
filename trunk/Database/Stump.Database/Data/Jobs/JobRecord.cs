using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Jobs
{
    [Serializable]
    [ActiveRecord("job")]
    [AttributeAssociatedFile("Jobs")]
    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    public sealed class JobRecord : DataBaseRecord<JobRecord>
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

       [D2OField("specializationOfId")]
       [Property("SpecializationOfId")]
       public int SpecializationOfId
       {
           get;
           set;
       }

       [D2OField("iconId")]
       [Property("IconId")]
       public int IconId
       {
           get;
           set;
       }

       [D2OField("toolIds")]
       [Property("ToolIds", ColumnType="Serializable")]
       public List<int> ToolIds
       {
           get;
           set;
       }

    }
}