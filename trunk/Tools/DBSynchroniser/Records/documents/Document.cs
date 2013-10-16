 


// Generated on 10/13/2013 12:21:14
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Documents")]
    [D2OClass("Document", "com.ankamagames.dofus.datacenter.documents")]
    public class DocumentRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        public uint titleId;
        public uint authorId;
        public uint subTitleId;
        public uint contentId;
        public String contentCSS;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }

        [D2OIgnore]
        public uint AuthorId
        {
            get { return authorId; }
            set { authorId = value; }
        }

        [D2OIgnore]
        public uint SubTitleId
        {
            get { return subTitleId; }
            set { subTitleId = value; }
        }

        [D2OIgnore]
        public uint ContentId
        {
            get { return contentId; }
            set { contentId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ContentCSS
        {
            get { return contentCSS; }
            set { contentCSS = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Document)obj;
            
            Id = castedObj.id;
            TypeId = castedObj.typeId;
            TitleId = castedObj.titleId;
            AuthorId = castedObj.authorId;
            SubTitleId = castedObj.subTitleId;
            ContentId = castedObj.contentId;
            ContentCSS = castedObj.contentCSS;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Document)parent : new Document();
            obj.id = Id;
            obj.typeId = TypeId;
            obj.titleId = TitleId;
            obj.authorId = AuthorId;
            obj.subTitleId = SubTitleId;
            obj.contentId = ContentId;
            obj.contentCSS = ContentCSS;
            return obj;
        
        }
    }
}