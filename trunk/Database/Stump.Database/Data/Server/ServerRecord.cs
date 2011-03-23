using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Servers
{
    [Serializable]
    [ActiveRecord("server")]
    [AttributeAssociatedFile("Servers")]
    [D2OClass("Server", "com.ankamagames.dofus.datacenter.servers")]
    public sealed class ServerRecord : DataBaseRecord<ServerRecord>
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

       [D2OField("commentId")]
       [Property("CommentId")]
       public uint CommentId
       {
           get;
           set;
       }

       [D2OField("openingDate")]
       [Property("OpeningDate")]
       public double OpeningDate
       {
           get;
           set;
       }

       [D2OField("language")]
       [Property("Language")]
       public String Language
       {
           get;
           set;
       }

       [D2OField("populationId")]
       [Property("PopulationId")]
       public int PopulationId
       {
           get;
           set;
       }

       [D2OField("gameTypeId")]
       [Property("GameTypeId")]
       public uint GameTypeId
       {
           get;
           set;
       }

       [D2OField("communityId")]
       [Property("CommunityId")]
       public int CommunityId
       {
           get;
           set;
       }

       [D2OField("restrictedToLanguages")]
       [Property("RestrictedToLanguages", ColumnType="Serializable")]
       public List<String> RestrictedToLanguages
       {
           get;
           set;
       }

    }
}