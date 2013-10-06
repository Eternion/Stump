 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Documents")]
    [D2OClass("Document")]
    public class DocumentRecord : ID2ORecord
    {
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        public uint titleId;
        public uint authorId;
        public uint subTitleId;
        public uint contentId;
        public String contentCSS;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }

        public uint AuthorId
        {
            get { return authorId; }
            set { authorId = value; }
        }

        public uint SubTitleId
        {
            get { return subTitleId; }
            set { subTitleId = value; }
        }

        public uint ContentId
        {
            get { return contentId; }
            set { contentId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Document();
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