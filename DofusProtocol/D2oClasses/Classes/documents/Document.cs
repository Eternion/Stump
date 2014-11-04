

// Generated on 10/26/2014 23:27:35
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public uint TitleId
        {
            get { return this.titleId; }
            set { this.titleId = value; }
        }
        [D2OIgnore]
        public uint AuthorId
        {
            get { return this.authorId; }
            set { this.authorId = value; }
        }
        [D2OIgnore]
        public uint SubTitleId
        {
            get { return this.subTitleId; }
            set { this.subTitleId = value; }
        }
        [D2OIgnore]
        public uint ContentId
        {
            get { return this.contentId; }
            set { this.contentId = value; }
        }
        [D2OIgnore]
        public String ContentCSS
        {
            get { return this.contentCSS; }
            set { this.contentCSS = value; }
        }
    }
}