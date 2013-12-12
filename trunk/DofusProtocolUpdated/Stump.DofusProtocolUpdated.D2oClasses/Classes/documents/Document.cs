

// Generated on 12/12/2013 16:57:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Document", "com.ankamagames.dofus.datacenter.documents")]
    [Serializable]
    public class Document : IDataObject, IIndexedData
    {
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        [I18NField]
        public uint titleId;
        [I18NField]
        public uint authorId;
        [I18NField]
        public uint subTitleId;
        [I18NField]
        public uint contentId;
        public String contentCSS;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
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
        public String ContentCSS
        {
            get { return contentCSS; }
            set { contentCSS = value; }
        }
    }
}